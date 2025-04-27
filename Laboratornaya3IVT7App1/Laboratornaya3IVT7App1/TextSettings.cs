using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratornaya3IVT7App1
{
    public partial class TextSettings : Form
    {
        public string InputText { get; private set; }

        public TextSettings(string currentText)
        {
            InitializeComponent();
            tb_input_text.Text = currentText;
        }

        private void b_OK_Click(object sender, EventArgs e)
        {
            InputText = tb_input_text.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void b_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
