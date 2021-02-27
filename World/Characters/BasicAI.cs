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
                private const int DefenseChance = 5;
                private const float SelfActivationDefenseLimit = 0.2f;
                private const float TargetActivationDefenseRequirement = 0.8f;

                private const int MoveChance = 100;

                /// <inheritdoc/>
                public void UpdateModeOf(Character character)
                {
                    Character target = character.Target;
                    if (character.ActiveMode == Mode.Defend && target != null)
                    {
                        if (target.CurrentAction != null && 
                            target.CurrentAction.Activation > TargetActivationDefenseRequirement)
                            return;
                    }

                    Mode potentialMode = Mode.Normal;
                    if (CharacterAt(character.Row - 1, character.Column) != null ||
                        CharacterAt(character.Row + 1, character.Column) != null ||
                        CharacterAt(character.Row, character.Column - 1) != null ||
                        CharacterAt(character.Row, character.Column + 1) != null)
                        potentialMode = Mode.Attack;

                    if (potentialMode == Mode.Attack && target != null)
                    {
                        World.Action selfAction = character.CurrentAction;
                        World.Action targetAction = target.CurrentAction;

                        if (selfAction != null && targetAction != null && 
                            selfAction.Activation < SelfActivationDefenseLimit && 
                            targetAction.Activation > TargetActivationDefenseRequirement)
                        {
                            Random r = new Random();
                            if (r.Next(DefenseChance) == 0)
                                potentialMode = Mode.Defend;
                        }
                    }

                    character.ActiveMode = potentialMode;
                }

                /// <inheritdoc/>
                public void UpdateTargetOf(Character character)
                {
                    if (character.Target != null && !character.Target.IsAlive)
                        character.Target = null;

                    if (character.ActiveMode == Mode.Attack && character.CurrentAction == null)
                        character.Target = RandomAdjacentCharacter(
                            character.Row, character.Column);
                }

                /// <inheritdoc/>
                public void InitiateActionFor(Character character)
                {
                    if (character.CurrentAction != null)
                        return;

                    if (character.Target != null && character.ActiveMode == Mode.Attack)
                        character.AutoAttackOrMove();

                    if (character.ActiveMode != Mode.Normal)
                        return;

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

                /// <summary>
                /// Provides a random Character that is adjacent to the specified position.
                /// </summary>
                /// <param name="row">The Y-Axis of the position.</param>
                /// <param name="column">The X-Axis of the position.</param>
                /// <returns>A random adjacent Character.</returns>
                private Character RandomAdjacentCharacter(int row, int column)
                {
                    List<Character> characters = new List<Character>();
                    Character up = CharacterAt(row - 1, column);
                    Character down = CharacterAt(row + 1, column);
                    Character left = CharacterAt(row, column - 1);
                    Character right = CharacterAt(row, column + 1);

                    if (up != null)
                        characters.Add(up);
                    if (down != null)
                        characters.Add(down);
                    if (left != null)
                        characters.Add(left);
                    if (right != null)
                        characters.Add(right);

                    if (characters.Count == 0)
                        return null;
                    if (characters.Count == 1)
                        return characters[0];

                    Random r = new Random();
                    return characters[r.Next(characters.Count)];
                }
            }
        }
    }
}
