namespace EditStateSprite.SpriteModifiers
{
    public class SpriteFlipModifier : Modifier
    {
        public SpriteFlipModifier(int[,] colors) : base(colors)
        {
        }

        public void FlipLeftRight()
        {
            var buffer = new int[_width, _height];

            for (var w = 0; w < _width; w++)
                for (var h = 0; h < _height; h++)
                    buffer[w, h] = _colors[_width - 1 - w, h];

            for (var w = 0; w < _width; w++)
                for (var h = 0; h < _height; h++)
                    _colors[w, h] = buffer[w, h];
        }

        public void FlipTopDown()
        {
            var buffer = new int[_width, _height];

            for (var w = 0; w < _width; w++)
                for (var h = 0; h < _height; h++)
                    buffer[w, h] = _colors[w, _height - 1 - h];

            for (var w = 0; w < _width; w++)
                for (var h = 0; h < _height; h++)
                    _colors[w, h] = buffer[w, h];
        }
    }
}