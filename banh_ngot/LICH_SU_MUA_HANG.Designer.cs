namespace banh_ngot
{
    partial class LICH_SU_MUA_HANG
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
            this.bt_tl = new System.Windows.Forms.Button();
            this.bt_th = new System.Windows.Forms.Button();
            this.bt_ct = new System.Windows.Forms.Button();
            this.dt_gh = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_tk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_tk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dt_gh)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_tl
            // 
            this.bt_tl.Location = new System.Drawing.Point(26, 5);
            this.bt_tl.Name = "bt_tl";
            this.bt_tl.Size = new System.Drawing.Size(146, 32);
            this.bt_tl.TabIndex = 48;
            this.bt_tl.Text = "trở lại";
            this.bt_tl.UseVisualStyleBackColor = true;
            this.bt_tl.Click += new System.EventHandler(this.bt_tl_Click);
            // 
            // bt_th
            // 
            this.bt_th.Location = new System.Drawing.Point(491, 515);
            this.bt_th.Name = "bt_th";
            this.bt_th.Size = new System.Drawing.Size(146, 32);
            this.bt_th.TabIndex = 47;
            this.bt_th.Text = "tổng hợp";
            this.bt_th.UseVisualStyleBackColor = true;
            this.bt_th.Click += new System.EventHandler(this.bt_th_Click);
            // 
            // bt_ct
            // 
            this.bt_ct.Location = new System.Drawing.Point(246, 515);
            this.bt_ct.Name = "bt_ct";
            this.bt_ct.Size = new System.Drawing.Size(146, 32);
            this.bt_ct.TabIndex = 46;
            this.bt_ct.Text = "xem chi tiết";
            this.bt_ct.UseVisualStyleBackColor = true;
            this.bt_ct.Click += new System.EventHandler(this.bt_ct_Click);
            // 
            // dt_gh
            // 
            this.dt_gh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_gh.Location = new System.Drawing.Point(12, 136);
            this.dt_gh.Name = "dt_gh";
            this.dt_gh.RowHeadersWidth = 51;
            this.dt_gh.RowTemplate.Height = 24;
            this.dt_gh.Size = new System.Drawing.Size(909, 338);
            this.dt_gh.TabIndex = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 44;
            this.label2.Text = "Tìm kiếm";
            // 
            // txt_tk
            // 
            this.txt_tk.Location = new System.Drawing.Point(268, 77);
            this.txt_tk.Name = "txt_tk";
            this.txt_tk.Size = new System.Drawing.Size(398, 22);
            this.txt_tk.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(321, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 32);
            this.label1.TabIndex = 42;
            this.label1.Text = "LỊCH SỬ MUA HÀNG";
            // 
            // bt_tk
            // 
            this.bt_tk.Location = new System.Drawing.Point(695, 75);
            this.bt_tk.Name = "bt_tk";
            this.bt_tk.Size = new System.Drawing.Size(146, 32);
            this.bt_tk.TabIndex = 52;
            this.bt_tk.Text = "tìm kiếm";
            this.bt_tk.UseVisualStyleBackColor = true;
            this.bt_tk.Click += new System.EventHandler(this.bt_tk_Click);
            // 
            // LICH_SU_MUA_HANG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 585);
            this.Controls.Add(this.bt_tk);
            this.Controls.Add(this.bt_tl);
            this.Controls.Add(this.bt_th);
            this.Controls.Add(this.bt_ct);
            this.Controls.Add(this.dt_gh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_tk);
            this.Controls.Add(this.label1);
            this.Name = "LICH_SU_MUA_HANG";
            this.Text = "LICH_SU_MUA_HANG";
            this.Load += new System.EventHandler(this.LICH_SU_MUA_HANG_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dt_gh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_tl;
        private System.Windows.Forms.Button bt_th;
        private System.Windows.Forms.Button bt_ct;
        private System.Windows.Forms.DataGridView dt_gh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_tk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_tk;
    }
}