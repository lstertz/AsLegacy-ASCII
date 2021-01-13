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
                public override int CurrentLegacy 
                { 
                    get => legacy;
                    protected set
                    {
                        legacy = value;
                        // TODO :: Update Legacy Record.
                    } 
                }
                private int legacy;

                private string firstCharacterName;
                // TODO :: Track the names of Characters of the lineage.

                private System.Action<int, int, string, Lineage> successorConstructor;
                private bool hasLivingCharacter = true;

                /// <summary>
                /// Constructs a new Lineage.
                /// </summary>
                /// <param name="firstCharacterName">The name of the first Character 
                /// of the new Lineage.</param>
                /// <param name="initialLegacy">The initial legacy of the new Lineage.</param>
                public Lineage(string firstCharacterName, int initialLegacy, 
                    System.Action<int, int, string, Lineage> successorConstructor) : 
                    base(initialLegacy)
                {
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

                public bool SpawnSuccessor()
                {
                    if (hasLivingCharacter)
                        return false;

                    // TODO :: Possibly enforce (check) for latest generation's death.
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
