namespace EditStateSprite.SpriteModifiers
{
    public class SpriteScrollModifier : Modifier
    {
        public SpriteScrollModifier(int[,] colors) : base(colors)
        {
        }

        public void ScrollUp()
        {
            var rowBuffer = new int[_width];

            for (var w = 0; w < _width; w++)
                rowBuffer[w] = _colors[w, 0];

            for (var h = 1; h < _height; h++)
                for (var w = 0; w < _width; w++)
                    _colors[w, h - 1] = _colors[w, h];

            for (var w = 0; w < _width; w++)
                _colors[w, _height - 1] = rowBuffer[w];
        }

        public void ScrollRight()
        {
            var columnBuffer = new int[_height];

            for (var h = 0; h < _height; h++)
                columnBuffer[h] = _colors[_width - 1, h];

            for (var w = _width - 2; w >= 0; w--)
                for (var h = 0; h < _height; h++)
                    _colors[w + 1, h] = _colors[w, h];

            for (var h = 0; h < _height; h++)
                _colors[0, h] = columnBuffer[h];
        }

        public void ScrollDown()
        {
            var rowBuffer = new int[_width];

            for (var w = 0; w < _width; w++)
                rowBuffer[w] = _colors[w, _height - 1];

            for (var h = _height - 2; h >= 0; h--)
                for (var w = 0; w < _width; w++)
                    _colors[w, h + 1] = _colors[w, h];

            for (var w = 0; w < _width; w++)
                _colors[w, 0] = rowBuffer[w];
        }

        public void ScrollLeft()
        {
            var columnBuffer = new int[_height];

            for (var h = 0; h < _height; h++)
                columnBuffer[h] = _colors[0, h];

            for (var w = 1; w < _width; w++)
                for (var h = 0; h < _height; h++)
                    _colors[w - 1, h] = _colors[w, h];

            for (var h = 0; h < _height; h++)
                _colors[_width - 1, h] = columnBuffer[h];
        }
    }
}