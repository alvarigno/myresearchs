using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsBrowserApp
{
    public partial class CodeForm : Form
    {
        public CodeForm()
        {
            InitializeComponent();
        }

        private void CodeForm_Load(object sender, EventArgs e)
        {

        }

        public string Code
        {
            get
            {
                if (richTextBox1.Text != null)
                {
                    return (richTextBox1.Text);
                }
                else
                {
                    return ("");
                }
            }
            set
            {
                richTextBox1.Text = value;
            }
        }

    }
}
