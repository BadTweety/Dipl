namespace Diploma
{
    partial class Settings
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
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label_compartment_name = new System.Windows.Forms.Label();
            this.tb_compartment = new System.Windows.Forms.TextBox();
            this.label_reversibility = new System.Windows.Forms.Label();
            this.clb_reverse = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(219, 293);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(75, 23);
            this.btn_apply.TabIndex = 1;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(300, 293);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // label_compartment_name
            // 
            this.label_compartment_name.AutoSize = true;
            this.label_compartment_name.Location = new System.Drawing.Point(13, 13);
            this.label_compartment_name.Name = "label_compartment_name";
            this.label_compartment_name.Size = new System.Drawing.Size(98, 13);
            this.label_compartment_name.TabIndex = 3;
            this.label_compartment_name.Text = "Compartment name";
            // 
            // tb_compartment
            // 
            this.tb_compartment.Location = new System.Drawing.Point(16, 29);
            this.tb_compartment.Name = "tb_compartment";
            this.tb_compartment.Size = new System.Drawing.Size(162, 20);
            this.tb_compartment.TabIndex = 4;
            // 
            // label_reversibility
            // 
            this.label_reversibility.AutoSize = true;
            this.label_reversibility.Location = new System.Drawing.Point(16, 56);
            this.label_reversibility.Name = "label_reversibility";
            this.label_reversibility.Size = new System.Drawing.Size(124, 13);
            this.label_reversibility.TabIndex = 5;
            this.label_reversibility.Text = "Reversibility of reactions:";
            // 
            // clb_reverse
            // 
            this.clb_reverse.BackColor = System.Drawing.SystemColors.Control;
            this.clb_reverse.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clb_reverse.CheckOnClick = true;
            this.clb_reverse.FormattingEnabled = true;
            this.clb_reverse.Items.AddRange(new object[] {
            "ESReaction",
            "Source",
            "GeneExpControl",
            "EFormation",
            "Elimination"});
            this.clb_reverse.Location = new System.Drawing.Point(19, 73);
            this.clb_reverse.Name = "clb_reverse";
            this.clb_reverse.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.clb_reverse.Size = new System.Drawing.Size(120, 90);
            this.clb_reverse.TabIndex = 6;
            this.clb_reverse.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 327);
            this.Controls.Add(this.clb_reverse);
            this.Controls.Add(this.label_reversibility);
            this.Controls.Add(this.tb_compartment);
            this.Controls.Add(this.label_compartment_name);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowIcon = false;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label_compartment_name;
        private System.Windows.Forms.TextBox tb_compartment;
        private System.Windows.Forms.Label label_reversibility;
        private System.Windows.Forms.CheckedListBox clb_reverse;
    }
}