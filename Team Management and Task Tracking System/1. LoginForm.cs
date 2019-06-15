using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Team_Management_and_Task_Tracking_System
{
    public partial class LoginForm : Form
    {
        public static string userId = "";
        string username;
        string password;
        bool validInput;

        public LoginForm()
        {
            InitializeComponent();
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            validateInput();

            if (validInput)
            {
                try
                {
                    conn = new MySqlConnection(connetionString);
                    conn.Open();

                    string query = "SELECT * FROM employee";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    reader = cmd.ExecuteReader();
                    bool userFound = false;

                    while (reader.Read())
                    {
                        if (reader[1].ToString().Equals(username))
                        {
                            if (reader[2].ToString().Equals(password))
                            {
                                userId = reader[0].ToString();
                                this.Hide();

                                if (reader[6].ToString().Equals("True"))
                                {
                                    ManagerForm managerForm = new ManagerForm();
                                    managerForm.Closed += (s, args) => this.Close();
                                    managerForm.Show();
                                }
                                else
                                {
                                    EmployeeForm employeeForm = new EmployeeForm();
                                    employeeForm.Closed += (s, args) => this.Close();
                                    employeeForm.Show();
                                }
                            }
                            else
                                MessageBox.Show("Wrong password", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            userFound = true;
                            break;
                        }
                    }

                    if (!userFound)
                        MessageBox.Show("No user by this username");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.ToString());
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }

                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void validateInput()
        {
            username = txtUsername.Text.ToString();
            password = txtPassword.Text.ToString();
            validInput = false;

            if (username.Equals(""))
            {
                loginErrorProvider.SetError(txtUsername, "Enter username");
            }
            else if (password.Equals(""))
            {
                loginErrorProvider.SetError(txtPassword, "Enter password");
            }
            else
                validInput = true;
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            loginErrorProvider.Clear();
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            loginErrorProvider.Clear();
        }

    }
}
