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
            /// <summary>
            /// How long, if unmodified, the effect lasts for each range unit.
            /// </summary>
            private const float BaseTimePerRangeUnit = 45.0f;

            /// <summary>
            /// The maximum modification for how long each effects lasts for each range unit.
            /// This value must be less than <see cref="BaseTimePerRangeUnit"/>, or else 
            /// the effect will never complete.
            /// </summary>
            private const float RangeTimeModifier = 30.0f;

            /// <summary>
            /// The base damage (the expected maximum) to be applied per range unit.
            /// </summary>
            public float BaseDamage { get; init; }

            /// <summary>
            /// The elemental type of the effect.
            /// </summary>
            public Skill.Element Element { get; init; }

            /// <summary>
            /// The range of the expanding ring.
            /// 0 is only the point of origin.
            /// 1+ is the point of origin and 1+ tile around the point of origin.
            /// </summary>
            public float Range { get; init; }

            private int _passedTime = 0;
            private int _lastBaseRadius = -1;

            private bool _dealtDamage = false;

            protected override void Update(int timeDelta)
            {
                _passedTime += timeDelta;

                float maxRadius = Range + 1;
                float unmodifiedRadius = _passedTime / BaseTimePerRangeUnit;
                float basePercentRadius = unmodifiedRadius / maxRadius;

                // Modify the radius to make it increase more slowly the further the 
                // value is from the origin, up to an increase defined by the RangeTimeModifier.
                float currentRadius = _passedTime / 
                    (BaseTimePerRangeUnit + RangeTimeModifier * basePercentRadius);
                if (currentRadius >= maxRadius)
                {
                    Stop();
                    return;
                }

                int baseRadius = (int)currentRadius;
                if (baseRadius != _lastBaseRadius)
                {
                    // TODO : 78 :: Calculate damage to deal to last ring's tiles.
                    // TODO : 78 :: Deal damage to last ring's tiles.
                    ClearGraphics();
                }
                _lastBaseRadius = baseRadius;

                float damage = BaseDamage;
                // TODO : 78 :: Calculate damage to deal to current ring's tiles.
                if (currentRadius < 1)
                    SetGraphic(Target.X, Target.Y, 219);
                else
                {
                    bool displayInnerRing = currentRadius - baseRadius < 0.5f;
                    int xMin = Target.X - baseRadius;
                    int xMax = Target.X + baseRadius;
                    int yMin = Target.Y - baseRadius;
                    int yMax = Target.Y + baseRadius;

                    for (int y = yMin; y < yMax; y++)
                    {
                        DealDamageAt(xMin, y, damage);
                        SetGraphic(xMin, y, displayInnerRing ? 222 : 221);

                        DealDamageAt(xMax, y, damage);
                        SetGraphic(xMax, y, displayInnerRing ? 221 : 222);
                    }

                    for (int x = xMin; x <= xMax; x++)
                    {
                        DealDamageAt(x, yMin, damage);
                        DealDamageAt(x, yMax, damage);

                        if (x == xMin)
                        {
                            SetGraphic(x, yMin, displayInnerRing ? 260 : 256);
                            SetGraphic(x, yMax, displayInnerRing ? 262 : 258);
                        }
                        else if (x == xMax)
                        {
                            SetGraphic(x, yMin, displayInnerRing ? 261 : 257);
                            SetGraphic(x, yMax, displayInnerRing ? 263 : 259);
                        }
                        else
                        {
                            SetGraphic(x, yMin, displayInnerRing ? 220 : 223);
                            SetGraphic(x, yMax, displayInnerRing ? 223 : 220);
                        }
                    }
                }
            }

            private void DealDamageAt(int x, int y, float damage)
            {
                Character c = CharacterAt(y, x);
                if (c == null || c == Performer)
                    return;

                // TODO : 80 :: Notify performer that its skill dealt damage (to gain a skill point).
                _dealtDamage = true;

                c.ReceiveDamage(Performer, damage, Element);
            }
        }
    }
}
