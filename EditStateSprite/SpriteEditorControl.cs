using System;
using System.Windows.Forms;
using EditStateSprite.CodeGeneration.Basic20;
using EditStateSprite.Dialogs;

namespace EditStateSprite
{
    public class SpriteEditorControl : Control
    {
        private int _currentColorIndex;
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
            var pos = Editor.GetCursorPosition();
            _sprite = sprite;
            Editor.ChangeCurrentSprite(_sprite);
            Editor.SetCursorPosition(pos);
            Invalidate();
        }

        public void SetCurrentColorIndex(int colorIndex)
        {
            _currentColorIndex = colorIndex;

            if (_currentColorIndex < 0)
                _currentColorIndex = 0;

            if (_sprite == null)
                return;

            if (_sprite.MultiColor)
            {
                if (_currentColorIndex > 3)
                    _currentColorIndex = 3;
            }
            else
            {
                if (_currentColorIndex > 1)
                    _currentColorIndex = 1;
            }
        }

        public void Scroll(FourWayDirection direction)
        {
            var cursorPosition = Editor.GetCursorPosition();
            Editor.Scroll(direction);
            Editor.SetCursorPosition(cursorPosition);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void Flip(TwoWayDirection direction)
        {
            var cursorPosition = Editor.GetCursorPosition();
            Editor.Flip(direction);
            Editor.SetCursorPosition(cursorPosition);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void Clear()
        {
            Editor.Clear();
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void SetPalette(params ColorName[] color)
        {
            if (color.Length <= 0)
                return;

            var count = Math.Min(color.Length, _sprite.SpriteColorPalette.Length);

            for (var i = 0; i < count; i++)
                _sprite.SpriteColorPalette[i] = color[i];

            Editor.UpdateEditorButtons();
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public void PickPaletteColors(IWin32Window owner)
        {
            using (var x = new FourColorPaletteColorPicker())
            {
                x.Palette = _sprite.SpriteColorPalette;

                if (x.ShowDialog(this) == DialogResult.OK)
                {
                    _sprite.SpriteColorPalette = x.Palette;
                    ConnectSprite(_sprite);
                }
            }
        }

        public void ModifyPalette(int paletteIndex, ColorName color)
        {
            _sprite.SpriteColorPalette[paletteIndex] = color;
            Editor.UpdateEditorButtons();
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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
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

        public void ToggleColorMode()
        {
            Editor.ResetCursorPosition();

            if (_sprite.MultiColor)
                _sprite.ConvertToMonochrome();
            else
                _sprite.ConvertToMultiColor();

            ConnectSprite(_sprite);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
        }

        public byte[] GetBytes() =>
            _sprite.GetBytes();

        public byte[] GetBytes64() =>
            _sprite.GetBytes64();

        public byte[] GetBytes64WithStartAddress(ushort startAddress) =>
            _sprite.GetBytes64WithStartAddress(startAddress);

        /// <summary>
        /// Generates Commodore 64 BASIC code for displaying a sprite.
        /// </summary>
        /// <param name="lineNumber">BASIC line number (0 - 63999)</param>
        /// <param name="spriteDataStartAddress"></param>
        /// <param name="includeInExportIndex">The index of the sprite in the list of sprites being exported</param>
        /// <param name="hwSpriteIndex">Hardware sprite (0 - 7)</param>
        /// <param name="x">C64 horizontal screen location</param>
        /// <param name="y">C64 vertical screen location</param>
        /// <returns>Commodore BASIC 2.0 second release source code.</returns>
        public string GetBasicCode(int lineNumber, int spriteDataStartAddress, int includeInExportIndex, int x, int y) =>
            new CommodoreBasic20Generator(_sprite).GetBasicCode(lineNumber, spriteDataStartAddress, includeInExportIndex, x, y);
    }
}