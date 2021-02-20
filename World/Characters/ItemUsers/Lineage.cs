using Microsoft.Xna.Framework;

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
            protected class Lineage : Combat.Legacy, ILineage
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

                private readonly string firstCharacterName;
                // TODO :: Track the names of Characters of the lineage.

                private readonly System.Action<int, int, string, Lineage> successorConstructor;
                private bool hasLivingCharacter = true;

                /// <summary>
                /// Constructs a new Lineage.
                /// </summary>
                /// <param name="firstCharacterName">The name of the first Character 
                /// of the Lineage.</param>
                /// <param name="initialLegacy">The initial legacy of the Lineage.</param>
                /// <param name="name">The name of the Linage.</param>
                /// <param name="successorConstructor">An Action to construct a 
                /// successor for a new generation of the Lineage.</param>
                public Lineage(string firstCharacterName, int initialLegacy, string name, 
                    System.Action<int, int, string, Lineage> successorConstructor) : 
                    base(initialLegacy)
                {
                    Name = name;

                    this.firstCharacterName = firstCharacterName;
                    this.successorConstructor = successorConstructor;
                }

                /// <summary>
                /// Updates this Lineage with knowledge of the current Characters death.
                /// </summary>
                public void UponCurrentsDeath()
                {
                    hasLivingCharacter = false;
                }

                /// <inheritdoc/>
                public bool SpawnSuccessor()
                {
                    if (hasLivingCharacter)
                        return false;

                    // TODO :: Update naming of successors.

                    Point point = GetRandomPassablePosition();
                    new World.Action(4000, () =>
                    {
                        successorConstructor(point.Y, point.X, firstCharacterName, this);
                        hasLivingCharacter = true;
                    });

                    return true;
                }
            }
        }
    }
}
