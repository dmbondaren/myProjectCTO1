using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Photo : Form
    {
        private Image[] images; // Масив для зберігання зображень
        private int currentImageIndex = 0; // Поточний індекс зображення
        private Form _userInterface4;

        public Photo(Form userInterface4)
        {
            InitializeComponent();

            // Ініціалізація масиву зображень із використанням зображень, вже призначених у PictureBox
            images = new Image[]
            {
                pictureBox1.Image,
                pictureBox2.Image,
                pictureBox3.Image
            };

            // Встановлюємо початкове зображення
            pictureBox1.Image = images[currentImageIndex];

            // Додаємо закруглену кнопку з градієнтом
            RoundButton button1 = new RoundButton();
            button1.Text = "Next";
            button1.Size = new Size(200, 80); // Збільшений розмір кнопки по ширині
            button1.Location = new Point(265, 363);
            button1.Click += Button1_Click; // Обробник для натискання на кнопку
            this.Controls.Add(button1); // Додаємо кнопку на форму
            _userInterface4 = userInterface4;
        }

        // Обробник для натискання на кнопку, який переключає зображення
        private void Button1_Click(object sender, EventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex >= images.Length)
            {
                currentImageIndex = 0;
            }

            // Встановлюємо поточне зображення для всіх PictureBox для накладання
            pictureBox1.Image = images[currentImageIndex];
            pictureBox2.Image = images[(currentImageIndex + 1) % images.Length];
            pictureBox3.Image = images[(currentImageIndex + 2) % images.Length];

            // Налаштовуємо порядок накладання картинок
            pictureBox1.BringToFront();
            pictureBox2.BringToFront();
            pictureBox3.BringToFront();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            _userInterface4.Visible = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void pictureBox2_Click(object sender, EventArgs e) { }

        private void Photo_Load(object sender, EventArgs e)
        {

        }
    }
}
