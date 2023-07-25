namespace EditStateSprite.SpriteModifiers
{
    public class SpriteScrollModifiers
    {
        private readonly int[,] _colors;
        private readonly int _width;
        private readonly int _height;

        public SpriteScrollModifiers(int[,] colors)
        {
            _colors = colors;
            _width = _colors.GetLength(0);
            _height = _colors.GetLength(1);
        }

        public void ScrollUp()
        {
            var rowBuffer = new int[_width];

            for (var w = 0; w < _width; w++)
            {
                rowBuffer[w] = _colors[w, 0];
            }

            for (var h = 1; h < _height; h++)
            {
                for (var w = 0; w < _width; w++)
                {
                    _colors[w, h - 1] = _colors[w, h];
                }
            }

            for (var w = 0; w < _width; w++)
            {
                _colors[w, _height - 1] = rowBuffer[w];
            }
        }

        public void ScrollRight()
        {

        }

        public void ScrollDown()
        {
            var rowBuffer = new int[_width];

            for (var w = 0; w < _width; w++)
            {
                rowBuffer[w] = _colors[w, _height - 1];
            }

            for (var h = _height - 2; h >= 0; h--)
            {
                for (var w = 0; w < _width; w++)
                {
                    _colors[w, h + 1] = _colors[w, h];
                }
            }

            for (var w = 0; w < _width; w++)
            {
                _colors[w, 0] = rowBuffer[w];
            }
        }

        public void ScrollLeft()
        {

        }
    }
}