using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace myProjectCTO
{
    public partial class Login2 : Form
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin";

        public Login2()
        {
            InitializeComponent();

            // Скрывать пароль по умолчанию
            textBox2.PasswordChar = '*';
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim(); // textBox1 - для логина
            string password = textBox2.Text.Trim(); // textBox2 - для пароля

            // Проверка администратора
            if (username == AdminUsername && password == AdminPassword)
            {
                Admin9 adminForm = new Admin9();
                adminForm.Show();
                this.Hide(); // Скрыть текущую форму
                return;
            }

            // Хэшируем введенный пароль перед проверкой
            string passwordHash = PasswordHasher.HashPassword(password);

            string connectionString = "Server=localhost;Database=myProjectCTO;User ID=root;Password=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Изменим запрос для получения UserID
                    string query = "SELECT UserID FROM Users WHERE Nickname = @Nickname AND PasswordHash = @PasswordHash";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nickname", username);
                    command.Parameters.AddWithValue("@PasswordHash", passwordHash); // Используем хэшированный пароль

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int userId = Convert.ToInt32(result);
                        MessageBox.Show("Успішний вхід.");

                        // Переход на UserInterface4 с передачей UserID
                        UserInterface4 userForm = new UserInterface4(userId);
                        userForm.Show();
                        this.Close(); // Закрыть текущую форму
                    }
                    else
                    {
                        MessageBox.Show("Неправильний логін або пароль.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при підключенні до бази даних: " + ex.Message);
                }
            }
        }




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Если чекбокс выбран, показать пароль, иначе скрыть
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

   

      
    }
}
