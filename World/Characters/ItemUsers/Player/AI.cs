using AsLegacy.Characters;
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
                if (character.Target == null || character.CurrentAction != null)
                    return;

                switch (character.ActiveMode)
                {
                    case Mode.Normal:
                        // TODO :: Move towards if 'following' is enabled.
                        break;
                    case Mode.Attack:
                        // TODO :: Move towards if not in range of attack.
                        Combat.PerformStandardAttack(character, character.Target);
                        return;
                    case Mode.Defend:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
