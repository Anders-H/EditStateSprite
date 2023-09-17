namespace EditStateSprite.SpriteModifiers
{
    public class SpriteScrollModifier : Modifier
    {
        public SpriteScrollModifier(int[,] colors) : base(colors)
        {
        }

        public void ScrollUp()
        {
            var rowBuffer = new int[Width];

            for (var w = 0; w < Width; w++)
                rowBuffer[w] = Colors[w, 0];

            for (var h = 1; h < Height; h++)
                for (var w = 0; w < Width; w++)
                    Colors[w, h - 1] = Colors[w, h];

            for (var w = 0; w < Width; w++)
                Colors[w, Height - 1] = rowBuffer[w];
        }

        public void ScrollRight()
        {
            var columnBuffer = new int[Height];

            for (var h = 0; h < Height; h++)
                columnBuffer[h] = Colors[Width - 1, h];

            for (var w = Width - 2; w >= 0; w--)
                for (var h = 0; h < Height; h++)
                    Colors[w + 1, h] = Colors[w, h];

            for (var h = 0; h < Height; h++)
                Colors[0, h] = columnBuffer[h];
        }

        public void ScrollDown()
        {
            var rowBuffer = new int[Width];

            for (var w = 0; w < Width; w++)
                rowBuffer[w] = Colors[w, Height - 1];

            for (var h = Height - 2; h >= 0; h--)
                for (var w = 0; w < Width; w++)
                    Colors[w, h + 1] = Colors[w, h];

            for (var w = 0; w < Width; w++)
                Colors[w, 0] = rowBuffer[w];
        }

        public void ScrollLeft()
        {
            var columnBuffer = new int[Height];

            for (var h = 0; h < Height; h++)
                columnBuffer[h] = Colors[0, h];

            for (var w = 1; w < Width; w++)
                for (var h = 0; h < Height; h++)
                    Colors[w - 1, h] = Colors[w, h];

            for (var h = 0; h < Height; h++)
                Colors[Width - 1, h] = columnBuffer[h];
        }
    }
}