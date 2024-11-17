using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace myProjectCTO
{
    public partial class Ticket6 : Form
    {
        private int userId;
        private Label labelTotalPrice; // Label для отображения итоговой цены

        public Ticket6(int currentUserId)
        {
            InitializeComponent();
            userId = currentUserId;

            // Добавляем labelTotalPrice программно
            labelTotalPrice = new Label();
            labelTotalPrice.Location = new Point(10, 350); // Устанавливаем позицию
            labelTotalPrice.Size = new Size(300, 30); // Устанавливаем размер
            labelTotalPrice.Font = new Font("Arial", 12, FontStyle.Bold);
            this.Controls.Add(labelTotalPrice); // Добавляем на форму

            LoadAndDisplayOrderDetails();
        }

        // Метод для завантаження і відображення даних про замовлення
        private void LoadAndDisplayOrderDetails()
        {
            DataTable orderDetails = LoadOrderDetails(); // Завантажуємо дані про замовлення
            decimal totalPrice = CalculateTotalPrice(orderDetails); // Рахуємо підсумкову ціну
            Bitmap orderImage = GenerateOrderImage(orderDetails, totalPrice); // Генеруємо зображення на основі даних

            // Відображаємо зображення в PictureBox
            pictureBox1.Image = orderImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            // Відображаємо підсумкову ціну в labelTotalPrice
            labelTotalPrice.Text = $"Итоговая цена: {totalPrice:C}";
        }

        // Метод для завантаження інформації про замовлення
        private DataTable LoadOrderDetails()
        {
            DataTable orderDetailsTable = new DataTable();

            string connectionString = "server=localhost;database=myProjectCTO;uid=root;pwd=root;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string queryOrder = @"
                        SELECT o.AppointmentDate, o.CarBrand, u.Nickname, u.PhoneNumber, u.Details, 
                               t.Price, t.Quantity, s.ServiceName
                        FROM Orders o
                        JOIN Users u ON o.UserID = u.UserID
                        JOIN Tickets t ON t.OrderID = o.OrderID
                        JOIN Services s ON s.ServiceID = t.ServiceID
                        WHERE o.UserID = @UserID
                        ORDER BY o.OrderID DESC"; // Прибираємо LIMIT 1, щоб отримати всі замовлення

                    MySqlCommand cmdOrder = new MySqlCommand(queryOrder, connection);
                    cmdOrder.Parameters.AddWithValue("@UserID", userId);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdOrder);
                    adapter.Fill(orderDetailsTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }

            return orderDetailsTable;
        }

        // Метод для створення зображення на основі даних замовлення
        private Bitmap GenerateOrderImage(DataTable orderDetails, decimal totalPrice)
        {
            int width = 600;
            int height = 400;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White); // Задаємо колір фону

            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;

            int yPosition = 10; // Початкова позиція за Y

            // Перевіряємо, чи є дані
            if (orderDetails.Rows.Count > 0)
            {
                DataRow row = orderDetails.Rows[0];

                g.DrawString("Информация о заказе:", new Font("Arial", 16, FontStyle.Bold), brush, new PointF(10, yPosition));
                yPosition += 40;
                g.DrawString("Имя: " + row["Nickname"], font, brush, new PointF(10, yPosition));
                yPosition += 30;
                g.DrawString("Телефон: " + row["PhoneNumber"], font, brush, new PointF(10, yPosition));
                yPosition += 30;
                g.DrawString("Дата записи: " + Convert.ToDateTime(row["AppointmentDate"]).ToString("dd.MM.yyyy"), font, brush, new PointF(10, yPosition));
                yPosition += 30;
                g.DrawString("Марка авто: " + row["CarBrand"], font, brush, new PointF(10, yPosition));
                yPosition += 30;
                g.DrawString("Дополнительные детали: " + row["Details"], font, brush, new PointF(10, yPosition));
                yPosition += 30;

                // Відображаємо інформацію про кожну послугу
                g.DrawString("Выбранные услуги:", new Font("Arial", 14, FontStyle.Bold), brush, new PointF(10, yPosition));
                yPosition += 30;

                // Обробляємо кожну послугу
                foreach (DataRow serviceRow in orderDetails.Rows)
                {
                    string serviceName = serviceRow["ServiceName"].ToString();
                    int quantity = Convert.ToInt32(serviceRow["Quantity"]);
                    decimal price = Convert.ToDecimal(serviceRow["Price"]);

                    // Відображаємо послугу, кількість і ціну за одиницю
                    g.DrawString($"Услуга: {serviceName}, Количество: {quantity}, Цена за единицу: {price:C}", font, brush, new PointF(10, yPosition));
                    yPosition += 30;
                }

                // Відображаємо підсумкову ціну
                g.DrawString($"Итоговая цена: {totalPrice:C}", new Font("Arial", 14, FontStyle.Bold), brush, new PointF(10, yPosition));
            }
            else
            {
                g.DrawString("Данные о заказе не найдены.", font, brush, new PointF(10, yPosition));
            }

            return bitmap;
        }


        // Метод для підрахунку підсумкової ціни
        private decimal CalculateTotalPrice(DataTable orderDetails)
        {
            decimal totalPrice = 0;

            // Використовуємо HashSet для зберігання унікальних послуг та їхньої кількості
            Dictionary<string, decimal> servicePrices = new Dictionary<string, decimal>();

            foreach (DataRow row in orderDetails.Rows)
            {
                string serviceName = row["ServiceName"].ToString();
                int quantity = Convert.ToInt32(row["Quantity"]);
                decimal price = Convert.ToDecimal(row["Price"]);

                // Якщо послуга вже існує, збільшуємо кількість, але не ціну
                if (servicePrices.ContainsKey(serviceName))
                {
                    servicePrices[serviceName] += price; // Додаємо до загальної ціни за одиницю послуги
                }
                else
                {
                    servicePrices.Add(serviceName, price); // Додаємо нову послугу з її ціною
                }
            }

            // Тепер рахуємо загальну вартість на основі зібраних даних
            foreach (var service in servicePrices)
            {
                // Рахуємо підсумкову ціну як ціна за одиницю * кількість
                totalPrice += service.Value * orderDetails.Rows.Cast<DataRow>()
                                  .Where(row => row["ServiceName"].ToString() == service.Key)
                                  .Sum(row => Convert.ToInt32(row["Quantity"]));
            }

            return totalPrice;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
