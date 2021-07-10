namespace AsLegacy.Characters
{
    /// <summary>
    /// Specifies an aspect of an <see cref="Affinity"/>, or how a <see cref="Concept"/> or 
    /// <see cref="Passive"/> may influence an <see cref="Affinity"/> 
    /// (through <see cref="Influence"/>) to produce the affect of the <see cref="Affinity"/>.
    /// </summary>
    public enum Aspect
    {
        Activation,
        AreaOfEffectDamage,
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
