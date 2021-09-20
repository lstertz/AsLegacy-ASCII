using Microsoft.Xna.Framework.Input;
using SadConsole.Controls;
using SadConsole.Input;

namespace AsLegacy.GUI.Elements
{
    /// <summary>
    /// Defines a <see cref="TextBox"/> that clears its contents on the first key press that 
    /// occurs after it has gained focus.
    /// </summary>
    public class ClearingTextBox : TextBox
    {
        private bool _shouldClear = false;


        /// <inheritdoc/>
        public ClearingTextBox(int width) : base(width) { }

        /// <inheritdoc/>
        public override void Focused()
        {
            base.Focused();
            _shouldClear = true;
        }

        /// <inheritdoc/>
        public override void FocusLost()
        {
            base.FocusLost();
            _shouldClear = false;
        }

        /// <inheritdoc/>
        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            int origLength = EditingText.Length;
            
            bool processed = base.ProcessKeyboard(info);
            if (!processed)
                return false;

            if (_shouldClear && info.KeysPressed.Count > 0 && 
                !info.KeysPressed.Contains(AsciiKey.Get(Keys.Enter, new KeyboardState())) &&
                !info.KeysPressed.Contains(AsciiKey.Get(Keys.Back, new KeyboardState())) &&
                origLength != EditingText.Length)
            {
                EditingText = EditingText.Substring(EditingText.Length - 1);
                _shouldClear = false;
            }

            return true;
        }
    }
}
