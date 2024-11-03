using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myProjectCTO
{
    public class RoundButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Настройка градиента
            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.LightBlue,  // Начальный цвет градиента
                Color.MediumSlateBlue, // Конечный цвет градиента
                45f)) // Угол градиента
            {
                e.Graphics.FillRectangle(gradientBrush, this.ClientRectangle);
            }

            // Настройка закругленных углов (увеличенный радиус)
            GraphicsPath path = new GraphicsPath();
            int cornerRadius = 40; // Увеличенный радиус для сильного закругления
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(this.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(this.Width - cornerRadius, this.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(0, this.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path); // Применяем округление
            e.Graphics.DrawString(this.Text, this.Font, Brushes.White, this.ClientRectangle,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}

