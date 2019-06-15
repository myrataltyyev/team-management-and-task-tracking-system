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
    public partial class ActivitiesForm : Form
    {
        public static string activityId;

        public ActivitiesForm()
        {
            InitializeComponent();
        }

        private void ActivitiesForm_Load(object sender, EventArgs e)
        {
            // Set titles for columns
            listViewActivities.Columns.Add("ID", 30, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Name", 200, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Role name", 200, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Employee name", 200, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Employee surname", 200, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Priority", 60, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Planned Start Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Planned End Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Actual Start Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Actual End Date", 150, HorizontalAlignment.Center);
            listViewActivities.Columns.Add("Description", 400, HorizontalAlignment.Center);

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

                string query = "SELECT activity.id, activity_name, role.role_name, " +
                    "employee.first_name, employee.last_name, priority, planned_start_date, " +
                    "planned_end_date, actual_start_time, actual_end_time, activity.description FROM activity, " +
                    "assigned, role, employee WHERE activity.id = assigned.activity_id AND " +
                    "assigned.role_id = role.id AND assigned.employee_id = employee.id AND task_id=" + TasksForm.taskId;

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem rootId = new ListViewItem(reader[0].ToString());

                    ListViewItem.ListViewSubItem name =
                        new ListViewItem.ListViewSubItem(rootId, reader[1].ToString());
                    ListViewItem.ListViewSubItem roleName =
                        new ListViewItem.ListViewSubItem(rootId, reader[2].ToString());
                    ListViewItem.ListViewSubItem employeeFirstName =
                        new ListViewItem.ListViewSubItem(rootId, reader[3].ToString());
                    ListViewItem.ListViewSubItem employeeLastName =
                        new ListViewItem.ListViewSubItem(rootId, reader[4].ToString());
                    ListViewItem.ListViewSubItem priority =
                        new ListViewItem.ListViewSubItem(rootId, reader[5].ToString());
                    ListViewItem.ListViewSubItem plannedStartDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[6].ToString());
                    ListViewItem.ListViewSubItem plannedEndDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[7].ToString());
                    ListViewItem.ListViewSubItem actualStartDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[8].ToString());
                    ListViewItem.ListViewSubItem actualEndDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[9].ToString());
                    ListViewItem.ListViewSubItem description =
                        new ListViewItem.ListViewSubItem(rootId, reader[10].ToString());

                    rootId.SubItems.Add(name);
                    rootId.SubItems.Add(roleName);
                    rootId.SubItems.Add(employeeFirstName);
                    rootId.SubItems.Add(employeeLastName);
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
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CreateActivityForm createActivityForm = new CreateActivityForm();
            createActivityForm.Show();
            this.Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            activityId = listViewActivities.FocusedItem.SubItems[0].Text;

            // Db connection
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            string query_delete = "DELETE FROM activity WHERE id=" + activityId;

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query_delete, conn);
                reader = cmd.ExecuteReader();

                MessageBox.Show("Deleted successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

            listViewActivities.Items.Clear();
            ActivitiesForm_Load(sender, e);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            activityId = listViewActivities.FocusedItem.SubItems[0].Text;

            UpdateActivityForm updateActivityForm = new UpdateActivityForm(
                listViewActivities.FocusedItem.SubItems[1].Text,
                listViewActivities.FocusedItem.SubItems[5].Text,
                listViewActivities.FocusedItem.SubItems[10].Text,
                listViewActivities.FocusedItem.SubItems[6].Text,
                listViewActivities.FocusedItem.SubItems[7].Text,
                listViewActivities.FocusedItem.SubItems[8].Text,
                listViewActivities.FocusedItem.SubItems[9].Text,
                listViewActivities.FocusedItem.SubItems[3].Text + " " +
                listViewActivities.FocusedItem.SubItems[4].Text,
                listViewActivities.FocusedItem.SubItems[2].Text);
            updateActivityForm.Show();
            this.Close();
        }
    }

}
