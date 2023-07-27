﻿using System;
using System.Windows.Forms;

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