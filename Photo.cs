using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Photo : Form
    {
        private Image[] images; // Массив для хранения изображений
        private int currentImageIndex = 0; // Текущий индекс изображения
        private Form _userInterface4;

        public Photo(Form userInterface4)
        {
            InitializeComponent();
            

            // Инициализация массива изображений с использованием изображений, уже назначенных в PictureBox
            images = new Image[]
            {
                pictureBox1.Image,
                pictureBox2.Image,
                pictureBox3.Image
            };

            // Устанавливаем начальное изображение
            pictureBox1.Image = images[currentImageIndex];

            // Добавляем закругленную кнопку с градиентом
            RoundButton button1 = new RoundButton();
            button1.Text = "Next";
            button1.Size = new Size(200, 80); // Увеличенный размер кнопки по ширине
            button1.Location = new Point(265, 363);
            button1.Click += Button1_Click; // Обработчик для нажатия на кнопку
            this.Controls.Add(button1); // Добавляем кнопку на форму
            _userInterface4 = userInterface4;
        }

        // Обработчик для нажатия на кнопку, который переключает изображение
        private void Button1_Click(object sender, EventArgs e)
        {
            currentImageIndex++;
            if (currentImageIndex >= images.Length)
            {
                currentImageIndex = 0;
            }

            // Устанавливаем текущее изображение для всех PictureBox для наложения
            pictureBox1.Image = images[currentImageIndex];
            pictureBox2.Image = images[(currentImageIndex + 1) % images.Length];
            pictureBox3.Image = images[(currentImageIndex + 2) % images.Length];

            // Настраиваем порядок наложения картинок
            pictureBox1.BringToFront();
            pictureBox2.BringToFront();
            pictureBox3.BringToFront();
        }

        private void pictureBox3_Click(object sender, EventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void pictureBox2_Click(object sender, EventArgs e) { }

        private void Photo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            _userInterface4.Visible = true;
        }
    }
}
