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
    public partial class How_to_use : Form
    {
        public How_to_use()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            tb_help.Text = "Basic help for using the parser\r\n\r\n\r\n";
            tb_help.Text += "Modelica file: - The file containing the model you want to translate to SBML\r\n";
            tb_help.Text += "SBML file: - The file you wish to save the converted model to\r\n";
            tb_help.Text += "Parse! - Opens the modelica file, parses it and saves the SBML version into the file chosen in \"SBML file\"\r\n";
            tb_help.Text += "Both files have to be selected in order for parser to work. Can only translate models, not individual elements. Currently only supporting basic functions, translation is solely visual. \r\n";
        }

        private void How_to_use_Load(object sender, EventArgs e)
        {

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_help_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
