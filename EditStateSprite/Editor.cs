using System;
using C64Color;
using System.Drawing;
using EditStateSprite.SpriteModifiers;

namespace EditStateSprite
{
    public class Editor
    {
        private int _cursorX;
        private int _cursorY;
        internal static Renderer Renderer { get; }
        internal static IResources Resources { get; }
        public int EditorButtonWidth { get; set; }
        public int EditorButtonHeight { get; set; }
        public ColorButton[,] EditorColorButtonMatrix { get; private set; }
        public SpriteRoot CurrentSprite { get; private set; }

        static Editor()
        {
            Renderer = new Renderer();
            Resources = new Resources();
        }

        public Editor(SpriteRoot currentSprite)
        {
            ChangeCurrentSprite(currentSprite);
        }

        public void SetPixel(int editorX, int editorY, int colorIndex)
        {
            var pixelX = editorX / EditorButtonWidth;
            var pixelY = editorY / EditorButtonHeight;

            if (pixelX < 0 || pixelX > CurrentSprite.ColorMap.Width || pixelY < 0 || pixelY > CurrentSprite.ColorMap.Height)
                return;

            _cursorX = pixelX;
            _cursorY = pixelY;
            EditorColorButtonMatrix[pixelX, pixelY].Color = CurrentSprite.SpriteColorPalette[colorIndex];
            CurrentSprite.SetPixel(pixelX, pixelY, colorIndex);
        }

        public void SetPixelAtCursor(int colorIndex)
        {
            if (_cursorX < 0 || _cursorX > CurrentSprite.ColorMap.Width || _cursorY < 0 || _cursorY > CurrentSprite.ColorMap.Height)
                return;

            EditorColorButtonMatrix[_cursorX, _cursorY].Color = CurrentSprite.SpriteColorPalette[colorIndex];
            CurrentSprite.SetPixel(_cursorX, _cursorY, colorIndex);
        }

        public void MoveCursor(int x, int y)
        {
            _cursorX += x;

            if (_cursorX < 0)
                _cursorX = CurrentSprite.ColorMap.Width - 1;
            else if (_cursorX >= CurrentSprite.ColorMap.Width)
                _cursorX = 0;

            _cursorY += y;

            if (_cursorY < 0)
                _cursorY = CurrentSprite.ColorMap.Height - 1;
            else if (_cursorY >= CurrentSprite.ColorMap.Height)
                _cursorY = 0;
        }

        internal void Scroll(FourWayDirection direction)
        {
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

        public void ChangeCurrentSprite(SpriteRoot currentSprite)
        {
            _cursorX = 0;
            _cursorY = 0;

            CurrentSprite = currentSprite;

            if (CurrentSprite.MultiColor)
            {
                EditorColorButtonMatrix = new ColorButton[CurrentSprite.ColorMap.Width, CurrentSprite.ColorMap.Height];
                EditorButtonWidth = 30;
                EditorButtonHeight = 15;
            }
            else
            {
                EditorColorButtonMatrix = new ColorButton[CurrentSprite.ColorMap.Width, CurrentSprite.ColorMap.Height];
                EditorButtonWidth = 15;
                EditorButtonHeight = 15;
            }

            var x = 0;
            var y = 0;
            var pw = EditorButtonWidth - 1;
            var ph = EditorButtonHeight - 1;

            for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
            {
                for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
                {
                    EditorColorButtonMatrix[w, h] = new ColorButton(Renderer, new Rectangle(x, y, pw, ph), CurrentSprite.ColorMap.GetColorNameFromPosition(w, h));
                    x += EditorButtonWidth;
                }

                x = 0;
                y += EditorButtonHeight;
            }
        }

        public void PaintEditor(Graphics g, bool hasFocus)
        {
            for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
            {
                for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
                {
                    if (hasFocus && w == _cursorX && h == _cursorY)
                        Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.Selected);
                    else
                        Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.None);
                }
            }
        }
    }
}