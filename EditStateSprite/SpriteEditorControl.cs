using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
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

        /// <summary>
        /// Generates Commodore 64 BASIC code for displaying a sprite.
        /// </summary>
        /// <param name="lineNumber">BASIC line number (0 - 63999)</param>
        /// <param name="hwSpriteIndex">Hardware sprite (0 - 7)</param>
        /// <returns>Commodore BASIC 2.0 second release source code.</returns>
        public string GetBasicCode(int lineNumber, int spriteDataStartAddress, int totalSpriteIndex, int hwSpriteIndex, int x, int y)
        {
            if (lineNumber < 0 || lineNumber > 63999 - 0)
                throw new ArgumentOutOfRangeException(nameof(lineNumber));

            if (hwSpriteIndex < 0 || hwSpriteIndex > 7)
                throw new ArgumentOutOfRangeException(nameof(hwSpriteIndex));

            var startAddress = spriteDataStartAddress / 64 + totalSpriteIndex;

            var turnOnFlagPosition = new[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var turnOffFlagPosition = new[] { 254, 253, 251, 247, 239, 223, 191, 127 };

            var s = new StringBuilder();

            s.AppendLine($"{lineNumber} poke53281,{(int)_sprite.SpriteColorPalette[0]}:poke2040,{startAddress + totalSpriteIndex}");

            lineNumber++;
            s.AppendLine($"{lineNumber} fora={spriteDataStartAddress}to{spriteDataStartAddress + 62}:readb:pokea,b:next");

            var bytes = GetBytes();
            for (var i = 0; i < 63; i++)
                bytes[i] = (byte)i;

            var chunks = new[] { 0, 11, 12, 23, 24, 35, 36, 48, 49, 62};

            for (var n = 0; n < 10; n += 2)
            {
                lineNumber++;
                s.Append($"{lineNumber} data{bytes[chunks[n]]}");
                for (var i = chunks[n] + 1; i < chunks[n + 1]; i++)
                    s.Append($",{bytes[i]}");
                s.AppendLine($",{bytes[chunks[n + 1]]}");
            }

            lineNumber++;

            if (x < 0)
                x = 0;
            else if (x > 511)
                x = 511;

            if (y < 0)
                y = 0;
            else if (y > 255)
                y = 255;

            s.AppendLine(x > 255
                ? $"{lineNumber} poke{53248 + hwSpriteIndex * 2},{x - 256}:poke53264,peek(53264)or{turnOnFlagPosition[hwSpriteIndex]}:poke{53249 + hwSpriteIndex * 2},{y}"
                : $"{lineNumber} poke{53248 + hwSpriteIndex * 2},{x}:poke53264,peek(53264)and{turnOffFlagPosition[hwSpriteIndex]}:poke{53249 + hwSpriteIndex * 2},{y}"
            );


            lineNumber++;
            if (_sprite.MultiColor)
                s.AppendLine($"{lineNumber} poke");
            else
                s.AppendLine($"{lineNumber} poke{53287 + hwSpriteIndex},{(int)_sprite.SpriteColorPalette[1]}");

            lineNumber++;
            s.AppendLine($"{lineNumber} poke53269,peek(53269)or{turnOnFlagPosition[hwSpriteIndex]}"); // Turn on sprite.
            return s.ToString();
        }
    }
}