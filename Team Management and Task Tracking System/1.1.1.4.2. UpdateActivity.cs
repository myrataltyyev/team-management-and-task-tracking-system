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
    public partial class UpdateActivityForm : Form
    {
        Dictionary<string, string> employee_role = new Dictionary<string, string>();
        Dictionary<string, string> employee_team = new Dictionary<string, string>();
        Dictionary<string, string> employee_id = new Dictionary<string, string>();
        Dictionary<string, string> employee_roleId = new Dictionary<string, string>();

        string activityName;
        string priority;
        string description;
        string plannedStartDate;
        string plannedEndDate;
        string actualStartDate;
        string actualEndDate;
        string employeeName;
        string employeeRole;
        string employeeTeam;

        public UpdateActivityForm(
            string activityName, 
            string priority, 
            string description,
            string plannedStartDate,
            string plannedEndDate,
            string actualStartDate,
            string actualEndDate,
            string employeeName,
            string employeeRole)
        {
            InitializeComponent();
            this.activityName = activityName;
            this.priority = priority;
            this.description = description;
            this.plannedStartDate = plannedStartDate;
            this.plannedEndDate = plannedEndDate;
            this.actualStartDate = actualStartDate;
            this.actualEndDate = actualEndDate;
            this.employeeName = employeeName;
            this.employeeRole = employeeRole;
        }

        private void UpdateActivityForm_Load(object sender, EventArgs e)
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

            // Set selected row values
            txtActivityName.Text = activityName;
            txtDescription.Text = description;
            comboBoxPriority.SelectedItem = priority;
            comboBoxEmployee.SelectedItem = employeeName;
            datePlannedStart.Text = plannedStartDate;
            datePlannedEnd.Text = plannedEndDate;
            dateActualStart.Text = actualStartDate;
            dateActualEnd.Text = actualEndDate;
            lblRole.Text = employeeRole;
            lblTeam.Text = employee_team[employeeName];

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

        private void BtnUpdate_Click(object sender, EventArgs e)
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

            string update_activity = "UPDATE activity SET activity_name = @activity_name, priority = @priority, " +
                "description = @description, planned_start_date = @planned_start_date, planned_end_date = " +
                "@planned_end_date, actual_start_time = @actual_start_time, actual_end_time = @actual_end_time " +
                "WHERE id = " + ActivitiesForm.activityId;

            string update_assigned = "UPDATE assigned SET role_id = @role_id, employee_id = @employee_id " +
                "WHERE activity_id = " + ActivitiesForm.activityId;

            MySqlConnection conn = null;
            MySqlDataReader reader = null;

            try
            {
                conn = new MySqlConnection(connetionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = update_activity;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@activity_name", activityName);
                cmd.Parameters.AddWithValue("@priority", int.Parse(priority));
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@planned_start_date", Convert.ToDateTime(plannedStartDate));
                cmd.Parameters.AddWithValue("@planned_end_date", Convert.ToDateTime(plannedEndDate));
                cmd.Parameters.AddWithValue("@actual_start_time", Convert.ToDateTime(actualStartDate));
                cmd.Parameters.AddWithValue("@actual_end_time", Convert.ToDateTime(actualEndDate));
                cmd.ExecuteNonQuery();

                cmd.CommandText = update_assigned;
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@role_id", employee_roleId[employeeName]);
                cmd.Parameters.AddWithValue("@employee_id", employee_id[employeeName]);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Updated successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
    }
}
