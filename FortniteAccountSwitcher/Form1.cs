using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using FortniteAccountSwitcher;

namespace FortniteAccountSwitcher { 
    public partial class Form1 : Form
    {
        private List<Account> accounts = new List<Account>();
        private Account selectedAccount;

        public Form1()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            // Load accounts from a file or database
            // Example:
            // accounts.Add(new Account { Username = "AccountName1", AuthCode = "AuthCode1" });
            // accounts.Add(new Account { Username = "AccountName2", AuthCode = "AuthCode2" });
        }

        private void SaveAccounts()
        {
            // Save accounts to a file or database
        }

        private void btnRunFortnite_Click(object sender, EventArgs e)
        {
            if (selectedAccount != null)
            {
                string authCode = selectedAccount.AuthCode;
                string arguments = $"-AUTH_LOGIN=unused -obfuscationid=fEHVr69mPbH-q7R4-UWdH4pTzY_xLA -AUTH_PASSWORD={authCode} -AUTH_TYPE=exchangecode -epicapp=Fortnite -EpicPortal";
                Process.Start(@"C:\Program Files\Epic Games\Fortnite\FortniteGame\Binaries\Win64\FortniteLauncher.exe", arguments);
            }
            else
            {
                MessageBox.Show("Please select an account first.");
            }
        }

        private void accountListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = accountListBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < accounts.Count)
            {
                selectedAccount = accounts[selectedIndex];
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            using (var form2 = new Form2())
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    string authCode = form2.AuthCode;
                    if (!string.IsNullOrEmpty(authCode))
                    {
                        Account newAccount = new Account { Username = "NewAccountName", AuthCode = authCode };
                        accounts.Add(newAccount);
                        SaveAccounts();
                    }
                }
            }
        }
    }

    public class Account
    {
        public string Username { get; set; }
        public string AuthCode { get; set; }
    }
}