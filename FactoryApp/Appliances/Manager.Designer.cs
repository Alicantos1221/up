namespace Appliances
{
    partial class Manager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewRequests = new System.Windows.Forms.DataGridView();
            this.labelTotalRequests = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RequestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrgTechType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orgTechModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProblemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewRequests
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewRequests.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewRequests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRequests.BackgroundColor = System.Drawing.Color.PaleGreen;
            this.dataGridViewRequests.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRequests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RequestID,
            this.StartDate,
            this.OrgTechType,
            this.orgTechModel,
            this.ProblemDescription,
            this.StatusName,
            this.Column6,
            this.Column7,
            this.Column9,
            this.ClientFullName,
            this.Column8});
            this.dataGridViewRequests.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridViewRequests.Location = new System.Drawing.Point(11, 115);
            this.dataGridViewRequests.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewRequests.Name = "dataGridViewRequests";
            this.dataGridViewRequests.RowHeadersWidth = 51;
            this.dataGridViewRequests.RowTemplate.Height = 24;
            this.dataGridViewRequests.Size = new System.Drawing.Size(1018, 201);
            this.dataGridViewRequests.TabIndex = 32;
            // 
            // labelTotalRequests
            // 
            this.labelTotalRequests.AutoSize = true;
            this.labelTotalRequests.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalRequests.Location = new System.Drawing.Point(12, 337);
            this.labelTotalRequests.Name = "labelTotalRequests";
            this.labelTotalRequests.Size = new System.Drawing.Size(65, 22);
            this.labelTotalRequests.TabIndex = 33;
            this.labelTotalRequests.Text = "label1";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.LimeGreen;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button5.Location = new System.Drawing.Point(921, 11);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(108, 30);
            this.button5.TabIndex = 34;
            this.button5.Text = "Выйти";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Appliances.Properties.Resources.Новый_точечный_рисунок;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 98);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LimeGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(843, 337);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(186, 30);
            this.button1.TabIndex = 36;
            this.button1.Text = "Назначить мастера";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(127, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 18);
            this.label1.TabIndex = 37;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(127, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 18);
            this.label2.TabIndex = 38;
            this.label2.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(127, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 18);
            this.label3.TabIndex = 39;
            this.label3.Text = "label1";
            // 
            // RequestID
            // 
            this.RequestID.HeaderText = "Номер заявки";
            this.RequestID.MinimumWidth = 6;
            this.RequestID.Name = "RequestID";
            // 
            // StartDate
            // 
            this.StartDate.HeaderText = "Дата создания";
            this.StartDate.MinimumWidth = 6;
            this.StartDate.Name = "StartDate";
            // 
            // OrgTechType
            // 
            this.OrgTechType.HeaderText = "Тип машины";
            this.OrgTechType.MinimumWidth = 6;
            this.OrgTechType.Name = "OrgTechType";
            // 
            // orgTechModel
            // 
            this.orgTechModel.HeaderText = "Модель";
            this.orgTechModel.Name = "orgTechModel";
            // 
            // ProblemDescription
            // 
            this.ProblemDescription.HeaderText = "Проблема";
            this.ProblemDescription.MinimumWidth = 6;
            this.ProblemDescription.Name = "ProblemDescription";
            // 
            // StatusName
            // 
            this.StatusName.HeaderText = "Статус заявки";
            this.StatusName.MinimumWidth = 6;
            this.StatusName.Name = "StatusName";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Дата выполнения";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Детали";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Мастер";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            // 
            // ClientFullName
            // 
            this.ClientFullName.HeaderText = "Клиент";
            this.ClientFullName.MinimumWidth = 6;
            this.ClientFullName.Name = "ClientFullName";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Комментарий";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            // 
            // Manager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(1034, 373);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.labelTotalRequests);
            this.Controls.Add(this.dataGridViewRequests);
            this.Name = "Manager";
            this.Text = "Manager";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRequests)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewRequests;
        private System.Windows.Forms.Label labelTotalRequests;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrgTechType;
        private System.Windows.Forms.DataGridViewTextBoxColumn orgTechModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProblemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}