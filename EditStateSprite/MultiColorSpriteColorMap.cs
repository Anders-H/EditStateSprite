namespace EditStateSprite
{
    public class MultiColorSpriteColorMap : SpriteColorMapBase
    {
        public override int Width => 12;
        public override int ColorCount => 4;

        public MultiColorSpriteColorMap(SpriteRoot parent) : base(parent)
        {
        }
    }
}