using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Appliances
{
    public partial class Customer : Form
    {
        private static SqlConnection connection;
        private SqlCommand command;
        public int UserId { get; set; }

        public Customer(int userId)
        {
            InitializeComponent();
            this.UserId = userId;
            StartPosition = FormStartPosition.CenterScreen;
            LoadUserInfo();
            LoadRequests();
        }

        private static void Connect()
        {
            try
            {
                connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=Leo_bd_up;Integrated Security=True;");
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }

        private void LoadUserInfo()
        {
            Connect();

            string userInfoQuery = @"
                SELECT fio, phone, type 
                    FROM dataUsers 
                    JOIN type ON dataUsers.typeID = type.typeID 
                    WHERE userID = @UserId";

            try
            {
                using (command = new SqlCommand(userInfoQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", this.UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label3.Text = reader["fio"].ToString();
                            label5.Text = reader["phone"].ToString();
                            label6.Text = reader["type"].ToString();
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка получения данных о пользователе: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void LoadRequests()
        {
            Connect();

            string requestsQuery = @"
        SELECT 
            r.requestID, 
            r.startDate, 
            m.carID AS Model, 
            r.problemID, 
            s.requestStatusID AS RequestStatus
        FROM 
            dataRequests r
        LEFT JOIN 
            status s ON r.requestStatusID = s.requestStatusID
        LEFT JOIN 
            cars m ON r.carID = m.carID
        WHERE 
            r.clientID = @UserId";

            try
            {
                using (var command = new SqlCommand(requestsQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", this.UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string[]> data = new List<string[]>();

                        while (reader.Read())
                        {
                            string[] row = new string[6];
                            row[0] = reader["requestID"].ToString();
                            row[1] = Convert.ToDateTime(reader["startDate"]).ToString("yyyy-MM-dd");
                            row[2] = reader["Model"].ToString();
                            row[3] = reader["problemID"].ToString();
                            row[4] = reader["RequestStatus"].ToString();

                            data.Add(row);
                        }

                        dataGridView1.Rows.Clear();
                        foreach (string[] row in data)
                        {
                            dataGridView1.Rows.Add(row);
                        }

                        int totalRecords = data.Count;
                        label4.Text = $"Количество записей: {totalRecords}";
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка получения данных заявок: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        private void buttonAddRequest_Click(object sender, EventArgs e)
        {
            АpplicationAdd applicationAdd = new АpplicationAdd(UserId);
            applicationAdd.Show();
            LoadRequests();
        }

        private void buttonChangeRequest_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int requestId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                string typeEquipment = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                string problem = dataGridView1.CurrentRow.Cells[3].Value.ToString();

                АpplicationСhange applicationChange = new АpplicationСhange(requestId);
                applicationChange.ShowDialog();
                LoadRequests();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для изменения.");
            }
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
            LoadRequests();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
            Authorization authorization = new Authorization();
            authorization.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            АpplicationAdd аpplicationAdd = new АpplicationAdd(UserId);
            аpplicationAdd.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadRequests();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int requestId = Convert.ToInt32(selectedRow.Cells[0].Value);

                string typeEquipment = GetTypeEquipmentForRequest(requestId);
                string problem = GetProblemForRequest(requestId);

                АpplicationСhange updateForm = new АpplicationСhange(requestId);
                updateForm.ShowDialog();

                LoadRequests();
            }
        }

        private string GetTypeEquipmentForRequest(int requestId)
        {
            string typeEquipment = string.Empty;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string query = @"
            SELECT m.carType 
            FROM dataRequests r 
            JOIN cars m ON r.carID = m.carID 
            WHERE r.requestID = @RequestId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestId", requestId);

                typeEquipment = command.ExecuteScalar()?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении типа оборудования: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return typeEquipment;
        }

        private string GetProblemForRequest(int requestId)
        {
            string problem = string.Empty;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                string query = @"
            SELECT problemID 
            FROM dataRequests 
            WHERE requestID = @RequestId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestId", requestId);

                problem = command.ExecuteScalar()?.ToString();

                if (problem == null)
                {
                    MessageBox.Show($"Описание проблемы для запроса {requestId} не найдено.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка SQL при получении описания проблемы: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return problem;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadRequests(textBox1.Text);
        }

        private void LoadRequests(string filter = "")
        {
            Connect();

            string requestsQuery = @"
    SELECT 
            r.requestID, 
            r.startDate, 
            m.carID AS Model, 
            r.problemID, 
            s.requestStatusID AS RequestStatus
        FROM 
            dataRequests r
        LEFT JOIN 
            status s ON r.requestStatusID = s.requestStatusID
        LEFT JOIN 
            cars m ON r.carID = m.carID
        WHERE 
            r.clientID = @UserId 
        AND (
            r.requestID LIKE @Filter OR
            r.startDate LIKE @Filter OR
            m.carID LIKE @Filter OR
            r.problemID LIKE @Filter OR
            s.requestStatusID LIKE @Filter
        )";

            try
            {
                using (var command = new SqlCommand(requestsQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", this.UserId);
                    command.Parameters.AddWithValue("@Filter", "%" + filter + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string[]> data = new List<string[]>();

                        while (reader.Read())
                        {
                            string[] row = new string[6];
                            row[0] = reader["requestID"].ToString();
                            row[1] = Convert.ToDateTime(reader["startDate"]).ToString("yyyy-MM-dd");
                            row[2] = reader["Model"].ToString();
                            row[3] = reader["problemID"].ToString();
                            row[4] = reader["RequestStatus"].ToString();

                            data.Add(row);
                        }

                        dataGridView1.Rows.Clear();
                        foreach (string[] row in data)
                        {
                            dataGridView1.Rows.Add(row);
                        }

                        int totalRecords = data.Count;
                        label4.Text = $"Количество записей: {totalRecords}";
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка получения данных заявок: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
