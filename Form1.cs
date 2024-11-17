using System;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки «Регистрация»
        private void registration_Click(object sender, EventArgs e)
        {
            // Переход на форму реєстрації
            Registration3 registrationForm = new Registration3();
            registrationForm.Show();
            this.Hide(); // Приховати поточну форму
        }

        // Обробник для кнопки «Вхід»
        private void login_Click(object sender, EventArgs e)
        {
            // Перехід на форму входу
            Login2 loginForm = new Login2();
            loginForm.Show();
            this.Hide(); // Приховати поточну форму
        }
    }
}
