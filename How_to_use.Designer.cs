namespace Diploma
{
    partial class How_to_use
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
            this.btn_close = new System.Windows.Forms.Button();
            this.tb_help = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(399, 247);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 0;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // tb_help
            // 
            this.tb_help.BackColor = System.Drawing.SystemColors.Control;
            this.tb_help.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_help.Location = new System.Drawing.Point(13, 13);
            this.tb_help.Multiline = true;
            this.tb_help.Name = "tb_help";
            this.tb_help.Size = new System.Drawing.Size(466, 228);
            this.tb_help.TabIndex = 1;
            this.tb_help.TextChanged += new System.EventHandler(this.tb_help_TextChanged);
            // 
            // How_to_use
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 282);
            this.Controls.Add(this.tb_help);
            this.Controls.Add(this.btn_close);
            this.Name = "How_to_use";
            this.ShowIcon = false;
            this.Text = "Help";
            this.Load += new System.EventHandler(this.How_to_use_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.TextBox tb_help;
    }
}