using C64Color;

namespace EditStateSprite
{
    public class SpriteRoot
    {
        public static Palette C64Palette { get; }
        public bool MultiColor { get; }
        public SpriteColorMapBase ColorMap { get; }
        public ColorName[] SpriteColorPalette { get; }
        public int PreviewOffsetX { get; set; }
        public int PreviewOffsetY { get; set; }
        public bool ExpandX { get; set; }
        public bool ExpandY { get; set; }
        public bool PreviewZoom { get; set; }

        static SpriteRoot()
        {
            C64Palette = new Palette();
        }

        public SpriteRoot(bool multiColor)
        {
            MultiColor = multiColor;

            if (MultiColor)
                ColorMap = new MultiColorSpriteColorMap(this);
            else
                ColorMap = new MonochromeSpriteColorMap(this);

            SpriteColorPalette = new ColorName[ColorMap.ColorCount];

            for (var i = 0; i < SpriteColorPalette.Length; i++)
                SpriteColorPalette[i] = (ColorName)i;

            PreviewOffsetX = 0;
            PreviewOffsetY = 0;
            ExpandX = false;
            ExpandY = false;
        }

        public int GetPreviewPixelWidth()
        {
            var w = MultiColor ? 2 : 1;

            if (ExpandX)
                w *= 2;

            if (PreviewZoom)
                w *= 2;

            return w;
        }

        public int GetPreviewPixelHeight()
        {
            var h = ExpandY ? 2 : 1;

            if (PreviewZoom)
                h *= 2;

            return h;
        }
    }
}