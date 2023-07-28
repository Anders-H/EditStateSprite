using System;
using System.Windows.Forms;
using C64Color;

namespace EditStateSprite
{
    public class SpriteEditorControl : Control
    {
        private int _currentColorIndex = 0;
        private SpriteRoot _sprite;
        private Editor Editor { get; }
        public event SpriteChangedDelegate SpriteChanged;

        public SpriteEditorControl()
        {
            _sprite = new SpriteRoot(false);
            Editor = new Editor(_sprite);
        }

        public void ConnectSprite(SpriteRoot sprite)
        {
            _sprite = sprite;
            Editor.ChangeCurrentSprite(_sprite);
            Invalidate();
        }

        public void Scroll(FourWayDirection direction)
        {
            Editor.Scroll(direction);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void Flip(TwoWayDirection direction)
        {
            Editor.Flip(direction);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void Clear()
        {
            Editor.Clear();
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void SetPalette(params C64ColorName[] color)
        {
            if (color.Length <= 0)
                return;

            var count = Math.Min(color.Length, _sprite.SpriteColorPalette.Length);

            for (var i = 0; i < count; i++)
                _sprite.SpriteColorPalette[i] = ConvertColor(color[i]);

            Editor.UpdateEditorButtons();
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void ModifyPalette(int paletteIndex, C64ColorName color)
        {
            _sprite.SpriteColorPalette[paletteIndex] = ConvertColor(color);
            Editor.UpdateEditorButtons();
        }

        private ColorName ConvertColor(C64ColorName color)
        {
            switch (color)
            {
                case C64ColorName.Black:
                    return ColorName.Black;
                case C64ColorName.White:
                    return ColorName.White;
                case C64ColorName.Red:
                    return ColorName.Red;
                case C64ColorName.Cyan:
                    return ColorName.Cyan;
                case C64ColorName.Violet:
                    return ColorName.Violet;
                case C64ColorName.Green:
                    return ColorName.Green;
                case C64ColorName.Blue:
                    return ColorName.Blue;
                case C64ColorName.Yellow:
                    return ColorName.Yellow;
                case C64ColorName.Orange:
                    return ColorName.Orange;
                case C64ColorName.Brown:
                    return ColorName.Brown;
                case C64ColorName.LightRed:
                    return ColorName.LightRed;
                case C64ColorName.DarkGrey:
                    return ColorName.DarkGrey;
                case C64ColorName.Grey:
                    return ColorName.Grey;
                case C64ColorName.LightGreen:
                    return ColorName.LightGreen;
                case C64ColorName.LightBlue:
                    return ColorName.LightBlue;
                case C64ColorName.LightGrey:
                    return ColorName.LightGrey;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Width = 359;
            Height = 314;
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Editor.PaintEditor(e.Graphics, Focused);
            base.OnPaint(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Editor.SetPixel(e.X, e.Y, _currentColorIndex);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    Editor.MoveCursor(0, -1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Down:
                    Editor.MoveCursor(0, 1);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Left:
                    Editor.MoveCursor(-1, 0);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Right:
                    Editor.MoveCursor(1, 0);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.D1:
                    Editor.SetPixelAtCursor(0);
                    break;
                case Keys.D2:
                    Editor.SetPixelAtCursor(1);
                    break;
                case Keys.D3:
                    if (Editor.CurrentSprite.MultiColor)
                        Editor.SetPixelAtCursor(2);
                    break;
                case Keys.D4:
                    if (Editor.CurrentSprite.MultiColor)
                        Editor.SetPixelAtCursor(3);
                    break;
            }

            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
            base.OnKeyDown(e);
        }
    }
}