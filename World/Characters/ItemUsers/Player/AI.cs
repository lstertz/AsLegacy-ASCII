using System;
using static AsLegacy.World;

namespace AsLegacy.Characters
{
    public partial class Player : ItemUser
    {
        /// <summary>
        /// Defines the AI for the Player Character.
        protected class AI : IAI
        {
            /// <inheritdoc/>
            public void UpdateModeOf(Character character) { }

            /// <inheritdoc/>
            public void UpdateTargetOf(Character character) { }

            /// <inheritdoc/>
            public void InitiateActionFor(Character character)
            {
                Player p = character as Player;
                if (p == null)
                    throw new InvalidOperationException("Player.AI is intended to only " +
                        "operate upon Player Characters.");

                if (character.Target != null && character.CurrentAction == null)
                    p.AutoAttackOrMove();
            }
        }
    }
}
