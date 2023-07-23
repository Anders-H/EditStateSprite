using System;
using System.Windows.Forms;

namespace EditStateSprite
{
    public class SpriteEditorControl : Control
    {
        private SpriteRoot _sprite;
        private Editor Editor { get; set; }

        public void ConnectSprite(SpriteRoot sprite)
        {
            _sprite = sprite;
            Editor = new Editor(_sprite);
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
            if (_sprite == null)
                return;

            Editor.PaintEditor(e.Graphics);
            base.OnPaint(e);
        }
    }
}