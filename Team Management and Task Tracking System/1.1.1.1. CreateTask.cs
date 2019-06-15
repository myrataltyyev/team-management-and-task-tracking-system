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
    public partial class CreateTaskForm : Form
    {
        string userId = LoginForm.userId;
        public CreateTaskForm()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
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

            string query_task = "INSERT INTO task(task_name, priority, description, status, planned_start_date," +
                    "planned_end_date, planned_budget, actual_start_date, actual_end_date, actual_budget)" +
                    "VALUES (@task_name, @priority, @description, @status, @planned_start_date, " +
                    "@planned_end_date, @planned_budget, @actual_start_date, @actual_end_date, @actual_budget)";
            string query_select = "SELECT id FROM task WHERE task_name='" + taskName + "'";
            string query_manager = "INSERT INTO task_manager(employee_id, task_id) VALUES(@employee_id, @task_id)";

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = query_task;
                cmd.Prepare();
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

                cmd = new MySqlCommand(query_select, conn);
                reader = cmd.ExecuteReader();
                string taskId = "";
                while (reader.Read())
                    taskId = reader[0].ToString();
                reader.Close();

                cmd.CommandText = query_manager;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@employee_id", 1);
                cmd.Parameters.AddWithValue("@task_id", taskId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Created successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            TasksForm tasksForm = new TasksForm();
            tasksForm.Show();
            this.Close();
        }

        private void CreateTaskForm_Load(object sender, EventArgs e)
        {
            comboBoxStatus.SelectedIndex = 0;
            comboBoxPriority.SelectedIndex = 0;

            datePlannedStart.Format = DateTimePickerFormat.Custom;
            datePlannedStart.CustomFormat = "dd MM yyyy";

            datePlannedEnd.Format = DateTimePickerFormat.Custom;
            datePlannedEnd.CustomFormat = "dd MM yyyy";

            dateActualStart.Format = DateTimePickerFormat.Custom;
            dateActualStart.CustomFormat = "dd MM yyyy";

            dateActualEnd.Format = DateTimePickerFormat.Custom;
            dateActualEnd.CustomFormat = "dd MM yyyy";
        }
    }
}
