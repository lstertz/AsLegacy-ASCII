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
    public class DependentBehaviorSelf
    {
        [Dependency(DependencySource.Self)]
        public TestA A { get; private init; } = new()
        {
            SomeProperty = 1
        };

        private DependentBehaviorSelf() { }
    }

    [Behavior]
    public class DependentBehaviorSelfInterface
    {
        [Dependency(DependencySource.Self)]
        public ITest I { get; private init; }

        private DependentBehaviorSelfInterface() => I = new TestB() { SomeProperty = 2 };
    }


    [Behavior]
    public class DependentBehaviorShared
    {
        [Dependency(DependencySource.Shared)]
        public TestA A { get; private init; }

        private DependentBehaviorShared() { }
    }

    [Behavior]
    public class DependentBehaviorUnique
    {
        [Dependency(DependencySource.Unique)]
        public TestA A { get; private init; }

        private DependentBehaviorUnique() { }
    }
}
