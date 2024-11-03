using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Photo : Form
    {
        private Image[] images; // Массив для хранения изображений
        private int currentImageIndex = 0; // Текущий индекс изображения

        public Photo()
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
        }


        // Обработчик нажатия на кнопку для перелистывания вперед
        private void button1_Click(object sender, EventArgs e)
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


        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
