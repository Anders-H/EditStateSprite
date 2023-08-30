using System;
using System.Windows.Forms;

namespace EditStateSprite.Dialogs
{
    public partial class FourColorPaletteColorPicker : Form
    {
        public ColorName[] Palette { get; set; }

        public FourColorPaletteColorPicker()
        {
            InitializeComponent();
        }

        private void FourColorPaletteColorPicker_Load(object sender, System.EventArgs e)
        {
            colorDropDown1.SetColor(Palette[0]);
            colorDropDown2.SetColor(Palette[1]);

            if (Palette.Length == 2)
            {
                colorDropDown3.Enabled = false;
                colorDropDown4.Enabled = false;
            }
            else if (Palette.Length == 4)
            {
                colorDropDown3.SetColor(Palette[2]);
                colorDropDown4.SetColor(Palette[3]);
            }
            else
            {
                throw new SystemException("Palette should have length 2 or 4.");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Palette[0] = (ColorName)colorDropDown1.SelectedItem;
            Palette[1] = (ColorName)colorDropDown2.SelectedItem;

            if (Palette.Length == 4)
            {
                Palette[2] = (ColorName)colorDropDown3.SelectedItem;
                Palette[3] = (ColorName)colorDropDown4.SelectedItem;
            }

            DialogResult = DialogResult.OK;
        }
    }
}