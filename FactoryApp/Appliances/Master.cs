using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Appliances
{
    public partial class Master : Form
    {

        static SqlConnection connection;
        SqlCommand command;
        public string Type { get; set; }
        public string Fio { get; set; }
        public int User { get; set; }
        public Master()
        {
            InitializeComponent();
            Add();
            StartPosition = FormStartPosition.CenterScreen;
            Add1();

        }
        public Master( int user) : this()
        {
            this.User = user;
            Add();
            StartPosition = FormStartPosition.CenterScreen;
            Add1();         
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
            { Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}"); }
        }
        private void Add()
        {
            Connect();
           

            string query = @"
        SELECT 
            r.requestID, 
            r.startDate, 
            h.carType, 
            h.carModel,
            r.problemID,  
            s.requestStatusID,  
            r.completionDate,  
            p.repairPartsID,  
            u.fio AS MasterFullName,  
            ud.fio AS ClientFullName,  
            c.message AS Comment  
        FROM 
            dataRequests r
        LEFT JOIN 
            cars h ON r.carID = h.carID  
        LEFT JOIN 
            status s ON r.requestStatusID = s.requestStatusID  
        LEFT JOIN 
            dataUsers u ON r.masterID = u.userID  
        LEFT JOIN 
            dataUsers ud ON r.clientID = ud.userID  
        LEFT JOIN 
            repairParts p ON r.repairPartsID = p.repairPartsID  
        LEFT JOIN 
            dataComments c ON r.requestID = c.requestID AND r.masterID = c.masterID  
 WHERE 
            r.masterID = @User AND r.requestStatusID = @StatusID
        ORDER BY 
            r.requestID ASC";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@User", User);
            command.Parameters.AddWithValue("@StatusID", 1);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

         
            dataGridView2.Rows.Clear();

         
            foreach (DataRow row in dt.Rows)
            {
                string[] rowData = new string[9];

                rowData[0] = row["requestID"].ToString();  
                rowData[1] = Convert.ToDateTime(row["startDate"]).ToString("yyyy-MM-dd");  
                rowData[2] = row["carType"]?.ToString() ?? "";  
                rowData[3] = row["carModel"]?.ToString() ?? "";  
                rowData[4] = row["problemID"]?.ToString() ?? "";  
                rowData[5] = row["requestStatusID"]?.ToString() ?? "";  
                rowData[6] = row["completionDate"] != DBNull.Value ? Convert.ToDateTime(row["CompletionDate"]).ToString("yyyy-MM-dd") : ""; 
                rowData[7] = row["repairPartsID"]?.ToString() ?? "";  
                rowData[8] = row["Comment"] != DBNull.Value ? row["Comment"].ToString() : "";  

                dataGridView1.Rows.Add(rowData);
            }

            connection.Close();
        }

        private void Add1()
        {
            Connect();
            string query = @"
        SELECT 
            r.requestID, 
            r.startDate, 
            h.carType AS ModelType, 
            h.carModel,
            r.problemID, 
            s.requestStatusID, 
            r.completionDate, 
            p.repairPartsID, 
            u.fio AS MasterFullName,            
            c.message AS MasterComment  
        FROM dataRequests r 
        INNER JOIN cars h ON r.carID = h.carID
        LEFT JOIN status s ON r.requestStatusID = s.requestStatusID
        LEFT JOIN repairParts p ON r.repairPartsID = p.repairPartsID
        LEFT JOIN dataUsers u ON r.masterID = u.userID
        LEFT JOIN dataUsers ud ON r.clientID = ud.userID  
        LEFT JOIN dataComments c ON r.requestID = c.requestID AND r.masterID = c.masterID 
        WHERE r.masterID = @User AND r.requestStatusID = 2";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@User", User);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<string[]> data = new List<string[]>();

                    while (reader.Read())
                    {
                       
                        string[] row = new string[9];

                        row[0] = reader["requestID"].ToString();
                        row[1] = Convert.ToDateTime(reader["startDate"]).ToString("yyyy-MM-dd");
                        row[2] = reader["ModelType"]?.ToString() ?? " ";
                        row[3] = reader["carModel"]?.ToString() ?? " ";
                        row[4] = reader["problemID"]?.ToString() ?? " ";
                        row[5] = reader["requestStatusID"]?.ToString() ?? " ";
                        row[6] = reader["completionDate"] != DBNull.Value ? Convert.ToDateTime(reader["completionDate"]).ToString("yyyy-MM-dd") : "";
                        row[7] = reader["repairPartsID"]?.ToString() ?? " ";

                        row[8] = reader["MasterComment"]?.ToString() ?? " "; 

                        data.Add(row);
                    }

                    dataGridView2.Rows.Clear();
                    foreach (string[] s in data)
                    {
                        dataGridView2.Rows.Add(s);
                    }
                }
            }

  
            command = new SqlCommand("SELECT COUNT(*) FROM dataRequests WHERE masterID = @User", connection);
            command.Parameters.AddWithValue("@User", User);
            int totalRecords1 = (int)command.ExecuteScalar();
            connection.Close();
            label4.Text = "Количество записей: " + dataGridView2.Rows.Count + " из " + totalRecords1;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            Order order = new Order(id, User);
            order.ShowDialog();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            Add();
            Add1();

        }
        public int id {  get; set; }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Authorization authorization = new Authorization();
            authorization.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateOper updateOper = new UpdateOper(id);
            updateOper.ShowDialog();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            Add();
            Add1();

        }
    }
}
