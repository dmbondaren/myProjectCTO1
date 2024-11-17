using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace myProjectCTO
{
    public class RoundButton : Button
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Налаштування градієнта
            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.LightBlue,  // Початковий колір градієнта
                Color.MediumSlateBlue, // Кінцевий колір градієнта
                45f)) // Кут градієнта
            {
                e.Graphics.FillRectangle(gradientBrush, this.ClientRectangle);
            }

            // Налаштування закруглених кутів (увеличений радіус)
            GraphicsPath path = new GraphicsPath();
            int cornerRadius = 40; // Увеличений радіус для сильного закруглення
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(this.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(this.Width - cornerRadius, this.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(0, this.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path); // Применяем округление

            // Налаштування шрифту для тексту кнопки (збільшений розмір та стиль жирний)
            Font largeFont = new Font(this.Font.FontFamily, 16, FontStyle.Bold); // Змінюємо розмір шрифту та стиль
            e.Graphics.DrawString(
                this.Text,
                largeFont,
                Brushes.White,
                this.ClientRectangle,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
            );
        }
    }
}
