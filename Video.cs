using AxWMPLib;
using System;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Video : Form
    {
        private Timer timer;
        private int elapsedMilliseconds = 0;
        private Form _userInterface4;

        public Video(Form UserInterface4)
        {
            InitializeComponent();
            _userInterface4 = UserInterface4;

            // Встановлюємо шлях до відеофайлу для елемента AxWindowsMediaPlayer
            axWindowsMediaPlayer1.URL = "C:\\Users\\admin\\source\\repos\\myProjectCTO\\bin\\Debug\\video1.mp4";
            axWindowsMediaPlayer1.Ctlcontrols.play();

            // Ініціалізуємо таймер і встановлюємо інтервал
            timer = new Timer();
            timer.Interval = 100;  // Встановлюємо інтервал 100 мілісекунд
            timer.Tick += Timer_Tick;  // Додаємо обробник події Tick
            timer.Start();  // Запускаємо таймер
        }

        // Метод для зупинки таймера
        public void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();  // Опціонально, вивільняємо ресурси, якщо таймер більше не потрібен
            }
        }

        // Обробник події для таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Збільшуємо час, що пройшов, на інтервал таймера (100 мс)
            elapsedMilliseconds += timer.Interval;

            // Перевіряємо, чи минуло 14 секунд (14000 мс)
            if (elapsedMilliseconds >= 14000)
            {
                StopTimer();  // Зупиняємо таймер
                this.Close();  // Закриваємо форму
                _userInterface4.Visible = true;  // Показуємо форму UserInterface4
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            // Код для обробки події при отриманні фокусу елементом медіаплеєра, якщо потрібно
        }
    }
}
