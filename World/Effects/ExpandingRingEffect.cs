using AsLegacy.Characters.Skills;
using Microsoft.Xna.Framework;

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
            private const float TimePerRangeUnit = 45.0f;

            /// <summary>
            /// The maximum modification for how long each effects lasts for each range unit.
            /// The closer to 1 this value is, the great the time spent for each range unit is 
            /// as the range reaches its limit. The value must be less than 1, otherwise the 
            /// effect will never complete.
            /// </summary>
            private const float RangeTimeModifier = 2.0f/3.0f;

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

            /// <summary>
            /// A callback that is invoked the first time damage is dealt.
            /// </summary>
            public System.Action UponDamageDealt { get; init; }

            private int _passedTime = 0;
            private float _lastRadius = 0.0f;

            private bool _dealtDamage = false;

            /// <inheritdoc/>
            protected override void Update(int timeDelta)
            {
                _passedTime += timeDelta;

                float maxRadius = Range + 1;
                float unmodifiedRadius = _passedTime / TimePerRangeUnit;
                float basePercentRadius = unmodifiedRadius / maxRadius;

                // Modify the radius to make it increase more slowly the further the 
                // value is from the origin.
                float timeUnitIncrease = TimePerRangeUnit * RangeTimeModifier * basePercentRadius;
                float currentRadius = _passedTime / (TimePerRangeUnit + timeUnitIncrease);
                float deltaRadius = currentRadius - _lastRadius;
                if (currentRadius >= maxRadius)
                {
                    DealRemainderDamage(currentRadius, deltaRadius);
                    _lastRadius = 0.0f;

                    Stop();
                    return;
                }

                float radiusRemainder = 0.0f;
                int baseRadius = (int)currentRadius;
                if (baseRadius != (int)_lastRadius)
                {
                    radiusRemainder = DealRemainderDamage(currentRadius, deltaRadius);
                    ClearGraphics();
                }
                _lastRadius = currentRadius;

                float damage = BaseDamage * (deltaRadius - radiusRemainder);
                if (currentRadius < 1)
                {
                    SetGraphic(Target.X, Target.Y, 219);
                    DealDamageAt(Target.X, Target.Y, damage);
                }
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

            /// <summary>
            /// Deals damage, if appropriate, to the <see cref="Character"/> at 
            /// the specific location.
            /// </summary>
            /// <param name="x">The x axis value of the location.</param>
            /// <param name="y">The y axis value of the location.</param>
            /// <param name="damage">The damage to dealt.</param>
            private void DealDamageAt(int x, int y, float damage)
            {
                Character c = CharacterAt(y, x);
                if (c == null || c == Performer)
                    return;

                if (!_dealtDamage)
                {
                    UponDamageDealt?.Invoke();
                    _dealtDamage = true;
                }

                c.ReceiveDamage(Performer, damage, Element);
            }

            /// <summary>
            /// Deals any damage that should be dealt within the last radius unit, 
            /// based on the current radius and the last delta.
            /// </summary>
            /// <param name="currentRadius">The current radius of the effect.</param>
            /// <param name="deltaRadius">The change in radius since the last effect update.</param>
            /// <returns>The remainder of the last radius unit for which damage was applied.</returns>
            private float DealRemainderDamage(float currentRadius, float deltaRadius)
            {
                float radiusRemainder = deltaRadius - (currentRadius - ((int)currentRadius));
                float damageRemainder = BaseDamage * radiusRemainder;

                Point[] lastLocations = EffectLocations;
                for (int c = 0, count = lastLocations.Length; c < count; c++)
                    DealDamageAt(lastLocations[c].X, lastLocations[c].Y, damageRemainder);

                return radiusRemainder;
            }
        }
    }
}
