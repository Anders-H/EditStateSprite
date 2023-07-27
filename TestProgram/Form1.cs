using System;
using System.Windows.Forms;
using EditStateSprite;

namespace TestProgram
{
    public partial class Form1 : Form
    {
        private readonly Random _rnd = new Random();
        private readonly SpriteList _sprites;

        public Form1()
        {
            InitializeComponent();
            _sprites = new SpriteList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _sprites.Add(new SpriteRoot(false));
            Randomize(_sprites[0], 420, 20);
            _sprites.Add(new SpriteRoot(true));
            Randomize(_sprites[1], 420, 150);

            spriteEditorControl1.ConnectSprite(_sprites[0]);
            spriteEditorControl2.ConnectSprite(_sprites[1]);
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
    }
}