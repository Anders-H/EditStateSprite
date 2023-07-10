using C64Color;

namespace EditStateSprite
{
    public class SpriteRoot
    {
        public bool MultiColor { get; }
        public SpriteColorMapBase ColorMap { get; }
        public ColorName[] SpritePalette { get; }
        public int PreviewOffsetX { get; set; }
        public int PreviewOffsetY { get; set; }
        public bool ExpandX { get; set; }
        public bool ExpandY { get; set; }

        public SpriteRoot(bool multiColor)
        {
            MultiColor = multiColor;

            if (MultiColor)
                ColorMap = new MultiColorSpriteColorMap();
            else
                ColorMap = new MonochromeSpriteColorMap();

            SpritePalette = new ColorName[ColorMap.ColorCount];

            for (var i = 0; i < SpritePalette.Length; i++)
                SpritePalette[i] = (ColorName)i;

            PreviewOffsetX = 0;
            PreviewOffsetY = 0;
            ExpandX = false;
            ExpandY = false;
        }
    }
}