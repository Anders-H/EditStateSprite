using System;
using System.Windows.Forms;
using EditStateSprite;

namespace TestProgram
{
    public partial class Form1 : Form
    {
        private readonly Random _rnd = new Random();
        private readonly SpriteList _sprites;
        private Editor Editor { get; set; }

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

            Editor = new Editor(_sprites[0]);
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
            Editor.PaintEditor(e.Graphics);
        }
    }
}