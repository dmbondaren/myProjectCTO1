using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace myProjectCTO
{
    public partial class Registration3 : Form
    {
        public Registration3()
        {

            InitializeComponent();

            // Скрывать пароль по умолчанию
            textBox3.PasswordChar = '*';
        }

        // Подія для кнопки "Зареєструватися"
        private void registration_Click(object sender, EventArgs e)
        {
            // Отримання даних з текстових полів
            string nickname = textBox1.Text.Trim(); // Нікнейм
            string email = textBox2.Text.Trim();    // Емейл
            string password = textBox3.Text.Trim(); // Пароль

            // Перевірка введених даних
            if (string.IsNullOrEmpty(nickname))
            {
                MessageBox.Show("Будь ласка, введіть нікнейм.");
                return;
            }
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Будь ласка, введіть емейл.");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Будь ласка, введіть пароль.");
                return;
            }

            // Хешуємо пароль перед збереженням у базу даних
            string passwordHash = PasswordHasher.HashPassword(password);

            // Підключення до бази даних
            string connectionString = "server=localhost;database=myProjectCTO;uid=root;pwd=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Перевірка, чи вже існує користувач з таким нікнеймом або емейлом
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Nickname = @Nickname OR Email = @Email";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@Nickname", nickname);
                    checkCommand.Parameters.AddWithValue("@Email", email);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Користувач з таким нікнеймом або емейлом вже існує.");
                        return;
                    }

                    // Додавання нового користувача в базу даних
                    string insertQuery = "INSERT INTO Users (Nickname, Email, PasswordHash) VALUES (@Nickname, @Email, @PasswordHash)";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Nickname", nickname);
                    insertCommand.Parameters.AddWithValue("@Email", email);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash); // Хешований пароль

                    insertCommand.ExecuteNonQuery();

                    MessageBox.Show("Реєстрація пройшла успішно.");

                    // Повернення на головну форму після реєстрації
                    Form1 mainForm = new Form1();
                    mainForm.Show();
                    this.Close(); // Закрити поточну форму
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при підключенні до бази даних: " + ex.Message);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Если чекбокс отмечен, показывать пароль, иначе скрывать
            if (checkBox1.Checked)
            {
                textBox3.PasswordChar = '\0'; // Пароль видим
            }
            else
            {
                textBox3.PasswordChar = '*'; // Пароль скрыт
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
