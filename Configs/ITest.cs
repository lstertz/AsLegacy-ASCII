namespace AsLegacy.Configs
{
    // TEMP implementation to validate configuration feature deserialization.

    public interface ITest
    {
        public int SomeProperty { get; init; }
    }

    public record TestA : ITest
    {
        public string SomeString { get; init; } = "Test";

        public int SomeProperty { get; init; }
    }

    public record TestB : ITest
    {
        public bool SomeBool { get; init; }

        public int SomeProperty { get; init; }
    }
}
