using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace myProjectCTO
{
    public partial class Services5 : Form
    {
        private DataTable servicesTable;
        private List<int> selectedServices;
        private int userId;

        public Services5(int currentUserId)
        {
            InitializeComponent();
            userId = currentUserId;
            selectedServices = new List<int>();

            comboBox1.Items.AddRange(new string[] { "Ціна за зростанням", "Ціна за спаданням", "За алфавітом" });
            LoadServices();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int serviceId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ServiceID"].Value); // отримаємо ServiceID
                string serviceName = dataGridView1.Rows[e.RowIndex].Cells["ServiceName"].Value.ToString();
                decimal price = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["Price"].Value);

                // Передаємо serviceId в GetOrCreateOrder для динамічної обробки
                int orderId = GetOrCreateOrder(serviceId);
                AddOrUpdateTicket(orderId, serviceId, serviceName, price);
            }
        }


        private int GetOrCreateOrder(int serviceId)
        {
            using (var connection = new MySqlConnection("server=localhost;database=myProjectCTO;uid=root;pwd=root;"))
            {
                connection.Open();

                // Отримуємо останнє замовлення користувача, якщо воно існує
                var getOrderCmd = new MySqlCommand("SELECT OrderID FROM Orders WHERE UserID = @UserID ORDER BY OrderID DESC LIMIT 1", connection);
                getOrderCmd.Parameters.AddWithValue("@UserID", userId);
                object orderIdObj = getOrderCmd.ExecuteScalar();

                if (orderIdObj == null)
                {
                    // Якщо замовлення немає, отримуємо Details з таблиці users
                    var getUserDetailsCmd = new MySqlCommand("SELECT Details FROM Users WHERE UserID = @UserID", connection);
                    getUserDetailsCmd.Parameters.AddWithValue("@UserID", userId);
                    string userDetails = getUserDetailsCmd.ExecuteScalar() as string;

                    if (string.IsNullOrEmpty(userDetails))
                    {
                        userDetails = "Деталі замовлення відсутні";
                    }

                    // Створюємо нове замовлення, використовуючи Details з таблиці users і динамічний serviceId
                    var insertOrderCmd = new MySqlCommand("INSERT INTO Orders (UserID, ServiceID, CarBrand, AppointmentDate) VALUES (@UserID, @ServiceID, @Details, @AppointmentDate)", connection);
                    insertOrderCmd.Parameters.AddWithValue("@UserID", userId);
                    insertOrderCmd.Parameters.AddWithValue("@ServiceID", serviceId); // Використовуємо Details з users
                    insertOrderCmd.Parameters.AddWithValue("@Details", userDetails); // Використовуємо Details з users
                    insertOrderCmd.Parameters.AddWithValue("@AppointmentDate", DateTime.Now.AddDays(1));
                    insertOrderCmd.ExecuteNonQuery();

                    return (int)insertOrderCmd.LastInsertedId;
                }

                return Convert.ToInt32(orderIdObj);
            }
        }


        private void AddOrUpdateTicket(int orderId, int serviceId, string serviceName, decimal price)
        {
            using (var connection = new MySqlConnection("server=localhost;database=myProjectCTO;uid=root;pwd=root;"))
            {
                connection.Open();

                var checkTicketCmd = new MySqlCommand("SELECT Quantity, Price FROM Tickets WHERE ServiceID = @ServiceID AND OrderID = @OrderID", connection);
                checkTicketCmd.Parameters.AddWithValue("@ServiceID", serviceId);
                checkTicketCmd.Parameters.AddWithValue("@OrderID", orderId);

                var reader = checkTicketCmd.ExecuteReader();
                if (reader.Read())
                {
                    int currentQuantity = Convert.ToInt32(reader["Quantity"]);
                    reader.Close();

                    var updateTicketCmd = new MySqlCommand("UPDATE Tickets SET Quantity = @Quantity, Price = @Price WHERE ServiceID = @ServiceID AND OrderID = @OrderID", connection);
                    updateTicketCmd.Parameters.AddWithValue("@Quantity", currentQuantity + 1);
                    updateTicketCmd.Parameters.AddWithValue("@Price", price * (currentQuantity + 1));
                    updateTicketCmd.Parameters.AddWithValue("@ServiceID", serviceId);
                    updateTicketCmd.Parameters.AddWithValue("@OrderID", orderId);
                    updateTicketCmd.ExecuteNonQuery();

                    MessageBox.Show($"Кількість послуги '{serviceName}' збільшена до {currentQuantity + 1}. Нова ціна: {price * (currentQuantity + 1)}.");
                }
                else
                {
                    reader.Close();

                    var insertTicketCmd = new MySqlCommand("INSERT INTO Tickets (OrderID, ServiceID, Quantity, Price, TicketDetails) VALUES (@OrderID, @ServiceID, 1, @Price, 'Деталі квитка')", connection);
                    insertTicketCmd.Parameters.AddWithValue("@OrderID", orderId);
                    insertTicketCmd.Parameters.AddWithValue("@ServiceID", serviceId);
                    insertTicketCmd.Parameters.AddWithValue("@Price", price);
                    insertTicketCmd.ExecuteNonQuery();
                    MessageBox.Show($"Послуга '{serviceName}' додана до кошика.");
                }
            }
        }

        private void LoadServices()
        {
            using (var connection = new MySqlConnection("server=localhost;database=myProjectCTO;uid=root;pwd=root;"))
            {
                try
                {
                    connection.Open();
                    var adapter = new MySqlDataAdapter("SELECT ServiceID, ServiceName, Price FROM Services", connection);
                    servicesTable = new DataTable();
                    adapter.Fill(servicesTable);
                    dataGridView1.DataSource = servicesTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при завантаженні послуг: " + ex.Message);
                }
            }
        }

        // Возвращаем обработчик события для оформления заказа
        private void order_Click(object sender, EventArgs e)
        {
            foreach (int serviceId in selectedServices)
            {
                // Получаем или создаем заказ с использованием выбранного serviceId
                int orderId = GetOrCreateOrder(serviceId); // Передаем serviceId

                DataRow[] selectedRows = servicesTable.Select($"ServiceID = {serviceId}");
                if (selectedRows.Length == 0) continue;

                decimal price = Convert.ToDecimal(selectedRows[0]["Price"]);
                string serviceName = selectedRows[0]["ServiceName"].ToString();

                AddOrUpdateTicket(orderId, serviceId, serviceName, price); // Обновляем или добавляем услуги
            }

            MessageBox.Show("Замовлення оформлено!");

            // Переход на следующий экран после оформления заказа
            Service8 service8Form = new Service8(userId);
            service8Form.Show();
            this.Hide();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (servicesTable != null)
            {
                DataView dataView = servicesTable.DefaultView;
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "Ціна за зростанням":
                        dataView.Sort = "Price ASC";
                        break;
                    case "Ціна за спаданням":
                        dataView.Sort = "Price DESC";
                        break;
                    case "За алфавітом":
                        dataView.Sort = "ServiceName ASC";
                        break;
                }
                dataGridView1.DataSource = dataView.ToTable();
            }
        }
    }
}
