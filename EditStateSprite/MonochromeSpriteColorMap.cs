#nullable enable
using System.Collections.Generic;
using System.Text;

namespace EditStateSprite;

public class MonochromeSpriteColorMap : SpriteColorMapBase
{
    public override int Width => 24;
    public override int ColorCount => 2;

    public MonochromeSpriteColorMap(SpriteRoot parent) : base(parent)
    {
    }

    public override string SerializeSpriteData(int y)
    {
        var s = new StringBuilder();

        for (var x = 0; x < 24; x++)
            s.Append(Colors[x, y].ToString());

        return s.ToString();
    }

    public override byte[] GetBytes()
    {
        var result = new List<byte>();

        for (var y = 0; y < 21; y++)
        {
            result.Add(GetByte(0, y));
            result.Add(GetByte(8, y));
            result.Add(GetByte(16, y));
        }

        return result.ToArray();
    }

    private byte GetByte(int x, int y)
    {
        var result = 0;

        if (GetColorIndex(x, y) == 1)
            result += 128;

        if (GetColorIndex(x + 1, y) == 1)
            result += 64;

        if (GetColorIndex(x + 2, y) == 1)
            result += 32;

        if (GetColorIndex(x + 3, y) == 1)
            result += 16;

        if (GetColorIndex(x + 4, y) == 1)
            result += 8;

        if (GetColorIndex(x + 5, y) == 1)
            result += 4;

        if (GetColorIndex(x + 6, y) == 1)
            result += 2;

        if (GetColorIndex(x + 7, y) == 1)
            result += 1;

        return (byte)result;
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