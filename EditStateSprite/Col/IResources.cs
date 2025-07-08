#nullable enable
using System;
using System.Drawing;

namespace EditStateSprite.Col;

public interface IResources : IDisposable
{
    SolidBrush GetColorBrush(ColorName color);
    SolidBrush GetShadowBrush();
}