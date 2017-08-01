using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            //fixed size
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //set compartment name based on MainWindow variables
            tb_compartment.Text = MainWindow.compartment_name;
            int index;
            //set checkboxlist based on MainWindow variables
            index = clb_reverse.Items.IndexOf("ESReaction");
            clb_reverse.SetItemChecked(index,MainWindow.reversibility_ESReaction);
            index = clb_reverse.Items.IndexOf("EFormation");
            clb_reverse.SetItemChecked(index, MainWindow.reversibility_EFormation);
            index = clb_reverse.Items.IndexOf("GeneExpControl");
            clb_reverse.SetItemChecked(index, MainWindow.reversibility_GenExp);
            index = clb_reverse.Items.IndexOf("Source");
            clb_reverse.SetItemChecked(index, MainWindow.reversibility_Source);
            index = clb_reverse.Items.IndexOf("Elimination");
            clb_reverse.SetItemChecked(index, MainWindow.reversibility_Elimination);
        }


        private void btn_apply_Click(object sender, EventArgs e)
        {
            //compartment
            MainWindow.compartment_name = tb_compartment.Text;
            //reversibility
            int index;
            index = clb_reverse.Items.IndexOf("ESReaction");
            MainWindow.reversibility_ESReaction = clb_reverse.GetItemChecked(index);
            index = clb_reverse.Items.IndexOf("EFormation");
            MainWindow.reversibility_EFormation = clb_reverse.GetItemChecked(index);
            index = clb_reverse.Items.IndexOf("GeneExpControl");
            MainWindow.reversibility_GenExp = clb_reverse.GetItemChecked(index);
            index = clb_reverse.Items.IndexOf("Source");
            MainWindow.reversibility_Source = clb_reverse.GetItemChecked(index);
            index = clb_reverse.Items.IndexOf("Elimination");
            MainWindow.reversibility_Elimination = clb_reverse.GetItemChecked(index);
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
