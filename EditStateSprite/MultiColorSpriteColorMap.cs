using System.Collections.Generic;
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

        public MultiColorSpriteColorMap(SpriteRoot parent, MultiColorSpriteColorMap originalColorMap) : base(parent)
        {
            for (var y = 0; y < 21; y++)
                for (var x = 0; x < 12; x++)
                    parent.SetPixel(x, y, originalColorMap.GetColorIndex(x, y));
        }

        public override string SerializeSpriteData(int y)
        {
            var s = new StringBuilder();

            for (var x = 0; x < 12; x++)
                s.Append(Colors[x, y].ToString());

            return s.ToString();
        }

        public override byte[] GetBytes()
        {
            var result = new List<byte>();

            for (var y = 0; y < 21; y++)
            {
                result.Add(GetByte(0, y));
                result.Add(GetByte(4, y));
                result.Add(GetByte(8, y));
            }

            return result.ToArray();
        }

        private byte GetByte(int x, int y)
        {
            var result = 0;

            if (GetColorIndex(x, y) == 1)
                result += 64;
            else if (GetColorIndex(x, y) == 2)
                result += 128;
            else if (GetColorIndex(x, y) == 3)
                result += 192;

            if (GetColorIndex(x + 1, y) == 1)
                result += 16;
            else if (GetColorIndex(x + 1, y) == 2)
                result += 32;
            else if (GetColorIndex(x + 1, y) == 3)
                result += 48;

            if (GetColorIndex(x + 2, y) == 1)
                result += 4;
            else if (GetColorIndex(x + 2, y) == 2)
                result += 8;
            else if (GetColorIndex(x + 2, y) == 3)
                result += 12;

            if (GetColorIndex(x + 3, y) == 1)
                result += 1;
            else if (GetColorIndex(x + 3, y) == 2)
                result += 2;
            else if (GetColorIndex(x + 3, y) == 3)
                result += 3;

            return (byte)result;
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