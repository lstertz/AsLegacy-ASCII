namespace AsLegacy.Characters
{
    /// <summary>
    /// Specifies an aspect of an <see cref="Affinity"/>, or how a <see cref="Concept"/> or 
    /// <see cref="Passive"/> may influence an <see cref="Affinity"/> 
    /// (through <see cref="Influence"/>) to produce the affect of the <see cref="Affinity"/>.
    /// </summary>
    public enum Aspect
    {
        /// <summary>
        /// The aspect pertaining directly to the activation time of a <see cref="Skills.Skill"/>.
        /// </summary>
        Activation,
        /// <summary>
        /// The aspect pertaining to the damage dealt in a single motion to a specific target.
        /// </summary>
        AdjacentTargetOneTimeDamage,
        /// <summary>
        /// The aspect pertaining to the damage dealt by an area 
        /// of effect <see cref="Skills.Skill"/>.
        /// </summary>
        AreaOfEffectDamage,
        /// <summary>
        /// The aspect pertaining to the range of an area 
        /// of effect <see cref="Skills.Skill"/>, i.e. area of effect based damage.
        /// </summary>
        AreaOfEffectRange,
        /// <summary>
        /// The aspect pertaining to the range of a callout.
        /// </summary>
        CalloutRange,
        /// <summary>
        /// The aspect pertaining to the cooldown of all <see cref="Skills.Skill"/>.
        /// </summary>
        Cooldown,
        /// <summary>
        /// The aspect pertaining to the cooldown of non-elemental <see cref="Skills.Skill"/>s.
        /// </summary>
        CooldownNonElementalSpells,
        /// <summary>
        /// The aspect pertaining to the conversion of received damage to additional 
        /// cooldown time.
        /// </summary>
        DamageToCooldown,
        /// <summary>
        /// The aspect pertaining to fire-typed damage.
        /// </summary>
        FireDamage,
        /// <summary>
        /// The aspect pertaining to ice-typed damage.
        /// </summary>
        IceDamage,
        /// <summary>
        /// The aspect pertaining to lightning-typed damage.
        /// </summary>
        LightningDamage,
        /// <summary>
        /// The aspect pertaining to the maximum health of a <see cref="World.Character"/>.
        /// </summary>
        MaxHealth,
        /// <summary>
        /// The aspect pertaining to the activation time of <see cref="World.Character"/> movement.
        /// </summary>
        MovementActivation,
        /// <summary>
        /// The aspect pertaining to an increasing reduction in both activation and cooldown 
        /// of the next cast spell, for each consecutive attack.
        /// </summary>
        NextSpellReductionForConsecutiveAttacks,
        /// <summary>
        /// The aspect pertaining to piercing-typed damage.
        /// </summary>
        PiercingDamage,
        /// <summary>
        /// The aspect pertaining physical-based damage.
        /// </summary>
        PhysicalDamage,
        /// <summary>
        /// The aspect pertaining to the activation time of all <see cref="Skills.Skill"/>s.
        /// </summary>
        SkillActivation
    }
}
