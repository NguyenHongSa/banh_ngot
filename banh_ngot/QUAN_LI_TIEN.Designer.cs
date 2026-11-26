namespace banh_ngot
{
    partial class QUAN_LI_TIEN
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
            this.bt_tc = new System.Windows.Forms.Button();
            this.bt_tl = new System.Windows.Forms.Button();
            this.dt_tien = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_tong = new System.Windows.Forms.TextBox();
            this.bt_hd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dt_tien)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_tc
            // 
            this.bt_tc.Location = new System.Drawing.Point(180, 529);
            this.bt_tc.Name = "bt_tc";
            this.bt_tc.Size = new System.Drawing.Size(146, 32);
            this.bt_tc.TabIndex = 69;
            this.bt_tc.Text = "Tùy chọn";
            this.bt_tc.UseVisualStyleBackColor = true;
            this.bt_tc.Click += new System.EventHandler(this.bt_tc_Click);
            // 
            // bt_tl
            // 
            this.bt_tl.Location = new System.Drawing.Point(95, 19);
            this.bt_tl.Name = "bt_tl";
            this.bt_tl.Size = new System.Drawing.Size(146, 32);
            this.bt_tl.TabIndex = 67;
            this.bt_tl.Text = "trở lại";
            this.bt_tl.UseVisualStyleBackColor = true;
            this.bt_tl.Click += new System.EventHandler(this.bt_tl_Click);
            // 
            // dt_tien
            // 
            this.dt_tien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_tien.Location = new System.Drawing.Point(95, 161);
            this.dt_tien.Name = "dt_tien";
            this.dt_tien.RowHeadersWidth = 51;
            this.dt_tien.RowTemplate.Height = 24;
            this.dt_tien.Size = new System.Drawing.Size(764, 281);
            this.dt_tien.TabIndex = 64;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(343, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 32);
            this.label1.TabIndex = 61;
            this.label1.Text = "DOANH THU CỬA HÀNG";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(155, 471);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 32);
            this.label2.TabIndex = 70;
            this.label2.Text = "TỔNG";
            // 
            // txt_tong
            // 
            this.txt_tong.Location = new System.Drawing.Point(329, 471);
            this.txt_tong.Name = "txt_tong";
            this.txt_tong.Size = new System.Drawing.Size(374, 22);
            this.txt_tong.TabIndex = 71;
            // 
            // bt_hd
            // 
            this.bt_hd.Location = new System.Drawing.Point(617, 529);
            this.bt_hd.Name = "bt_hd";
            this.bt_hd.Size = new System.Drawing.Size(146, 32);
            this.bt_hd.TabIndex = 72;
            this.bt_hd.Text = "Xem hóa đơn";
            this.bt_hd.UseVisualStyleBackColor = true;
            this.bt_hd.Click += new System.EventHandler(this.bt_hd_Click);
            // 
            // QUAN_LI_TIEN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 573);
            this.Controls.Add(this.bt_hd);
            this.Controls.Add(this.txt_tong);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bt_tc);
            this.Controls.Add(this.bt_tl);
            this.Controls.Add(this.dt_tien);
            this.Controls.Add(this.label1);
            this.Name = "QUAN_LI_TIEN";
            this.Text = "QUAN_LI_TIEN";
            this.Load += new System.EventHandler(this.QUAN_LI_TIEN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_tien)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_tc;
        private System.Windows.Forms.Button bt_tl;
        private System.Windows.Forms.DataGridView dt_tien;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_tong;
        private System.Windows.Forms.Button bt_hd;
    }
}