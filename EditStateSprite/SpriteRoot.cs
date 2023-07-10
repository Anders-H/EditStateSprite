using C64Color;

namespace EditStateSprite
{
    public class SpriteRoot
    {
        public bool MultiColor { get; }
        public SpriteColorMapBase ColorMap { get; }
        public ColorName[] SpritePalette { get; }

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
        }
    }
}