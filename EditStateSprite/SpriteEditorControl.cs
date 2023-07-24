using System;
using System.Windows.Forms;

namespace EditStateSprite
{
    public class SpriteEditorControl : Control
    {
        private int _currentColorIndex = 0;
        private SpriteRoot _sprite;
        private Editor Editor { get; set; }
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

        protected override void OnResize(EventArgs e)
        {
            Width = 359;
            Height = 314;
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Editor.PaintEditor(e.Graphics);
            base.OnPaint(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Editor.SetPixel(e.X, e.Y, _currentColorIndex);
            Invalidate();
            SpriteChanged?.Invoke(this, new SpriteChangedEventArgs(Editor.CurrentSprite));
            base.OnMouseClick(e);
        }
    }
}