using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace FortniteAccountSwitcher
{
    public partial class Form2 : Form
    {
        public string AuthCode { get; private set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            AuthCode = txtAuthCode.Text;
            DialogResult = DialogResult.OK;
        }

        private void linkGoogle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.google.com");
        }
    }
}
