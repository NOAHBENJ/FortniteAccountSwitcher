using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using FortniteAccountSwitcher;

namespace FortniteAccountSwitcher { 
    public partial class Form1 : Form
    {
        private Account selectedAccount;
        public string accountsJsonLocation = "./accounts.json";

        public Form1()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private List<Account> accounts = new List<Account>();

        private void LoadAccounts()
        {
            if (File.Exists(accountsJsonLocation))
            {
                string json = File.ReadAllText(accountsJsonLocation);
                accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                accountsListBox.Items.Clear();
                accountListBox.Items.Clear();
                foreach (Account account in accounts)
                {
                    accountsListBox.Items.Add(account.Username);
                    accountListBox.Items.Add(account.Username);
                }
            }
        }
        
        private void btnRunFortnite_Click(object sender, EventArgs e)
        {
            if (selectedAccount != null)
            {
                string selectedUsername = accountListBox.SelectedItem.ToString();
                Account accountToRun = accounts.Find(a => a.Username == selectedUsername);
                var form2 = new Form2();
                string status = form2.launch(accountToRun.AccountId, "./" + accountToRun.AccountId + ".json");
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
                    if (form2.txtUsername != null && form2.txtAccountID != null)
                    {
                        Account newAccount = new Account { Username = form2.txtUsername, AccountId = form2.txtAccountID };
                        string json = File.ReadAllText(accountsJsonLocation);
                        accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                        foreach (Account account in accounts)
                        {
                            if (account.Username == newAccount.Username)
                            {
                                MessageBox.Show("Account already exists.");
                                return;
                            }
                        }
                        accounts.Add(newAccount);
                        json = JsonConvert.SerializeObject(accounts);
                        File.WriteAllText(accountsJsonLocation, json);
                        LoadAccounts();
                    }
                }
            }
        }

        private void btnRemoveAccount_Click(object sender, EventArgs e)
        {
            if (accountsListBox.SelectedItem != null)
            {
                string selectedUsername = accountsListBox.SelectedItem.ToString();
                Account accountToRemove = accounts.Find(a => a.Username == selectedUsername);
                accounts.Remove(accountToRemove);
                string json = JsonConvert.SerializeObject(accounts);
                File.WriteAllText(accountsJsonLocation, json);
                accountsListBox.Items.Remove(selectedUsername);
                accountListBox.Items.Remove(selectedUsername);
                LoadAccounts();
            }
        }
    }

    public class Account
    {
        public string Username { get; set; }
        public string AccountId { get; set; }
    }
}
