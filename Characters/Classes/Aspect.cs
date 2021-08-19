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
        /// The aspect pertaining to the damage dealt by an area 
        /// of effect <see cref="Skills.Skill"/>.
        /// </summary>
        AreaOfEffectDamage,
        /// <summary>
        /// The aspect pertaining to the range of an area 
        /// of effect <see cref="Skills.Skill"/>.
        /// </summary>
        AreaOfEffectRange,
        Cooldown,
        CooldownNonElementalSpells,
        DamageToCooldown,
        FireDamage,
        IceDamage,
        LightningDamage,
        MaxHealth,
        NextSpellReductionForConsecutiveAttacks
    }
}
