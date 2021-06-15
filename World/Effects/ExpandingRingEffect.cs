using AsLegacy.Characters.Skills;

namespace AsLegacy
{
    public static partial class World
    {
        /// <summary>
        /// Defines the expanding ring effect, which is an <see cref="Effect"/> that simulates 
        /// an expanding ring (an area of effect) up to a specified range, 
        /// of some elemental type, dealing damage to those caught in the ring.
        /// </summary>
        private class ExpandingRingEffect : Effect
        {
            private const float TimePerRangeHalfUnit = 300.0f;
            private const int TimeForCenterDisplay = 90;

            public float BaseDamage { get; init; }

            public Skill.Element Element { get; init; }

            public float Range { get; init; }

            private int _passedTime = 0;
            private int _lastRangeHalfUnit = -1;

            protected override void Update(int timeDelta)
            {
                _passedTime += timeDelta;

                // TODO : 78 :: Perform practical effect (apply damage to affected characters).
                float rangeHalfUnit = _passedTime / TimePerRangeHalfUnit;
                if (rangeHalfUnit >= Range * 2)
                {
                    Stop();
                    return;
                }

                int roundedRangeHalfUnit = (int)rangeHalfUnit;
                if (roundedRangeHalfUnit == 0)
                    SetGraphic(Target.X, Target.Y,
                        _passedTime < TimeForCenterDisplay ? 219 : 32);

                if (roundedRangeHalfUnit == _lastRangeHalfUnit)
                    return;

                bool displayInnerRing = roundedRangeHalfUnit % 2 == 0;
                //if (displayInnerRing)
                  //  ClearGraphics();

                int xMin = Target.X - roundedRangeHalfUnit;
                int xMax = Target.X + roundedRangeHalfUnit;
                int yMin = Target.Y - roundedRangeHalfUnit;
                int yMax = Target.Y + roundedRangeHalfUnit;
                for (int y = yMin; y < yMax; y++)
                {
                    // Verticals.
                }
                for (int x = xMin; x <= xMax; x++)
                {
                    if (x == xMin)
                    {
                        SetGraphic(x, yMin, displayInnerRing ? 260 : 256);
                        // yLimit
                    }
                    else if (x == xMax)
                    {
                        SetGraphic(x, yMin, displayInnerRing ? 261 : 257);
                        // yLimit
                    }
                    else
                    {
                        SetGraphic(x, yMin, displayInnerRing ? 220 : 223);
                        // yLimit
                    }
                }

                _lastRangeHalfUnit = (int)rangeHalfUnit;
            }
        }
    }
}
