﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EditStateSprite;

namespace TestProgram;

public partial class Form1 : Form
{
    private readonly Random _rnd = new();
    private readonly SpriteList _sprites;

    public Form1()
    {
        InitializeComponent();
        _sprites = [];
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        _sprites.Add(new SpriteRoot(false));
        Randomize(_sprites[0], 420, 20);
        _sprites.Add(new SpriteRoot(true));
        Randomize(_sprites[1], 420, 150);

        spriteEditorControl1.ConnectSprite(_sprites[0]);
        spriteEditorControl2.ConnectSprite(_sprites[1]);

        _sprites[0].Name = "Mr. Sprite part one";
        _sprites[1].Name = "Mr. Sprite part two";
    }

    private void Randomize(SpriteRoot sprite, int posX, int posY)
    {
        var colorCount = sprite.MultiColor ? 4 : 2;
        sprite.PreviewOffsetX = posX;
        sprite.PreviewOffsetY = posY;
        sprite.ExpandX = true;
        sprite.ExpandY = true;
        sprite.PreviewZoom = true;

        for (var y = 0; y < sprite.ColorMap.Height; y++)
        {
            for (var x = 0; x < sprite.ColorMap.Width; x++)
            {
                sprite.ColorMap.SetColorIndex(x, y, _rnd.Next(colorCount));
            }
        }
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        _sprites.PaintPreview(e.Graphics);
    }

    private void spriteEditorControl1_SpriteChanged(object sender, SpriteChangedEventArgs e)
    {
        Invalidate();
    }

    private void spriteEditorControl2_SpriteChanged(object sender, SpriteChangedEventArgs e)
    {
        Invalidate();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Scroll(FourWayDirection.Up);
        spriteEditorControl2.Scroll(FourWayDirection.Up);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Scroll(FourWayDirection.Down);
        spriteEditorControl2.Scroll(FourWayDirection.Down);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Scroll(FourWayDirection.Left);
        spriteEditorControl2.Scroll(FourWayDirection.Left);
    }

    private void button4_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Scroll(FourWayDirection.Right);
        spriteEditorControl2.Scroll(FourWayDirection.Right);
    }

    private void button5_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Flip(TwoWayDirection.LeftRight);
        spriteEditorControl2.Flip(TwoWayDirection.LeftRight);
    }

    private void button6_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Flip(TwoWayDirection.TopDown);
        spriteEditorControl2.Flip(TwoWayDirection.TopDown);
    }

    private void spriteEditorControl1_MouseClick(object sender, MouseEventArgs e)
    {
        spriteEditorControl1.Focus();
    }

    private void spriteEditorControl2_MouseClick(object sender, MouseEventArgs e)
    {
        spriteEditorControl2.Focus();
    }

    private void button7_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.Clear();
        spriteEditorControl2.Clear();
    }

    private void button8_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.SetPalette(ColorName.Green, ColorName.LightGreen);
        spriteEditorControl2.SetPalette(ColorName.Blue, ColorName.LightBlue, ColorName.Orange, ColorName.Brown);
    }

    private void button9_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.ToggleColorMode();
        spriteEditorControl2.ToggleColorMode();
    }

    private void button10_Click(object sender, EventArgs e)
    {
        _sprites.Load(@"C:\Users\hbom\Desktop\sprite.sprdef");
        spriteEditorControl1.ConnectSprite(_sprites[0]);
        spriteEditorControl2.ConnectSprite(_sprites[1]);
        Invalidate();
        MessageBox.Show(_sprites[1].Name);
    }

    private void button11_Click(object sender, EventArgs e)
    {
        _sprites.Save(@"D:\Temp\sprites.sprdef");
    }

    private void button12_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.PickPaletteColors(this);
        spriteEditorControl2.PickPaletteColors(this);
        Invalidate();
    }

    private void button13_Click(object sender, EventArgs e)
    {
        var bytes = spriteEditorControl1.GetBytes();
        var s = new StringBuilder();

        for (var i = 0; i < bytes.Length; i++)
            s.Append($@"{bytes[i]}{(i < bytes.Length - 1 ? ", " : "")}");

        MessageBox.Show(s.ToString());
    }

    private void button14_Click(object sender, EventArgs e)
    {
        spriteEditorControl1.ConnectSprite(_sprites[0]);
        spriteEditorControl1.Focus();
    }

    private void button15_Click(object sender, EventArgs e)
    {
        var code = spriteEditorControl1.GetBasicCode(10, 8192, 0, 110, 110);
        code += spriteEditorControl2.GetBasicCode(10, 8192, 1, 120, 120);
        Clipboard.SetText(code);
        MessageBox.Show(code);
    }

    private void button16_Click(object sender, EventArgs e)
    {
        var bytes = new List<byte>();
        bytes.AddRange(spriteEditorControl1.GetBytes64WithStartAddress(3584));
        bytes.AddRange(spriteEditorControl2.GetBytes64());
        File.WriteAllBytes(@"D:\Temp\test.prg", bytes.ToArray());
    }

    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
        if (radioButton1.Checked)
        {
            spriteEditorControl1.SetEditorTool(EditorToolEnum.PixelEditor);
            spriteEditorControl2.SetEditorTool(EditorToolEnum.PixelEditor);
        }
    }

    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
        if (radioButton2.Checked)
        {
            spriteEditorControl1.SetEditorTool(EditorToolEnum.FreeHand);
            spriteEditorControl2.SetEditorTool(EditorToolEnum.FreeHand);
        }
    }

    private void Form1_Shown(object sender, EventArgs e)
    {
        spriteEditorControl1.SetCurrentColorIndex(1);
        spriteEditorControl2.SetCurrentColorIndex(1);
    }

    private void button17_Click(object sender, EventArgs e)
    {
        using var b1 = _sprites[0].GetBitmap16x16NoAttributes();
        using var b2 = _sprites[1].GetBitmap16x16NoAttributes();
    }
}