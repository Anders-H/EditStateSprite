using C64Color;
using System.Drawing;

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