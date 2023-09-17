namespace EditStateSprite.SpriteModifiers
{
    public class SpriteFlipModifier : Modifier
    {
        public SpriteFlipModifier(int[,] colors) : base(colors)
        {
        }

        public void FlipLeftRight()
        {
            var buffer = new int[Width, Height];

            for (var w = 0; w < Width; w++)
                for (var h = 0; h < Height; h++)
                    buffer[w, h] = Colors[Width - 1 - w, h];

            for (var w = 0; w < Width; w++)
                for (var h = 0; h < Height; h++)
                    Colors[w, h] = buffer[w, h];
        }

        public void FlipTopDown()
        {
            var buffer = new int[Width, Height];

            for (var w = 0; w < Width; w++)
                for (var h = 0; h < Height; h++)
                    buffer[w, h] = Colors[w, Height - 1 - h];

            for (var w = 0; w < Width; w++)
                for (var h = 0; h < Height; h++)
                    Colors[w, h] = buffer[w, h];
        }
    }
}