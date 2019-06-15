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
    public partial class ManagerForm : Form
    {
        public static TasksForm tasksForm = null;

        public ManagerForm()
        {
            InitializeComponent();
        }

        private void BtnTasks_Click(object sender, EventArgs e)
        {
            tasksForm = new TasksForm();
            tasksForm.Show();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Closed += (s, args) => this.Close();
            loginForm.Show();
        }

        private void BtnEmployees_Click(object sender, EventArgs e)
        {
            EmployeesAndRolesForm employeesAndRolesForm = new EmployeesAndRolesForm();
            employeesAndRolesForm.Show();
        }

        private void BtnClients_Click(object sender, EventArgs e)
        {
            ClientsForm clientsForm = new ClientsForm();
            clientsForm.Show();
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
