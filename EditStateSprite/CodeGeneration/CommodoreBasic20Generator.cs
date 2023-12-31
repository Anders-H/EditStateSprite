﻿using System.Text;
using System;

namespace EditStateSprite.CodeGeneration
{
    public class CommodoreBasic20Generator
    {
        private readonly SpriteRoot _sprite;

        public CommodoreBasic20Generator(SpriteRoot sprite)
        {
            _sprite = sprite;
        }

        public string GetBasicCode(int lineNumber, int spriteDataStartAddress, int totalSpriteIndex, int hwSpriteIndex, int x, int y)
        {
            if (lineNumber < 0 || lineNumber > 63999 - 10)
                throw new ArgumentOutOfRangeException(nameof(lineNumber));

            if (hwSpriteIndex < 0 || hwSpriteIndex > 7)
                throw new ArgumentOutOfRangeException(nameof(hwSpriteIndex));

            var startAddress = spriteDataStartAddress / 64 + totalSpriteIndex;

            var turnOnFlagPosition = new[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var turnOffFlagPosition = new[] { 254, 253, 251, 247, 239, 223, 191, 127 };

            var s = new StringBuilder();

            s.AppendLine($"{lineNumber} poke{Commodore64SpriteRegisters.BackgroundColorRegister},{(int)_sprite.SpriteColorPalette[0]}:poke{Commodore64SpriteRegisters.ImageLocationPointers + hwSpriteIndex},{startAddress + totalSpriteIndex}:m={Commodore64SpriteRegisters.MulticolorFlags}:o={Commodore64SpriteRegisters.EnableFlags}");

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
                ? $"{lineNumber} poke{Commodore64SpriteRegisters.HorizontalExpansion},peek({Commodore64SpriteRegisters.HorizontalExpansion})or{turnOnFlagPosition[hwSpriteIndex]}:"
                : $"{lineNumber} poke{Commodore64SpriteRegisters.HorizontalExpansion},peek({Commodore64SpriteRegisters.HorizontalExpansion})and{turnOffFlagPosition[hwSpriteIndex]}:";
            
            var expandY = _sprite.ExpandY
                ? $"poke{Commodore64SpriteRegisters.VerticalExpansion},peek({Commodore64SpriteRegisters.VerticalExpansion})or{turnOnFlagPosition[hwSpriteIndex]}"
                : $"poke{Commodore64SpriteRegisters.VerticalExpansion},peek({Commodore64SpriteRegisters.VerticalExpansion})and{turnOffFlagPosition[hwSpriteIndex]}";
            
            s.AppendLine($"{expandX}{expandY}--------");

            lineNumber++;

            if (x < 0)
                x = 0;
            else if (x > 511)
                x = 511;

            if (y < 0)
                y = 0;
            else if (y > 255)
                y = 255;

            var xm = Commodore64SpriteRegisters.SpritePositionXPositionMostSignificantBit;

            s.AppendLine(x > 255
                ? $"{lineNumber} poke{Commodore64SpriteRegisters.SpritePositionRegisters + hwSpriteIndex * 2},{x - 256}:poke{xm},peek({xm})or{turnOnFlagPosition[hwSpriteIndex]}:poke{Commodore64SpriteRegisters.SpritePositionRegisters + 1 + hwSpriteIndex * 2},{y}"
                : $"{lineNumber} poke{Commodore64SpriteRegisters.SpritePositionRegisters + hwSpriteIndex * 2},{x}:poke{xm},peek({xm})and{turnOffFlagPosition[hwSpriteIndex]}:poke{Commodore64SpriteRegisters.SpritePositionRegisters + 1 + hwSpriteIndex * 2},{y}"
            );

            lineNumber++;
            var turnOn = $"pokeo,peek(o)or{turnOnFlagPosition[hwSpriteIndex]}";
            if (_sprite.MultiColor)
            {
                s.Append($"{lineNumber} pokem,peek(m)or{turnOnFlagPosition[hwSpriteIndex]}:");
                s.AppendLine($"poke{Commodore64SpriteRegisters.ForeColorRegisters + hwSpriteIndex},{(int)_sprite.SpriteColorPalette[1]}:poke{Commodore64SpriteRegisters.ExtraColorOne},{(int)_sprite.SpriteColorPalette[2]}:poke{Commodore64SpriteRegisters.ExtraColorTwo},{(int)_sprite.SpriteColorPalette[3]}:{turnOn}");
            }
            else
            {
                s.Append($"{lineNumber} pokem,peek(m)and{turnOffFlagPosition[hwSpriteIndex]}:");
                s.AppendLine($"poke{Commodore64SpriteRegisters.ForeColorRegisters + hwSpriteIndex},{(int)_sprite.SpriteColorPalette[1]}:{turnOn}");
            }

            return s.ToString();
        }
    }
}