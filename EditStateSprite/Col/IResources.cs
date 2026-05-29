#nullable enable
using System;
using System.Drawing;

namespace EditStateSprite.Col;

public interface IResources : IDisposable
{
    SolidBrush GetColorBrush(ColorName color);
    SolidBrush GetShadowBrush();
    Pen GetColorPen(ColorName color);
    Pen GetColorPen(int colorIndex);
}