#nullable enable
using EditStateSprite.Col;
using EditStateSprite.SpriteModifiers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EditStateSprite;

public class Editor
{
    private Point _cursor;
    private bool _clearFlag;
    internal static Renderer Renderer { get; }
    internal static IResources Resources { get; }
    internal int PixelWidth;
    internal int PixelHeight;
    public ColorButton[,]? EditorColorButtonMatrix { get; private set; }
    public SpriteRoot? CurrentSprite { get; private set; }
    public Color BackgroundColor { get; set; }

    static Editor()
    {
        Renderer = new Renderer();
        Resources = new Resources();
    }

    public Editor(SpriteRoot currentSprite, int pixelWidth, int pixelHeight)
    {
        PixelWidth = pixelWidth;
        PixelHeight = pixelHeight;
        ResetCursorPosition();
        ChangeCurrentSprite(currentSprite);
    }

    public Point GetCursorPosition() =>
        new(_cursor.X, _cursor.Y);

    public void SetCursorPosition(Point position)
    {
        if (EditorColorButtonMatrix == null)
            throw new InvalidOperationException("EditorColorButtonMatrix is not initialized.");

        var y = position.Y;

        if (y >= EditorColorButtonMatrix.GetLength(0))
            y = EditorColorButtonMatrix.GetLength(0) - 1;

        _cursor.X = position.X;
        _cursor.Y = y;
    }

    public void ResetCursorPosition()
    {
        _cursor.X = 0;
        _cursor.Y = 0;
    }

    public void MoveCursorTo(int editorX, int editorY, bool multiColor)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        var pw = multiColor ? PixelWidth * 2 : PixelWidth;
        var pixelX = editorX / pw;
        var pixelY = editorY / PixelHeight;

        if (pixelX < 0 || pixelX > CurrentSprite.ColorMap.Width || pixelY < 0 || pixelY > CurrentSprite.ColorMap.Height)
            return;

        _cursor.X = pixelX;
        _cursor.Y = pixelY;
    }

    public void SetPixel(int editorX, int editorY, bool multiColor, int colorIndex)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        var pw = multiColor ? PixelWidth * 2 : PixelWidth;
        var pixelX = editorX / pw;
        var pixelY = editorY / PixelHeight;

        if (pixelX < 0 || pixelX >= CurrentSprite.ColorMap.Width || pixelY < 0 || pixelY >= CurrentSprite.ColorMap.Height)
            return;

        if (EditorColorButtonMatrix == null)
            throw new InvalidOperationException("EditorColorButtonMatrix is not initialized.");

        _cursor.X = pixelX;
        _cursor.Y = pixelY;
        EditorColorButtonMatrix[pixelX, pixelY].Color = CurrentSprite.SpriteColorPalette[colorIndex];
        CurrentSprite.SetPixel(pixelX, pixelY, colorIndex);
    }

    public void SetPixelAtCursor(int colorIndex)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        if (_cursor.X < 0 || _cursor.X > CurrentSprite.ColorMap.Width || _cursor.Y < 0 || _cursor.Y > CurrentSprite.ColorMap.Height)
            return;

        if (EditorColorButtonMatrix == null)
            throw new InvalidOperationException("EditorColorButtonMatrix is not initialized.");

        EditorColorButtonMatrix[_cursor.X, _cursor.Y].Color = CurrentSprite.SpriteColorPalette[colorIndex];
        CurrentSprite.SetPixel(_cursor.X, _cursor.Y, colorIndex);
    }

    public void MoveCursor(int x, int y)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        _cursor.X += x;

        if (_cursor.X < 0)
            _cursor.X = CurrentSprite.ColorMap.Width - 1;
        else if (_cursor.X >= CurrentSprite.ColorMap.Width)
            _cursor.X = 0;

        _cursor.Y += y;

        if (_cursor.Y < 0)
            _cursor.Y = CurrentSprite.ColorMap.Height - 1;
        else if (_cursor.Y >= CurrentSprite.ColorMap.Height)
            _cursor.Y = 0;
    }

    public void Clear()
    {
        if (EditorColorButtonMatrix == null)
            throw new InvalidOperationException("EditorColorButtonMatrix is not initialized.");

        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
        {
            for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
            {
                CurrentSprite.ColorMap.SetColorIndex(w, h, 0);
                EditorColorButtonMatrix[w, h].Color = CurrentSprite.SpriteColorPalette[0];
            }
        }
    }

    public void DrawLine(Point from, Point to, int colorIndex)
    {
        if (CurrentSprite == null || EditorColorButtonMatrix == null)
            return;

        foreach (var point in GetLinePoints(from, to))
        {
            if (point.X < 0 || point.X >= CurrentSprite.ColorMap.Width) continue;
            if (point.Y < 0 || point.Y >= CurrentSprite.ColorMap.Height) continue;
            EditorColorButtonMatrix[point.X, point.Y].Color = CurrentSprite.SpriteColorPalette[colorIndex];
            CurrentSprite.SetPixel(point.X, point.Y, colorIndex);
        }
    }

    public static IEnumerable<Point> GetLinePoints(Point from, Point to)
    {
        // Bresenham
        int x0 = from.X, y0 = from.Y, x1 = to.X, y1 = to.Y;
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx + dy;
        while (true)
        {
            yield return new Point(x0, y0);
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }

    internal void Scroll(FourWayDirection direction)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        switch (direction)
        {
            case FourWayDirection.Up:
                new SpriteScrollModifier(CurrentSprite.ColorMap.Colors).ScrollUp();
                break;
            case FourWayDirection.Right:
                new SpriteScrollModifier(CurrentSprite.ColorMap.Colors).ScrollRight();
                break;
            case FourWayDirection.Down:
                new SpriteScrollModifier(CurrentSprite.ColorMap.Colors).ScrollDown();
                break;
            case FourWayDirection.Left:
                new SpriteScrollModifier(CurrentSprite.ColorMap.Colors).ScrollLeft();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        ChangeCurrentSprite(CurrentSprite);
    }

    internal void Flip(TwoWayDirection direction)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        switch (direction)
        {
            case TwoWayDirection.LeftRight:
                new SpriteFlipModifier(CurrentSprite.ColorMap.Colors).FlipLeftRight();
                break;
            case TwoWayDirection.TopDown:
                new SpriteFlipModifier(CurrentSprite.ColorMap.Colors).FlipTopDown();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        ChangeCurrentSprite(CurrentSprite);
    }

    internal void UpdateEditorButtons()
    {
        if (EditorColorButtonMatrix == null)
            throw new InvalidOperationException("EditorColorButtonMatrix is not initialized.");

        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
            for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
                EditorColorButtonMatrix[w, h].Color = CurrentSprite.SpriteColorPalette[CurrentSprite.ColorMap.Colors[w, h]];
    }

    public void ChangeCurrentSprite(SpriteRoot currentSprite)
    {
        _cursor.X = 0;
        _cursor.Y = 0;
        CurrentSprite = currentSprite;
        EditorColorButtonMatrix = new ColorButton[CurrentSprite.ColorMap.Width, CurrentSprite.ColorMap.Height];
        var x = 0;
        var y = 0;
        var pw = PixelWidth - 1;
        var ph = PixelHeight - 1;
        var stepWidth = CurrentSprite.MultiColor ? PixelWidth * 2 : PixelWidth;

        if (CurrentSprite.MultiColor)
            pw = (PixelWidth * 2) - 1;

        for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
        {
            for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
            {
                EditorColorButtonMatrix[w, h] = new ColorButton(Renderer, new Rectangle(x, y, pw, ph), CurrentSprite.ColorMap.GetColorNameFromPosition(w, h));
                x += stepWidth;
            }

            x = 0;
            y += PixelHeight;
        }
    }

    internal void RecreateButtons()
    {
        if (CurrentSprite == null)
            return;

        if (EditorColorButtonMatrix == null)
            return;

        _clearFlag = true;
        var pixelWidth = PixelWidth - 1;

        if (CurrentSprite.MultiColor)
            pixelWidth = (PixelWidth * 2) - 1;

        var pixelHeight = PixelHeight - 1;
        var stepWidth = CurrentSprite.MultiColor ? PixelWidth * 2 : PixelWidth;

        for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
        {
            for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
            {
                EditorColorButtonMatrix[w, h].ResizeButton(w * stepWidth, h * PixelHeight, pixelWidth, pixelHeight);
            }
        }
    }

    public void PaintEditor(Graphics g, EditorToolEnum tool, bool hasFocus, IEnumerable<Point>? previewPixels, int penColorIndex)
    {
        if (_clearFlag)
        {
            _clearFlag = false;
            g.Clear(BackgroundColor);
        }

        if (EditorColorButtonMatrix == null)
            throw new InvalidOperationException("EditorColorButtonMatrix is not initialized.");

        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        // Bygg en HashSet för O(1)-uppslagning
        var previewSet = previewPixels != null
            ? new HashSet<Point>(previewPixels)
            : null;

        var previewPenColor = Resources.GetColorPen(CurrentSprite.SpriteColorPalette[penColorIndex]);

        for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
        {
            for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
            {
                RendererFlags flags;

                if (previewSet != null && previewSet.Contains(new Point(w, h)))
                    flags = RendererFlags.Preview;
                else if (hasFocus && w == _cursor.X && h == _cursor.Y)
                    flags = RendererFlags.Selected;
                else
                    flags = RendererFlags.None;

                Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, flags, previewPenColor);
            }
        }
    }
}