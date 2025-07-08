#nullable enable
using System;
using System.Linq;
using System.Text;
using EditStateSprite.CodeGeneration.Basic20;
using EditStateSprite.Col;
using EditStateSprite.Serialization;
using EditStateSprite.SpriteModifiers;

namespace EditStateSprite;

public class SpriteRoot
{
    private static Random Rnd { get; }
    public string Name { get; set; }
    public static Palette C64Palette { get; }
    public bool MultiColor { get; private set; }
    public SpriteColorMapBase ColorMap { get; private set; }
    public ColorName[] SpriteColorPalette { get; internal set; }
    public int PreviewOffsetX { get; set; }
    public int PreviewOffsetY { get; set; }
    public bool ExpandX { get; set; }
    public bool ExpandY { get; set; }
    public bool PreviewZoom { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public PreviewAnimationBehaviour PreviewAnimationBehaviour { get; set; }

    static SpriteRoot()
    {
        Rnd = new Random();
        C64Palette = new Palette();
    }

    public SpriteRoot(bool multiColor)
    {
        Name = "";
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

    public SpriteRoot(SpriteRoot sprite)
    {
        Name = $"{sprite.Name} (copy)";
        MultiColor = sprite.MultiColor;

        if (MultiColor)
            ColorMap = new MultiColorSpriteColorMap(this, (MultiColorSpriteColorMap)sprite.ColorMap);
        else
            ColorMap = new MonochromeSpriteColorMap(this, (MonochromeSpriteColorMap)sprite.ColorMap);

        SpriteColorPalette = sprite.GetSpriteColorPalette();

        PreviewOffsetX = sprite.PreviewOffsetX + 10;
        PreviewOffsetY = sprite.PreviewOffsetY + 10;
        ExpandX = sprite.ExpandX;
        ExpandY = sprite.ExpandY;
    }

    public static SpriteRoot Parse(SpriteChunkParser chunk) =>
        new SpriteRootParser(chunk).Parse();
    
    private ColorName[] GetSpriteColorPalette()
    {
        var result = MultiColor ? new ColorName[4] : new ColorName[2];

        for (var i = 0; i < SpriteColorPalette.Length; i++)
            result[i] = SpriteColorPalette[i];

        return result;
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
            ret = RandomColor();
            found = exclude.Any(e => e == ret);
        } while (found);

        return ret;
    }

    private ColorName RandomColor() =>
        (ColorName)Rnd.Next(0, 16);

    public void Serialize(StringBuilder s)
    {
        s.AppendLine($"NAME={Name}");
        s.AppendLine($"MULTICOLOR={(MultiColor ? "YES" : "NO")}");
        s.AppendLine($"PREVIEW OFFSET={PreviewOffsetX},{PreviewOffsetY}");
        s.AppendLine($"PREVIEW ANIMATION={PreviewAnimationBehaviourHelper.Serialize(PreviewAnimationBehaviour)}");
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
        s.AppendLine($"X={X}");
        s.AppendLine($"Y={Y}");
        s.AppendLine($"PREVIEW ZOOM={(PreviewZoom ? "YES" : "NO")}");
        s.AppendLine($"COLOR PALETTE={SerializeColorPalette()}");
            
        for (var y = 0; y < 21; y++)
            s.AppendLine($"SPRITE ROW DATA ({y + 1:00}/21)={ColorMap.SerializeSpriteData(y)}");
    }

    private string SerializeColorPalette() =>
        MultiColor
            ? $"{SpriteColorPalette[0]}-{SpriteColorPalette[1]}-{SpriteColorPalette[2]}-{SpriteColorPalette[3]}"
            : $"{SpriteColorPalette[0]}-{SpriteColorPalette[1]}";

    public byte[] GetBytes() =>
        ColorMap.GetBytes();

    public byte[] GetBytes64() =>
        ColorMap.GetBytes64();

    public byte[] GetBytes64WithStartAddress(ushort startAddress) =>
        ColorMap.GetBytes64WithStartAddress(startAddress);

    /// <summary>
    /// Generates Commodore 64 BASIC code for displaying a sprite.
    /// </summary>
    /// <param name="lineNumber">BASIC line number (0 - 63999)</param>
    /// <param name="spriteDataStartAddress"></param>
    /// <param name="includeInExportIndex">The index of the sprite in the list of sprites being exported</param>
    /// <param name="x">C64 horizontal screen location</param>
    /// <param name="y">C64 vertical screen location</param>
    /// <returns>Commodore BASIC 2.0 second release source code.</returns>
    public string GetBasicCode(int lineNumber, int spriteDataStartAddress, int includeInExportIndex, int x, int y) =>
        new CommodoreBasic20Generator(this).GetBasicCode(lineNumber, spriteDataStartAddress, includeInExportIndex, x, y);
}