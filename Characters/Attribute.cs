using System.Collections.ObjectModel;

namespace AsLegacy.Characters
{
    public record Attribute
    {
        public ReadOnlyCollection<Aspect> Aspects { get; init; } = 
            new(System.Array.Empty<Aspect>());

        public float BaseScale { get; init; } = 1.0f;

        public float BaseValue { get; init; } = 0.0f;

        public string Name { get; init; }
    }
}
