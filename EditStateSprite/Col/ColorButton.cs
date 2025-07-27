#nullable enable
using System.Drawing;

namespace EditStateSprite.Col;

public class ColorButton
{
    private readonly Renderer _renderer;
    public Rectangle Location { get; private set; }
    public ColorName Color { get; set; }
    public bool Selected { get; set; }

    public ColorButton(Renderer renderer, Rectangle location, ColorName color)
    {
        _renderer = renderer;
        Location = location;
        Color = color;
    }

    public bool HitTest(Point point) =>
        Location.IntersectsWith(new Rectangle(point.X, point.Y, 1, 1));

    public void Render(Graphics g, IResources resources)
    {
        if (Selected)
            _renderer.Render(g, resources, Location, Color, RendererFlags.Outline | RendererFlags.Shadow | RendererFlags.Selected);
        else
            _renderer.Render(g, resources, Location, Color, RendererFlags.Outline | RendererFlags.Shadow);
    }

    internal void ResizeButton(int x, int y, int width, int height) =>
        Location = new Rectangle(x, y, width, height);
}