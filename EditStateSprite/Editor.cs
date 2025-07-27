#nullable enable
using System;
using System.Drawing;
using EditStateSprite.Col;
using EditStateSprite.SpriteModifiers;

namespace EditStateSprite;

public class Editor
{
    private Point _cursor;
    private bool _clearFlag;
    internal static Renderer Renderer { get; }
    internal static IResources Resources { get; }
    internal int PixelWidth;
    internal int PixelHeight;
    public Size EditorButtonSize { get; set; }
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

    public void MoveCursorTo(int editorX, int editorY)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        var pixelX = editorX / EditorButtonSize.Width;
        var pixelY = editorY / EditorButtonSize.Height;

        if (pixelX < 0 || pixelX > CurrentSprite.ColorMap.Width || pixelY < 0 || pixelY > CurrentSprite.ColorMap.Height)
            return;

        _cursor.X = pixelX;
        _cursor.Y = pixelY;
    }

    public void SetPixel(int editorX, int editorY, int colorIndex)
    {
        if (CurrentSprite == null)
            throw new InvalidOperationException("CurrentSprite is not set.");

        var pixelX = editorX / EditorButtonSize.Width;
        var pixelY = editorY / EditorButtonSize.Height;

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

        if (CurrentSprite.MultiColor)
        {
            EditorColorButtonMatrix = new ColorButton[CurrentSprite.ColorMap.Width, CurrentSprite.ColorMap.Height];
            EditorButtonSize = new Size(PixelWidth * 2, PixelHeight);
        }
        else
        {
            EditorColorButtonMatrix = new ColorButton[CurrentSprite.ColorMap.Width, CurrentSprite.ColorMap.Height];
            EditorButtonSize = new Size(PixelWidth, PixelHeight);
        }

        var x = 0;
        var y = 0;
        var pw = EditorButtonSize.Width - 1;
        var ph = EditorButtonSize.Height - 1;

        for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
        {
            for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
            {
                EditorColorButtonMatrix[w, h] = new ColorButton(Renderer, new Rectangle(x, y, pw, ph), CurrentSprite.ColorMap.GetColorNameFromPosition(w, h));
                x += EditorButtonSize.Width;
            }

            x = 0;
            y += EditorButtonSize.Height;
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
                EditorColorButtonMatrix[w, h].ResizeButton(w * stepWidth, h * PixelHeight,  pixelWidth, pixelHeight);
            }
        }
    }

    public void PaintEditor(Graphics g, EditorToolEnum tool, bool hasFocus)
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

        switch (tool)
        {
            case EditorToolEnum.PixelEditor:
                for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
                {
                    for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
                    {
                        if (hasFocus && w == _cursor.X && h == _cursor.Y)
                            Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.Selected);
                        else
                            Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.None);
                    }
                }

                break;
            case EditorToolEnum.FreeHand:
                for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
                {
                    for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
                    {
                        if (hasFocus && w == _cursor.X && h == _cursor.Y)
                            Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.Selected);
                        else
                            Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.None);
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tool), tool, null);
        }
    }
}