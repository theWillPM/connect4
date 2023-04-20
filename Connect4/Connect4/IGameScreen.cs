namespace connect4
{
    partial class Program
    {
        public interface IGameScreen
        {
            public abstract string Title { get; }
            public abstract List<string> Options { get; }
            public abstract int CursorPosition { get; }
        }
    }
}

