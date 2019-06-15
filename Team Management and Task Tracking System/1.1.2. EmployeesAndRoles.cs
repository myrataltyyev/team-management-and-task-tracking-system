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
    public partial class EmployeesAndRolesForm : Form
    {
        public EmployeesAndRolesForm()
        {
            InitializeComponent();
        }

        private void EmployeesAndRolesForm_Load(object sender, EventArgs e)
        {
            // Set titles for columns
            listViewEmployees.Columns.Add("ID", 30, HorizontalAlignment.Center);
            listViewEmployees.Columns.Add("First Name", 150, HorizontalAlignment.Center);
            listViewEmployees.Columns.Add("Last Name", 150, HorizontalAlignment.Center);
            listViewEmployees.Columns.Add("Role", 200, HorizontalAlignment.Center);
            listViewEmployees.Columns.Add("Team Name", 150, HorizontalAlignment.Center);
            listViewEmployees.Columns.Add("email", 200, HorizontalAlignment.Center);
            listViewEmployees.Columns.Add("Employment Date", 150, HorizontalAlignment.Center);

            // Db connection
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();

                string query = "SELECT employee.id, first_name, last_name, role_name, team_name, email, " +
                    "employment_date FROM employee, role, team, team_member WHERE team_member.employee_id = " +
                    "employee.id AND team_member.role_id = role.id AND team_member.team_id = team.id AND " +
                    "is_task_manager = 0";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem rootId = new ListViewItem(reader[0].ToString());
                    ListViewItem.ListViewSubItem firstName =
                        new ListViewItem.ListViewSubItem(rootId, reader[1].ToString());
                    ListViewItem.ListViewSubItem lastName =
                        new ListViewItem.ListViewSubItem(rootId, reader[2].ToString());
                    ListViewItem.ListViewSubItem role =
                        new ListViewItem.ListViewSubItem(rootId, reader[3].ToString());
                    ListViewItem.ListViewSubItem teamName =
                        new ListViewItem.ListViewSubItem(rootId, reader[4].ToString());
                    ListViewItem.ListViewSubItem email =
                        new ListViewItem.ListViewSubItem(rootId, reader[5].ToString());
                    ListViewItem.ListViewSubItem employmentDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[6].ToString());

                    rootId.SubItems.Add(firstName);
                    rootId.SubItems.Add(lastName);
                    rootId.SubItems.Add(role);
                    rootId.SubItems.Add(teamName);
                    rootId.SubItems.Add(email);
                    rootId.SubItems.Add(employmentDate);
                    listViewEmployees.Items.Add(rootId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (conn != null)
                    conn.Close();
            }
        }
    }
}
