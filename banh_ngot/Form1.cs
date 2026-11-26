using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Ẩn mật khẩu khi nhập vào ô txt_mk trên màn hình chính
            txt_mk.PasswordChar = '*';
            // Hoặc có thể dùng ký tự chấm tròn của hệ thống:
            // txt_mk.UseSystemPasswordChar = true;
        }

        // ================= HÀM MÃ HÓA MẬT KHẨU SHA-256 ================= //
        private string MaHoaSHA256(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // ================= 1. ĐĂNG NHẬP ================= //
        private void bt_dn_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txt_ten.Text.Trim();
            string matKhau = txt_mk.Text.Trim();
            string vaiTroChon = cb_role.SelectedItem != null ? cb_role.SelectedItem.ToString().Trim() : "";

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string matKhauMaHoa = MaHoaSHA256(matKhau);

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsTaiKhoan = db.TaiKhoans.ToList();

                    var tk = dsTaiKhoan.FirstOrDefault(x =>
                        x.TenDangNhap.Trim().Equals(tenDangNhap, StringComparison.OrdinalIgnoreCase) &&
                        x.MatKhau.Trim() == matKhauMaHoa);

                    if (tk != null)
                    {
                        if (tk.TrangThai == false)
                        {
                            MessageBox.Show("Tài khoản này đã bị VÔ HIỆU HÓA! Vui lòng liên hệ Quản lý.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                        string vaiTroDb = tk.VaiTro != null ? tk.VaiTro.Trim() : "";

                        if (!string.Equals(vaiTroDb, vaiTroChon, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show($"Tài khoản [{tenDangNhap}] thuộc vai trò '{vaiTroDb}', không phải '{vaiTroChon}'!", "Sai vai trò", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (vaiTroDb.Equals("Khách hàng", StringComparison.OrdinalIgnoreCase))
                        {
                            KH_1 frmCustomer = new KH_1(tenDangNhap);
                            frmCustomer.Show();
                        }
                        else
                        {
                            NV frmQuanLy = new NV(tenDangNhap);
                            frmQuanLy.Show();
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================= 2. ĐĂNG KÝ TÀI KHOẢN ================= //
        private void bt_dk_Click(object sender, EventArgs e)
        {
            Form frmDK = new Form()
            {
                Width = 370,
                Height = 330,
                Text = "Đăng ký tài khoản mới",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lbl1 = new Label() { Text = "Tên đăng nhập:", Left = 20, Top = 20, Width = 110 };
            TextBox txtUser = new TextBox() { Left = 140, Top = 20, Width = 180 };

            Label lbl2 = new Label() { Text = "Mật khẩu:", Left = 20, Top = 60, Width = 110 };
            TextBox txtPass = new TextBox() { Left = 140, Top = 60, Width = 180, PasswordChar = '*' };

            Label lbl3 = new Label() { Text = "Nhập lại MK:", Left = 20, Top = 100, Width = 110 };
            TextBox txtPass2 = new TextBox() { Left = 140, Top = 100, Width = 180, PasswordChar = '*' };

            Label lbl4 = new Label() { Text = "Chọn vai trò:", Left = 20, Top = 140, Width = 110 };
            ComboBox cbRoleDK = new ComboBox() { Left = 140, Top = 140, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cbRoleDK.Items.AddRange(new object[] { "Khách hàng", "Quản lí" });
            cbRoleDK.SelectedIndex = 0;

            Button btnSubmit = new Button() { Text = "Đăng ký", Left = 140, Top = 190, Width = 90, Height = 30, DialogResult = DialogResult.OK };

            frmDK.Controls.AddRange(new Control[] { lbl1, txtUser, lbl2, txtPass, lbl3, txtPass2, lbl4, cbRoleDK, btnSubmit });
            frmDK.AcceptButton = btnSubmit;

            if (frmDK.ShowDialog() == DialogResult.OK)
            {
                string user = txtUser.Text.Trim();
                string pass = txtPass.Text.Trim();
                string pass2 = txtPass2.Text.Trim();
                string role = cbRoleDK.SelectedItem.ToString();

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    MessageBox.Show("Vui lòng không để trống thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (pass != pass2)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (var db = new QuanlytiembanhEntities())
                    {
                        var existing = db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == user);
                        if (existing != null)
                        {
                            MessageBox.Show("Tên đăng nhập này đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        TaiKhoan tkMoi = new TaiKhoan()
                        {
                            TenDangNhap = user,
                            MatKhau = MaHoaSHA256(pass),
                            VaiTro = role,
                            TrangThai = true
                        };

                        db.TaiKhoans.Add(tkMoi);
                        db.SaveChanges();
                        MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi lưu CSDL: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ================= 3. QUÊN MẬT KHẨU ================= //
        private void bt_qmk_Click(object sender, EventArgs e)
        {
            Form frmQMK = new Form()
            {
                Width = 350,
                Height = 230,
                Text = "Quên mật khẩu",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lbl1 = new Label() { Text = "Tên đăng nhập:", Left = 20, Top = 20, Width = 100 };
            TextBox txtUser = new TextBox() { Left = 130, Top = 20, Width = 170 };

            Label lbl2 = new Label() { Text = "Mật khẩu mới:", Left = 20, Top = 60, Width = 100 };
            TextBox txtNewPass = new TextBox() { Left = 130, Top = 60, Width = 170, PasswordChar = '*' };

            Button btnSubmit = new Button() { Text = "Xác nhận", Left = 130, Top = 110, Width = 90, Height = 30, DialogResult = DialogResult.OK };

            frmQMK.Controls.AddRange(new Control[] { lbl1, txtUser, lbl2, txtNewPass, btnSubmit });
            frmQMK.AcceptButton = btnSubmit;

            if (frmQMK.ShowDialog() == DialogResult.OK)
            {
                string user = txtUser.Text.Trim();
                string newPass = txtNewPass.Text.Trim();

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(newPass))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var db = new QuanlytiembanhEntities())
                {
                    var tk = db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == user);
                    if (tk != null)
                    {
                        tk.MatKhau = MaHoaSHA256(newPass);
                        db.SaveChanges();
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tên đăng nhập trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}