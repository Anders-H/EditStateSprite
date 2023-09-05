using System.Text;

namespace EditStateSprite
{
    public class MonochromeSpriteColorMap : SpriteColorMapBase
    {
        public override int Width => 24;
        public override int ColorCount => 2;

        public MonochromeSpriteColorMap(SpriteRoot parent) : base(parent)
        {
        }

        public MonochromeSpriteColorMap(SpriteRoot parent, MonochromeSpriteColorMap originalColorMap) : base(parent)
        {
            for (var y = 0; y < 21; y++)
                for (var x = 0; x < 24; x++)
                    parent.SetPixel(x, y, originalColorMap.GetColorIndex(x, y));
        }

        public override string SerializeSpriteData(int y)
        {
            var s = new StringBuilder();

            for (var x = 0; x < 24; x++)
                s.Append(Colors[x, y].ToString());

            return s.ToString();
        }

        public void InitializeFromMultiColor(SpriteColorMapBase colorMap)
        {
            for (var y = 0; y < 21; y++)
            {
                var targetX = 0;

                for (var x = 0; x < 12; x++)
                {
                    var targetColor = colorMap.GetColorIndex(x, y);

                    switch (targetColor)
                    {
                        case 1:
                            SetColorIndex(targetX, y, 0);
                            SetColorIndex(targetX + 1, y, 1);
                            break;
                        case 2:
                            SetColorIndex(targetX, y, 1);
                            SetColorIndex(targetX + 1, y, 0);
                            break;
                        case 3:
                            SetColorIndex(targetX, y, 1);
                            SetColorIndex(targetX + 1, y, 1);
                            break;
                        default:
                            SetColorIndex(targetX, y, 0);
                            SetColorIndex(targetX + 1, y, 0);
                            break;
                    }

                    targetX += 2;
                }
            }
        }
    }
}