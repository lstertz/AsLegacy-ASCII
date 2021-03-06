﻿using AsLegacy.Characters.Skills;
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
            Spellcaster
        }

        /// <summary>
        /// The names of the available Classes.
        /// </summary>
        public static readonly ReadOnlyCollection<string> Names = new(new string[]
        {
            "Spellcaster"
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
                                Element = Skill.Element.Lightning,
                                Activation = 1.0f,
                                Cooldown = 1.0f,
                                CustomAttributes = new(new Attribute[] 
                                {
                                    new()
                                    {
                                        Aspects = new(new Aspect[] 
                                        {
                                            Aspect.AreaOfEffectDamage,
                                            Aspect.LightningDamage
                                        }),
                                        BaseValue = 0,
                                        Name = "Lightning Damage"
                                    },
                                    new()
                                    {
                                        Aspects = new(new Aspect[]
                                        {
                                            Aspect.AreaOfEffectRange
                                        }),
                                        BaseValue = 1,
                                        Name = "Range"
                                    }
                                }),
                                FormattableDescription = $"Creates an expanding ring of \nlightning that deals damage to \nall enemies that it touches.",
                            }
                        }),
                        Name = "Nova",
                        Category = Skill.Category.Tertiary,
                        Type = Skill.Type.AreaOfEffect,
                        FormattableDescription = $" {0} damage to enemies within range.",
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
                        FormattableDescription = $"Reduce Activation time for all Skills by {0}%.", // Prefixed space for alignment.
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.Activation
                        },
                        Algorithm = (investment) => investment
                    },
                    new()
                    {
                        Name = "Energy Absorption",
                        AffectColor = Color.Red,
                        FormattableDescription = $"Convert {0}% of received Damage to Cooldown for the next used Skill.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.DamageToCooldown
                        },
                        Algorithm = (investment) => investment
                    },
                    new()
                    {
                        Name = "Clear Mind",
                        AffectColor = Color.Green,
                        FormattableDescription = $"For each consecutive Non-Spell Attack, reduce the Activation and Cooldown of the next cast Spell by {0}%.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.NextSpellReductionForConsecutiveAttacks
                        },
                        Algorithm = (investment) => investment
                    },
                    new()
                    {
                        Name = "Energy Reuse",
                        AffectColor = Color.CornflowerBlue,
                        FormattableDescription = $"Reduce Cooldown time for all Non-Elemental Spells by {0}%.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.CooldownNonElementalSpells
                        },
                        Algorithm = (investment) =>investment
                    },
                    new()
                    {
                        Name = "Lightning Mastery",
                        AffectColor = Color.Yellow,
                        FormattableDescription = $"+{0}% Lightning damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.LightningDamage
                        },
                        Algorithm = (investment) => 2.0f * investment
                    },
                    new()
                    {
                        Name = "Fire Mastery",
                        AffectColor = Color.OrangeRed,
                        FormattableDescription = $"+{0}% Fire damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.FireDamage
                        },
                        Algorithm = (investment) => 2.0f * investment
                    },
                    new()
                    {
                        Name = "Ice Mastery",
                        AffectColor = Color.Blue,
                        FormattableDescription = $"+{0}% Ice damage.",
                        Influence = new()
                        {
                            AffectOnAspect = Influence.Purpose.Scale,
                            AffectedAspect = Aspect.IceDamage
                        },
                        Algorithm = (investment) => 2.0f * investment
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
