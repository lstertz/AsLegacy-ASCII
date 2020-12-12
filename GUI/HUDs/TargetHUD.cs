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
        public TargetHUD(int width) : base(width, 196, null)
        {
            IsVisible = false;
        }

        /// <summary>
        /// Updates the visuals of this TargetHUD.
        /// </summary>
        /// <param name="timeElapsed">The time passed since the last update.</param>
        public override void Update(TimeSpan timeElapsed)
        {
            World.Character target = World.Player.Target;
            if (target == World.Player)
                target = null;

            if (target != focus)
            {
                focus = target;

                SetFrame();
                IsVisible = target != null;
            }

            base.Update(timeElapsed);
        }
    }
}
