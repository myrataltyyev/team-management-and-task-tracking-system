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
    public partial class UpdateTaskForm : Form
    {
        string taskId = TasksForm.taskId;

        public UpdateTaskForm()
        {
            InitializeComponent();
        }

        private void UpdateTaskForm_Load(object sender, EventArgs e)
        {
            // Set format of date
            datePlannedStart.Format = DateTimePickerFormat.Custom;
            datePlannedStart.CustomFormat = "dd MM yyyy";

            datePlannedEnd.Format = DateTimePickerFormat.Custom;
            datePlannedEnd.CustomFormat = "dd MM yyyy";

            dateActualStart.Format = DateTimePickerFormat.Custom;
            dateActualStart.CustomFormat = "dd MM yyyy";

            dateActualEnd.Format = DateTimePickerFormat.Custom;
            dateActualEnd.CustomFormat = "dd MM yyyy";

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

                string query = "SELECT task_name, status, priority, description, planned_budget, " +
                    "actual_budget, planned_start_date, planned_end_date, actual_start_date, actual_end_date " +
                    "FROM task_manager, task WHERE task_manager.task_id = task.id AND task_id='" + taskId + "'";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    txtTaskName.Text = reader[0].ToString();
                    comboBoxStatus.SelectedItem = reader[1].ToString();
                    comboBoxPriority.SelectedItem = reader[2].ToString();
                    txtDescription.Text = reader[3].ToString();
                    txtPlannedBudget.Text = reader[4].ToString();
                    txtActualBudget.Text = reader[5].ToString();
                    datePlannedStart.Text = reader[6].ToString();
                    datePlannedEnd.Text = reader[7].ToString();
                    dateActualStart.Text = reader[8].ToString();
                    dateActualEnd.Text = reader[9].ToString();
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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            // Get values from components
            string taskName = txtTaskName.Text;
            string status = comboBoxStatus.SelectedItem.ToString();
            string priority = comboBoxPriority.SelectedItem.ToString();
            string description = txtDescription.Text;
            string plannedStartDate = datePlannedStart.Text;
            string plannedEndDate = datePlannedEnd.Text;
            string plannedBudget = txtPlannedBudget.Text;
            string actualStartDate = dateActualStart.Text;
            string actualEndDate = dateActualEnd.Text;
            string actualBudget = txtActualBudget.Text;

            // Db connection
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            string query_update = "UPDATE task SET task_name=@task_name, priority=@priority, description=" +
                "@description, status=@status, planned_start_date=@planned_start_date, planned_end_date=" +
                "@planned_end_date, planned_budget=@planned_budget, actual_start_date=@actual_start_date, " +
                "actual_end_date=@actual_end_date, actual_budget=@actual_budget WHERE id=@id";

            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = query_update;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", taskId);
                cmd.Parameters.AddWithValue("@task_name", taskName);
                cmd.Parameters.AddWithValue("@priority", int.Parse(priority));
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@planned_start_date", Convert.ToDateTime(plannedStartDate));
                cmd.Parameters.AddWithValue("@planned_end_date", Convert.ToDateTime(plannedEndDate));
                cmd.Parameters.AddWithValue("@planned_budget", decimal.Parse(plannedBudget));
                cmd.Parameters.AddWithValue("@actual_start_date", Convert.ToDateTime(actualStartDate));
                cmd.Parameters.AddWithValue("@actual_end_date", Convert.ToDateTime(actualEndDate));
                cmd.Parameters.AddWithValue("@actual_budget", decimal.Parse(actualBudget));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Updated successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            TasksForm tasksForm = new TasksForm();
            tasksForm.Show();
            this.Close();
        }

    }
}
