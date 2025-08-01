#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace EditStateSprite;

public abstract class SpriteColorMapBase
{
    internal int[,] Colors;
    protected SpriteRoot Parent { get; }
    public abstract int Width { get; }
    public int Height => 21;
    public abstract int ColorCount { get; }
    public bool PaintBackground { get; set; }

    protected SpriteColorMapBase(SpriteRoot parent) : this(parent, false)
    {
    }

    protected SpriteColorMapBase(SpriteRoot parent, bool paintBackground)
    {
        Parent = parent;
        // ReSharper disable once VirtualMemberCallInConstructor
        Colors = new int[Width, Height];
        PaintBackground = paintBackground;
    }

    public void SetFrom(SpriteColorMapBase colorMap)
    {
        for (var h = 0; h < Height; h++)
        {
            for (var w = 0; w < Width; w++)
            {
                Colors[w, h] = colorMap.Colors[w, h];
            }
        }
    }

    public int GetColorIndex(int x, int y) =>
        Colors[x, y];

    public void SetColorIndex(int x, int y, int color) =>
        Colors[x, y] = color;

    public ColorName GetColorNameFromPosition(int x, int y) =>
        Parent.SpriteColorPalette[Colors[x, y]];

    public ColorName GetColorNameFromPosition(int x, int y, out bool isBackground)
    {
        var c = Colors[x, y];
        isBackground = c == 0;
        return Parent.SpriteColorPalette[Colors[x, y]];
    }

    public Color GetColorFromPosition(int x, int y) =>
        SpriteRoot.C64Palette.GetColor(GetColorNameFromPosition(x, y));

    public Color GetColorFromPosition(int x, int y, out bool isBackground) =>
        SpriteRoot.C64Palette.GetColor(GetColorNameFromPosition(x, y, out isBackground));

    public abstract string SerializeSpriteData(int y);

    public Bitmap GetBitmapNoAttributes()
    {
        var b = new Bitmap(24, 21);

        if (Width > 20)
        {
            for (var y = 0; y < 21; y++)
            {
                for (var x = 0; x < 24; x++)
                {
                    var c = GetColorFromPosition(x, y, out _);
                    b.SetPixel(x, y, c);
                }
            }
        }
        else
        {
            var xPos = 0;

            for (var y = 0; y < 21; y++)
            {
                for (var x = 0; x < 12; x++)
                {
                    var c = GetColorFromPosition(x, y, out _);
                    b.SetPixel(xPos, y, c);
                    b.SetPixel(xPos + 1, y, c);
                    xPos += 2;
                }

                xPos = 0;
            }
        }
        return b;
    }

    public Bitmap GetBitmap16x16NoAttributes()
    {
        var b = new Bitmap(16, 16);
        using var spriteBitmap = GetBitmapNoAttributes();
        using var g = Graphics.FromImage(b);
        g.SmoothingMode = SmoothingMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBilinear;
        g.DrawImage(spriteBitmap, 0, 0, 16, 16);
        return b;
    }

    public void PaintPreview(Graphics g, int locationX, int locationY)
    {
        var pixelWidth = Parent.GetPreviewPixelWidth();
        var pixelHeight = Parent.GetPreviewPixelHeight();

        using var b = new Bitmap(pixelWidth * Width, pixelHeight * Height);
        var x = 0;
        var y = 0;

        if (pixelWidth == 1 && pixelHeight == 1)
        {
            for (var indexY = 0; indexY < Height; indexY++)
            {
                for (var indexX = 0; indexX < Width; indexX++)
                {
                    var c = GetColorFromPosition(indexX, indexY, out var isBackground);
                            
                    if (!isBackground || PaintBackground)
                        b.SetPixel(x, y, c);
                            
                    x += pixelWidth;
                }

                x = 0;
                y += pixelHeight;
            }
        }
        else if (pixelWidth == 1)
        {
            for (var indexY = 0; indexY < Height; indexY++)
            {
                for (var indexX = 0; indexX < Width; indexX++)
                {
                    var c = GetColorFromPosition(indexX, indexY, out var isBackground);

                    if (!isBackground || PaintBackground)
                    {
                        for (var pixelHeightIndex = 0; pixelHeightIndex < pixelHeight; pixelHeightIndex++)
                        {
                            b.SetPixel(x, y + pixelHeightIndex, c);
                        }
                    }

                    x += pixelWidth;
                }

                x = 0;
                y += pixelHeight;
            }
        }
        else if (pixelHeight == 1)
        {
            for (var indexY = 0; indexY < Height; indexY++)
            {
                for (var indexX = 0; indexX < Width; indexX++)
                {
                    var c = GetColorFromPosition(indexX, indexY, out var isBackground);

                    if (!isBackground || PaintBackground)
                    {
                        for (var pixelWidthIndex = 0; pixelWidthIndex < pixelWidth; pixelWidthIndex++)
                        {
                            b.SetPixel(x + pixelWidthIndex, y, c);
                        }
                    }

                    x += pixelWidth;
                }

                x = 0;
                y += pixelHeight;
            }
        }
        else
        {
            for (var indexY = 0; indexY < Height; indexY++)
            {
                for (var indexX = 0; indexX < Width; indexX++)
                {
                    for (var pixelHeightIndex = 0; pixelHeightIndex < pixelHeight; pixelHeightIndex++)
                    {
                        var c = GetColorFromPosition(indexX, indexY, out var isBackground);

                        if (!isBackground || PaintBackground)
                        {
                            for (var pixelWidthIndex = 0; pixelWidthIndex < pixelWidth; pixelWidthIndex++)
                            {
                                b.SetPixel(x + pixelWidthIndex, y + pixelHeightIndex, c);
                            }
                        }
                    }

                    x += pixelWidth;
                }

                x = 0;
                y += pixelHeight;
            }
        }

        g.DrawImage(b, locationX, locationY);
    }

    public void PaintPreview(Graphics g) =>
        PaintPreview(g, Parent.PreviewOffsetX, Parent.PreviewOffsetY);

    public abstract byte[] GetBytes();

    public byte[] GetBytes64()
    {
        var bytes = GetBytes().ToList();
        bytes.Add(0);
        return bytes.ToArray();
    }

    public byte[] GetBytes64WithStartAddress(ushort startAddress)
    {
        var bytes = new List<byte>();
        var adr = BitConverter.GetBytes(startAddress).ToList();
        bytes.Add(adr[0]);
        bytes.Add(adr[1]);
        bytes.AddRange(GetBytes().ToList());
        bytes.Add(0);
        return bytes.ToArray();
    }
}