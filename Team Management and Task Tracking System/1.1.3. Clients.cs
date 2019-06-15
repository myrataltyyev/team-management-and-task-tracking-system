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
    public partial class ClientsForm : Form
    {
        public ClientsForm()
        {
            InitializeComponent();
        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            // Set titles for columns
            listViewClients.Columns.Add("ID", 30, HorizontalAlignment.Center);
            listViewClients.Columns.Add("Client Name", 200, HorizontalAlignment.Center);
            listViewClients.Columns.Add("Address", 300, HorizontalAlignment.Center);
            listViewClients.Columns.Add("Details", 300, HorizontalAlignment.Center);
            listViewClients.Columns.Add("Task name", 150, HorizontalAlignment.Center);
            listViewClients.Columns.Add("Start date", 150, HorizontalAlignment.Center);
            listViewClients.Columns.Add("End date", 150, HorizontalAlignment.Center);
            listViewClients.Columns.Add("Description", 300, HorizontalAlignment.Center);

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

                string query = "SELECT client.id, client_name, client_address, client_details, task_name, date_start, " +
                    "date_end, on_task.description FROM client, on_task, task WHERE client.id = on_task.client_id AND " +
                    "on_task.task_id = task.id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ListViewItem rootId = new ListViewItem(reader[0].ToString());
                    ListViewItem.ListViewSubItem clientName =
                        new ListViewItem.ListViewSubItem(rootId, reader[1].ToString());
                    ListViewItem.ListViewSubItem address =
                        new ListViewItem.ListViewSubItem(rootId, reader[2].ToString());
                    ListViewItem.ListViewSubItem details =
                        new ListViewItem.ListViewSubItem(rootId, reader[3].ToString());
                    ListViewItem.ListViewSubItem taskName =
                        new ListViewItem.ListViewSubItem(rootId, reader[4].ToString());
                    ListViewItem.ListViewSubItem startDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[5].ToString());
                    ListViewItem.ListViewSubItem endDate =
                        new ListViewItem.ListViewSubItem(rootId, reader[6].ToString());
                    ListViewItem.ListViewSubItem description =
                        new ListViewItem.ListViewSubItem(rootId, reader[7].ToString());

                    rootId.SubItems.Add(clientName);
                    rootId.SubItems.Add(address);
                    rootId.SubItems.Add(details);
                    rootId.SubItems.Add(taskName);
                    rootId.SubItems.Add(startDate);
                    rootId.SubItems.Add(endDate);
                    rootId.SubItems.Add(description);
                    listViewClients.Items.Add(rootId);
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
