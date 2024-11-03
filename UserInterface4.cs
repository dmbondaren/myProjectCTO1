using System;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class UserInterface4 : Form
    {
        private int userId; // Хранит ID пользователя

        public UserInterface4(int currentUserId)
        {
            InitializeComponent();
            userId = currentUserId; // Получаем текущий ID пользователя
        }

        // Обработчик для кнопки "Послуги"
        private void services_Click(object sender, EventArgs e)
        {
            Services5 servicesForm = new Services5(userId); // Передаем ID пользователя
            servicesForm.Show();
            this.Hide();
        }

        // Обработчик для кнопки "Переваги"
        private void advantages_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Тут буде інформація про переваги.");
        }
        // Добавляем обработчик для кнопки, которая будет открывать форму "Photo"
        private void openPhotoForm_Click(object sender, EventArgs e)
        {
            Photo photoForm = new Photo(); // Создаем объект формы Photo
            photoForm.Show(); // Показываем форму Photo
            this.Hide(); // Скрываем текущую форму
        }

      
    }
}
