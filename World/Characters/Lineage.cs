﻿namespace AsLegacy
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

                protected readonly string firstCharacterName;
                // TODO :: Track the names of Characters of the lineage.

                private bool hasLivingCharacter = true;

                /// <summary>
                /// Constructs a new Lineage.
                /// </summary>
                /// <param name="firstCharacterName">The name of the first Character 
                /// of the Lineage.</param>
                /// <param name="initialLegacy">The initial legacy of the Lineage.</param>
                /// <param name="name">The name of the Linage.</param>
                public Lineage(string firstCharacterName, int initialLegacy, string name) : 
                    base(initialLegacy)
                {
                    Name = name;
                    this.firstCharacterName = firstCharacterName;
                }

                /// <summary>
                /// Updates this Lineage with knowledge of the current Characters death.
                /// </summary>
                public void UponCurrentsDeath()
                {
                    hasLivingCharacter = false;
                }

                /// <inheritdoc/>
                public virtual bool SpawnSuccessor()
                {
                    if (hasLivingCharacter)
                        return false;

                    // TODO :: Update naming of successors.

                    new World.Action(spawnTime, () =>
                    {
                        OnSpawnSuccessor();
                        hasLivingCharacter = true;
                    });

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
