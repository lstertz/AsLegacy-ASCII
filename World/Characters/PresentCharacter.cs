﻿using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Represents an abstraction of a Character that has a presence within the World.
        /// Such characters exist in a formal sense and can be interacted with.
        /// </summary>
        public abstract class PresentCharacter : Character
        {
            /// <summary>
            /// Defines the glyph to be shown when the Character is in attack mode.
            /// </summary>
            protected abstract int attackGlyph { get; }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in defend mode.
            /// </summary>
            protected abstract int defendGlyph { get; }

            /// <summary>
            /// Defines the glyph to be shown when the Character is in normal mode.
            /// </summary>
            protected abstract int normalGlyph { get; }


            /// <summary>
            /// Defines the standard directions, for immediate actions, available to the Character.
            /// </summary>
            public enum Direction
            {
                Left,
                Right,
                Up,
                Down
            }

            /// <summary>
            /// Defines the different modes of a Character, which heavily influence the state and 
            /// available actions of a Character.
            /// </summary>
            public enum Mode
            {
                Normal,
                Attack,
                Defend
            }

            /// <summary>
            /// The character's present mode.
            /// </summary>
            public Mode ActiveMode
            {
                get 
                { 
                    return mode;
                }
                private set 
                { 
                    mode = value;

                    switch (value)
                    {
                        case Mode.Normal:
                            Glyph = normalGlyph;
                            break;
                        case Mode.Attack:
                            Glyph = attackGlyph;
                            break;
                        case Mode.Defend:
                            Glyph = defendGlyph;
                            break;
                        default:
                            break;
                    }
                }
            }
            private Mode mode;
            private bool attackEnabled = false;
            private bool defenseEnabled = false;


            /// <summary>
            /// Specifies whether the Character is highlighted, generally for 
            /// some kind of anticipated interaction. Highlighting a Character 
            /// changes its glyph's color.
            /// </summary>
            public bool Highlighted
            {
                get => highlighted;
                set
                {
                    if (highlighted == value)
                        return;

                    highlighted = value;
                    GlyphColor = highlighted ? highlightedGlyphColor : selected ?
                        selectedGlyphColor : originalGlyphColor;

                    if (value)
                    {
                        HighlightedCharacter.Highlighted = false;
                        HighlightedCharacter = this;
                    }
                }
            }
            private bool highlighted;

            /// <summary>
            /// Specifies whether the Character is selected, generally for being targeted. 
            /// Selecting a Character changes its glyph's color.
            /// </summary>
            public bool Selected
            {
                get => selected;
                set
                {
                    if (selected == value)
                        return;

                    selected = value;
                    GlyphColor = highlighted ? highlightedGlyphColor : selected ?
                        selectedGlyphColor : originalGlyphColor;

                    if (value)
                    {
                        SelectedCharacter.Selected = false;
                        SelectedCharacter = this;
                    }
                }
            }
            private bool selected;

            /// <summary>
            /// The name of this Present Character.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Specifies the target of this Present Character.
            /// The target will be the recipient of certain actions performed 
            /// by this Present Character.
            /// </summary>
            public virtual PresentCharacter Target
            {
                get { return target; }
                set { target = value; }
            }
            protected PresentCharacter target = null;


            /// <summary>
            /// Constructs a new Present Character.
            /// </summary>
            /// <param name="row">The row position of the new Present Character.</param>
            /// <param name="column">The column position of the new Present Character.</param>
            /// <param name="glyphColor">The color of the glyph visually displayed to represent 
            /// the new Present Character.</param>
            /// <param name="glyph">The glyph visually displayed to represent 
            /// the new Present Character.</param>
            /// <param name="name">The string name given to the new Present Character.</param>
            protected PresentCharacter(int row, int column, 
                Color glyphColor, int glyph, string name) :
                base(row, column, Color.Transparent, glyphColor, glyph, false)
            {
                mode = Mode.Normal;
                Name = name;
            }

            /// <summary>
            /// Performs the appropriate action for the character's present state, in the 
            /// specified direction, if possible.
            /// </summary>
            /// <param name="direction">The direction in which the action is 
            /// to be performed.</param>
            public void PerformInDirection(Direction direction)
            {
                switch (mode)
                {
                    case Mode.Normal:
                        switch (direction)
                        {
                            case Direction.Left:
                                if (World.IsPassable(Row, Column - 1))
                                    Move(this, Row, Column - 1);
                                break;
                            case Direction.Right:
                                if (World.IsPassable(Row, Column + 1))
                                    Move(this, Row, Column + 1);
                                break;
                            case Direction.Up:
                                if (World.IsPassable(Row - 1, Column))
                                    Move(this, Row - 1, Column);
                                break;
                            case Direction.Down:
                                if (World.IsPassable(Row + 1, Column))
                                    Move(this, Row + 1, Column);
                                break;
                            default:
                                break;
                        }
                        break;
                    case Mode.Attack:
                        break;
                    case Mode.Defend:
                        break;
                    default:
                        break;
                }
            }

            /// <summary>
            /// Toggles between the 3 modes of the Character.
            /// Normal proceeds to Attack, Attack to Defend, and Defend to Normal.
            /// </summary>
            public void ToggleMode()
            {
                switch (mode)
                {
                    case Mode.Normal:
                        ActiveMode = Mode.Attack;
                        break;
                    case Mode.Attack:
                        ActiveMode = Mode.Defend;
                        break;
                    case Mode.Defend:
                        ActiveMode = Mode.Normal;
                        break;
                    default:
                        ActiveMode = Mode.Normal;
                        break;
                }
            }

            /// <summary>
            /// Toggles whether Attack Mode is enabled, and updates 
            /// the current active mode.
            /// </summary>
            public void ToggleAttackMode()
            {
                attackEnabled = !attackEnabled;
                UpdateActiveMode();
            }

            /// <summary>
            /// Specifies whether the Defend Mode is enabled, and updates 
            /// the current active mode.
            /// </summary>
            /// <param name="enabled">Whether the Defend Mode should be enabled.</param>
            public void EnableDefense(bool enabled)
            {
                if (defenseEnabled == enabled)
                    return;

                defenseEnabled = enabled;
                UpdateActiveMode();
            }

            /// <summary>
            /// Updates the active mode of the character, based on the current state of 
            /// enabled defense/attack.
            /// </summary>
            private void UpdateActiveMode()
            {
                if (defenseEnabled)
                    ActiveMode = Mode.Defend;
                else if (attackEnabled)
                    ActiveMode = Mode.Attack;
                else
                    ActiveMode = Mode.Normal;
            }
        }
    }
}
