using System;
using C64Color;
using System.Drawing;
using EditStateSprite.SpriteModifiers;

namespace EditStateSprite
{
    public class Editor
    {
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

            EditorColorButtonMatrix[pixelX, pixelY].Color = CurrentSprite.SpriteColorPalette[colorIndex];
            CurrentSprite.SetPixel(pixelX, pixelY, colorIndex);
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

        public void PaintEditor(Graphics g)
        {
            for (var h = 0; h < CurrentSprite.ColorMap.Height; h++)
            {
                for (var w = 0; w < CurrentSprite.ColorMap.Width; w++)
                {
                    Renderer.Render(g, Resources, EditorColorButtonMatrix[w, h].Location, EditorColorButtonMatrix[w, h].Color, RendererFlags.None);
                }
            }
        }
    }
}