using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class LICH_SU_MUA_HANG : Form
    {
        private string tenDangNhapHienTai = "";

        public LICH_SU_MUA_HANG()
        {
            InitializeComponent();
        }

        public LICH_SU_MUA_HANG(string tenDangNhap)
        {
            InitializeComponent();
            this.tenDangNhapHienTai = tenDangNhap != null ? tenDangNhap.Trim() : "";
        }

        private void LICH_SU_MUA_HANG_Load(object sender, EventArgs e)
        {
            LoadDataLichSu("", "tất cả"); // Mặc định hiển thị TẤT CẢ lịch sử khi mở form
        }

        private void LICH_SU_MUA_HANG_Load_1(object sender, EventArgs e)
        {
            LoadDataLichSu("", "tất cả");
        }

        // 1. Tải dữ liệu lịch sử mua hàng
        public void LoadDataLichSu(string maTimKiem = "", string locTrangThai = "tất cả")
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsGioHang = db.GioHangs.ToList();
                    var dsSanPham = db.SanPhams.ToList();

                    // LOẠI BỎ "Chờ đặt hàng" khỏi Lịch sử
                    var query = dsGioHang.Where(g => !string.IsNullOrEmpty(g.TrangThai) &&
                                                     !g.TrangThai.Trim().Equals("Chờ đặt hàng", StringComparison.OrdinalIgnoreCase)).AsEnumerable();

                    // Lọc theo người dùng đăng nhập
                    if (!string.IsNullOrEmpty(tenDangNhapHienTai))
                    {
                        query = query.Where(g => g.TenDangNhap != null &&
                                                 g.TenDangNhap.Trim().Equals(tenDangNhapHienTai.Trim(), StringComparison.OrdinalIgnoreCase));
                    }

                    // Lọc theo trạng thái
                    if (!string.IsNullOrEmpty(locTrangThai) && !locTrangThai.Equals("tất cả", StringComparison.OrdinalIgnoreCase))
                    {
                        query = query.Where(g => g.TrangThai.ToLower().Contains(locTrangThai.ToLower()));
                    }

                    // Lọc theo Mã đơn hàng
                    if (!string.IsNullOrEmpty(maTimKiem))
                    {
                        string tk = maTimKiem.Trim();
                        query = query.Where(g => g.MaGioHang.ToString().Contains(tk));
                    }

                    var danhSachHienThi = (from gh in query
                                           join sp in dsSanPham
                                           on (gh.MaBanh ?? "").Trim().ToLower()
                                           equals (sp.MaBanh ?? "").Trim().ToLower() into groupSP
                                           from sp in groupSP.DefaultIfEmpty()
                                           select new
                                           {
                                               MaGioHang = gh.MaGioHang,
                                               MaBanh = (gh.MaBanh ?? "").Trim(),
                                               TenBanh = sp != null ? sp.TenBanh : "N/A",
                                               DonGia = sp != null ? (sp.DonGiaBan ?? 0) : 0,
                                               SoLuong = gh.SoLuong ?? 0,
                                               TongTien = gh.TongTien ?? ((gh.SoLuong ?? 0) * (sp != null ? (sp.DonGiaBan ?? 0) : 0)),
                                               PhuongThucThanhToan = gh.PhuongThucThanhToan ?? "N/A",
                                               TrangThai = gh.TrangThai,
                                               NgayDat = gh.NgayDat ?? gh.NgayThem
                                           }).OrderByDescending(g => g.NgayDat).ToList();

                    // Hiển thị lên DataGridView
                    dt_gh.DataSource = null;
                    dt_gh.Columns.Clear();
                    dt_gh.AutoGenerateColumns = true;
                    dt_gh.DataSource = danhSachHienThi;
                    dt_gh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dt_gh.Columns["MaGioHang"] != null) dt_gh.Columns["MaGioHang"].HeaderText = "Mã Đơn";
                    if (dt_gh.Columns["MaBanh"] != null) dt_gh.Columns["MaBanh"].HeaderText = "Mã Bánh";
                    if (dt_gh.Columns["TenBanh"] != null) dt_gh.Columns["TenBanh"].HeaderText = "Tên Bánh";
                    if (dt_gh.Columns["DonGia"] != null) dt_gh.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dt_gh.Columns["SoLuong"] != null) dt_gh.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dt_gh.Columns["TongTien"] != null) dt_gh.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    if (dt_gh.Columns["PhuongThucThanhToan"] != null) dt_gh.Columns["PhuongThucThanhToan"].HeaderText = "Thanh Toán";
                    if (dt_gh.Columns["TrangThai"] != null) dt_gh.Columns["TrangThai"].HeaderText = "Trạng Thái";
                    if (dt_gh.Columns["NgayDat"] != null) dt_gh.Columns["NgayDat"].HeaderText = "Ngày Đặt";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử mua hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Nút Tìm kiếm
        private void bt_tk_Click(object sender, EventArgs e)
        {
            string maCanTim = txt_tk.Text.Trim();
            LoadDataLichSu(maCanTim, "tất cả");
        }

        // 3. Nút Xem chi tiết & Xuất Excel
        private void bt_ct_Click(object sender, EventArgs e)
        {
            if (dt_gh.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maGioHang = Convert.ToInt32(dt_gh.CurrentRow.Cells["MaGioHang"].Value);

            using (var db = new QuanlytiembanhEntities())
            {
                var dh = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                if (dh == null) return;

                string maBanh = dh.MaBanh != null ? dh.MaBanh.Trim() : "";
                var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == maBanh);
                string tenBanh = sp != null ? sp.TenBanh : maBanh;
                decimal donGia = sp != null ? (sp.DonGiaBan ?? 0) : 0;

                Form formHoaDon = new Form
                {
                    Text = "CHI TIẾT LỊCH SỬ ĐƠN HÀNG",
                    Size = new Size(420, 520),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                RichTextBox rtbHoaDon = new RichTextBox
                {
                    Dock = DockStyle.Top,
                    Height = 400,
                    ReadOnly = true,
                    Font = new Font("Consolas", 10, FontStyle.Regular)
                };

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("==========================================");
                sb.AppendLine("           CHI TIẾT ĐƠN HÀNG              ");
                sb.AppendLine("==========================================");
                sb.AppendLine($"Mã đơn hàng : {dh.MaGioHang}");
                sb.AppendLine($"Ngày đặt    : {dh.NgayDat:dd/MM/yyyy HH:mm}");
                sb.AppendLine($"Khách hàng  : {dh.HoTen}");
                sb.AppendLine($"Số điện thoại: {dh.SDT}");
                sb.AppendLine($"Địa chỉ     : {dh.DiaChi}");
                sb.AppendLine("------------------------------------------");
                sb.AppendLine($"Sản phẩm    : {tenBanh}");
                sb.AppendLine($"Số lượng    : {dh.SoLuong}");
                sb.AppendLine($"Đơn giá     : {donGia:N0} VNĐ");
                sb.AppendLine($"Phí vận chuyển: {(dh.PhiShip ?? 0):N0} VNĐ");
                sb.AppendLine("------------------------------------------");
                sb.AppendLine($"TỔNG TIỀN   : {(dh.TongTien ?? 0):N0} VNĐ");
                sb.AppendLine($"Thanh toán  : {dh.PhuongThucThanhToan}");
                sb.AppendLine($"Trạng thái  : {dh.TrangThai}");
                sb.AppendLine("==========================================");

                rtbHoaDon.Text = sb.ToString();

                Button btnXuatExcel = new Button
                {
                    Text = "Xuất Excel (.csv)",
                    Size = new Size(140, 35),
                    Location = new Point(130, 420)
                };

                // ĐÃ SỬA: Xuất file CSV chuẩn chia cột bằng dấu phẩy và bao quanh bằng ngoặc kép
                // Logic xuất Excel (.csv) có tiêu đề HÓA ĐƠN BÁN HÀNG ở trên cùng
                btnXuatExcel.Click += (s, ev) =>
                {
                    SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "Excel CSV File (*.csv)|*.csv",
                        FileName = $"HoaDon_{dh.MaGioHang}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                            {
                                // 1. Ghi tiêu đề HÓA ĐƠN BÁN HÀNG ở dòng đầu tiên
                                sw.WriteLine("\"HÓA ĐƠN BÁN HÀNG\"");
                                sw.WriteLine(); // Dòng trống phân cách tiêu đề với dữ liệu

                                // 2. Trích xuất dữ liệu từ đối tượng dh
                                string maDon = dh.MaGioHang.ToString();
                                string ngayDat = dh.NgayDat.HasValue ? dh.NgayDat.Value.ToString("dd/MM/yyyy HH:mm") : DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                string khachHang = !string.IsNullOrEmpty(dh.HoTen) ? dh.HoTen : "";
                                string sdt = !string.IsNullOrEmpty(dh.SDT) ? dh.SDT : "";
                                string diaChi = !string.IsNullOrEmpty(dh.DiaChi) ? dh.DiaChi : "";
                                int soLuong = dh.SoLuong ?? 1;
                                string phiShip = (dh.PhiShip ?? 0).ToString("#,##0") + " VNĐ";
                                string tongTien = (dh.TongTien ?? 0).ToString("#,##0") + " VNĐ";
                                string thanhToan = !string.IsNullOrEmpty(dh.PhuongThucThanhToan) ? dh.PhuongThucThanhToan : "";
                                string trangThai = !string.IsNullOrEmpty(dh.TrangThai) ? dh.TrangThai : "";

                                // 3. Ghi dữ liệu chi tiết theo từng dòng
                                sw.WriteLine($"\"Mã đơn hàng\",\"{maDon}\"");
                                sw.WriteLine($"\"Ngày đặt\",\"{ngayDat}\"");
                                sw.WriteLine($"\"Khách hàng\",\"{khachHang}\"");
                                sw.WriteLine($"\"Số điện thoại\",=\"{sdt}\""); // Dùng ="" để giữ nguyên số 0 đầu
                                sw.WriteLine($"\"Địa chỉ\",\"{diaChi}\"");
                                sw.WriteLine($"\"Mã bánh\",\"{maBanh}\"");
                                sw.WriteLine($"\"Sản phẩm\",\"{tenBanh}\"");
                                sw.WriteLine($"\"Số lượng\",\"{soLuong}\"");
                                sw.WriteLine($"\"Phí vận chuyển\",\"{phiShip}\"");
                                sw.WriteLine($"\"TỔNG TIỀN\",\"{tongTien}\"");
                                sw.WriteLine($"\"Thanh toán\",\"{thanhToan}\"");
                                sw.WriteLine($"\"Trạng thái\",\"{trangThai}\"");
                            }

                            MessageBox.Show("Xuất hóa đơn ra file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception exEx)
                        {
                            MessageBox.Show("Lỗi khi xuất file: " + exEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                };

                formHoaDon.Controls.Add(rtbHoaDon);
                formHoaDon.Controls.Add(btnXuatExcel);
                formHoaDon.ShowDialog();
            }
        }

        // 4. Nút Tổng hợp / Lọc theo nhóm
        private void bt_th_Click(object sender, EventArgs e)
        {
            using (var db = new QuanlytiembanhEntities())
            {
                var dsGioHang = db.GioHangs.Where(g => g.TrangThai != null && !g.TrangThai.Trim().Equals("Chờ đặt hàng", StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(tenDangNhapHienTai))
                {
                    dsGioHang = dsGioHang.Where(g => g.TenDangNhap != null && g.TenDangNhap.Trim().Equals(tenDangNhapHienTai, StringComparison.OrdinalIgnoreCase));
                }

                var list = dsGioHang.ToList();
                int tongDon = list.Count;
                int donDaDat = list.Count(g => g.TrangThai.ToLower().Equals("đã đặt hàng") || g.TrangThai.ToLower().Contains("đã đặt"));
                int donDaGiao = list.Count(g => g.TrangThai.ToLower().Contains("giao") || g.TrangThai.ToLower().Contains("nhận") || g.TrangThai.ToLower().Contains("nhan"));
                int donDaHuy = list.Count(g => g.TrangThai.ToLower().Contains("hủy") || g.TrangThai.ToLower().Contains("huy"));

                Form frmTongHop = new Form
                {
                    Text = "TỔNG HỢP & SẮP XẾP ĐƠN HÀNG",
                    Size = new Size(380, 320),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                Label lblThongKe = new Label
                {
                    Text = $"TỔNG SỐ ĐƠN HÀNG: {tongDon}\n\n• Đơn đã đặt: {donDaDat}\n• Đơn đã nhận/giao: {donDaGiao}\n• Đơn đã hủy: {donDaHuy}",
                    Location = new Point(30, 20),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                Label lblChon = new Label
                {
                    Text = "Chọn loại đơn muốn lọc/sắp xếp:",
                    Location = new Point(30, 130),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                };

                Button btnDaGiao = new Button { Text = "Đã nhận hàng", Location = new Point(30, 160), Size = new Size(100, 35) };
                Button btnDaHuy = new Button { Text = "Đơn đã hủy", Location = new Point(135, 160), Size = new Size(100, 35) };
                Button btnDaDat = new Button { Text = "Đơn đã đặt", Location = new Point(240, 160), Size = new Size(100, 35) };
                Button btnTatCa = new Button { Text = "Xem tất cả", Location = new Point(30, 210), Size = new Size(310, 35) };

                btnDaGiao.Click += (s, ev) => { LoadDataLichSu("", "nhận"); frmTongHop.Close(); };
                btnDaHuy.Click += (s, ev) => { LoadDataLichSu("", "hủy"); frmTongHop.Close(); };
                btnDaDat.Click += (s, ev) => { LoadDataLichSu("", "đã đặt"); frmTongHop.Close(); };
                btnTatCa.Click += (s, ev) => { LoadDataLichSu("", "tất cả"); frmTongHop.Close(); };

                frmTongHop.Controls.Add(lblThongKe);
                frmTongHop.Controls.Add(lblChon);
                frmTongHop.Controls.Add(btnDaGiao);
                frmTongHop.Controls.Add(btnDaHuy);
                frmTongHop.Controls.Add(btnDaDat);
                frmTongHop.Controls.Add(btnTatCa);

                frmTongHop.ShowDialog();
            }
        }

        // 5. Nút Trở lại
        private void bt_tl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}