using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace EditStateSprite.Serialization
{
    public class SpriteRootParser
    {
        private readonly SpriteChunkParser _chunk;

        public SpriteRootParser(SpriteChunkParser chunk)
        {
            _chunk = chunk;
        }

        public SpriteRoot Parse()
        {
            var result = new SpriteRoot(_chunk.GetMulticolor())
            {
                Name = _chunk.GetName()
            };

            foreach (var line in _chunk)
            {
                if (line.StartsWith("NAME="))
                {
                    result.Name = line.Substring(5).Trim();
                }
                else if (line.StartsWith("PREVIEW OFFSET="))
                {
                    var offset = line.Split('=')[1];
                    var offsetParts = offset.Split(',');
                    result.PreviewOffsetX = int.Parse(offsetParts[0]);
                    result.PreviewOffsetY = int.Parse(offsetParts[1]);
                }
                else if (line.StartsWith("EXPAND="))
                {
                    var expand = line.Split('=')[1];
                    switch (expand)
                    {
                        case "XY":
                            result.ExpandX = true;
                            result.ExpandY = true;
                            break;
                        case "X":
                            result.ExpandX = true;
                            result.ExpandY = false;
                            break;
                        case "Y":
                            result.ExpandX = false;
                            result.ExpandY = true;
                            break;
                        default:
                            result.ExpandX = false;
                            result.ExpandY = false;
                            break;
                    }
                }
                else if (line == "PREVIEW ZOOM=YES")
                {
                    result.PreviewZoom = true;
                }
                else if (line == "PREVIEW ZOOM=NO")
                {
                    result.PreviewZoom = false;
                }
                else if (line.StartsWith("PREVIEW ZOOM="))
                {
                    throw new SerializationException("Unknown value for PREVIEW ZOOM.");
                }
                else if (line.StartsWith("COLOR PALETTE="))
                {
                    ParseSpriteColorPalette(line.Split('=')[1], ref result);
                }
                else if (line.StartsWith("SPRITE ROW DATA ("))
                {
                    var colorRowMatch = Regex.Match(line, @"^SPRITE ROW DATA \(([0-9]+)\/21\)=[0-3]+$");

                    if (!colorRowMatch.Success)
                        throw new SerializationException("Invalid SPRITE ROW DATA");

                    var rowIndex = int.Parse(colorRowMatch.Groups[1].Value) - 1;

                    var pixelString = line.Split('=')[1];

                    for (var i = 0; i < result.ColorMap.Width; i++)
                        result.SetPixel(i, rowIndex, int.Parse(pixelString.Substring(i, 1)));
                }
            }

            return result;
        }

        private static void ParseSpriteColorPalette(string colorsSource, ref SpriteRoot sprite)
        {
            var colorsRaw = colorsSource.Split('-');

            if (sprite.MultiColor)
            {
                if (colorsRaw.Length != 4)
                    throw new SerializationException("Multicolor sprites should have four colors.");

                sprite.SpriteColorPalette = new ColorName[4];
            }
            else
            {
                if (colorsRaw.Length != 2)
                    throw new SerializationException("Monochrome sprites should have two colors.");

                sprite.SpriteColorPalette = new ColorName[2];
            }

            for (var i = 0; i < colorsRaw.Length; i++)
            {
                if (Enum.TryParse<ColorName>(colorsRaw[i], true, out var colorName))
                    sprite.SpriteColorPalette[i] = colorName;
                else
                    throw new SerializationException($"Unknown color name: {colorsRaw[i]}");
            }
        }
    }
}