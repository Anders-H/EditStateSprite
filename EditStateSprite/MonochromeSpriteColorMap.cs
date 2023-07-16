﻿using System.Drawing;

namespace EditStateSprite
{
    public class MonochromeSpriteColorMap : SpriteColorMapBase
    {
        public override int Width => 24;
        public override int ColorCount => 2;

        public MonochromeSpriteColorMap(SpriteRoot parent) : base(parent)
        {
        }
    }
}