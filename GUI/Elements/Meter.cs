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
        public const int MeterLength = 10;

        /// <summary>
        /// The glyph for when a cell is empty.
        /// </summary>
        public const int EmptyGlyph = 0;
        /// <summary>
        /// The glyph for when a cell is mostly empty.
        /// </summary>
        public const int MostlyEmptyGlyph = 176;
        /// <summary>
        /// The glyph for when a cell is half full.
        /// </summary>
        public const int HalfFullGlyph = 177;
        /// <summary>
        /// The glyph for when a cell is mostly full.
        /// </summary>
        public const int MostlyFullGlyph = 178;
        /// <summary>
        /// The glyph for when a cell is full.
        /// </summary>
        public const int FullGlyph = 219;

        private const float CellIncrement = 1.0f / MeterLength;
        private const float MostlyEmptyGlyphIncrement = 1.0f / 3.0f;
        private const float HalfFullGlyphIncrement = MostlyEmptyGlyphIncrement * 2;

        private int _headerGlyph;
        Func<float> _valueRetriever;
        private int _x;
        private int _y;

        private Color _background;
        private Color _fill;
        private float _lastValue = float.NaN;

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
            _x = x;
            _y = y;
            _valueRetriever = valueRetriever;

            _headerGlyph = headerGlyph;
            _background = background;
            _fill = fill;
        }

        /// <summary>
        /// Initializes unchanging aspects of the Meter's display on the added Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter is being added.</param>
        public override void OnAdded(SadConsole.Console console)
        {
            base.OnAdded(console);

            int x = _x;
            if (_headerGlyph != 0)
            {
                console.SetGlyph(x, _y, _headerGlyph);
                console.SetForeground(x, _y, _fill);
                x++;
            }

            for (int c = 0; c < MeterLength; c++, x++)
            {
                console.SetBackground(x, _y, _background);
                console.SetForeground(x, _y, _fill);
            }
        }

        /// <summary>
        /// Draws the Meter, with updated cell glyphs and header colors, to its Console.
        /// </summary>
        /// <param name="console">The Console to which this Meter draws.</param>
        /// <param name="delta">The time passed since the last draw.</param>
        public override void Draw(SadConsole.Console console, TimeSpan delta)
        {
            float value = _valueRetriever();
            if (_lastValue == value)
                return;

            int x = _x;
            if (_headerGlyph != 0)
            {
                console.SetForeground(x, _y, value > 0 ? _fill : _background);
                x++;
            }

            float cellValue = value / CellIncrement;
            for (int c = 0; c < MeterLength; c++, x++)
            {
                if (c >= cellValue)
                    console.SetGlyph(x, _y, EmptyGlyph);
                else if (c < (int) cellValue)
                    console.SetGlyph(x, _y, FullGlyph);
                else
                {
                    float glyphValue = cellValue % 1.0f;
                    if (glyphValue < MostlyEmptyGlyphIncrement)
                        console.SetGlyph(x, _y, MostlyEmptyGlyph);
                    else if (glyphValue < HalfFullGlyphIncrement)
                        console.SetGlyph(x, _y, HalfFullGlyph);
                    else
                        console.SetGlyph(x, _y, MostlyFullGlyph);
                }
            }
        }
    }
}
