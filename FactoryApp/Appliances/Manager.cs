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

namespace Appliances
{
    public partial class Manager : Form
    {
        private static SqlConnection connection;
        private SqlCommand command;
        private int userId; 

        public Manager(int userId)
        {
            InitializeComponent();
            dataGridViewRequests.DefaultCellStyle.Font = new Font("Arial", 12);
            dataGridViewRequests.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12);

            StartPosition = FormStartPosition.CenterScreen;
            this.userId = userId;
            LoadUserInfo();
            LoadRequests();
        }

        static private void Connect()
        {
            try
            {
                connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=Leo_bd_up;Integrated Security=True;");
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
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
                    command.Parameters.AddWithValue("@UserId", this.userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label1.Text = reader["fio"].ToString();
                            label2.Text = reader["phone"].ToString(); 
                            label3.Text = reader["type"].ToString(); 
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

            string query = @"
                 SELECT 
        r.requestID, 
        r.startDate, 
        h.carType AS ModelType, 
        h.carModel,
        r.problemID, 
        s.requestStatus, 
        r.completionDate, 
        p.repairParts, 
        u.fio AS MasterFullName,  
        ud.fio AS ClientFullName,  
        c.message AS MasterComment  
    FROM dataRequests r 
    INNER JOIN cars h ON r.carID = h.carID
    LEFT JOIN status s ON r.requestStatusID = s.requestStatusID
    LEFT JOIN repairParts p ON r.repairPartsID = p.repairPartsID
    LEFT JOIN dataUsers u ON r.masterID = u.userID
    LEFT JOIN dataUsers ud ON r.clientID = ud.userID  
    LEFT JOIN dataComments c ON r.requestID = c.requestID AND r.masterID = c.masterID  
    WHERE r.masterID IS NULL"; 

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dataGridViewRequests.Rows.Clear();
            foreach (DataRow row in dt.Rows)
            {
                string[] rowData = new string[]
                {
                    row["requestID"].ToString(),
                    Convert.ToDateTime(row["startDate"]).ToString("yyyy-MM-dd HH:mm"),
                    row["ModelType"].ToString(),
                    row["carModel"].ToString(),
                    row["problemID"].ToString(),
                    row["requestStatus"].ToString(),
                    row["completionDate"] != DBNull.Value ? Convert.ToDateTime(row["completionDate"]).ToString("yyyy-MM-dd HH:mm") : "",
                    row["repairParts"].ToString(),
                    row["MasterFullName"].ToString(),
                    row["ClientFullName"].ToString(),
                    row["MasterComment"].ToString()
                };
                dataGridViewRequests.Rows.Add(rowData);
            }

            labelTotalRequests.Text = "Всего заявок: " + dt.Rows.Count;
            connection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
            Authorization authorization = new Authorization();
            authorization.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewRequests.SelectedRows.Count > 0)
            {

                int requestId = Convert.ToInt32(dataGridViewRequests.SelectedRows[0].Cells["requestID"].Value);
                DateTime start = Convert.ToDateTime(dataGridViewRequests.SelectedRows[0].Cells["startDate"].Value);
                string type1 = dataGridViewRequests.SelectedRows[0].Cells["OrgTechType"].Value.ToString();
                string problem = dataGridViewRequests.SelectedRows[0].Cells["ProblemDescription"].Value.ToString();
                string status = dataGridViewRequests.SelectedRows[0].Cells["StatusName"].Value.ToString();
                string client = dataGridViewRequests.SelectedRows[0].Cells["ClientFullName"].Value.ToString();
                string model = dataGridViewRequests.SelectedRows[0].Cells["orgTechModel"].Value.ToString();

                AddOper addOperForm = new AddOper(requestId, start, type1, model, problem, status, client);
                addOperForm.ShowDialog();


                dataGridViewRequests.Rows.Clear();
                LoadRequests();
            }
            else
            {

                MessageBox.Show("Пожалуйста, выберите заказ.");
            }

            LoadRequests();
        }
    }
}

