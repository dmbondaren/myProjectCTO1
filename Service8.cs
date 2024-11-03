using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Service8 : Form
    {
        private int userId; // ID пользователя

        public Service8(int currentUserId)
        {
            InitializeComponent();
            userId = currentUserId; // Получаем ID пользователя
        }

        private void appointment_Click(object sender, EventArgs e)
        {
            string phone = textBox2.Text;
            string details = richTextBox1.Text;
            string carBrand = textBox3.Text;
            DateTime appointmentDate = dateTimePicker1.Value;

            string connectionString = "server=localhost;database=myProjectCTO;uid=root;pwd=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Обновляем данные заказа
                    string updateOrderQuery = "UPDATE Orders SET AppointmentDate = @AppointmentDate" +
                        (string.IsNullOrWhiteSpace(carBrand) ? "" : ", CarBrand = @CarBrand") +
                        " WHERE UserID = @UserID";

                    MySqlCommand updateOrderCmd = new MySqlCommand(updateOrderQuery, connection);
                    updateOrderCmd.Parameters.AddWithValue("@AppointmentDate", appointmentDate);

                    // Если carBrand не пустой, добавляем параметр
                    if (!string.IsNullOrWhiteSpace(carBrand))
                    {
                        updateOrderCmd.Parameters.AddWithValue("@CarBrand", carBrand);
                    }

                    updateOrderCmd.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = updateOrderCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Дані успішно оновлено.");
                        // Открываем форму Ticket6 после успешного обновления
                        Ticket6 ticketForm = new Ticket6(userId);
                        ticketForm.Show();  // Открываем новую форму
                        this.Hide();        // Скрываем текущую форму
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося знайти запис для оновлення.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні інформації: " + ex.Message);
                }
            }
        }




        private void textPhone_TextChanged(object sender, EventArgs e)
        {
            // Логика для обновления номера телефона
            string newPhone = textBox2.Text;

            if (string.IsNullOrWhiteSpace(newPhone))
            {
                MessageBox.Show("Номер телефону не може бути порожнім.");
                return;
            }

            string connectionString = "server=localhost;database=myProjectCTO;uid=root;pwd=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updatePhoneQuery = "UPDATE Users SET PhoneNumber = @PhoneNumber WHERE UserID = @UserID";
                    MySqlCommand updatePhoneCmd = new MySqlCommand(updatePhoneQuery, connection);
                    updatePhoneCmd.Parameters.AddWithValue("@PhoneNumber", newPhone);
                    updatePhoneCmd.Parameters.AddWithValue("@UserID", userId);
                    updatePhoneCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні номера телефону: " + ex.Message);
                }
            }
        }

        // Остальные обработчики событий
        private void textDetails_TextChanged(object sender, EventArgs e)
        {
            string details = richTextBox1.Text; // Отримуємо введені деталі

            string connectionString = "server=localhost;database=myProjectCTO;uid=root;pwd=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updateDetailsQuery = "UPDATE Users SET Details = @Details WHERE UserID = @UserID";
                    MySqlCommand updateDetailsCmd = new MySqlCommand(updateDetailsQuery, connection);
                    updateDetailsCmd.Parameters.AddWithValue("@Details", details);
                    updateDetailsCmd.Parameters.AddWithValue("@UserID", userId);
                    updateDetailsCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при оновленні деталей: " + ex.Message);
                }
            }
        }

        private void textCar_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
    }
}
