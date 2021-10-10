using AsLegacy.Characters.Skills;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static AsLegacy.World;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Class, which describes the conceptual and passive talents for a specific
    /// type of <see cref="Character"/>.
    /// </summary>
    public record Class
    {
        /// <summary>
        /// The types of Classes available.
        /// </summary>
        public enum Type
        {
            Bear,
            GiantRat,
            Spellcaster,
            Wolf
        }

        /// <summary>
        /// The names of the available Classes.
        /// </summary>
        public static readonly ReadOnlyCollection<string> Names = new(new string[]
        {
            "Bear",
            "Giant Rat",
            "Spellcaster",
            "Wolf"
        });

        private static readonly Dictionary<Type, Class> Classes = new();


        /// <summary>
        /// Provides the instance of the <see cref="Class"/> of the given Type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of <see cref="Class"/> 
        /// to be retrieved.</param>
        /// <returns>The instance of the <see cref="Class"/>.</returns>
        public static Class Get(Type type) => Classes[type];

        /// <summary>
        /// Provides the instance of the <see cref="Class"/> identified by the provided name.
        /// </summary>
        /// <param name="name">The name of the <see cref="Class"/> to be retrieved.</param>
        /// <returns>The instance of the <see cref="Class"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown if the name does not 
        /// match a known <see cref="Type"/>.</exception>
        public static Class Get(string name) => Classes[NameToType(name)];

        /// <summary>
        /// Initializes all of the Classes (the instances).
        /// </summary>
        public static void Init()
        {
            Classes.Clear();

            // TODO :: Retrieve data-driven details (talents) and construct classes around them 
            //          for each Type of class. Populate Classes.
            // Populate manually for now.
            Classes.Add(Type.GiantRat, new()
            {
                Concepts = new(new Concept[]
                {
                    new()
                    {
                        Affinities = new(new Affinity[]
                        {
                            new()
                            {
                                Name = "Quick Bite",
                                AffectColor = Color.LightGray,
                                BaseActivation = 0.5f,
                                BaseCooldown = 0.75f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.BaseDamage, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AdjacentTargetOneTimeDamage,
                                                Aspect.PhysicalDamage,
                                                Aspect.PiercingDamage
                                            }),
                                            Name = "Damage"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Physical,
                                FormattableDescription = $"A quick bite.",
                                Performance = Skill.Performance.Attack
                            },
                            new()
                            {
                                Name = "Clench",
                                AffectColor = Color.DarkGray,
                                BaseActivation = 1.5f,
                                BaseCooldown = 0.5f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.BaseDamage, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AdjacentTargetOneTimeDamage,
                                                Aspect.PhysicalDamage,
                                                Aspect.PiercingDamage
                                            }),
                                            BaseScale = 2.0f,
                                            Name = "Damage"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Physical,
                                FormattableDescription = $"A focused forceful bite.",
                                Performance = Skill.Performance.Attack
                            }
                        }),
                        Name = "Bite",
                        Category = Skill.Category.Primary,
                        Type = Skill.Type.AdjacentAttack,
                        FormattableDescription = $" {0} base damage to adjacent target.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Add,
                            AffectedAspect = Aspect.AdjacentTargetOneTimeDamage
                        },
                        Algorithm = (investment) => 1 * investment
                    }
                }),
                Passives = new(new Passive[]
                {
                    new()
                    {
                        Name = "Agility",
                        AffectColor = Color.Silver,
                        FormattableDescription = $"-{0}% Activation for all actions.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.Activation
                        },
                        Algorithm = (investment) => investment / 50.0f
                    },
                    new()
                    {
                        Name = "Hearty",
                        AffectColor = Color.DarkRed,
                        FormattableDescription = $"+{0} Max Health.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Add,
                            AffectedAspect = Aspect.MaxHealth
                        },
                        Algorithm = (investment) => 0.25f * investment
                    },
                    new()
                    {
                        Name = "Strength",
                        AffectColor = Color.Gold,
                        FormattableDescription = $"+{0}% Physical Damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleUp,
                            AffectedAspect = Aspect.PhysicalDamage
                        },
                        Algorithm = (investment) => investment / 50.0f
                    }
                })
            });
            Classes.Add(Type.Spellcaster, new()
            {
                Concepts = new(new Concept[]
                {
                    new()
                    {
                        Affinities = new(new Affinity[]
                        {
                            new()
                            {
                                Name = "Shock Ring",
                                AffectColor = Color.Yellow,
                                BaseActivation = 1.0f,
                                BaseCooldown = 1.0f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.BaseDamage, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AreaOfEffectDamage,
                                                Aspect.LightningDamage
                                            }),
                                            Name = "Lightning Damage"
                                        }
                                    },
                                    {
                                        Affect.Setting.Range, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AreaOfEffectRange
                                            }),
                                            BaseValue = 1,
                                            Name = "Range"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Lightning,
                                FormattableDescription = $"Creates an expanding ring of \nlightning that deals damage to \nall enemies that it touches.",
                                Performance = Skill.Performance.Spell
                            },
                            new()
                            {
                                Name = "Band of Ice",
                                AffectColor = Color.LightSkyBlue,
                                BaseActivation = 1.5f,
                                BaseCooldown = 1.0f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.BaseDamage, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AreaOfEffectDamage,
                                                Aspect.IceDamage
                                            }),
                                            BaseScale = 1.1f,
                                            Name = "Ice Damage"
                                        }
                                    },
                                    {
                                        Affect.Setting.Range, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AreaOfEffectRange
                                            }),
                                            BaseValue = 1,
                                            Name = "Range"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Ice,
                                FormattableDescription = $"Creates an expanding ring of\nice that deals damage to all\nenemies that it touches.",
                                Performance = Skill.Performance.Spell
                            },
                            new()
                            {
                                Name = "Exploding Star",
                                AffectColor = Color.Orange,
                                BaseActivation = 1.5f,
                                BaseCooldown = 1.5f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.BaseDamage, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AreaOfEffectDamage,
                                                Aspect.FireDamage
                                            }),
                                            BaseScale = 1.2f,
                                            Name = "Fire Damage"
                                        }
                                    },
                                    {
                                        Affect.Setting.Range, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.AreaOfEffectRange
                                            }),
                                            BaseValue = 1,
                                            Name = "Range"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Fire,
                                FormattableDescription = $"Creates an expanding ring of\nfire that deals damage to all\nenemies that it touches.",
                                Performance = Skill.Performance.Spell
                            }
                        }),
                        Name = "Nova",
                        Category = Skill.Category.Tertiary,
                        Type = Skill.Type.AreaOfEffect,
                        FormattableDescription = $" {0} base damage within range.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Add,
                            AffectedAspect = Aspect.AreaOfEffectDamage
                        },
                        Algorithm = (investment) => 1 * investment
                    }
                }),
                Passives = new(new Passive[]
                {
                    new()
                    {
                        Name = "Focus",
                        AffectColor = Color.Goldenrod,
                        FormattableDescription = $"Reduce Activation time for all Skills by {0}%.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.SkillActivation
                        },
                        Algorithm = (investment) => investment / 100.0f
                    },
                    new()
                    {
                        Name = "Energy Absorption",
                        AffectColor = Color.Red,
                        FormattableDescription = $"Convert {0}% of received Damage to Cooldown (1 damage to .1 seconds) for the next used Skill.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleUp,
                            AffectedAspect = Aspect.DamageToCooldown
                        },
                        Algorithm = (investment) => investment / 100.0f
                    },
                    new()
                    {
                        Name = "Clear Mind",
                        AffectColor = Color.Green,
                        FormattableDescription = $"For each consecutive Non-Spell Attack, reduce the final Activation and Cooldown of the next cast Spell by {0}%.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.NextSpellReductionForConsecutiveAttacks
                        },
                        Algorithm = (investment) => investment / 100.0f
                    },
                    new()
                    {
                        Name = "Energy Reuse",
                        AffectColor = Color.CornflowerBlue,
                        FormattableDescription = $"Reduce Cooldown time for all Non-Elemental Spells by {0}%.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.CooldownNonElementalSpells
                        },
                        Algorithm = (investment) => investment / 100.0f
                    },
                    new()
                    {
                        Name = "Lightning Mastery",
                        AffectColor = Color.Yellow,
                        FormattableDescription = $"+{0}% Lightning damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleUp,
                            AffectedAspect = Aspect.LightningDamage
                        },
                        Algorithm = (investment) => investment / 50.0f
                    },
                    new()
                    {
                        Name = "Ice Mastery",
                        AffectColor = Color.Blue,
                        FormattableDescription = $"+{0}% Ice damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleUp,
                            AffectedAspect = Aspect.IceDamage
                        },
                        Algorithm = (investment) => investment / 50.0f
                    },
                    new()
                    {
                        Name = "Fire Mastery",
                        AffectColor = Color.OrangeRed,
                        FormattableDescription = $"+{0}% Fire damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleUp,
                            AffectedAspect = Aspect.FireDamage
                        },
                        Algorithm = (investment) => investment / 50.0f
                    }
                })
            });
            Classes.Add(Type.Wolf, new()
            {
                Concepts = new(new Concept[]
                {
                    new()
                    {
                        Affinities = new(new Affinity[]
                        {
                            new()
                            {
                                Name = "Call to Attack",
                                AffectColor = Color.Red,
                                BaseActivation = 1.5f,
                                BaseCooldown = 1.0f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.Range, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.CalloutRange
                                            }),
                                            BaseValue = 2,
                                            Name = "Effect Range"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Vocal,
                                FormattableDescription = $"A howl that calls out a target to be attacked.",
                                Performance = Skill.Performance.Expression
                            },
                            new()
                            {
                                Name = "Call to Defend",
                                AffectColor = Color.MediumBlue,
                                BaseActivation = 1.0f,
                                BaseCooldown = 1.5f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.Range, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.CalloutRange
                                            }),
                                            BaseValue = 2,
                                            Name = "Effect Range"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Vocal,
                                FormattableDescription = $"A howl that calls out a target to be defended.",
                                Performance = Skill.Performance.Expression
                            },
                            new()
                            {
                                Name = "Call to Avoid",
                                AffectColor = Color.YellowGreen,
                                BaseActivation = 2.0f,
                                BaseCooldown = 1.0f,
                                CustomAttributes = new(new Dictionary<Affect.Setting, Attribute>()
                                {
                                    {
                                        Affect.Setting.Range, new()
                                        {
                                            Aspects = new(new Aspect[]
                                            {
                                                Aspect.CalloutRange
                                            }),
                                            BaseValue = 2,
                                            Name = "Effect Range"
                                        }
                                    }
                                }),
                                Element = Skill.Element.Vocal,
                                FormattableDescription = $"A howl that calls out a target to be avoided.",
                                Performance = Skill.Performance.Expression
                            }
                        }),
                        Name = "Howl",
                        Category = Skill.Category.Primary,
                        Type = Skill.Type.Callout,
                        FormattableDescription = $" {0} range of the howl's effect.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Add,
                            AffectedAspect = Aspect.CalloutRange
                        },
                        Algorithm = (investment) => investment / 10.0f
                    }
                }),
                Passives = new(new Passive[]
                {
                    new()
                    {
                        Name = "Cold Resistance",
                        AffectColor = Color.LightBlue,
                        FormattableDescription = $"-{0}% Damage from ice.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.IceDamage
                        },
                        Algorithm = (investment) => investment / 50.0f
                    },
                    new()
                    {
                        Name = "Endurance",
                        AffectColor = Color.CornflowerBlue,
                        FormattableDescription = $"-{0}% Cooldown.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.Cooldown
                        },
                        Algorithm = (investment) => investment / 50.0f
                    },
                    new()
                    {
                        Name = "Quick Feet",
                        AffectColor = Color.Goldenrod,
                        FormattableDescription = $"-{0}% Activation for movement.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.ScaleDown,
                            AffectedAspect = Aspect.MovementActivation
                        },
                        Algorithm = (investment) => investment / 50.0f
                    }
                })
            });
        }

        /// <summary>
        /// Provides the <see cref="Type"/> identified by the provided name.
        /// </summary>
        /// <param name="name">The name identifying a <see cref="Type"/>.</param>
        /// <returns>The <see cref="Type"/> identified by the provided name.</returns>
        /// <exception cref="NotSupportedException">Thrown if the name does not 
        /// match a known <see cref="Type"/>.</exception>
        public static Type NameToType(string name)
        {
            if (Enum.TryParse(name, out Type type))
                return type;

            throw new NotSupportedException($"The specified name, {name}, " +
                "has no equivalent Type.");
        }

        /// <summary>
        /// Provides the name for a specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> whose name is to be retrieved.</param>
        /// <returns>The name of the specified <see cref="Type"/>.</returns>
        public static string TypeToName(Type type) => Names[(int)type];


        /// <summary>
        /// The <see cref="Concept"/>s of this <see cref="Class"/>.
        /// </summary>
        public ReadOnlyCollection<Concept> Concepts { get; init; }

        /// <summary>
        /// The <see cref="Passive"/>s of this <see cref="Class"/>.
        /// </summary>
        public ReadOnlyCollection<Passive> Passives { get; init; }
    }
}
