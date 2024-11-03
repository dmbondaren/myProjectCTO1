using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
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

        // Метод для загрузки и отображения данных о заказе
        private void LoadAndDisplayOrderDetails()
        {
            DataTable orderDetails = LoadOrderDetails(); // Загружаем данные о заказе
            decimal totalPrice = CalculateTotalPrice(orderDetails); // Считаем итоговую цену
            Bitmap orderImage = GenerateOrderImage(orderDetails, totalPrice); // Генерируем изображение на основе данных

            // Отображаем изображение в PictureBox
            pictureBox1.Image = orderImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            // Отображаем итоговую цену в labelTotalPrice
            labelTotalPrice.Text = $"Итоговая цена: {totalPrice:C}";
        }

        // Метод для загрузки информации о заказе
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
                        ORDER BY o.OrderID DESC"; // Убираем LIMIT 1, чтобы получить все заказы

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

        // Метод для создания изображения на основе данных заказа
        private Bitmap GenerateOrderImage(DataTable orderDetails, decimal totalPrice)
        {
            int width = 600;
            int height = 400;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White); // Задаем цвет фона

            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;

            int yPosition = 10; // Начальная позиция по Y

            // Проверяем, есть ли данные
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

                // Отображаем информацию о каждой услуге
                g.DrawString("Выбранные услуги:", new Font("Arial", 14, FontStyle.Bold), brush, new PointF(10, yPosition));
                yPosition += 30;

                // Обрабатываем каждую услугу
                foreach (DataRow serviceRow in orderDetails.Rows)
                {
                    string serviceName = serviceRow["ServiceName"].ToString();
                    int quantity = Convert.ToInt32(serviceRow["Quantity"]);
                    decimal price = Convert.ToDecimal(serviceRow["Price"]);

                    // Отображаем услугу, количество и цену за единицу
                    g.DrawString($"Услуга: {serviceName}, Количество: {quantity}, Цена за единицу: {price:C}", font, brush, new PointF(10, yPosition));
                    yPosition += 30;
                }

                // Отображаем итоговую цену
                g.DrawString($"Итоговая цена: {totalPrice:C}", new Font("Arial", 14, FontStyle.Bold), brush, new PointF(10, yPosition));
            }
            else
            {
                g.DrawString("Данные о заказе не найдены.", font, brush, new PointF(10, yPosition));
            }

            return bitmap;
        }

        // Метод для подсчета итоговой цены
        private decimal CalculateTotalPrice(DataTable orderDetails)
        {
            decimal totalPrice = 0;

            foreach (DataRow row in orderDetails.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                decimal price = Convert.ToDecimal(row["Price"]);

                // Умножаем цену на количество для каждой услуги
                totalPrice += price * quantity; // Считаем общую стоимость для каждой услуги
            }

            return totalPrice;
        }
    }
}
