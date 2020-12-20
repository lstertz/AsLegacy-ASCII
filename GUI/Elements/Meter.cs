using Microsoft.Xna.Framework;
using SadConsole.Components;
using System;

namespace AsLegacy.GUI.Elements
{
    /// <summary>
    /// Defines a Meter GUI Element, which displays an updating meter to visually depict 
    /// a retrieved value, with an optional header glyph.
    /// </summary>
    public class Meter : DrawConsoleComponent
    {
        /// <summary>
        /// The length, in cells, of the meter itself (excludes the option header glyph).
        /// </summary>
        public const int meterLength = 10;

        /// <summary>
        /// The glyph for when a cell is empty.
        /// </summary>
        public const int emptyGlyph = 0;
        /// <summary>
        /// The glyph for when a cell is mostly empty.
        /// </summary>
        public const int mostlyEmptyGlyph = 176;
        /// <summary>
        /// The glyph for when a cell is half full.
        /// </summary>
        public const int halfFullGlyph = 177;
        /// <summary>
        /// The glyph for when a cell is mostly full.
        /// </summary>
        public const int mostlyFullGlyph = 178;
        /// <summary>
        /// The glyph for when a cell is full.
        /// </summary>
        public const int fullGlyph = 219;

        private const float cellIncrement = 1.0f / meterLength;
        private const float mostlyEmptyGlyphIncrement = 1.0f / 3.0f;
        private const float halfFullGlyphIncrement = mostlyEmptyGlyphIncrement * 2;

        private int headerGlyph;
        Func<float> valueRetriever;
        private int x;
        private int y;

        private Color background;
        private Color fill;
        private float lastValue = float.NaN;

        /// <summary>
        /// Constructs a new Meter.
        /// </summary>
        /// <param name="x">The local x position of the Meter in its Console.</param>
        /// <param name="y">The local y position of the Meter in its Console.</param>
        /// <param name="valueRetriever">A Func, from which the value represented by 
        /// the Meter can be retrieved.</param>
        /// <param name="fill">The color of any fill that may be present in a Meter cell.</param>
        /// <param name="background">The background (empty) color of a Meter cell.</param>
        /// <param name="headerGlyph">An option header glyph, to be displayed 
        /// immediately to the left of the Meter.</param>
        public Meter(int x, int y, Func<float> valueRetriever, 
            Color fill, Color background, int headerGlyph = 0)
        {
            this.x = x;
            this.y = y;
            this.valueRetriever = valueRetriever;

            this.headerGlyph = headerGlyph;
            this.background = background;
            this.fill = fill;
        }

        /// <summary>
        /// Initializes unchanging aspects of the Meter's display on the added Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter is being added.</param>
        public override void OnAdded(SadConsole.Console console)
        {
            base.OnAdded(console);

            int x = this.x;
            if (headerGlyph != 0)
            {
                console.SetGlyph(x, y, headerGlyph);
                console.SetForeground(x, y, fill);
                x++;
            }

            for (int c = 0; c < meterLength; c++, x++)
            {
                console.SetBackground(x, y, background);
                console.SetForeground(x, y, fill);
            }
        }

        /// <summary>
        /// Draws the Meter, with updated cell glyphs and header colors, to its Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter draws.</param>
        /// <param name="delta">The time passed since the last draw.</param>
        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            float value = valueRetriever();
            if (lastValue == value)
                return;

            int x = this.x;
            if (headerGlyph != 0)
            {
                console.SetForeground(x, y, value > 0 ? fill : background);
                x++;
            }

            float cellValue = value / cellIncrement;
            for (int c = 0; c < meterLength; c++, x++)
            {
                if (c >= cellValue)
                    console.SetGlyph(x, y, emptyGlyph);
                else if (c < (int) cellValue)
                    console.SetGlyph(x, y, fullGlyph);
                else
                {
                    float glyphValue = cellValue % 1.0f;
                    if (glyphValue < mostlyEmptyGlyphIncrement)
                        console.SetGlyph(x, y, mostlyEmptyGlyph);
                    else if (glyphValue < halfFullGlyphIncrement)
                        console.SetGlyph(x, y, halfFullGlyph);
                    else
                        console.SetGlyph(x, y, mostlyFullGlyph);
                }
            }
        }
    }
}
