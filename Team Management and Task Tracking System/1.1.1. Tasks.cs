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
    public partial class TasksForm : Form
    {
        string userId = LoginForm.userId;
        public static string taskId = "";
        public TasksForm()
        {
            InitializeComponent();
        }

        private void TasksForm_Load(object sender, EventArgs e)
        {
            // Set titles for columns
            listViewTasks.Columns.Add("ID", 30, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Name", 200, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Priority", 60, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Description", 400, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Status", 60, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Planned Start Date", 150, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Planned End Date", 150, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Planned Budget", 130, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Actual Start Date", 150, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Actual End Date", 150, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Actual Budget", 130, HorizontalAlignment.Center);

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

                string query = "SELECT task.id, task_name, priority, description, status, planned_start_date, " +
                    "planned_end_date, planned_budget, actual_start_date, actual_end_date, actual_budget " +
                    "FROM task_manager, task WHERE task_manager.task_id = task.id AND task_manager.employee_id = " + 
                    userId;

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem rootId = new ListViewItem(reader[0].ToString());
                    ListViewItem.ListViewSubItem name = 
                        new ListViewItem.ListViewSubItem(rootId, reader[1].ToString());
                    ListViewItem.ListViewSubItem priority = 
                        new ListViewItem.ListViewSubItem(rootId, reader[2].ToString());
                    ListViewItem.ListViewSubItem description =
                        new ListViewItem.ListViewSubItem(rootId, reader[3].ToString());
                    ListViewItem.ListViewSubItem status =
                        new ListViewItem.ListViewSubItem(rootId, reader[4].ToString());
                    ListViewItem.ListViewSubItem plannedStartDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[5].ToString());
                    ListViewItem.ListViewSubItem plannedEndDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[6].ToString());
                    ListViewItem.ListViewSubItem plannedBudget =
                        new ListViewItem.ListViewSubItem(rootId, reader[7].ToString());
                    ListViewItem.ListViewSubItem actualStartDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[8].ToString());
                    ListViewItem.ListViewSubItem actualEndDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[9].ToString());
                    ListViewItem.ListViewSubItem actualBudget =
                        new ListViewItem.ListViewSubItem(rootId, reader[10].ToString());

                    rootId.SubItems.Add(name);
                    rootId.SubItems.Add(priority);
                    rootId.SubItems.Add(description);
                    rootId.SubItems.Add(status);
                    rootId.SubItems.Add(plannedStartDate);
                    rootId.SubItems.Add(plannedEndDate);
                    rootId.SubItems.Add(plannedBudget);
                    rootId.SubItems.Add(actualStartDate);
                    rootId.SubItems.Add(actualEndDate);
                    rootId.SubItems.Add(actualBudget);
                    listViewTasks.Items.Add(rootId);
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

        private void BtnActivities_Click(object sender, EventArgs e)
        {
            if (listViewTasks.FocusedItem != null)
            {
                taskId = listViewTasks.FocusedItem.SubItems[0].Text;

                ActivitiesForm activitiesForm = new ActivitiesForm();
                activitiesForm.Show();
            }
            else
            {
                MessageBox.Show("Please select one item", "Attention", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CreateTaskForm createTaskForm = new CreateTaskForm();
            createTaskForm.Show();
            this.Close();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            taskId = listViewTasks.FocusedItem.SubItems[0].Text;

            UpdateTaskForm updateTaskForm = new UpdateTaskForm();
            updateTaskForm.Show();
            this.Close();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            taskId = listViewTasks.FocusedItem.SubItems[0].Text;

            // Db connection
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            string query_delete = "DELETE FROM task WHERE id=" + taskId;

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

            listViewTasks.Items.Clear();
            TasksForm_Load(sender, e);
        }
    }
}
