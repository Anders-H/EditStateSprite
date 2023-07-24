namespace EditStateSprite.SpriteModifiers
{
    public class SpriteScrollModifiers
    {
        private readonly SpriteColorMapBase _colors;
        private readonly int _width;
        private readonly int _height;

        public SpriteScrollModifiers(SpriteColorMapBase colors)
        {
            _colors = colors;
            _width = _colors.Colors.GetLength(0);
            _height = _colors.Colors.GetLength(1);
        }

        public void ScrollUp()
        {
            var spare = new int[_width];

            for (var w = 0; w < _width; w++)
            {
                spare[w] = _colors.Colors[w, 0];
            }

            for (var h = 1; h < _height; h++)
            {
                for (var w = _width; w < _width; w++)
                {
                    _colors.Colors[w, h - 1] = _colors.Colors[w, h];
                }
            }

            for (var w = 0; w < _width; w++)
            {
                _colors.Colors[w, _height - 1] = spare[w];
            }
        }

        public void ScrollRight()
        {

        }

        public void ScrollDown()
        {

        }

        public void ScrollLeft()
        {

        }
    }
}