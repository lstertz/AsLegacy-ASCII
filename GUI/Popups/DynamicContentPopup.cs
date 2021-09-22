using Microsoft.Xna.Framework;
using SadConsole;
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

        /// <summary>
        /// The content of the pop-up.
        /// </summary>
        public string Content
        {
            get => _content;
            set => UpdateContent(value);
        }
        private string _content;

        /// <inheritdoc/>
        public override string Title
        {
            get => base.Title;
            set
            {
                base.Title = value;

                _requiredTitleWidth = Title.Length + FrameSpaceHorizontal;
                Width = _requiredLineWidth > _requiredTitleWidth ?
                    _requiredLineWidth : _requiredTitleWidth;
            }
        }

        /// <inheritdoc/>
        public override int Width
        {
            get => _width;
            protected set
            {
                _width = value;
                UpdateCloseButtonPosition(_width);
            }
        }
        private int _width;

        /// <inheritdoc/>
        public override int Height { get => _height; protected set => _height = value; }
        private int _height;

        private HorizontalAlignment _alignment;

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
        /// <param name="hasCloseButton">Whether the pop-up has the default close button 
        /// in the top-right.</param>
        /// <param name="alignment">The alignment of the pop-up content.</param>
        public DynamicContentPopup(string title, int minWidth,
            int maxWidth, int maxHeight, bool hasCloseButton = false,
            HorizontalAlignment alignment = HorizontalAlignment.Left) :
            base(title, maxWidth, maxHeight, hasCloseButton)
        {
            _width = Width;
            _height = Height;

            _minWidth = minWidth;
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;

            _alignment = alignment;

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
        private void UpdateContent(string newContent)
        {
            int longestContentWidth = 0;
            List<string> splitContent = GetContentLines(newContent);

            for (int c = 0, count = splitContent.Count; c < count; c++)
                if (splitContent[c].Length > longestContentWidth)
                    longestContentWidth = splitContent[c].Length;

            for (int c = 0, count = splitContent.Count; c < count; c++)
            {
                string content = splitContent[c];
                if (c >= _textLines.Count)
                {
                    Label newLine = new(_maxLineWidth)
                    {
                        Alignment = _alignment,
                        Position = new(FrameSpaceHorizontal / 2,
                            TitleHeightSpace + FrameSpaceVertical / 2 + c)
                    };
                    _textLines.Add(newLine);
                    Add(newLine);
                }

                if (_alignment == HorizontalAlignment.Center)
                {
                    int adjustedX = 1 - ((_maxWidth - longestContentWidth - FrameSpaceHorizontal) / 2);
                    _textLines[c].Position = new(adjustedX,
                        TitleHeightSpace + FrameSpaceVertical / 2 + c);
                }
                else if (_alignment == HorizontalAlignment.Right)
                {
                    int adjustedX = 2 - (_maxWidth - longestContentWidth - FrameSpaceHorizontal);
                    _textLines[c].Position = new(adjustedX,
                        TitleHeightSpace + FrameSpaceVertical / 2 + c);
                }

                _textLines[c].DisplayText = content;
                _textLines[c].IsEnabled = true;
                _textLines[c].IsDirty = true;
                _textLines[c].IsVisible = true;
            }

            _requiredLineWidth = longestContentWidth + FrameSpaceHorizontal;
            Width = _requiredLineWidth > _requiredTitleWidth ?
                _requiredLineWidth : _requiredTitleWidth;
            Height = splitContent.Count + FrameSpaceVertical + TitleHeightSpace;

            for (int c = _textLines.Count - 1; c > splitContent.Count - 1; c--)
                _textLines[c].IsVisible = false;

            _content = newContent;
            Invalidate();
        }

        /// <summary>
        /// Provides the lines that the provided content should be split into.
        /// Splitting will be done in a manner to avoid splitting in the middle of words, and 
        /// will trim whitespace at the start of a line.
        /// </summary>
        /// <param name="content">The content to be split.</param>
        /// <returns>A list of the lines of split content.</returns>
        private List<string> GetContentLines(string content)
        {
            string[] newLineContent = content.Split('\n');
            List<string> splitContent = new();

            for (int c = 0, count = newLineContent.Length; c < count; c++)
            {
                string remainingContent = newLineContent[c].TrimStart();
                while (remainingContent.Length > 0)
                {
                    string lineContent = remainingContent;
                    if (remainingContent.Length > _maxLineWidth)
                    {
                        lineContent = remainingContent.Substring(0, _maxLineWidth);
                        if (remainingContent[_maxLineWidth] != ' ')
                        {
                            for (int lc = lineContent.Length - 1; lc >= 0; lc--)
                            {
                                if (lineContent[lc] == ' ')
                                {
                                    lineContent = lineContent.Substring(0, lc);
                                    break;
                                }
                            }
                        }
                    }
                    remainingContent = remainingContent.Substring(lineContent.Length).TrimStart();
                    splitContent.Add(lineContent);
                }
            }

            return splitContent;
        }
    }
}
