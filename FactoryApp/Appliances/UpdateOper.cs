using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Appliances
{
    public partial class UpdateOper : Form
    {
        public int id { get; set; }

        static SqlConnection connection;
        SqlCommand command;
        public UpdateOper()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox6.ReadOnly = true;
            richTextBox1.ReadOnly = true;
        }
        public UpdateOper(int id) : this()
        {
            this.id = id;
          
        }
        static public void Connect()
        {
            try
            {
                //connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=$ЯремаЗелепугин;Integrated Security=True;");
                connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=Leo_bd_up;Integrated Security=True;");
                connection.Open();
            }
            catch (SqlException ex)
            { Console.WriteLine($"Ощибка доступа к базе данных. Исключение: {ex.Message}"); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Connect();

                string updateQuery = $@"
            UPDATE dataRequests 
            SET 
                requestStatusID = {2}, 
                completionDate = '{dateTimePicker2.Value}' 
            WHERE requestID = {id}";

                command = new SqlCommand(updateQuery, connection);
                command.ExecuteNonQuery();

                MessageBox.Show("Заявка обновлена успешно.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка обновления заявки: {ex.Message}");
            }
        }

        private void UpdateOper_Load(object sender, EventArgs e)
        {
            Connect();

            try
            {
                string query = @"
            SELECT 
                r.startDate,               -- Дата создания
                h.carType,             -- Тип оборудования
                h.carModel,            -- Модель техники
                r.problemID,      -- Проблема
                u.fio AS MasterFullName, -- ФИО мастера
                r.completionDate,          -- Дата выполнения
                p.repairParts,          -- Необходимые детали
                ud.fio AS ClientFullName, -- Клиент
                c.message AS Comment       -- Комментарий
            FROM 
                dataRequests r
            LEFT JOIN 
                cars h ON r.carID = h.carID
            LEFT JOIN 
                dataUsers u ON r.masterID = u.userID
            LEFT JOIN 
                dataUsers ud ON r.clientID = ud.userID
            LEFT JOIN 
                repairParts p ON r.repairPartsID = p.repairPartsID
            LEFT JOIN 
                dataComments c ON r.requestID = c.requestID AND r.masterID = c.masterID
            WHERE 
                r.requestID = @RequestID";

                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestID", id); 

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dateTimePicker1.Value = Convert.ToDateTime(reader["StartDate"]); 
                        textBox1.Text = reader["carType"].ToString(); 
                        textBox2.Text = reader["carModel"].ToString(); 
                        richTextBox1.Text = reader["problemID"].ToString(); 
                        textBox5.Text = reader["MasterFullName"].ToString(); 
                        textBox4.Text = reader["repairParts"] != DBNull.Value ? reader["repairParts"].ToString() : ""; 
                        textBox6.Text = reader["ClientFullName"].ToString();
                        textBox7.Text = reader["Comment"] != DBNull.Value ? reader["Comment"].ToString() : ""; 

                        if (reader["completionDate"] != DBNull.Value)
                        {
                            dateTimePicker2.Value = Convert.ToDateTime(reader["completionDate"]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данные не найдены для данной заявки.");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
