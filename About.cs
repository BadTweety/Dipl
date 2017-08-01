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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            tb_about.Text = "SysBio Library -> SBML translator / parser \r\n\r\n\r\n";
            tb_about.Text += "Made for purposes of diploma at the Faculty of Computer and Information Science Ljubljana \r\n";
            tb_about.Text += "Author: Timotej Osolin\r\n";
            tb_about.Text += "Mentors: Miha Moškon, Miha Mraz\r\n";
        }

        private void About_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
