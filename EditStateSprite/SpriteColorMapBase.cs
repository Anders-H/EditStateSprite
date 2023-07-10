using C64Color;

namespace EditStateSprite
{
    public abstract class SpriteColorMapBase
    {
        protected int[,] Colors;
        public abstract int Width { get; }
        public int Height => 21;
        public abstract int ColorCount { get; }

        protected SpriteColorMapBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Colors = new int[Width, Height];
        }

        public int GetColor(int x, int y) =>
            Colors[x, y];

        public void SetColor(int x, int y, int color) =>
            Colors[x, y] = color;
    }
}