using System;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class UserInterface4 : Form
    {
        private int userId; // Хранит ID пользователя
        UserInterface4 _userInterface4;
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
            Photo photoForm = new Photo(this); // Создаем объект формы Photo
            photoForm.Show(); // Показываем форму Photo
            Visible = false; // Скрываем текущую форму
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Video videoForm = new Video(this); // Создаем объект формы Video
            videoForm.Show(); // Показываем форму Video
            Visible = false; // Скрываем текущую форму
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
