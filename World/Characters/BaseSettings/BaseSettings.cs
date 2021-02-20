﻿using Microsoft.Xna.Framework;

namespace AsLegacy
{
    public partial class World
    {
        public partial class Character
        {
            /// <summary>
            /// Defines BaseSettings, the basic attributes of a Character, 
            /// prior to instance-specific adjustments.
            /// </summary>
            protected abstract class BaseSettings
            {
                /// <summary>
                /// Defines the color of the Character's Glyphs.
                /// </summary>
                public abstract Color GlyphColor { get; }
                /// <summary>
                /// Defines the glyph to be shown when the Character is in attack mode.
                /// </summary>
                public abstract int AttackGlyph { get; }
                /// <summary>
                /// Defines the glyph to be shown when the Character is in defend mode.
                /// </summary>
                public abstract int DefendGlyph { get; }
                /// <summary>
                /// Defines the glyph to be shown when the Character is in normal mode.
                /// </summary>
                public abstract int NormalGlyph { get; }

                /// <summary>
                /// The initial base maximum health for this specific type of Character.
                /// </summary>
                public abstract float InitialBaseMaxHealth { get; }
                /// <summary>
                /// The initial standard attack damage for this specific type of Character.
                /// </summary>
                public abstract float InitialAttackDamage { get; }
                /// <summary>
                /// The initial standard attack interval for this specific type of Character.
                /// </summary>
                public abstract int InitialAttackInterval { get; }
            }
        }
    }
}
