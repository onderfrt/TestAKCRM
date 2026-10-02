namespace DirectFNCRM.AdminViews
{
    partial class formSedatKontrol
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.rtxtSSOolmayanlar = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtxtCRMolmayanlar = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblsayac = new System.Windows.Forms.Label();
            this.btnPasifMusteri = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboEventType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "SSO listesi Oku Başla";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rtxtSSOolmayanlar
            // 
            this.rtxtSSOolmayanlar.Location = new System.Drawing.Point(255, 63);
            this.rtxtSSOolmayanlar.Name = "rtxtSSOolmayanlar";
            this.rtxtSSOolmayanlar.Size = new System.Drawing.Size(174, 141);
            this.rtxtSSOolmayanlar.TabIndex = 1;
            this.rtxtSSOolmayanlar.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "SSO da Olup CRM de Olmayanlar";
            // 
            // rtxtCRMolmayanlar
            // 
            this.rtxtCRMolmayanlar.Location = new System.Drawing.Point(24, 63);
            this.rtxtCRMolmayanlar.Name = "rtxtCRMolmayanlar";
            this.rtxtCRMolmayanlar.Size = new System.Drawing.Size(174, 141);
            this.rtxtCRMolmayanlar.TabIndex = 1;
            this.rtxtCRMolmayanlar.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "CRM de Olup SSO da Olmayanlar";
            // 
            // lblsayac
            // 
            this.lblsayac.AutoSize = true;
            this.lblsayac.Location = new System.Drawing.Point(231, 18);
            this.lblsayac.Name = "lblsayac";
            this.lblsayac.Size = new System.Drawing.Size(45, 17);
            this.lblsayac.TabIndex = 3;
            this.lblsayac.Text = "sayac";
            // 
            // btnPasifMusteri
            // 
            this.btnPasifMusteri.Location = new System.Drawing.Point(574, 18);
            this.btnPasifMusteri.Name = "btnPasifMusteri";
            this.btnPasifMusteri.Size = new System.Drawing.Size(300, 23);
            this.btnPasifMusteri.TabIndex = 4;
            this.btnPasifMusteri.Text = "Pasif Müşterilerin Sözleşmelerini Pasife Çek";
            this.btnPasifMusteri.UseVisualStyleBackColor = true;
            this.btnPasifMusteri.Click += new System.EventHandler(this.btnPasifMusteri_Click);
            // 
            // chart1
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = 90;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(52, 226);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.LabelAngle = 90;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1002, 334);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // comboEventType
            // 
            this.comboEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEventType.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboEventType.FormattingEnabled = true;
            this.comboEventType.Location = new System.Drawing.Point(602, 62);
            this.comboEventType.Name = "comboEventType";
            this.comboEventType.Size = new System.Drawing.Size(206, 25);
            this.comboEventType.TabIndex = 51;
            this.comboEventType.SelectedIndexChanged += new System.EventHandler(this.comboEventType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(508, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 17);
            this.label5.TabIndex = 52;
            this.label5.Text = "Olay Tip";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(849, 190);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(51, 30);
            this.button2.TabIndex = 53;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(915, 190);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(51, 30);
            this.button3.TabIndex = 53;
            this.button3.Text = "-";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // formSedatKontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 683);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboEventType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnPasifMusteri);
            this.Controls.Add(this.lblsayac);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtxtCRMolmayanlar);
            this.Controls.Add(this.rtxtSSOolmayanlar);
            this.Controls.Add(this.button1);
            this.Name = "formSedatKontrol";
            this.Text = "formSedatKontrol";
            this.Load += new System.EventHandler(this.formSedatKontrol_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox rtxtSSOolmayanlar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtxtCRMolmayanlar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblsayac;
        private System.Windows.Forms.Button btnPasifMusteri;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ComboBox comboEventType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}