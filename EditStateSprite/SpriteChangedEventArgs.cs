using System;

namespace EditStateSprite
{
    public class SpriteChangedEventArgs : EventArgs
    {
        public SpriteRoot Sprite { get; }

        public SpriteChangedEventArgs(SpriteRoot sprite)
        {
            Sprite = sprite;
        }
    }
}