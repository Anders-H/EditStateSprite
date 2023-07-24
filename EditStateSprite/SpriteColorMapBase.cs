using System.Drawing;
using C64Color;

namespace EditStateSprite
{
    public abstract class SpriteColorMapBase
    {
        internal int[,] Colors;
        protected SpriteRoot Parent { get; }
        public abstract int Width { get; }
        public int Height => 21;
        public abstract int ColorCount { get; }
        
        protected SpriteColorMapBase(SpriteRoot parent)
        {
            Parent = parent;
            Colors = new int[Width, Height];
        }

        public int GetColorIndex(int x, int y) =>
            Colors[x, y];

        public void SetColorIndex(int x, int y, int color) =>
            Colors[x, y] = color;

        public ColorName GetColorNameFromPosition(int x, int y) =>
            Parent.SpriteColorPalette[Colors[x, y]];

        public Color GetColorFromPosition(int x, int y) =>
            SpriteRoot.C64Palette.GetColor(GetColorNameFromPosition(x, y));

        public void PaintPreview(Graphics g)
        {
            var pixelWidth = Parent.GetPreviewPixelWidth();
            var pixelHeight = Parent.GetPreviewPixelHeight();

            using (var b = new Bitmap(pixelWidth * Width, pixelHeight * Height))
            {
                var x = 0;
                var y = 0;

                if (pixelWidth == 1 && pixelHeight == 1)
                {
                    for (var indexY = 0; indexY < Height; indexY++)
                    {
                        for (var indexX = 0; indexX < Width; indexX++)
                        {
                            b.SetPixel(x, y, GetColorFromPosition(indexX, indexY));
                            x += pixelWidth;
                        }

                        x = 0;
                        y += pixelHeight;
                    }
                }
                else if (pixelWidth == 1)
                {
                    for (var indexY = 0; indexY < Height; indexY++)
                    {
                        for (var indexX = 0; indexX < Width; indexX++)
                        {
                            for (var pixelHeightIndex = 0; pixelHeightIndex < pixelHeight; pixelHeightIndex++)
                            {
                                b.SetPixel(x, y + pixelHeightIndex, GetColorFromPosition(indexX, indexY));
                            }

                            x += pixelWidth;
                        }

                        x = 0;
                        y += pixelHeight;
                    }
                }
                else if (pixelHeight == 1)
                {
                    for (var indexY = 0; indexY < Height; indexY++)
                    {
                        for (var indexX = 0; indexX < Width; indexX++)
                        {
                            for (var pixelWidthIndex = 0; pixelWidthIndex < pixelWidth; pixelWidthIndex++)
                            {
                                b.SetPixel(x + pixelWidthIndex, y, GetColorFromPosition(indexX, indexY));
                            }

                            x += pixelWidth;
                        }

                        x = 0;
                        y += pixelHeight;
                    }
                }
                else
                {
                    for (var indexY = 0; indexY < Height; indexY++)
                    {
                        for (var indexX = 0; indexX < Width; indexX++)
                        {
                            for (var pixelHeightIndex = 0; pixelHeightIndex < pixelHeight; pixelHeightIndex++)
                            {
                                for (var pixelWidthIndex = 0; pixelWidthIndex < pixelWidth; pixelWidthIndex++)
                                {
                                    b.SetPixel(x + pixelWidthIndex, y + pixelHeightIndex, GetColorFromPosition(indexX, indexY));
                                }
                            }

                            x += pixelWidth;
                        }

                        x = 0;
                        y += pixelHeight;
                    }
                }

                g.DrawImage(b, Parent.PreviewOffsetX, Parent.PreviewOffsetY);
            }
        }
    }
}