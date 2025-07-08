#nullable enable
using System;

namespace EditStateSprite.Col;

[Flags]
public enum RendererFlags
{
    None = 0,
    Outline = 1,
    Selected = 2,
    Shadow = 4
}