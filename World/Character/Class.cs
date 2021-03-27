using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static AsLegacy.World;

namespace AsLegacy.Characters
{
    /// <summary>
    /// Defines a Class, which describes the conceptual and passive talents for a specific
    /// type of Character.
    /// </summary>
    public record Class
    {
        public enum Type
        {
            Spellcaster
        }

        public static readonly ReadOnlyCollection<string> Names = new(new string[]
        {
            "Spellcaster"
        });

        private static readonly Dictionary<Type, Class> Classes = new();


        public static Class Get(Type type) => Classes[type];

        public static Class Get(string name) => Classes[NameToType(name)];

        public static void Init()
        {
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
                    new Passive(Character.Attribute.BaseHealth)
                }));
        }

        public static Type NameToType(string name)
        {
            if(Enum.TryParse(name, out Type type))
                return type;

            throw new NotSupportedException($"The specified name, {name}, " +
                "has no equivalent Type.");
        }

        public static string TypeToName(Type type) => Names[(int)type];


        public ReadOnlyCollection<Concept> Concepts { get; private set; }

        public ReadOnlyCollection<Passive> Passives { get; private set; }

        private Class(Concept[] concepts, Passive[] passives) 
        {
            Concepts = new(concepts);
            Passives = new(passives);
        }
    }

    public abstract record Talent
    {
        public Character.Attribute AffectedAttribute { get; private set; }

        public int ID => GetHashCode();
        
        // TODO :: Algorithm.

        public Talent(Character.Attribute affectedAttribute)
        {
            // TODO :: Require algorithm and description generator.
            AffectedAttribute = affectedAttribute;
        }

        // TODO :: Description formatted with a calculated value, given an investment.
    }

    public record Concept : Talent
    {
        public Concept(Character.Attribute affectedAttribute) :
            base(affectedAttribute)
        {
        }
    }

    public record Passive : Talent
    {
        public Passive(Character.Attribute affectedAttribute) :
            base(affectedAttribute)
        {
            // TODO :: Generates a consistent description from the attribute.
        }
    }
}
