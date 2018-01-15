namespace trading
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnRun = new System.Windows.Forms.Button();
            this.txtIna = new System.Windows.Forms.TextBox();
            this.txtInb = new System.Windows.Forms.TextBox();
            this.txtInc = new System.Windows.Forms.TextBox();
            this.txtOut1 = new System.Windows.Forms.TextBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(864, 355);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(89, 33);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtIna
            // 
            this.txtIna.Location = new System.Drawing.Point(12, 24);
            this.txtIna.Multiline = true;
            this.txtIna.Name = "txtIna";
            this.txtIna.Size = new System.Drawing.Size(100, 364);
            this.txtIna.TabIndex = 1;
            // 
            // txtInb
            // 
            this.txtInb.Location = new System.Drawing.Point(118, 25);
            this.txtInb.Multiline = true;
            this.txtInb.Name = "txtInb";
            this.txtInb.Size = new System.Drawing.Size(100, 364);
            this.txtInb.TabIndex = 2;
            // 
            // txtInc
            // 
            this.txtInc.Location = new System.Drawing.Point(224, 25);
            this.txtInc.Multiline = true;
            this.txtInc.Name = "txtInc";
            this.txtInc.Size = new System.Drawing.Size(100, 364);
            this.txtInc.TabIndex = 3;
            // 
            // txtOut1
            // 
            this.txtOut1.Location = new System.Drawing.Point(377, 25);
            this.txtOut1.Multiline = true;
            this.txtOut1.Name = "txtOut1";
            this.txtOut1.Size = new System.Drawing.Size(100, 364);
            this.txtOut1.TabIndex = 4;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(492, 29);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(460, 183);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 401);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.txtOut1);
            this.Controls.Add(this.txtInc);
            this.Controls.Add(this.txtInb);
            this.Controls.Add(this.txtIna);
            this.Controls.Add(this.btnRun);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtIna;
        private System.Windows.Forms.TextBox txtInb;
        private System.Windows.Forms.TextBox txtInc;
        private System.Windows.Forms.TextBox txtOut1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

