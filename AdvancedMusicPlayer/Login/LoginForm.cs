using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AdvancedMusicPlayer.Login;

namespace AdvancedMusicPlayer
{
    public partial class LoginForm : Form
    {
        PasswordServices ps = new PasswordServices();
        Authentication au = new Authentication();
        List<Account> accntList = new List<Account>();

        public LoginForm()
        {
            InitializeComponent();
            GetAccountList();
        }

        /// <summary>
        /// Gets account list from csv file
        /// </summary>
        private void GetAccountList()
        {
            try
            {
                using (StreamReader reader = new StreamReader("account.csv"))
                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    accntList = csv.GetRecords<Account>().ToList();
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Please close CSV file before you open it" + ex);
            }
        }

        /// <summary>
        /// Adds account to account list and csv file
        /// </summary>
        /// <param name="newAccount"></param>
        private void SaveAccount(Account newAccount)
        {
            accntList.Add(newAccount);
            WriteCSV(newAccount);
        }

        /// <summary>
        /// Writes account into CSV file
        /// </summary>
        /// <param name="account"></param>
        private void WriteCSV(Account account)
        {
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = !File.Exists("account.csv"),
            };

            using (var writer = new StreamWriter("account.csv", true))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecord(account);
                csv.NextRecord();
            }
        }

        /// <summary>
        /// Checks if thext fieldd are empty
        /// </summary>
        /// <returns></returns>
        private bool IsTextFieldsEmpty()
        {
            if (!String.IsNullOrEmpty(username.Text) && !String.IsNullOrEmpty(password.Text))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears text fields
        /// </summary>
        private void ClearTextFields()
        {
            username.Clear();
            password.Clear();
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (IsTextFieldsEmpty())
            {
                (string username, string password) account = (username.Text, password.Text);
                if (au.Authenticate(account, accntList))
                {
                    loginStat.Text = "Authentication is successful.";
                    statusStrip1.Update();
                    Thread.Sleep(1000);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Your password or username is invalid.");
                    loginStat.Text = "Authentication Failed";
                }
            }
            else
            {
                MessageBox.Show("Please enter Username and Password.");
            }
            ClearTextFields();
        }

        /// <summary>
        /// Creates account and hash password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateAccount_Click(object sender, EventArgs e)
        {
            if (IsTextFieldsEmpty())
            {
                // get matched account from list and if it is not null then it is duplicated account
                Account matchedAccount = accntList.SingleOrDefault(u => u.Username == username.Text);
                if (matchedAccount != null)
                {
                    MessageBox.Show(username.Text + " is already exited. Please try different username");
                    loginStat.Text = "Creating account failed.";
                }
                else
                {
                    var hashedPassword = ps.getHashedPassword(password.Text); // gets hashed password
                    SaveAccount(new Account(username.Text, hashedPassword));
                    loginStat.Text = "Account is created.";
                }
            }
            else
            {
                MessageBox.Show("Please enter Username and Password.");
            }
            ClearTextFields();
        }

        /// <summary>
        /// Shows password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            password.PasswordChar = showPass.Checked ? '\0' : '*';
        }
    }
}
