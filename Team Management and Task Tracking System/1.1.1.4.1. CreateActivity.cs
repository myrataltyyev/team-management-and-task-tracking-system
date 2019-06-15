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
    public partial class CreateActivityForm : Form
    {
        Dictionary<string, string> employee_role = new Dictionary<string, string>();
        Dictionary<string, string> employee_team = new Dictionary<string, string>();
        Dictionary<string, string> employee_id = new Dictionary<string, string>();
        Dictionary<string, string> employee_roleId = new Dictionary<string, string>();
        public CreateActivityForm()
        {
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            // Get values from components
            string activityName = txtActivityName.Text;
            string priority = comboBoxPriority.SelectedItem.ToString();
            string description = txtDescription.Text;
            string plannedStartDate = datePlannedStart.Text;
            string plannedEndDate = datePlannedEnd.Text;
            string actualStartDate = dateActualStart.Text;
            string actualEndDate = dateActualEnd.Text;
            string employeeName = comboBoxEmployee.SelectedItem.ToString();

            // Db connection
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            string create_activity = "INSERT INTO activity(activity_name, task_id, priority, description, " +
                "planned_start_date, planned_end_date, actual_start_time, actual_end_time) VALUES(@activity_name, " +
                "@task_id, @priority, @description, @planned_start_date, @planned_end_date, @actual_start_time, " +
                "@actual_end_time)";

            string create_assigned = "INSERT INTO assigned(activity_id, role_id, employee_id) " +
                "VALUES(@activity_id, @role_id, @employee_id)";

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = create_activity;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@activity_name", activityName);
                cmd.Parameters.AddWithValue("@task_id", TasksForm.taskId);
                cmd.Parameters.AddWithValue("@priority", int.Parse(priority));
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@planned_start_date", Convert.ToDateTime(plannedStartDate));
                cmd.Parameters.AddWithValue("@planned_end_date", Convert.ToDateTime(plannedEndDate));
                cmd.Parameters.AddWithValue("@actual_start_time", Convert.ToDateTime(actualStartDate));
                cmd.Parameters.AddWithValue("@actual_end_time", Convert.ToDateTime(actualEndDate));
                cmd.ExecuteNonQuery();

                // get last inserted activities id
                long activityId = cmd.LastInsertedId;

                cmd.CommandText = create_assigned;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@activity_id", activityId);
                cmd.Parameters.AddWithValue("@role_id", employee_roleId[employeeName]);
                cmd.Parameters.AddWithValue("@employee_id", employee_id[employeeName]);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Created successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ActivitiesForm activitiesForm = new ActivitiesForm();
                activitiesForm.Show();
                this.Close();
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

        private void ComboBoxEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblRole.Text = employee_role[comboBoxEmployee.SelectedItem.ToString()];
            lblTeam.Text = employee_team[comboBoxEmployee.SelectedItem.ToString()];
        }

        private void CreateActivityForm_Load(object sender, EventArgs e)
        {
            // Db connection
            string server = "server = localhost;";
            string database = "database = team_management;";
            string user = "user = root;";
            string pass = "password = root;";
            string connetionString = server + database + user + pass;

            string query_employee = "SELECT first_name, last_name, role_name, team_name, employee.id, role.id " +
                "FROM team_member, employee, role, team " +
                "WHERE employee.id = team_member.employee_id " +
                "AND role.id = team_member.role_id " +
                "AND team.id = team_member.team_id";

            string query_task = "SELECT task_name FROM task";

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query_employee, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employee_role[reader[0] + " " + reader[1]] = reader[2].ToString();
                    employee_team[reader[0] + " " + reader[1]] = reader[3].ToString();
                    employee_id[reader[0] + " " + reader[1]] = reader[4].ToString();
                    employee_roleId[reader[0] + " " + reader[1]] = reader[5].ToString();
                }
                reader.Close();

                cmd = new MySqlCommand(query_task, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxTask.Items.Add(reader[0].ToString());
                }

                for (int i = 0; i < employee_role.Count; i++)
                {
                    comboBoxEmployee.Items.Add(employee_role.Keys.ElementAt(i));
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

            comboBoxTask.SelectedIndex = 0;
            comboBoxPriority.SelectedIndex = 0;
            comboBoxEmployee.SelectedIndex = 0;
            lblRole.Text = employee_role.Values.ElementAt(0);
            lblTeam.Text = employee_team.Values.ElementAt(0);

            // Set format of date
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
