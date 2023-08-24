using System.Text;

namespace EditStateSprite
{
    public class MultiColorSpriteColorMap : SpriteColorMapBase
    {
        public override int Width => 12;
        public override int ColorCount => 4;

        public MultiColorSpriteColorMap(SpriteRoot parent) : base(parent)
        {
        }

        public override string SerializeSpriteData(int y)
        {
            var s = new StringBuilder();

            for (var x = 0; x < 12; x++)
                s.Append(Colors[x, y].ToString());

            return s.ToString();
        }

        public void InitializeFromMonochrome(SpriteColorMapBase colorMap)
        {
            for (var y = 0; y < 21; y++)
            {
                var sourceX = 0;

                for (var x = 0; x < 12; x++)
                {
                    var sourceColor1 = colorMap.GetColorIndex(sourceX, y);
                    var sourceColor2 = colorMap.GetColorIndex(sourceX + 1, y);

                    if (sourceColor1 == 0 && sourceColor2 > 0)
                        SetColorIndex(x, y, 1);
                    else if (sourceColor1 > 0 && sourceColor2 == 0)
                        SetColorIndex(x, y, 2);
                    else if (sourceColor1 > 0 && sourceColor2 > 0)
                        SetColorIndex(x, y, 3);
                    else
                        SetColorIndex(x, y, 0);

                    sourceX += 2;
                }
            }
        }
    }
}