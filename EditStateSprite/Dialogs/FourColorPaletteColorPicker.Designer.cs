namespace EditStateSprite.Dialogs
{
    partial class FourColorPaletteColorPicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDropDown1 = new EditStateSprite.ColorDropDown();
            this.colorDropDown2 = new EditStateSprite.ColorDropDown();
            this.colorDropDown3 = new EditStateSprite.ColorDropDown();
            this.colorDropDown4 = new EditStateSprite.ColorDropDown();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colorDropDown1
            // 
            this.colorDropDown1.FormattingEnabled = true;
            this.colorDropDown1.Location = new System.Drawing.Point(16, 16);
            this.colorDropDown1.Name = "colorDropDown1";
            this.colorDropDown1.Size = new System.Drawing.Size(240, 21);
            this.colorDropDown1.TabIndex = 0;
            // 
            // colorDropDown2
            // 
            this.colorDropDown2.FormattingEnabled = true;
            this.colorDropDown2.Location = new System.Drawing.Point(16, 44);
            this.colorDropDown2.Name = "colorDropDown2";
            this.colorDropDown2.Size = new System.Drawing.Size(240, 21);
            this.colorDropDown2.TabIndex = 1;
            // 
            // colorDropDown3
            // 
            this.colorDropDown3.FormattingEnabled = true;
            this.colorDropDown3.Location = new System.Drawing.Point(16, 72);
            this.colorDropDown3.Name = "colorDropDown3";
            this.colorDropDown3.Size = new System.Drawing.Size(240, 21);
            this.colorDropDown3.TabIndex = 2;
            // 
            // colorDropDown4
            // 
            this.colorDropDown4.FormattingEnabled = true;
            this.colorDropDown4.Location = new System.Drawing.Point(16, 100);
            this.colorDropDown4.Name = "colorDropDown4";
            this.colorDropDown4.Size = new System.Drawing.Size(240, 21);
            this.colorDropDown4.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(100, 152);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FourColorPaletteColorPicker
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(273, 189);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.colorDropDown4);
            this.Controls.Add(this.colorDropDown3);
            this.Controls.Add(this.colorDropDown2);
            this.Controls.Add(this.colorDropDown1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FourColorPaletteColorPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sprite palette";
            this.Load += new System.EventHandler(this.FourColorPaletteColorPicker_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ColorDropDown colorDropDown1;
        private ColorDropDown colorDropDown2;
        private ColorDropDown colorDropDown3;
        private ColorDropDown colorDropDown4;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}