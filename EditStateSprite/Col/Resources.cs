#nullable enable
using System.Drawing;

namespace EditStateSprite.Col;

public class Resources : IResources
{
    private readonly SolidBrush[] _solids;
    private readonly SolidBrush _shadowBrush;
    private readonly Pen[] _pens;

    public Resources()
    {
        _solids = new SolidBrush[16];
        _shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0));
        _pens = new Pen[16];
        var palette = new Palette();

        for (var i = 0; i < 16; i++)
        {
            _solids[i] = new SolidBrush(palette.GetColor(i));
            _pens[i] = new Pen(palette.GetColor(i));
        }
    }

    public SolidBrush GetColorBrush(ColorName color) =>
        _solids[(int)color];

    public SolidBrush GetShadowBrush() =>
        _shadowBrush;

    public Pen GetColorPen(ColorName color) =>
        _pens[(int)color];

    public Pen GetColorPen(int colorIndex) =>
        _pens[colorIndex];

    public void Dispose()
    {
        try
        {
            _shadowBrush.Dispose();
        }
        catch
        {
            // ignored
        }
            
        for (var i = 0; i < 16; i++)
        {
            try
            {
                _solids[i].Dispose();
            }
            catch
            {
                // ignored
            }
            try
            {
                _pens[i].Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}