using Microsoft.Xna.Framework;
using SadConsole.Controls;
using SadConsole.Input;
using System.Collections.Generic;

namespace AsLegacy.GUI.Popups
{
    /// <summary>
    /// Defines the DynamicContentPopup, a pop-up that adjusts its width and height to 
    /// fit its content.
    /// </summary>
    public class DynamicContentPopup : Popup
    {
        private const int TitleHeightSpace = 2;
        private const int FrameSpace = 2;

        public override int Width => _width;
        private int _width;
        public override int Height => _height;
        private int _height;

        private readonly int _maxLineWidth;

        private readonly int _maxHeight;
        private readonly int _maxWidth;
        private readonly int _minWidth;

        private readonly List<Label> _textLines = new();

        /// <summary>
        /// Constructs a new <see cref="DynamicContentPopup"/>.
        /// </summary>
        /// <param name="title">The initial title of the pop-up.</param>
        /// <param name="minWidth">The minimum width of the pop-up.</param>
        /// <param name="maxWidth">The maximum width that the pop-up will resize to.</param>
        /// <param name="maxHeight">The maximum height that the pop-up will resize to.</param>
        public DynamicContentPopup(string title, int minWidth,
            int maxWidth, int maxHeight) : base(title, maxWidth, maxHeight, false)
        {
            _width = Width;
            _height = Height;

            _minWidth = minWidth;
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;

            _maxLineWidth = _maxWidth - FrameSpace;

            // TODO :: Support a scrollbar if over max height.
            // TODO :: Support a bullet at the start of each label.
        }

        /// <inheritdoc/>
        protected override void Invalidate()
        {
            base.Invalidate();
        }

        /// <inheritdoc/>
        public override bool ProcessMouse(MouseConsoleState state)
        {
            Point p = state.ConsoleCellPosition;
            if (p.X >= Width || p.Y >= Height)
                return false;

            return base.ProcessMouse(state);
        }

        /// <summary>
        /// Updates the content of this pop-up, which may also resize the pop-up view.
        /// </summary>
        /// <param name="newContent">The new content of the pop-up.</param>
        public void UpdateContent(string newContent)
        {
            // TODO :: Support splitting by new line.

            int contentLineCount = newContent.Length / _maxLineWidth;
            if (newContent.Length % _maxLineWidth != 0)
                contentLineCount++;
            int requiredWidth = contentLineCount > 1 ? _maxWidth :
                newContent.Length > _minWidth ? newContent.Length + FrameSpace : _minWidth;

            _width = requiredWidth;
            _height = contentLineCount + FrameSpace + TitleHeightSpace;

            string remainingContent = newContent;
            for (int c = 0; c < contentLineCount; c++)
            {
                if (c >= _textLines.Count)
                {
                    Label newLine = new(_maxLineWidth)
                    {
                        Position = new(FrameSpace / 2, TitleHeightSpace + FrameSpace / 2 + c)
                    };
                    _textLines.Add(newLine);
                    Add(newLine);
                }

                string content = remainingContent;
                if (c < contentLineCount - 1)
                {
                    content = remainingContent.Substring(0, _maxLineWidth);
                    remainingContent = remainingContent.Substring(_maxLineWidth);
                }

                _textLines[c].DisplayText = content;
                _textLines[c].IsEnabled = true;
                _textLines[c].IsDirty = true;
            }

            for (int c = _textLines.Count - 1; c > contentLineCount - 1; c--)
                _textLines[c].IsEnabled = false;

            Invalidate();
        }

        /// <summary>
        /// Updates the title of the pop-up.
        /// </summary>
        /// <param name="newTitle">The new title.</param>
        public void UpdateTitle(string newTitle)
        {
            Title = newTitle;

            // TODO :: Expand width to fit title if needed.
        }
    }
}
