using System;

namespace AsLegacy.GUI.HUDs
{
    /// <summary>
    /// Defines the TargetHUD aspect of a Display, 
    /// which is responsible for showing the details of a targeted Character.
    /// </summary>
    public class TargetHUD : HUD
    {
        /// <summary>
        /// Constructs a new TargetHUD.
        /// The constructed TargetHUD has a default frame and is not visible.
        /// </summary>
        /// <param name="width">The width of the TargetHUD.</param>
        public TargetHUD(int width) : base(width, 196, 
            () => 
            {
                if (GameExecution.Focus != null && GameExecution.Focus.Target != GameExecution.Focus)
                    return GameExecution.Focus.Target;
                return null;
            })
        {
            IsVisible = false;
        }

        /// <inheritdoc/>
        public override void Update(TimeSpan timeElapsed)
        {
            SetFrame();
            IsVisible = Character != null;

            base.Update(timeElapsed);
        }
    }
}
