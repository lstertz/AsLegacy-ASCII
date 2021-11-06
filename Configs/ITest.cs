using ContextualProgramming;

namespace AsLegacy.Configs
{
    // TEMP implementation to validate configuration feature deserialization.

    public interface ITest
    {
        public int SomeProperty { get; init; }
    }

    public record TestA : Context, ITest
    {
        public string SomeString { get; init; } = "Test";

        public int SomeProperty { get; init; }
    }

    public record TestB : Context, ITest
    {
        public bool SomeBool { get; init; }

        public int SomeProperty { get; init; }
    }


    [Behavior]
    public class IndependentBehavior
    {

    }

    [Behavior]
    public class DependentBehaviorA
    {
        [Dependency(DependencySource.Shared)]
        public TestA A { get; private init; }
    }

    [Behavior]
    public class DependentBehaviorI
    {

        [Dependency(DependencySource.Shared)]
        public ITest I { get; private init; }
    }
}
