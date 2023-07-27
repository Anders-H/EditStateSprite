namespace EditStateSprite.SpriteModifiers
{
    public abstract class Modifier
    {
        protected readonly int[,] _colors;
        protected readonly int _width;
        protected readonly int _height;

        protected Modifier(int[,] colors)
        {
            _colors = colors;
            _width = _colors.GetLength(0);
            _height = _colors.GetLength(1);
        }
    }
}