namespace AsLegacy
{
    public static partial class World
    {
        private abstract class Effect
        {
            public Effect Followup { get; init; } = null;

            private IAction _action = null;

            public void Start()
            {
                _action = new Action(0, Update, null, true);
            }

            public void Stop()
            {
                _action.Cancel();
                _action = null;
            }

            protected abstract void Update();
        }
    }
}
