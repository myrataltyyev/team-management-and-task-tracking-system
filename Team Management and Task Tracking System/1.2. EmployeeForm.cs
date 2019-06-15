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
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
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

                string query = "SELECT employee.id, first_name, last_name, username, email, employment_date, role_name, team_name " +
                    "FROM employee, role, team, team_member WHERE team_member.employee_id = " +
                    "employee.id AND team_member.role_id = role.id AND team_member.team_id = team.id AND employee.id = " + 
                    LoginForm.userId;

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Set labels
                    lblId.Text = reader[0].ToString();
                    lblName.Text = reader[1] + " " + reader[2];
                    lblUsername.Text = reader[3].ToString();
                    lblEmail.Text = reader[4].ToString();
                    lblEmpDate.Text = reader[5].ToString();
                    lblRole.Text = reader[6].ToString();
                    lblTeam.Text = reader[7].ToString();
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

            // Set titles for columns
            listViewActivities.Columns.Add("ID", 30, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Name", 200, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Priority", 60, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Planned Start Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Planned End Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Actual Start Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Actual End Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Description", 400, HorizontalAlignment.Center);

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();

                string query = "SELECT activity.id, activity_name, priority, planned_start_date, " +
                    "planned_end_date, actual_start_time, actual_end_time, activity.description FROM activity, " +
                    "assigned, role, employee WHERE activity.id = assigned.activity_id AND " +
                    "assigned.role_id = role.id AND assigned.employee_id = employee.id AND employee.id = " +
                    LoginForm.userId;

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem rootId = new ListViewItem(reader[0].ToString());
                    ListViewItem.ListViewSubItem name =
                        new ListViewItem.ListViewSubItem(rootId, reader[1].ToString());
                    ListViewItem.ListViewSubItem priority =
                        new ListViewItem.ListViewSubItem(rootId, reader[2].ToString());
                    ListViewItem.ListViewSubItem plannedStartDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[3].ToString());
                    ListViewItem.ListViewSubItem plannedEndDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[4].ToString());
                    ListViewItem.ListViewSubItem actualStartDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[5].ToString());
                    ListViewItem.ListViewSubItem actualEndDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[6].ToString());
                    ListViewItem.ListViewSubItem description =
                        new ListViewItem.ListViewSubItem(rootId, reader[7].ToString());

                    rootId.SubItems.Add(name);
                    rootId.SubItems.Add(priority);
                    rootId.SubItems.Add(plannedStartDate);
                    rootId.SubItems.Add(plannedEndDate);
                    rootId.SubItems.Add(actualStartDate);
                    rootId.SubItems.Add(actualEndDate);
                    rootId.SubItems.Add(description);
                    listViewActivities.Items.Add(rootId);
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
