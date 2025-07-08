#nullable enable
namespace EditStateSprite.SpriteModifiers;

public abstract class Modifier
{
    protected readonly int[,] Colors;
    protected readonly int Width;
    protected readonly int Height;

    protected Modifier(int[,] colors)
    {
        Colors = colors;
        Width = Colors.GetLength(0);
        Height = Colors.GetLength(1);
    }
}