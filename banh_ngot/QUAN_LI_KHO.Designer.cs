namespace banh_ngot
{
    partial class QUAN_LI_KHO
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
            this.bt_xk = new System.Windows.Forms.Button();
            this.bt_nk = new System.Windows.Forms.Button();
            this.dt_don = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_sl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_gia = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_t = new System.Windows.Forms.TextBox();
            this.cb_ten = new System.Windows.Forms.ComboBox();
            this.cb_m = new System.Windows.Forms.ComboBox();
            this.cb_dv = new System.Windows.Forms.ComboBox();
            this.cb_dm = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.bt_them = new System.Windows.Forms.Button();
            this.bt_sua = new System.Windows.Forms.Button();
            this.bt_xoa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dt_don)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_tl
            // 
            this.bt_tl.Location = new System.Drawing.Point(40, 37);
            this.bt_tl.Name = "bt_tl";
            this.bt_tl.Size = new System.Drawing.Size(146, 32);
            this.bt_tl.TabIndex = 67;
            this.bt_tl.Text = "trở lại";
            this.bt_tl.UseVisualStyleBackColor = true;
            this.bt_tl.Click += new System.EventHandler(this.bt_trolai_Click);
            // 
            // bt_xk
            // 
            this.bt_xk.Location = new System.Drawing.Point(771, 545);
            this.bt_xk.Name = "bt_xk";
            this.bt_xk.Size = new System.Drawing.Size(146, 32);
            this.bt_xk.TabIndex = 66;
            this.bt_xk.Text = "XUẤT KHO";
            this.bt_xk.UseVisualStyleBackColor = true;
            this.bt_xk.Click += new System.EventHandler(this.bt_xuatkho_Click);
            // 
            // bt_nk
            // 
            this.bt_nk.Location = new System.Drawing.Point(575, 545);
            this.bt_nk.Name = "bt_nk";
            this.bt_nk.Size = new System.Drawing.Size(146, 32);
            this.bt_nk.TabIndex = 65;
            this.bt_nk.Text = "NHẬP KHO";
            this.bt_nk.UseVisualStyleBackColor = true;
            this.bt_nk.Click += new System.EventHandler(this.bt_nhapkho_Click);
            // 
            // dt_don
            // 
            this.dt_don.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_don.Location = new System.Drawing.Point(426, 118);
            this.dt_don.Name = "dt_don";
            this.dt_don.RowHeadersWidth = 51;
            this.dt_don.RowTemplate.Height = 24;
            this.dt_don.Size = new System.Drawing.Size(643, 410);
            this.dt_don.TabIndex = 64;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 63;
            this.label2.Text = "Tên bánh";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(432, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 32);
            this.label1.TabIndex = 61;
            this.label1.Text = "QUẢN LÍ KHO HÀNG";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 70;
            this.label3.Text = "Mã bánh";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 16);
            this.label4.TabIndex = 72;
            this.label4.Text = "Đơn vị tính";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 380);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 16);
            this.label5.TabIndex = 74;
            this.label5.Text = "Số lượng";
            // 
            // txt_sl
            // 
            this.txt_sl.Location = new System.Drawing.Point(106, 380);
            this.txt_sl.Name = "txt_sl";
            this.txt_sl.Size = new System.Drawing.Size(248, 22);
            this.txt_sl.TabIndex = 73;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 454);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 16);
            this.label6.TabIndex = 76;
            this.label6.Text = "Giá ( 1 sp )";
            // 
            // txt_gia
            // 
            this.txt_gia.Location = new System.Drawing.Point(106, 454);
            this.txt_gia.Name = "txt_gia";
            this.txt_gia.Size = new System.Drawing.Size(248, 22);
            this.txt_gia.TabIndex = 75;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 512);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 16);
            this.label7.TabIndex = 77;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 522);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 16);
            this.label8.TabIndex = 78;
            this.label8.Text = "TỔNG";
            // 
            // txt_t
            // 
            this.txt_t.Location = new System.Drawing.Point(106, 512);
            this.txt_t.Name = "txt_t";
            this.txt_t.Size = new System.Drawing.Size(248, 22);
            this.txt_t.TabIndex = 79;
            // 
            // cb_ten
            // 
            this.cb_ten.FormattingEnabled = true;
            this.cb_ten.Items.AddRange(new object[] {
            "Bánh mì hoa cúc",
            "Bánh mì ốc quế nhân kem",
            "Bánh sừng trâu bơ sữa",
            "Bánh mì nhân bơ sữa",
            "Bánh mì ngọt nhân đậu đỏ",
            "Bánh da lợn",
            "Bánh bò",
            "Bánh pía",
            "Bánh cam",
            "Bánh tiêu",
            "Tiramisu truyền thống",
            "Tiramisu Vị Hạt Dẻ Cười Pistachio",
            "Tiramisu vị dâu tây",
            "Tiramisu vị chanh vàng",
            "Tiramisu vị Matcha"});
            this.cb_ten.Location = new System.Drawing.Point(114, 119);
            this.cb_ten.Name = "cb_ten";
            this.cb_ten.Size = new System.Drawing.Size(240, 24);
            this.cb_ten.TabIndex = 80;
            // 
            // cb_m
            // 
            this.cb_m.FormattingEnabled = true;
            this.cb_m.Items.AddRange(new object[] {
            "B01",
            "B02",
            "B03",
            "B04",
            "B05",
            "B06",
            "B07",
            "B08",
            "B09",
            "B10",
            "B11",
            "B12",
            "B13",
            "B14",
            "B15"});
            this.cb_m.Location = new System.Drawing.Point(114, 185);
            this.cb_m.Name = "cb_m";
            this.cb_m.Size = new System.Drawing.Size(240, 24);
            this.cb_m.TabIndex = 81;
            // 
            // cb_dv
            // 
            this.cb_dv.FormattingEnabled = true;
            this.cb_dv.Items.AddRange(new object[] {
            "Ổ",
            "Cái",
            "Hộp"});
            this.cb_dv.Location = new System.Drawing.Point(114, 243);
            this.cb_dv.Name = "cb_dv";
            this.cb_dv.Size = new System.Drawing.Size(240, 24);
            this.cb_dv.TabIndex = 82;
            this.cb_dv.SelectedIndexChanged += new System.EventHandler(this.cb_dv_SelectedIndexChanged);
            // 
            // cb_dm
            // 
            this.cb_dm.FormattingEnabled = true;
            this.cb_dm.Location = new System.Drawing.Point(114, 318);
            this.cb_dm.Name = "cb_dm";
            this.cb_dm.Size = new System.Drawing.Size(240, 24);
            this.cb_dm.TabIndex = 84;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 318);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 16);
            this.label9.TabIndex = 83;
            this.label9.Text = "Danh mục";
            // 
            // bt_them
            // 
            this.bt_them.Location = new System.Drawing.Point(534, 599);
            this.bt_them.Name = "bt_them";
            this.bt_them.Size = new System.Drawing.Size(146, 32);
            this.bt_them.TabIndex = 85;
            this.bt_them.Text = "THÊM SẢN PHẨM";
            this.bt_them.UseVisualStyleBackColor = true;
            this.bt_them.Click += new System.EventHandler(this.bt_them_Click_1);
            // 
            // bt_sua
            // 
            this.bt_sua.Location = new System.Drawing.Point(702, 599);
            this.bt_sua.Name = "bt_sua";
            this.bt_sua.Size = new System.Drawing.Size(146, 32);
            this.bt_sua.TabIndex = 86;
            this.bt_sua.Text = "SỬA HÓA ĐƠN";
            this.bt_sua.UseVisualStyleBackColor = true;
            // 
            // bt_xoa
            // 
            this.bt_xoa.Location = new System.Drawing.Point(866, 599);
            this.bt_xoa.Name = "bt_xoa";
            this.bt_xoa.Size = new System.Drawing.Size(146, 32);
            this.bt_xoa.TabIndex = 87;
            this.bt_xoa.Text = "XÓA";
            this.bt_xoa.UseVisualStyleBackColor = true;
            // 
            // QUAN_LI_KHO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 693);
            this.Controls.Add(this.bt_xoa);
            this.Controls.Add(this.bt_sua);
            this.Controls.Add(this.bt_them);
            this.Controls.Add(this.cb_dm);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cb_dv);
            this.Controls.Add(this.cb_m);
            this.Controls.Add(this.cb_ten);
            this.Controls.Add(this.txt_t);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_gia);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_sl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bt_tl);
            this.Controls.Add(this.bt_xk);
            this.Controls.Add(this.bt_nk);
            this.Controls.Add(this.dt_don);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "QUAN_LI_KHO";
            this.Text = "QUAN_LI_KHO";
            this.Load += new System.EventHandler(this.QUAN_LI_KHO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dt_don)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bt_tl;
        private System.Windows.Forms.Button bt_xk;
        private System.Windows.Forms.Button bt_nk;
        private System.Windows.Forms.DataGridView dt_don;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_sl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_gia;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_t;
        private System.Windows.Forms.ComboBox cb_ten;
        private System.Windows.Forms.ComboBox cb_m;
        private System.Windows.Forms.ComboBox cb_dv;
        private System.Windows.Forms.ComboBox cb_dm;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button bt_them;
        private System.Windows.Forms.Button bt_sua;
        private System.Windows.Forms.Button bt_xoa;
    }
}