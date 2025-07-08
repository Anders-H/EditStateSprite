#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;
using EditStateSprite.Col;

namespace EditStateSprite;

public class ColorDropDown : ComboBox
{
    internal static IResources Resources { get; }

    static ColorDropDown()
    {
        Resources = new Resources();
    }

    public ColorDropDown()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        MaxDropDownItems = 16;
        DrawMode = DrawMode.OwnerDrawFixed;

        foreach (var c in (ColorName[])Enum.GetValues(typeof(ColorName)))
            Items.Add(c);
    }

    public void SetColor(ColorName color)
    {
        foreach (var item in Items)
        {
            if (color != (ColorName)item)
                continue;

            SelectedItem = item;
            return;
        }
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (Items.Count > 0 && e.Index >= 0 && e.Index < Items.Count)
        {
            var col = (ColorName)Items[e.Index];
            e.DrawBackground();
            var r = new Rectangle(e.Bounds.X + 100, e.Bounds.Y + 2, e.Bounds.Width - 100, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(Resources.GetColorBrush(col), r);
            e.Graphics.DrawString(col.ToString(), Font, Brushes.Black, e.Bounds);
        }

        base.OnDrawItem(e);
    }
}