using System.Collections.Generic;
using System.Drawing;

namespace EditStateSprite
{
    public class SpriteList : List<SpriteRoot>
    {
        public void PaintPreview(Graphics g)
        {
            foreach (var s in this)
            {
                s.ColorMap.PaintPreview(g);
            }
        }
    }
}