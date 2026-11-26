using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class QUAN_LI_TK : Form
    {
        public QUAN_LI_TK()
        {
            InitializeComponent();
        }

        private void QUAN_LI_TK_Load(object sender, EventArgs e)
        {
            LoadDataTaiKhoan();
        }

        // 1. Tải danh sách tài khoản (Chỉ lấy Tên đăng nhập, Vai trò, Trạng thái)
        private void LoadDataTaiKhoan(string tuKhoa = "")
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var query = db.TaiKhoans.AsQueryable();

                    // Lọc bỏ tài khoản Quản lí
                    query = query.Where(tk => tk.VaiTro != "Quản lí");

                    // Tìm kiếm theo Tên đăng nhập
                    if (!string.IsNullOrEmpty(tuKhoa))
                    {
                        query = query.Where(tk => tk.TenDangNhap.Contains(tuKhoa));
                    }

                    var rawList = query.ToList();

                    // Map sang danh sách hiển thị (Đã lược bỏ MaNhanVien)
                    var dsTaiKhoan = rawList.Select(tk => new
                    {
                        TenDangNhap = tk.TenDangNhap.Trim(),
                        VaiTro = tk.VaiTro != null ? tk.VaiTro.Trim() : "",
                        TrangThai = (tk.TrangThai == true) ? "Hoạt động" : "Vô hiệu"
                    }).ToList();

                    dt_tk.DataSource = null;
                    dt_tk.DataSource = dsTaiKhoan;

                    // Định dạng tên cột DataGridView
                    if (dt_tk.Columns["TenDangNhap"] != null) dt_tk.Columns["TenDangNhap"].HeaderText = "Tên Đăng Nhập";
                    if (dt_tk.Columns["VaiTro"] != null) dt_tk.Columns["VaiTro"].HeaderText = "Vai Trò";
                    if (dt_tk.Columns["TrangThai"] != null) dt_tk.Columns["TrangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Nút Tìm kiếm (bt_tk)
        private void bt_tk_Click(object sender, EventArgs e)
        {
            string tuKhoa = txt_tk.Text.Trim();
            LoadDataTaiKhoan(tuKhoa);
        }

        // 3. Nút Vô hiệu hóa tài khoản (bt_d)
        private void bt_d_Click(object sender, EventArgs e)
        {
            CapNhatTrangThaiTaiKhoan("Vô hiệu");
        }

        // 4. Nút Kích hoạt tài khoản (bt_m)
        private void bt_m_Click(object sender, EventArgs e)
        {
            CapNhatTrangThaiTaiKhoan("Hoạt động");
        }

        // Cập nhật trạng thái tài khoản
        private void CapNhatTrangThaiTaiKhoan(string trangThaiMoi)
        {
            if (dt_tk.CurrentRow == null || dt_tk.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần thực hiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenDangNhap = dt_tk.CurrentRow.Cells["TenDangNhap"].Value?.ToString();
            if (string.IsNullOrEmpty(tenDangNhap)) return;

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var tk = db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap.Trim() == tenDangNhap.Trim());
                    if (tk != null)
                    {
                        tk.TrangThai = (trangThaiMoi == "Hoạt động");
                        db.SaveChanges();

                        MessageBox.Show($"Đã chuyển trạng thái tài khoản [{tenDangNhap}] sang '{trangThaiMoi}'!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDataTaiKhoan(txt_tk.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật trạng thái: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5. Nút Trở lại (bt_tl)
        private void bt_tl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}