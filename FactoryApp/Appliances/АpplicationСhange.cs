using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Appliances
{
    public partial class АpplicationСhange : Form
    {
        private SqlConnection connection;

        public int ID { get; set; }
        public string TypeEquipment { get; set; }
        public string Problem { get; set; }

        public АpplicationСhange(int requestId)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.ID = requestId;
            Connect(); 
            LoadRequestDetails();
        }

        private void Connect()
        {
            try
            {
                if (connection == null || connection.State == System.Data.ConnectionState.Closed)
                {
                    //connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=$ЯремаЗелепугин;Integrated Security=True;");
                    connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=Leo_bd_up;Integrated Security=True;");
                    connection.Open();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка доступа к базе данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRequestDetails()
        {
            try
            {
                SqlCommand command = new SqlCommand(
                    "SELECT m.carType, m.carModel " +
                    "FROM dataRequests r " +
                    "JOIN cars m ON r.carID = m.carID " +
                    "WHERE r.requestID = @requestId", connection);
                command.Parameters.AddWithValue("@requestId", this.ID);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //Problem = reader["ProblemDescription"].ToString();
                        TypeEquipment = reader["carType"].ToString();
                        string model = reader["carModel"].ToString();

                        comboBox1.Text = TypeEquipment;
                        comboBox2.Text = model;
                        //richTextBox1.Text = Problem;
                    }
                }

                SqlCommand commandType = new SqlCommand("SELECT DISTINCT carType FROM cars", connection);
                using (var reader = commandType.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["carType"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                try
                {
                    SqlCommand command = new SqlCommand(
                        "SELECT carModel FROM cars WHERE carType = @orgTechType", connection);
                    command.Parameters.AddWithValue("@orgTechType", comboBox1.SelectedItem.ToString());

                    using (var reader = command.ExecuteReader())
                    {
                        comboBox2.Items.Clear();
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["carModel"].ToString());
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Ошибка при загрузке моделей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void АpplicationСhange_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Text = TypeEquipment;
                SqlCommand command = new SqlCommand($"SELECT carModel FROM cars WHERE carType = '{TypeEquipment}'", connection);
                comboBox2.Text = command.ExecuteScalar()?.ToString();
                //richTextBox1.Text = Problem;
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных на форму: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите тип оборудования и модель.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SqlCommand command = new SqlCommand(
                    "SELECT carID FROM cars WHERE carModel = @orgTechModel", connection);
                command.Parameters.AddWithValue("@orgTechModel", comboBox2.SelectedItem.ToString());
                int modelId = Convert.ToInt32(command.ExecuteScalar());

                var result = MessageBox.Show("Вы точно хотите изменить запись?", "Подтверждение изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SqlCommand updateCommand = new SqlCommand(
                        "UPDATE dataRequests SET carID = @modelId " +
                        "WHERE requestID = @requestId", connection);

                    updateCommand.Parameters.AddWithValue("@modelId", modelId);
                    //updateCommand.Parameters.AddWithValue("@problemDescription", richTextBox1.Text);
                    updateCommand.Parameters.AddWithValue("@requestId", this.ID);

                    updateCommand.ExecuteNonQuery();

                    MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Изменение записи отменено.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (connection != null && connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }
    }
}
