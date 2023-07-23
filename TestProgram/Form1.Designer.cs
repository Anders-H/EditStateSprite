namespace TestProgram
{
    partial class Form1
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
            this.spriteEditorControl1 = new EditStateSprite.SpriteEditorControl();
            this.spriteEditorControl2 = new EditStateSprite.SpriteEditorControl();
            this.SuspendLayout();
            // 
            // spriteEditorControl1
            // 
            this.spriteEditorControl1.Location = new System.Drawing.Point(16, 308);
            this.spriteEditorControl1.Name = "spriteEditorControl1";
            this.spriteEditorControl1.Size = new System.Drawing.Size(359, 314);
            this.spriteEditorControl1.TabIndex = 0;
            this.spriteEditorControl1.Text = "spriteEditorControl1";
            // 
            // spriteEditorControl2
            // 
            this.spriteEditorControl2.Location = new System.Drawing.Point(396, 308);
            this.spriteEditorControl2.Name = "spriteEditorControl2";
            this.spriteEditorControl2.Size = new System.Drawing.Size(359, 314);
            this.spriteEditorControl2.TabIndex = 1;
            this.spriteEditorControl2.Text = "spriteEditorControl2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 686);
            this.Controls.Add(this.spriteEditorControl2);
            this.Controls.Add(this.spriteEditorControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private EditStateSprite.SpriteEditorControl spriteEditorControl1;
        private EditStateSprite.SpriteEditorControl spriteEditorControl2;
    }
}

