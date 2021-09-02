using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// The <see cref="Zone"/>s for spawning Characters.
        /// </summary>
        public enum SpawnZone
        {
            /// <summary>
            /// The spawn zone for NPCs.
            /// </summary>
            NPC,
            /// <summary>
            /// The spawn zone for the Player.
            /// </summary>
            Player
        }

        /// <summary>
        /// Defines an area of the world.
        /// </summary>
        private class Zone
        {
            private readonly Rectangle[] _bounds;
            private readonly List<Point> _openPositions = new();

            /// <summary>
            /// Constructs a new <see cref="Zone"/> that consists of the specified bounds.
            /// </summary>
            /// <param name="bounds">The rectangular bounds that define the zone.</param>
            public Zone(Rectangle[] bounds)
            {
                _bounds = bounds;
            }

            /// <summary>
            /// Provides a random position from the zone's known open positions.
            /// </summary>
            /// <returns>The random position.</returns>
            public Point GetRandomOpenPosition()
            {
                Random r = new();
                return _openPositions[r.Next(0, _openPositions.Count)];
            }

            /// <summary>
            /// Registers a position as an open position for this zone, if it is 
            /// within the bounds of the zone.
            /// </summary>
            /// <param name="point">The point to be registered.</param>
            /// <returns>Whether the position was registered; true if the provided 
            /// point was within the bounds of the zone, false otherwise.</returns>
            public bool RegisterOpenPosition(Point point)
            {
                for (int c = 0, count = _bounds.Length; c < count; c++)
                    if (_bounds[c].Contains(point))
                    {
                        _openPositions.Add(point);
                        return true;
                    }


                return false;
            }
        }
    }
}
