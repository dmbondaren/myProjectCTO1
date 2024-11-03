using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


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

            // Set the URL to the video file for the AxWindowsMediaPlayer control
            axWindowsMediaPlayer1.URL = "C:\\Users\\admin\\source\\repos\\myProjectCTO\\bin\\Debug\\video1.mp4";
            axWindowsMediaPlayer1.Ctlcontrols.play();

            // Initialize the timer and set the interval
            timer = new Timer();
            timer.Interval = 100;  // Set interval to 100 milliseconds
            timer.Tick += Timer_Tick;  // Attach the Tick event handler
            timer.Start();  // Start the timer
        }

        // Method to stop the timer
        public void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();  // Optionally, release resources if the timer is no longer needed
            }
        }

        // Event handler for the timer tick event
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Increment the elapsed time by the timer interval (100 ms)
            elapsedMilliseconds += timer.Interval;

            // Check if 14 seconds (14000 ms) have passed
            if (elapsedMilliseconds >= 14000)
            {
                StopTimer();  // Stop the timer
                this.Close();  // Close the form
                _userInterface4.Visible = true;  // Show the UserInterface4 form
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            // Code for handling when the media player control gains focus, if needed
        }
    }
}

