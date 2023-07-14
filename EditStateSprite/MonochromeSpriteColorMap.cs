using System.Drawing;

namespace EditStateSprite
{
    public class MonochromeSpriteColorMap : SpriteColorMapBase
    {
        public override int Width => 24;
        public override int ColorCount => 2;

        public MonochromeSpriteColorMap(SpriteRoot parent) : base(parent)
        {
        }

        public override void PaintPreview(Bitmap b, Graphics g)
        {
            var pixelWidth = Parent.GetPreviewPixelWidth();
            var pixelHeight = Parent.GetPreviewPixelHeight();

            var x = Parent.PreviewOffsetX;
            var y = Parent.PreviewOffsetY;

            if (pixelWidth == 1 && pixelHeight == 1)
            {
                for (var indexY = 0; indexY < Height; indexY++)
                {
                    for (var indexX = 0; indexX < pixelWidth; indexX++)
                    {

                        x += pixelWidth;
                    }

                    x = Parent.PreviewOffsetX;
                    y += pixelHeight;
                }
            }
            else if (pixelWidth == 1)
            {

            }
            else if (pixelHeight == 1)
            {

            }
            else
            {
                
            }
        }
    }
}