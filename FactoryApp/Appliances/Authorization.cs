using QRCoder;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Appliances
{
    public partial class Authorization : Form
    {
        private SqlConnection connection;
        private bool captchaRequired = false;
        private int captchaAttempts = 0;
        private Timer lockoutTimer;
        private string captchaText;

        public Authorization()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            connection = new SqlConnection("Data Source=ADCLG1;Initial Catalog=Leo_bd_up;Integrated Security=True;");
            this.DoubleBuffered = true;

            // Настройка таймера для блокировки
            lockoutTimer = new Timer
            {
                Interval = 180000 // 3 минуты
            };
            lockoutTimer.Tick += LockoutTimer_Tick;
        }

        private void Authorization_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            textBox3.Visible = false;
            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (captchaRequired && textBox3.Text != captchaText)
            {
                MessageBox.Show("Неверная капча.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                captchaAttempts++;

                if (captchaAttempts >= 2)
                {
                    BlockLogin();
                    return;
                }

                GenerateCaptcha();
                return;
            }

            captchaAttempts = 0;
            int userId = ValidateUser(username, password);
            int role = ValidateUserRole(username, password);

            if (userId != -1)
            {
                Addfailure_plus(userId);

                Form userForm;
                switch (role)
                {
                    case 1:
                        userForm = new Manager(userId);
                        break;
                    case 2:
                        userForm = new Master(userId);
                        break;
                    case 3:
                        userForm = new Operator(userId);
                        break;
                    case 4:
                        userForm = new Customer(userId);
                        break;
                    default:
                        userForm = null;
                        MessageBox.Show("Такой роли нет", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }

                if (userForm != null)
                {
                    userForm.Show();
                    Hide();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Addfailure_minus(userId);  

                if (!captchaRequired)
                {
                    captchaRequired = true;
                    ShowCaptcha();
                }

                GenerateCaptcha();
            }
        }

        private int ValidateUser(string username, string password)
        {
            string query = "SELECT userId FROM dataUsers WHERE login = @Username AND password = @Password";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                connection.Open();
                object result = cmd.ExecuteScalar();
                connection.Close();

                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        private int ValidateUserRole(string username, string password)
        {
            string query = "SELECT typeID FROM dataUsers WHERE login = @Username AND password = @Password";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                connection.Open();
                object result = cmd.ExecuteScalar();
                connection.Close();

                return result != null ? Convert.ToInt32(result) : -1;
            }
        }

        private void GenerateCaptcha()
        {
            captchaText = new Random().Next(1000, 9999).ToString();
            textBox3.Clear();
            pictureBox1.Image = GenerateCaptchaImage(captchaText);
        }

        private Image GenerateCaptchaImage(string text)
        {
            int width = 120, height = 60;
            Bitmap bitmap = new Bitmap(width, height);
            Random random = new Random();

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                // Добавление случайных линий для усложнения капчи
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(0, width);
                    int y1 = random.Next(0, height);
                    int x2 = random.Next(0, width);
                    int y2 = random.Next(0, height);
                    g.DrawLine(Pens.Gray, x1, y1, x2, y2);
                }

                Font font = new Font("Arial", 24, FontStyle.Bold | FontStyle.Italic);
                int charOffset = 5;

                for (int i = 0; i < text.Length; i++)
                {
                    float angle = (float)(random.NextDouble() * 30 - 15);
                    float x = 10 + i * (font.Size + charOffset) + random.Next(-5, 5);
                    float y = random.Next(5, 20);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddString(text[i].ToString(), font.FontFamily, (int)FontStyle.Bold, font.Size, new PointF(x, y), StringFormat.GenericDefault);
                        g.RotateTransform(angle);
                        g.DrawPath(Pens.Black, path);
                        g.RotateTransform(-angle);
                    }
                }
            }

            return bitmap;
        }

        private void ShowCaptcha()
        {
            pictureBox1.Visible = true;
            textBox3.Visible = true;
            button2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e) => GenerateCaptcha();

        private void checkBox1_CheckedChanged(object sender, EventArgs e) =>
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';

        private void BlockLogin()
        {
            button1.Enabled = false;
            lockoutTimer.Start();
            MessageBox.Show("Вы заблокированы на 3 минуты из-за неверных попыток.", "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LockoutTimer_Tick(object sender, EventArgs e)
        {
            lockoutTimer.Stop();
            button1.Enabled = true;
            captchaAttempts = 0;
        }

        private void Addfailure_minus(int userID)
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO loginHistory (entryTime, login, isEntryGranted) VALUES (@Date, @UserId, 0);", connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userID == -1 ? (object)DBNull.Value : userID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }


        private void Addfailure_plus(int userID)
        {
            if (userID == -1) return;

            using (SqlCommand cmd = new SqlCommand($"INSERT INTO loginHistory (entryTime, login, isEntryGranted) VALUES (@Date,@UserId , 1);", connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
