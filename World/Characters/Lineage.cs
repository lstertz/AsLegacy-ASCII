using System;

namespace AsLegacy
{
    public partial class World
    {
        public partial class Character
        {
            /// <summary>
            /// The interface for a Lineage, defining public facing functionality to 
            /// retrieve Lineage details or to progress the Lineage.
            /// </summary>
            public interface ILineage
            {
                /// <summary>
                /// The current legacy of this Lineage, represented as a numerical value (points).
                /// </summary>
                int CurrentLegacy { get; }

                /// <summary>
                /// The highest recorded legacy of this Lineage, 
                /// represented as a numerical value (points).
                /// </summary>
                int LegacyRecord { get; }

                /// <summary>
                /// The name of the Lineage.
                /// </summary>
                string Name { get; }

                /// <summary>
                /// Spawns a new successor (next generation) of this Lineage, 
                /// if the last Character of this Lineage has died.
                /// </summary>
                /// <returns>Whether a successor will be spawned, if false, then 
                /// the last Character of this Lineage is still alive.</returns>
                bool SpawnSuccessor();
            }

            /// <summary>
            /// Defines Lineage, which is responsible for tracking the legacy, Characters, 
            /// and Items that define a lineage across generations.
            /// </summary>
            protected abstract class Lineage : Combat.Legacy, ILineage
            {
                /// <summary>
                /// The current legacy of this Lineage, represented as a numerical value (points).
                /// </summary>
                public override int CurrentLegacy 
                { 
                    get => legacy;
                    protected set
                    {
                        legacy = value;
                        if (legacy > LegacyRecord)
                            LegacyRecord = legacy;
                    } 
                }
                private int legacy;

                /// <inheritdoc/>
                public int LegacyRecord { get; private set; } = 0;

                /// <inheritdoc/>
                public string Name { get; private set; }

                /// <summary>
                /// The time that passes after the initiation of a successor spawn 
                /// before a successor is actually spawned.
                /// </summary>
                protected virtual int spawnTime => 4000;

                protected string CharacterName => character == null ? "" : character.Name;
                private Character character = null;

                // TODO :: Track the names of Characters of the lineage.

                /// <summary>
                /// Constructs a new Lineage.
                /// </summary>
                /// <param name="initialLegacy">The initial legacy of the Lineage.</param>
                /// <param name="name">The name of the Lineage.</param>
                public Lineage(int initialLegacy, string name) : base(initialLegacy)
                {
                    Name = name;
                }

                public void Update(Character newCharacter)
                {
                    if (character == null || !character.IsAlive)
                    {
                        character = newCharacter;
                    }
                    else
                        throw new InvalidOperationException("A Lineage Character cannot be " +
                            "updated while it has a living active Character.");
                }

                /// <inheritdoc/>
                public virtual bool SpawnSuccessor()
                {
                    if (character == null || character.IsAlive)
                        return false;

                    // TODO :: Update naming of successors.

                    new World.Action(spawnTime, OnSpawnSuccessor);
                    return true;
                }

                /// <summary>
                /// Called when a successor should be spawned.
                /// </summary>
                protected abstract void OnSpawnSuccessor();
            }
        }
    }
}
