namespace web_wallpaper.Controller
{
    partial class SetURLForm
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
            this.selectBtn = new System.Windows.Forms.Button();
            this.urlBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // selectBtn
            // 
            this.selectBtn.Location = new System.Drawing.Point(613, 108);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(218, 68);
            this.selectBtn.TabIndex = 0;
            this.selectBtn.Text = "확인";
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // urlBox
            // 
            this.urlBox.Location = new System.Drawing.Point(12, 33);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(819, 52);
            this.urlBox.TabIndex = 1;
            // 
            // SetURLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(18F, 44F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 195);
            this.Controls.Add(this.urlBox);
            this.Controls.Add(this.selectBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetURLForm";
            this.ShowIcon = false;
            this.Text = "Set wallpaper URL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectBtn;
        private System.Windows.Forms.TextBox urlBox;
    }
}