using System.Drawing;

namespace EditStateSprite
{
    public abstract class SpriteColorMapBase
    {
        protected int[,] Colors;
        protected SpriteRoot Parent { get; }
        public abstract int Width { get; }
        public int Height => 21;
        public abstract int ColorCount { get; }
        
        protected SpriteColorMapBase(SpriteRoot parent)
        {
            Parent = parent;
            Colors = new int[Width, Height];
        }

        public int GetColor(int x, int y) =>
            Colors[x, y];

        public void SetColor(int x, int y, int color) =>
            Colors[x, y] = color;

        public abstract void PaintPreview(Graphics g);
    }
}