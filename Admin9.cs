using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace myProjectCTO
{
    public partial class Admin9 : Form
    {
        public Admin9()
        {
            InitializeComponent();
        }

        // Load user count into DataGridView
        private void numberOfUsers_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=myProjectCTO;User ID=root;Password=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) AS UserCount FROM Users";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Виведення результату у DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при підключенні до бази даних: " + ex.Message);
                }
            }
        }

        // Load user details into DataGridView
        private void checkTickets_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Database=myProjectCTO;User ID=root;Password=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT u.Nickname, u.Email, u.Details
                    FROM Users u"; // Теперь выбираем детали пользователей

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable; // Виведення результату у DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при підключенні до бази даних: " + ex.Message);
                }
            }
        }
    }
}
