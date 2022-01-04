using ContextualProgramming;

namespace AsLegacy.Configs
{
    // TEMP implementation to validate configuration feature deserialization.

    public interface ITest
    {
        public ContextState<int> SomeProperty { get; init; }
    }

    [Context]
    public class TestA : ITest
    {
        public ContextState<string> SomeString { get; init; } = "Test";

        public ContextState<int> SomeProperty { get; init; }
    }

    [Context]
    public class TestB : ITest
    {
        public ContextState<bool> SomeBool { get; init; }

        public ContextState<int> SomeProperty { get; init; }
    }
}
