using AsLegacy.Characters;
using System;
using System.Collections.Generic;

namespace AsLegacy
{
    public static partial class World
    {
        public abstract partial class Character
        {
            /// <summary>
            /// Defines a projection of a Character.
            /// A projection adds its own stat changes on top of its base Character's stats, 
            /// and can stand-in for its base Character when potential stat changes are needed.
            /// It does not have a presence on the map and cannot interact with other Characters.
            /// </summary>
            public class Projection : ICharacter
            {
                /// <inheritdoc/>
                public int AvailableSkillPoints { get; private set; }

                /// <summary>
                /// Specifies whether the projection is different than its base Character.
                /// </summary>
                public bool HasChanged => _talentInvestments.Count > 0;

                private readonly Dictionary<Talent, int> _talentInvestments = new();
                private readonly Dictionary<Aspect, List<Talent>> _aspectInfluencers = new();

                private readonly Character _base;

                /// <summary>
                /// Constructs a new projection of a Character.
                /// </summary>
                public Projection(Character character)
                {
                    _base = character;
                    AvailableSkillPoints = _base.AvailableSkillPoints;
                    _base.OnAvailableSkillPointGain += (amount) => AvailableSkillPoints += amount;
                }


                /// <summary>
                /// Applies the changes captured in this character projection to 
                /// its underlying <see cref="Character"/>.
                /// </summary>
                public void ApplyToBase()
                {
                    foreach (Talent talent in _talentInvestments.Keys)
                        _base.InvestInTalent(talent, _talentInvestments[talent]);

                    _talentInvestments.Clear();
                    _aspectInfluencers.Clear();
                    AvailableSkillPoints = _base.AvailableSkillPoints;
                }

                /// <inheritdoc/>
                public float GetAffect(Characters.Attribute affectAttribute)
                {
                    float baseValue = affectAttribute.BaseValue;
                    float scale = 1.0f;

                    for (int c = 0, count = affectAttribute.Aspects.Count; c < count; c++)
                    {
                        GetAspectInfluences(affectAttribute.Aspects[c], out float aspectBaseValue,
                            out float aspectScale);
                        baseValue += aspectBaseValue;
                        scale += aspectScale;
                    }
                    scale = affectAttribute.BaseScale * scale;

                    return baseValue * scale;
                }

                /// <inheritdoc/>
                public int GetInvestment(Talent talent)
                {
                    _talentInvestments.TryGetValue(talent, out int amount);
                    return amount + _base.GetInvestment(talent);
                }

                /// <summary>
                /// Provides the amount of projected investment for the specified talent.
                /// </summary>
                /// <param name="talent">The talent whose projected investment
                /// is to be provided.</param>
                /// <returns>The projected investment.</returns>
                public int GetProjectedInvestment(Talent talent)
                {
                    _talentInvestments.TryGetValue(talent, out int amount);
                    return amount;
                }

                /// <inheritdoc/>
                public int InvestInTalent(Talent talent, int amount)
                {
                    if (AvailableSkillPoints == 0)
                        return 0;

                    int actualAmount = AvailableSkillPoints < amount ?
                        AvailableSkillPoints : amount;

                    AvailableSkillPoints -= actualAmount;
                    IncreaseTalentInvestment(talent, actualAmount);

                    return actualAmount;
                }

                /// <summary>
                /// Divests the specified amount from the specified talent.
                /// </summary>
                /// <param name="talent">The talent to be divested.</param>
                /// <param name="amount">The amount to attempt to divested.</param>
                /// <returns>The actual amount divested.</returns>
                public int DivestInTalent(Talent talent, int amount)
                {
                    int divestedAmount = DecreaseTalentInvestment(talent, amount);
                    AvailableSkillPoints += divestedAmount;
                    return divestedAmount;
                }

                /// <summary>
                /// Provides the cumulative base value and scale change associated with the 
                /// specified <see cref="Aspect"/> for this <see cref="Character.Projection"/>.
                /// </summary>
                /// <param name="aspect">The aspect whose cumulative base value and scale change
                /// are to be provided.</param>
                /// <param name="totalBaseValue">The cumulative base value.</param>
                /// <param name="totalScaleChange">The cumulative scale change.</param>
                protected virtual void GetAspectInfluences(Aspect aspect, out float totalBaseValue,
                    out float totalScaleChange)
                {
                    totalBaseValue = 0.0f;
                    totalScaleChange = 0.0f;

                    HashSet<Talent> influencers = new();
                    if (_aspectInfluencers.ContainsKey(aspect))
                        influencers.UnionWith(_aspectInfluencers[aspect]);
                    if (_base._aspectInfluencers.ContainsKey(aspect))
                        influencers.UnionWith(_base._aspectInfluencers[aspect]);

                    foreach (Talent talent in influencers)
                    {
                        if (!_talentInvestments.ContainsKey(talent) && 
                            !_base._talentInvestments.ContainsKey(talent))
                            continue;

                        float affect = talent.GetAffect(GetInvestment(talent));
                        Influence influence = talent.Influence;
                        switch (influence.AffectOnAspect)
                        {
                            case Influence.Purpose.Add:
                                totalBaseValue += affect;
                                break;
                            case Influence.Purpose.ScaleDown:
                                totalScaleChange -= affect;
                                break;
                            case Influence.Purpose.ScaleUp:
                                totalScaleChange += affect;
                                break;
                            case Influence.Purpose.Subtract:
                                totalBaseValue -= affect;
                                break;
                            default:
                                throw new NotImplementedException($"The influence purpose " +
                                    $"{influence.AffectOnAspect} is not supported.");
                        }
                    }
                }


                /// <summary>
                /// Decreases the investment in the specified <see cref="Talent"/> 
                /// by the specified amount if there had been a previous investment.
                /// </summary>
                /// <param name="talent">The talent whose investment is to decrease.</param>
                /// <param name="amount">The amount to decrease, limited by 0 
                /// remaining investment.</param>
                /// <returns>How much was actually decreased.</returns>
                private int DecreaseTalentInvestment(Talent talent, int amount)
                {
                    if (!_talentInvestments.ContainsKey(talent))
                        return 0;

                    int actualAmount = _talentInvestments[talent] < amount ? 
                        _talentInvestments[talent] : amount;
                    _talentInvestments[talent] -= actualAmount;

                    if (_talentInvestments[talent] <= 0)
                    {
                        Aspect affectedAttribute = talent.Influence.AffectedAspect;
                        if (_aspectInfluencers.ContainsKey(affectedAttribute))
                            _aspectInfluencers[affectedAttribute].Remove(talent);
                        if (_aspectInfluencers[affectedAttribute].Count == 0)
                            _aspectInfluencers.Remove(affectedAttribute);

                        _talentInvestments.Remove(talent);
                    }

                    return actualAmount;
                }

                /// <summary>
                /// Increases the investment in the specified <see cref="Talent"/> 
                /// by the specified amount, or defines a new investment if there had been no
                /// previous investment.
                /// </summary>
                /// <param name="talent">The talent whose investment is to increase.</param>
                /// <param name="amount">The amount to increase.</param>
                private void IncreaseTalentInvestment(Talent talent, int amount)
                {
                    if (!_talentInvestments.ContainsKey(talent))
                    {
                        Aspect affectedAttribute = talent.Influence.AffectedAspect;
                        if (!_aspectInfluencers.ContainsKey(affectedAttribute))
                            _aspectInfluencers.Add(affectedAttribute, new());
                        _aspectInfluencers[affectedAttribute].Add(talent);

                        _talentInvestments.Add(talent, amount);
                    }
                    else
                        _talentInvestments[talent] += amount;
                }
            }
        }
    }
}
