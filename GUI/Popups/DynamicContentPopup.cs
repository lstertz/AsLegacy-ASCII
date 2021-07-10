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
        private const int FrameSpaceHorizontal = 4;
        private const int FrameSpaceVertical = 2;

        public override int Width => _width;
        private int _width;
        public override int Height => _height;
        private int _height;

        private readonly int _maxLineWidth;
        private int _requiredTitleWidth;
        private int _requiredLineWidth;

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

            _maxLineWidth = _maxWidth - FrameSpaceHorizontal;
            _requiredLineWidth = _minWidth + FrameSpaceHorizontal;
            _requiredTitleWidth = title.Length + FrameSpaceHorizontal;

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
            int totalContentLineCount = 0;
            int longestContentWidth = 0;
            string[] splitContent = newContent.Split('\n');

            for (int c = 0, count = splitContent.Length; c < count; c++)
            {
                string content = splitContent[c];
                int contentLineCount = content.Length / _maxLineWidth;
                if (content.Length % _maxLineWidth != 0)
                    contentLineCount++;

                int contentWidth = contentLineCount > 1 ? _maxWidth :
                    content.Length > _minWidth ? content.Length + FrameSpaceHorizontal : _minWidth;
                if (contentWidth > longestContentWidth)
                    longestContentWidth = contentWidth;

                string remainingContent = content;
                for (int cc = 0; cc < contentLineCount; cc++)
                {
                    int totalC = totalContentLineCount + cc;
                    if (totalC >= _textLines.Count)
                    {
                        Label newLine = new(_maxLineWidth)
                        {
                            Position = new(FrameSpaceHorizontal / 2, 
                                TitleHeightSpace + FrameSpaceVertical / 2 + totalC)
                        };
                        _textLines.Add(newLine);
                        Add(newLine);
                    }

                    string currentContent = remainingContent;
                    if (cc < contentLineCount - 1)
                    {
                        currentContent = remainingContent.Substring(0, _maxLineWidth);
                        remainingContent = remainingContent.Substring(_maxLineWidth);
                    }

                    _textLines[totalC].DisplayText = currentContent;
                    _textLines[totalC].IsEnabled = true;
                    _textLines[totalC].IsDirty = true;
                    _textLines[totalC].IsVisible = true;
                }

                totalContentLineCount += contentLineCount;
            }

            _requiredLineWidth = longestContentWidth;
            _width = _requiredLineWidth > _requiredTitleWidth ? 
                _requiredLineWidth : _requiredTitleWidth;
            _height = totalContentLineCount + FrameSpaceVertical + TitleHeightSpace;

            for (int c = _textLines.Count - 1; c > totalContentLineCount - 1; c--)
                _textLines[c].IsVisible = false;

            Invalidate();
        }

        /// <summary>
        /// Updates the title of the pop-up, which may also resize the pop-up view.
        /// </summary>
        /// <param name="newTitle">The new title.</param>
        public void UpdateTitle(string newTitle)
        {
            Title = newTitle;

            _requiredTitleWidth = Title.Length + FrameSpaceHorizontal;
            _width = _requiredLineWidth > _requiredTitleWidth ? 
                _requiredLineWidth : _requiredTitleWidth;
        }
    }
}
