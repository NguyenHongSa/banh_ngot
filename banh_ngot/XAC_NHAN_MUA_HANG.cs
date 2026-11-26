using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class XAC_NHAN_MUA_HANG : Form
    {
        private string tenDangNhapHienTai = "";
        private int maGioHangCanMua = -1;
        private decimal donGiaBanh = 0;
        private int soLuongBanh = 0;
        private decimal phiShip = 15000;
        private decimal tongTienDonHang = 0;

        private Dictionary<string, string> dsAnhThuCong = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "B01", "Ảnh1.jpg" },
            { "B02", "Ảnh2.jpg" },
            { "B03", "Ảnh3.jpg" },
            { "B04", "Ảnh4.jpg" },
            { "B05", "Ảnh5.jpg" },
            { "B06", "Ảnh6.png" },
            { "B07", "Ảnh7.png" },
            { "B08", "Ảnh8.png" },
            { "B09", "Ảnh9.png" },
            { "B10", "Ảnh10.png" },
            { "B11", "Ảnh11.jpg" },
            { "B12", "Ảnh12.jpg" },
            { "B13", "Ảnh13.jpg" },
            { "B14", "Ảnh14.jpg" },
            { "B15", "Ảnh15.jpg" }
        };

        public XAC_NHAN_MUA_HANG()
        {
            InitializeComponent();
            this.Load += XAC_NHAN_MUA_HANG_Load;
        }

        public XAC_NHAN_MUA_HANG(string tenDangNhap)
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap != null ? tenDangNhap.Trim() : "";
            this.Load += XAC_NHAN_MUA_HANG_Load;
        }

        public XAC_NHAN_MUA_HANG(string tenDangNhap, int maGioHang)
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap != null ? tenDangNhap.Trim() : "";
            this.maGioHangCanMua = maGioHang;
            this.Load += XAC_NHAN_MUA_HANG_Load;
        }

        private void XAC_NHAN_MUA_HANG_Load(object sender, EventArgs e)
        {
            LoadThongTinKhachHang();
            LoadThongTinDonHang();
        }

        private void LoadThongTinKhachHang()
        {
            if (txt_ten != null) txt_ten.Text = "";
            if (txt_sdt != null) txt_sdt.Text = "";
            if (txt_email != null) txt_email.Text = "";
            if (txt_dc != null) txt_dc.Text = "";
        }

        private void LoadThongTinDonHang()
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    GioHang itemGH = null;

                    if (maGioHangCanMua != -1)
                    {
                        itemGH = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHangCanMua);
                    }
                    else
                    {
                        itemGH = db.GioHangs.Where(g => g.TenDangNhap == tenDangNhapHienTai)
                                            .OrderByDescending(g => g.MaGioHang)
                                            .FirstOrDefault();
                    }

                    if (itemGH != null)
                    {
                        maGioHangCanMua = itemGH.MaGioHang;
                        string maBanhTim = itemGH.MaBanh != null ? itemGH.MaBanh.Trim() : "";
                        var itemSP = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == maBanhTim);

                        if (itemSP != null)
                        {
                            // Kiểm tra tồn kho ngay khi tải đơn hàng
                            int tonKho = itemSP.SoLuongKho ?? 0;
                            if (tonKho <= 0)
                            {
                                MessageBox.Show($"Mặt hàng '{itemSP.TenBanh}' hiện đã HẾT HÀNG trong kho!", "Cảnh báo hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                if (bt_mua != null) bt_mua.Enabled = false; // Vô hiệu hóa nút mua
                            }

                            if (txt_banh != null) txt_banh.Text = itemSP.TenBanh;
                            soLuongBanh = itemGH.SoLuong ?? 1;
                            if (txt_sl != null) txt_sl.Text = soLuongBanh.ToString();

                            donGiaBanh = itemSP.DonGiaBan ?? 0;
                            if (txt_gia != null) txt_gia.Text = donGiaBanh.ToString("N0") + " VNĐ";

                            if (txt_phi != null) txt_phi.Text = phiShip.ToString("N0") + " VNĐ";
                            tongTienDonHang = (soLuongBanh * donGiaBanh) + phiShip;
                            if (txt_tong != null) txt_tong.Text = tongTienDonHang.ToString("N0") + " VNĐ";

                            DateTime ngayGiaoDuKien = DateTime.Now.AddDays(3);
                            if (txt_nn != null)
                            {
                                txt_nn.Text = ngayGiaoDuKien.ToString("dd/MM/yyyy");
                                txt_nn.ReadOnly = true;
                            }

                            string tenFileAnh = "";
                            if (dsAnhThuCong.ContainsKey(maBanhTim))
                            {
                                tenFileAnh = dsAnhThuCong[maBanhTim];
                            }

                            LoadAnhVaoPt1(tenFileAnh, maBanhTim);
                        }

                        if (txt_banh != null) txt_banh.ReadOnly = true;
                        if (txt_sl != null) txt_sl.ReadOnly = true;
                        if (txt_gia != null) txt_gia.ReadOnly = true;
                        if (txt_phi != null) txt_phi.ReadOnly = true;
                        if (txt_tong != null) txt_tong.ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy đơn hàng nào để thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin đơn hàng: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAnhVaoPt1(string tenFileAnh, string maBanh)
        {
            if (pt_1 == null) return;

            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string startupPath = Application.StartupPath;

                string[] cacThuMucAnh = new string[]
                {
                    Path.Combine(startupPath, "Anh"),
                    Path.Combine(baseDir, "Anh"),
                    Path.Combine(baseDir, "..", "..", "Anh"),
                    Path.Combine(baseDir, "..", "..", "..", "Anh"),
                    Path.Combine(Directory.GetCurrentDirectory(), "Anh")
                };

                string folderAnh = cacThuMucAnh.FirstOrDefault(d => Directory.Exists(d));
                if (string.IsNullOrEmpty(folderAnh)) return;

                string[] files = Directory.GetFiles(folderAnh);

                string pathChon = files.FirstOrDefault(f =>
                    (!string.IsNullOrEmpty(tenFileAnh) && Path.GetFileName(f).Equals(tenFileAnh, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(tenFileAnh) && Path.GetFileNameWithoutExtension(f).Equals(Path.GetFileNameWithoutExtension(tenFileAnh), StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(maBanh) && Path.GetFileNameWithoutExtension(f).Equals(maBanh, StringComparison.OrdinalIgnoreCase))
                );

                if (!string.IsNullOrEmpty(pathChon) && File.Exists(pathChon))
                {
                    byte[] imageBytes = File.ReadAllBytes(pathChon);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pt_1.Image = Image.FromStream(ms);
                    }
                    pt_1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi tải ảnh: " + ex.Message);
            }
        }

        public bool TruSoLuongTon(QuanlytiembanhEntities db, string maBanh, int soLuongMua)
        {
            if (string.IsNullOrEmpty(maBanh)) return false;

            string maChuan = maBanh.Trim().ToLower();
            var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim().ToLower() == maChuan);

            if (sp == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int soLuongHienTai = sp.SoLuongKho ?? 0;

            // Báo hết hàng nếu tồn kho <= 0
            if (soLuongHienTai <= 0)
            {
                MessageBox.Show($"Mặt hàng '{sp.TenBanh}' hiện đã HẾT HÀNG!", "Thông báo hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Báo không đủ số lượng nếu tồn kho nhỏ hơn số lượng khách muốn đặt
            if (soLuongHienTai < soLuongMua)
            {
                MessageBox.Show($"Bánh '{sp.TenBanh}' không đủ số lượng trong kho (Hiện còn: {soLuongHienTai})!", "Cảnh báo kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            sp.SoLuongKho = soLuongHienTai - soLuongMua;
            return true;
        }

        private void bt_xacnhan_Click(object sender, EventArgs e)
        {
            if (txt_ten == null || string.IsNullOrWhiteSpace(txt_ten.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người nhận hàng!", "Cảnh báo nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_ten?.Focus();
                return;
            }

            if (txt_sdt == null || string.IsNullOrWhiteSpace(txt_sdt.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại người nhận!", "Cảnh báo nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_sdt?.Focus();
                return;
            }

            string sdt = txt_sdt.Text.Trim();
            if (!sdt.All(char.IsDigit) || sdt.Length < 9 || sdt.Length > 11)
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng chỉ nhập từ 9 đến 11 chữ số.", "Cảnh báo nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_sdt?.Focus();
                return;
            }

            if (txt_dc == null || string.IsNullOrWhiteSpace(txt_dc.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ giao hàng!", "Cảnh báo nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_dc?.Focus();
                return;
            }

            bool isChuyenKhoan = KiemTraPhuongThucThanhToan(this, "CHUYỂN");
            bool isTienMat = KiemTraPhuongThucThanhToan(this, "MẶT") || KiemTraPhuongThucThanhToan(this, "TIỀN");

            if (!isChuyenKhoan && !isTienMat)
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán (TIỀN MẶT hoặc CHUYỂN KHOAN)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phuongThucText = isChuyenKhoan ? "Chuyển khoản" : "Tiền mặt";

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var donHang = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHangCanMua);
                    if (donHang == null)
                    {
                        MessageBox.Show("Không tìm thấy đơn hàng cần thanh toán!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string maBanh = donHang.MaBanh != null ? donHang.MaBanh.Trim() : "";
                    int soLuongDat = donHang.SoLuong ?? 1;

                    if (soLuongDat <= 0)
                    {
                        MessageBox.Show("Số lượng mua phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (TruSoLuongTon(db, maBanh, soLuongDat))
                    {
                        // Cập nhật trạng thái chính xác thành "Đã đặt hàng"
                        donHang.TrangThai = "Đã đặt hàng";

                        // Cập nhật đầy đủ các thuộc tính có thể có trong CSDL
                        SetPropValue(donHang, "PhuongThucThanhToan", phuongThucText);
                        SetPropValue(donHang, "PhuongThuc", phuongThucText);

                        SetPropValue(donHang, "HoTen", txt_ten.Text.Trim());
                        SetPropValue(donHang, "TenKhachHang", txt_ten.Text.Trim());

                        SetPropValue(donHang, "SDT", txt_sdt.Text.Trim());
                        SetPropValue(donHang, "SoDienThoai", txt_sdt.Text.Trim());

                        SetPropValue(donHang, "DiaChi", txt_dc.Text.Trim());
                        SetPropValue(donHang, "Email", txt_email != null ? txt_email.Text.Trim() : "");

                        SetPropValue(donHang, "PhiShip", phiShip);
                        SetPropValue(donHang, "TongTien", tongTienDonHang);

                        donHang.NgayDat = DateTime.Now;

                        db.SaveChanges();

                        MessageBox.Show("Đặt hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (isChuyenKhoan)
                        {
                            HienThiMaQRChuyenKhoan(donHang.MaGioHang);
                        }

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý đơn hàng: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraPhuongThucThanhToan(Control parent, string keyWord)
        {
            if (parent == null) return false;

            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is CheckBox chk && chk.Checked)
                {
                    if (chk.Text.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        chk.Name.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
                if (ctrl is RadioButton rad && rad.Checked)
                {
                    if (rad.Text.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        rad.Name.IndexOf(keyWord, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }

                if (ctrl.HasChildren && KiemTraPhuongThucThanhToan(ctrl, keyWord))
                    return true;
            }
            return false;
        }

        private void HienThiMaQRChuyenKhoan(int maDH)
        {
            try
            {
                string nganHang = "MB";
                string soTaiKhoan = "0383123456";
                string tenTaiKhoan = "TIEM BANH NGOT";
                string noiDungCK = $"DH{maDH} {txt_sdt.Text.Trim()}";

                string qrUrl = $"https://img.vietqr.io/image/{nganHang}-{soTaiKhoan}-compact2.png?amount={(long)tongTienDonHang}&addInfo={Uri.EscapeDataString(noiDungCK)}&accountName={Uri.EscapeDataString(tenTaiKhoan)}";

                Form qrForm = new Form
                {
                    Text = "QUÉT MÃ QR THANH TOÁN",
                    Size = new Size(380, 490),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                PictureBox picQR = new PictureBox
                {
                    Size = new Size(320, 320),
                    Location = new Point(22, 10),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    ImageLocation = qrUrl
                };

                Label lblInfo = new Label
                {
                    Text = $"STK: {soTaiKhoan} ({nganHang})\nChủ TK: {tenTaiKhoan}\nSố tiền: {tongTienDonHang:N0} VNĐ\nNội dung: {noiDungCK}",
                    Size = new Size(340, 65),
                    Location = new Point(12, 335),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };

                Button btnXacNhan = new Button
                {
                    Text = "Tôi đã chuyển khoản xong",
                    Size = new Size(180, 32),
                    Location = new Point(92, 405)
                };
                btnXacNhan.Click += (s, e) => { qrForm.Close(); };

                qrForm.Controls.Add(picQR);
                qrForm.Controls.Add(lblInfo);
                qrForm.Controls.Add(btnXacNhan);
                qrForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải mã QR thanh toán: " + ex.Message, "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetPropValue(object obj, string propName, object value)
        {
            if (obj == null) return;
            PropertyInfo pi = obj.GetType().GetProperty(propName);
            if (pi != null && pi.CanWrite)
            {
                Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                object safeValue = (value == null) ? null : Convert.ChangeType(value, t);
                pi.SetValue(obj, safeValue, null);
            }
        }

        private void bt_huy_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var donHang = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHangCanMua);
                    if (donHang != null)
                    {
                        db.GioHangs.Remove(donHang);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xóa đơn tạm: " + ex.Message);
            }

            this.Close();
        }

        private void cb_ck_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}