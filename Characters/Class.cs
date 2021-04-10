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

            // Investments live on the Character and are passed through Talent algorithms to
            //      determine the value of Effects and Skill qualities.

            // TODO :: Retrieve data-driven details (talents) and construct classes around them 
            //          for each Type of class. Populate Classes.
            // Populate manually for now.
            Classes.Add(Type.Spellcaster, new(
                new Concept[]
                {

                },
                new Passive[]
                {
                    new (Character.Affect.AdditionalMaxHealth, "Endurance",
                        (investment) => investment / 10.0f)
                }));
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
        public ReadOnlyCollection<Concept> Concepts { get; private set; }

        /// <summary>
        /// The <see cref="Passive"/>s of this <see cref="Class"/>.
        /// </summary>
        public ReadOnlyCollection<Passive> Passives { get; private set; }

        private Class(Concept[] concepts, Passive[] passives)
        {
            Concepts = new(concepts);
            Passives = new(passives);
        }
    }
}
