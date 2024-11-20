using System;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Appliances
{
    public partial class AddOper : Form
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public string type1 { get; set; }
        public string problem { get; set; }
        public string status { get; set; }
        public string client { get; set; }

        public string model { get; set; }
        static SqlConnection connection;
        SqlCommand command;

        public AddOper()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            textBox1.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            
            richTextBox1.ReadOnly = true;
            comboBox2.Enabled = false;
            comboBoxModels.Visible = true;
            comboBoxModels.Enabled = false;
            comboBoxModels.Visible = false; 
        }

        public AddOper(int id, DateTime start, string type1, string model,string problem, string status, string client)
        {
            InitializeComponent();
            this.id = id;
            this.start = start;
            this.type1 = type1;
            this.problem = problem;
            this.status = status;
            this.client = client;
            this.model = model;

            comboBox2.Text = status;
            comboBoxModels.Text = model;

            comboBoxModels.Enabled = false;

            textBox1.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;

            richTextBox1.ReadOnly = true;
            comboBox2.Enabled = false;

        }

        static public void Connect()
        {
            try
            {
                connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=Leo_bd_up;Integrated Security=True;");
                //connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=$ЯремаЗелепугин;Integrated Security=True;");
                connection.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка доступа к базе данных. Исключение: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddOper_Load(object sender, EventArgs e)
        {
            Connect();

            command = new SqlCommand("SELECT fio FROM dataUsers WHERE typeID = 1", connection); 
            using (var reader = command.ExecuteReader())
            {
                comboBoxMaster.Items.Clear();
                while (reader.Read())
                {
                    comboBoxMaster.Items.Add(reader["fio"].ToString());
                }
            }

            command = new SqlCommand("SELECT fio FROM dataUsers WHERE typeID = 2", connection); 
            using (var reader = command.ExecuteReader())
            {
                comboBoxMaster.Items.Clear(); 
                while (reader.Read())
                {
                    comboBoxMaster.Items.Add(reader["fio"].ToString());
                }
            }

            dateTimePicker1.Value = start;
            textBox1.Text = type1;

            command = new SqlCommand("SELECT carModel FROM cars WHERE carType = @type1", connection);
            command.Parameters.AddWithValue("@type1", type1);

            LoadModels();

            if (comboBoxModels.Items.Count == 0)
            {
                comboBoxModels.Visible = false;  
            }

            richTextBox1.Text = problem;
            textBox4.Text = client;
            textBox5.Text = Convert.ToString(id);

            connection.Close();
        }

        private void LoadModels()
        {

            comboBoxModels.Items.Clear();

            command = new SqlCommand("SELECT carModel FROM cars WHERE carType = @type1", connection);
            command.Parameters.AddWithValue("@type1", type1);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBoxModels.Items.Add(reader["carModel"].ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
            command = new SqlCommand("SELECT userID FROM dataUsers WHERE fio = @fullName AND typeID = 2", connection); 
            command.Parameters.AddWithValue("@fullName", comboBoxMaster.Text);  
            var resultMaster = command.ExecuteScalar();

            if (resultMaster == null)
            {
                MessageBox.Show("Мастер с таким именем не найден в базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int masterID = Convert.ToInt32(resultMaster);

            var result = MessageBox.Show("Вы точно хотите принять заказ?", "Подтверждение заказа", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
             
                command.CommandText = "UPDATE dataRequests SET requestStatusID = 1, masterID = @masterID WHERE requestID = @requestID";
                command.Parameters.Clear(); 
                command.Parameters.AddWithValue("@masterID", masterID);
                command.Parameters.AddWithValue("@requestID", id);
                command.ExecuteNonQuery();

                MessageBox.Show("Заказ успешно принят и назначен мастером!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Прием заказа отменён.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
