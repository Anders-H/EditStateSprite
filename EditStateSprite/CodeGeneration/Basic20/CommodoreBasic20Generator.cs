using System;
using System.Text;

namespace EditStateSprite.CodeGeneration.Basic20
{
    public class CommodoreBasic20Generator
    {
        private readonly SpriteRoot _sprite;
        public const int LineNumbersNeeded = 11;

        public CommodoreBasic20Generator(SpriteRoot sprite)
        {
            _sprite = sprite;
        }

        public string GetBasicCode(int lineNumber, int spriteDataStartAddress, int includeInExportIndex, int x, int y)
        {
            var r = includeInExportIndex;
            var bc = Commodore64SpriteRegisters.BackgroundColorRegister;
            var il = Commodore64SpriteRegisters.ImageLocationPointers;
            var mc = Commodore64SpriteRegisters.MulticolorFlags;
            var ef = Commodore64SpriteRegisters.EnableFlags;
            var he = Commodore64SpriteRegisters.HorizontalExpansion;
            var ve = Commodore64SpriteRegisters.VerticalExpansion;
            var sp = Commodore64SpriteRegisters.SpritePositionRegisters;
            var fc = Commodore64SpriteRegisters.ForeColorRegisters;
            lineNumber += LineNumbersNeeded * r;

            if (lineNumber < 0 || lineNumber > 63999 - (LineNumbersNeeded - 1))
                throw new ArgumentOutOfRangeException(nameof(lineNumber));

            if (r < 0 || r > 7)
                throw new ArgumentOutOfRangeException(nameof(r));

            if (x < 0)
                x = 0;
            else if (x > 511)
                x = 511;

            if (y < 0)
                y = 0;
            else if (y > 255)
                y = 255;

            spriteDataStartAddress += (r * 64);
            var startAddress = spriteDataStartAddress / 64;
            var turnOnFlagPosition = new[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var turnOffFlagPosition = new[] { 254, 253, 251, 247, 239, 223, 191, 127 };

            var s = new StringBuilder();

            s.AppendLine($@"{lineNumber++} rem""sprite {r}");
            s.AppendLine(r == 0
                ? $"{lineNumber} poke{bc},{(int)_sprite.SpriteColorPalette[0]}:poke{il},{startAddress}:x={x}:y={y}:m={mc}:o={ef}"
                : $"{lineNumber} poke{il + r},{startAddress}:x={x}:y={y}:");

            lineNumber++;
            s.AppendLine($"{lineNumber} fora={spriteDataStartAddress}to{spriteDataStartAddress + 62}:readb:pokea,b:next");
            var bytes = _sprite.GetBytes();
            var chunks = new[] { 0, 11, 12, 23, 24, 35, 36, 48, 49, 62 };

            for (var n = 0; n < 10; n += 2)
            {
                lineNumber++;
                s.Append($"{lineNumber} data{bytes[chunks[n]]}");
                for (var i = chunks[n] + 1; i < chunks[n + 1]; i++)
                    s.Append($",{bytes[i]}");
                s.AppendLine($",{bytes[chunks[n + 1]]}");
            }

            lineNumber++;
            
            var expandX = _sprite.ExpandX
                ? $"{lineNumber} poke{he},peek({he})or{turnOnFlagPosition[r]}:"
                : $"{lineNumber} poke{he},peek({he})and{turnOffFlagPosition[r]}:";
            
            var expandY = _sprite.ExpandY
                ? $"poke{ve},peek({ve})or{turnOnFlagPosition[r]}"
                : $"poke{ve},peek({ve})and{turnOffFlagPosition[r]}";
            
            s.AppendLine($"{expandX}{expandY}");

            lineNumber++;

            var xm = Commodore64SpriteRegisters.SpritePositionXPositionMostSignificantBit;

            s.AppendLine(x > 255
                ? $"{lineNumber} poke{sp + r * 2},x-256:poke{xm},peek({xm})or{turnOnFlagPosition[r]}:poke{sp + 1 + r * 2},y"
                : $"{lineNumber} poke{sp + r * 2},x:poke{xm},peek({xm})and{turnOffFlagPosition[r]}:poke{sp + 1 + r * 2},y"
            );

            lineNumber++;
            var turnOn = $"pokeo,peek(o)or{turnOnFlagPosition[r]}";
            var p1 = (int)_sprite.SpriteColorPalette[1];

            if (_sprite.MultiColor)
            {
                var x1 = Commodore64SpriteRegisters.ExtraColorOne;
                var x2 = Commodore64SpriteRegisters.ExtraColorTwo;
                var p2 = (int)_sprite.SpriteColorPalette[2];
                var p3 = (int)_sprite.SpriteColorPalette[3];
                s.Append($"{lineNumber} pokem,peek(m)or{turnOnFlagPosition[r]}:");
                s.AppendLine($"poke{fc + r},{p1}:poke{x1},{p2}:poke{x2},{p3}:{turnOn}");
            }
            else
            {
                s.Append($"{lineNumber} pokem,peek(m)and{turnOffFlagPosition[r]}:");
                s.AppendLine($"poke{fc + r},{p1}:{turnOn}");
            }

            return s.ToString();
        }
    }
}