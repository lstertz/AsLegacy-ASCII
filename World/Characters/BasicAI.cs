using System;
using System.Collections.Generic;

namespace AsLegacy
{
    public partial class World
    {
        public partial class Character
        {
            /// <summary>
            /// Defines the BasicAI that performs minimal AI functionality for any Character.
            protected class BasicAI : IAI
            {
                private const int MoveChance = 100;

                /// <inheritdoc/>
                public void UpdateModeOf(Character character) { }

                /// <inheritdoc/>
                public void UpdateTargetOf(Character character) { }

                /// <inheritdoc/>
                public void InitiateActionFor(Character character)
                {
                    if (character.CurrentAction != null)
                        return;

                    if (character.Target != null && character.ActiveMode == Mode.Attack)
                        character.AutoAttackOrMove();

                    Random r = new Random();
                    if (r.Next(MoveChance) == 0)
                    {
                        List<Direction> passableDirections = new List<Direction>();
                        if (IsPassable(character.Row - 1, character.Column))
                            passableDirections.Add(Direction.Up);
                        if (IsPassable(character.Row + 1, character.Column))
                            passableDirections.Add(Direction.Down);
                        if (IsPassable(character.Row, character.Column - 1))
                            passableDirections.Add(Direction.Left);
                        if (IsPassable(character.Row, character.Column + 1))
                            passableDirections.Add(Direction.Right);    

                        if (passableDirections.Count != 0)
                            character.MoveInDirection(
                                passableDirections[r.Next(passableDirections.Count)]);
                    }

                }
            }
        }
    }
}
