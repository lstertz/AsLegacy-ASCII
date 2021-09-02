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
                /// The number of successor points available to Characters of this Lineage.
                /// </summary>
                int SuccessorPoints { get; }

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
                /// The name of the current Character of this Lineage.
                /// An empty string is provided if there is no current Character.
                /// </summary>
                protected string CharacterName => _character == null ? "" : _character.Name;
                private Character _character = null;

                /// <inheritdoc/>
                public override int CurrentLegacy 
                { 
                    get => _legacy;
                    protected set
                    {
                        _legacy = value;
                        if (_legacy > LegacyRecord)
                            LegacyRecord = _legacy;
                    } 
                }
                private int _legacy;

                /// <inheritdoc/>
                public int LegacyRecord { get; private set; } = 0;

                /// <inheritdoc/>
                public string Name { get; private set; }

                /// <summary>
                /// The time that passes after the initiation of a successor spawn 
                /// before a successor is actually spawned.
                /// </summary>
                protected virtual int SpawnTime => 4000;

                /// <inheritdoc/>
                public int SuccessorPoints => (int)_successorPoints;
                private float _successorPoints = 0.0f;

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

                /// <summary>
                /// Increases the internal tracking of successor points by the 
                /// appropriate amount for the specified investment 
                /// in a <see cref="Characters.Passive"/>.
                /// </summary>
                /// <param name="investmentIncrease">The amount of investment 
                /// placed in a <see cref="Characters.Passive"/>.</param>
                public void IncreaseSuccessorPoints(int investmentIncrease)
                {
                    _successorPoints += investmentIncrease / 2.0f;
                }

                /// <summary>
                /// Updates the current Character of the Lineage, replacing either no 
                /// preceeding Character or a preceeding Character that has died.
                /// </summary>
                /// <param name="newCharacter">The Character to become the new 
                /// current Character of the Lineage.</param>
                /// <exception cref="InvalidOperationException">Thrown if the Lineage's 
                /// current Character exists and is still alive.</exception>
                public void Update(Character newCharacter)
                {
                    if (_character == null || !_character.IsAlive)
                    {
                        RankedCharacters.Remove(_character);
                        RankedCharacters.Add(newCharacter);
                        _character = newCharacter;
                    }
                    else
                        throw new InvalidOperationException("A Lineage Character cannot be " +
                            "updated while it has a living active Character.");
                }

                /// <inheritdoc/>
                public virtual bool SpawnSuccessor()
                {
                    if (_character == null || _character.IsAlive)
                        return false;

                    // TODO :: Update naming of successors.
                    _successorPoints = MathF.Floor(_successorPoints);

                    _ = new World.Action(SpawnTime, OnSpawnSuccessor);
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
