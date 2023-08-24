using System;
using System.Text;
using EditStateSprite.Col;

namespace EditStateSprite
{
    public class SpriteRoot
    {
        private static Random Rnd { get; }
        public static Palette C64Palette { get; }
        public bool MultiColor { get; private set; }
        public SpriteColorMapBase ColorMap { get; private set; }
        public ColorName[] SpriteColorPalette { get; private set; }
        public int PreviewOffsetX { get; set; }
        public int PreviewOffsetY { get; set; }
        public bool ExpandX { get; set; }
        public bool ExpandY { get; set; }
        public bool PreviewZoom { get; set; }

        static SpriteRoot()
        {
            Rnd = new Random();
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

        public void SetPixel(int x, int y, int colorIndex)
        {
            if (x < 0 || x > ColorMap.Width || y < 0 || y > ColorMap.Height)
                throw new ArgumentOutOfRangeException($@"{x}*{y}");

            if (colorIndex < 0 || colorIndex > ColorMap.ColorCount)
                throw new ArgumentOutOfRangeException($@"colorIndex: {colorIndex}");

            ColorMap.SetColorIndex(x, y, colorIndex);
        }

        public void ConvertToMultiColor()
        {
            if (MultiColor)
                return;

            var newColorMap = new MultiColorSpriteColorMap(this);

            MultiColor = true;
            CreateMultiColorPalette();
            newColorMap.InitializeFromMonochrome(ColorMap);
            ColorMap = newColorMap;
        }

        public void ConvertToMonochrome()
        {
            if (!MultiColor)
                return;

            var newColorMap = new MonochromeSpriteColorMap(this);

            MultiColor = false;
            newColorMap.InitializeFromMultiColor(ColorMap);
            ColorMap = newColorMap;
            CreateMonochromePalette();
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

        private void CreateMonochromePalette()
        {
            var p = new ColorName[2];
            p[0] = SpriteColorPalette[0];
            p[1] = SpriteColorPalette[1];
            SpriteColorPalette = p;
        }

        private void CreateMultiColorPalette()
        {
            var p = new ColorName[4];
            p[0] = SpriteColorPalette[0];
            p[1] = SpriteColorPalette[1];
            p[2] = GetRandomColor(p[0], p[1]);
            p[3] = GetRandomColor(p[0], p[1], p[2]);
            SpriteColorPalette = p;
        }

        private ColorName GetRandomColor(params ColorName[] exclude)
        {
            bool found;
            ColorName ret;
            do
            {
                found = false;
                ret = RandomColor();
                foreach (var e in exclude)
                {
                    if (e == ret)
                    {
                        found = true;
                        break;
                    }
                }

            } while (found);

            return ret;
        }

        private ColorName RandomColor() =>
            (ColorName)Rnd.Next(0, 16);

        public void Serialize(StringBuilder s)
        {
            s.AppendLine($"MULTICOLOR={(MultiColor ? "YES" : "NO")}");
            s.AppendLine($"PREVIEW OFFSET={PreviewOffsetX},{PreviewOffsetY}");

            var expandString = ExpandY ? "Y" : "NO";

            switch (ExpandX)
            {
                case true when ExpandY:
                    expandString = "XY";
                    break;
                case true:
                    expandString = "X";
                    break;
            }

            s.AppendLine($"EXPAND={expandString}");
            s.AppendLine($"PREVIEW ZOOM={(PreviewZoom ? "YES" : "NO")}");
            s.AppendLine($"COLOR PALETTE={SerializeColorPalette()}");
            
            for (var y = 0; y < 21; y++)
                s.AppendLine($"SPRITE ROW DATA ({y + 1:00}/21)={ColorMap.SerializeSpriteData(y)}");
        }

        private string SerializeColorPalette() =>
            MultiColor
                ? $"{SpriteColorPalette[0]}-{SpriteColorPalette[1]}-{SpriteColorPalette[2]}-{SpriteColorPalette[3]}"
                : $"{SpriteColorPalette[0]}-{SpriteColorPalette[1]}";
    }
}