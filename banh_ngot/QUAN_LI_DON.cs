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
    public partial class QUAN_LI_DON : Form
    {
        // Biến lưu trữ trạng thái lọc và sắp xếp hiện tại
        private string currentFilterStatus = "Tất cả";
        private bool isAscendingDate = false; // false: Mới nhất trước, true: Cũ nhất trước

        public QUAN_LI_DON()
        {
            InitializeComponent();
        }

        private void QUAN_LI_DON_Load(object sender, EventArgs e)
        {
            LoadDanhSachDonHang();
        }

        // 1. Hàm tải danh sách TẤT CẢ các đơn hàng trong CSDL
        public void LoadDanhSachDonHang(string maTimKiem = "")
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsGioHang = db.GioHangs.ToList();
                    var dsSanPham = db.SanPhams.ToList();

                    // Lấy toàn bộ danh sách đơn hàng
                    var query = dsGioHang.AsEnumerable();

                    // Lọc theo trạng thái được chọn từ nút Tùy chọn (nếu khác "Tất cả")
                    if (!currentFilterStatus.Equals("Tất cả", StringComparison.OrdinalIgnoreCase))
                    {
                        query = query.Where(gh => gh.TrangThai != null &&
                                                  gh.TrangThai.Trim().Equals(currentFilterStatus, StringComparison.OrdinalIgnoreCase));
                    }

                    // Lọc theo Mã đơn hàng nếu có nhập tìm kiếm
                    if (!string.IsNullOrEmpty(maTimKiem))
                    {
                        string tk = maTimKiem.Trim();
                        query = query.Where(gh => gh.MaGioHang.ToString().Contains(tk));
                    }

                    var querySelect = (from gh in query
                                       join sp in dsSanPham
                                       on (gh.MaBanh ?? "").Trim().ToLower()
                                       equals (sp.MaBanh ?? "").Trim().ToLower() into groupSP
                                       from sp in groupSP.DefaultIfEmpty()
                                       select new
                                       {
                                           MaGioHang = gh.MaGioHang,
                                           TenDangNhap = (gh.TenDangNhap ?? "").Trim(),
                                           MaBanh = (gh.MaBanh ?? "").Trim(),
                                           TenBanh = sp != null ? sp.TenBanh : "N/A",
                                           SoLuong = gh.SoLuong ?? 0,
                                           DonGia = sp != null ? (sp.DonGiaBan ?? 0) : 0,
                                           ThanhTien = (gh.SoLuong ?? 0) * (sp != null ? (sp.DonGiaBan ?? 0) : 0),
                                           PhuongThucThanhToan = gh.PhuongThucThanhToan ?? "Thanh toán khi nhận hàng",
                                           TrangThai = gh.TrangThai != null ? gh.TrangThai.Trim() : "",
                                           // Lấy cột NgayDat từ CSDL (nếu NgayDat bị null thì lấy NgayThem)
                                           NgayDat = gh.NgayDat ?? gh.NgayThem
                                       });

                    // Sắp xếp theo ngày đặt (Mới nhất hoặc Cũ nhất)
                    var danhSachHienThi = isAscendingDate
                        ? querySelect.OrderBy(x => x.NgayDat).ToList()
                        : querySelect.OrderByDescending(x => x.NgayDat).ToList();

                    // Hiển thị lên DataGridView
                    dt_don.DataSource = null;
                    dt_don.DataSource = danhSachHienThi;

                    // Đặt tiêu đề tiếng Việt cho các cột
                    if (dt_don.Columns["MaGioHang"] != null) dt_don.Columns["MaGioHang"].HeaderText = "Mã Đơn";
                    if (dt_don.Columns["TenDangNhap"] != null) dt_don.Columns["TenDangNhap"].HeaderText = "Khách Hàng";
                    if (dt_don.Columns["MaBanh"] != null) dt_don.Columns["MaBanh"].HeaderText = "Mã Bánh";
                    if (dt_don.Columns["TenBanh"] != null) dt_don.Columns["TenBanh"].HeaderText = "Tên Bánh";
                    if (dt_don.Columns["SoLuong"] != null) dt_don.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dt_don.Columns["DonGia"] != null) dt_don.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dt_don.Columns["ThanhTien"] != null) dt_don.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                    if (dt_don.Columns["PhuongThucThanhToan"] != null) dt_don.Columns["PhuongThucThanhToan"].HeaderText = "Phương Thức TT";
                    if (dt_don.Columns["TrangThai"] != null) dt_don.Columns["TrangThai"].HeaderText = "Trạng Thái";

                    // Định dạng cột Ngày Đặt thành Ngày/Tháng/Năm Giờ:Phút
                    if (dt_don.Columns["NgayDat"] != null)
                    {
                        dt_don.Columns["NgayDat"].HeaderText = "Ngày Đặt";
                        dt_don.Columns["NgayDat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }

                    dt_don.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. NÚT DUYỆT ĐƠN (bt_dd)
        private void bt_dd_Click(object sender, EventArgs e)
        {
            if (dt_don.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần duyệt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maGioHang = Convert.ToInt32(dt_don.CurrentRow.Cells["MaGioHang"].Value);
            string trangThaiHienTai = dt_don.CurrentRow.Cells["TrangThai"].Value?.ToString().Trim() ?? "";

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var donHang = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                    if (donHang != null)
                    {
                        if (trangThaiHienTai.Equals("Yêu cầu hủy", StringComparison.OrdinalIgnoreCase))
                        {
                            // Chấp nhận yêu cầu hủy của khách
                            donHang.TrangThai = "Đã hủy";
                            MessageBox.Show($"Đã chấp nhận yêu cầu hủy đơn [{maGioHang}]!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (trangThaiHienTai.Equals("Đã đặt hàng", StringComparison.OrdinalIgnoreCase) ||
                                 trangThaiHienTai.Equals("Chờ đặt hàng", StringComparison.OrdinalIgnoreCase))
                        {
                            // Duyệt đơn -> Đổi trạng thái sang "Đang giao"
                            donHang.TrangThai = "Đang giao";
                            MessageBox.Show($"Đã duyệt đơn hàng [{maGioHang}]! Đơn đã chuyển sang trạng thái 'Đang giao'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Đơn hàng đang ở trạng thái '{trangThaiHienTai}', không cần duyệt lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        db.SaveChanges();
                        LoadDanhSachDonHang(txt_tk.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi duyệt đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. NÚT TỪ CHỐI (bt_tc)
        private void bt_tc_Click(object sender, EventArgs e)
        {
            if (dt_don.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng cần từ chối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maGioHang = Convert.ToInt32(dt_don.CurrentRow.Cells["MaGioHang"].Value);
            string trangThaiHienTai = dt_don.CurrentRow.Cells["TrangThai"].Value?.ToString() ?? "";

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var donHang = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                    if (donHang != null)
                    {
                        if (trangThaiHienTai.Equals("Yêu cầu hủy", StringComparison.OrdinalIgnoreCase))
                        {
                            // Từ chối yêu cầu hủy của khách -> Trả lại trạng thái "Đang giao"
                            donHang.TrangThai = "Đang giao";
                            MessageBox.Show($"Đã từ chối yêu cầu hủy của đơn [{maGioHang}]. Đơn chuyển về trạng thái 'Đang giao'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Từ chối/Hủy trực tiếp đơn hàng mới -> Đổi trạng thái thành "Đã hủy"
                            donHang.TrangThai = "Đã hủy";
                            MessageBox.Show($"Đã từ chối đơn hàng [{maGioHang}]!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        db.SaveChanges();
                        LoadDanhSachDonHang(txt_tk.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi từ chối đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. NÚT XEM CHI TIẾT ĐƠN HÀNG (bt_ct)
        private void bt_ct_Click(object sender, EventArgs e)
        {
            if (dt_don.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maGioHang = Convert.ToInt32(dt_don.CurrentRow.Cells["MaGioHang"].Value);

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dh = db.GioHangs.FirstOrDefault(g => g.MaGioHang == maGioHang);
                    if (dh == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin chi tiết đơn hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string maBanh = dh.MaBanh != null ? dh.MaBanh.Trim() : "";
                    var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == maBanh);
                    string tenBanh = sp != null ? sp.TenBanh : "N/A";
                    decimal donGia = sp != null ? (sp.DonGiaBan ?? 0) : 0;
                    DateTime ngayDatHienThi = dh.NgayDat ?? dh.NgayThem ?? DateTime.Now;

                    Form formChiTiet = new Form
                    {
                        Text = $"CHI TIẾT ĐƠN HÀNG #{dh.MaGioHang}",
                        Size = new Size(420, 540),
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        MaximizeBox = false,
                        MinimizeBox = false
                    };

                    RichTextBox rtbChiTiet = new RichTextBox
                    {
                        Dock = DockStyle.Top,
                        Height = 420,
                        ReadOnly = true,
                        Font = new Font("Consolas", 10, FontStyle.Regular)
                    };

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("==========================================");
                    sb.AppendLine("           CHI TIẾT ĐƠN HÀNG              ");
                    sb.AppendLine("==========================================");
                    sb.AppendLine($"Mã đơn hàng : {dh.MaGioHang}");
                    sb.AppendLine($"Tài khoản   : {dh.TenDangNhap}");
                    sb.AppendLine($"Ngày đặt    : {ngayDatHienThi:dd/MM/yyyy HH:mm}");
                    sb.AppendLine($"Khách hàng  : {dh.HoTen}");
                    sb.AppendLine($"Số điện thoại: {dh.SDT}");
                    sb.AppendLine($"Địa chỉ     : {dh.DiaChi}");
                    sb.AppendLine("------------------------------------------");
                    sb.AppendLine($"Sản phẩm    : {tenBanh}");
                    sb.AppendLine($"Số lượng    : {dh.SoLuong}");
                    sb.AppendLine($"Đơn giá     : {donGia:N0} VNĐ");
                    sb.AppendLine($"Phí vận chuyển: {(dh.PhiShip ?? 0):N0} VNĐ");
                    sb.AppendLine("------------------------------------------");
                    sb.AppendLine($"TỔNG TIỀN   : {(dh.TongTien ?? ((dh.SoLuong ?? 0) * donGia + (dh.PhiShip ?? 0))):N0} VNĐ");
                    sb.AppendLine($"Thanh toán  : {dh.PhuongThucThanhToan}");
                    sb.AppendLine($"Trạng thái  : {dh.TrangThai}");
                    sb.AppendLine("==========================================");

                    rtbChiTiet.Text = sb.ToString();

                    Button btnXuatExcel = new Button
                    {
                        Text = "Xuất Excel (.csv)",
                        Size = new Size(140, 35),
                        Location = new Point(130, 440)
                    };

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
                    formChiTiet.Controls.Add(rtbChiTiet);
                    formChiTiet.Controls.Add(btnXuatExcel);
                    formChiTiet.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 5. NÚT TÙY CHỌN (bt_t) - SẮP XẾP VÀ LỌC ĐƠN
        private void bt_t_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menuTuyChon = new ContextMenuStrip();

            // Nhóm sắp xếp
            ToolStripMenuItem itemMoiNhat = new ToolStripMenuItem("Sắp xếp: Mới nhất xếp trước");
            itemMoiNhat.Click += (s, ev) => { isAscendingDate = false; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            ToolStripMenuItem itemCuNhat = new ToolStripMenuItem("Sắp xếp: Cũ nhất xếp trước");
            itemCuNhat.Click += (s, ev) => { isAscendingDate = true; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            // Nhóm lọc trạng thái
            ToolStripMenuItem itemTatCa = new ToolStripMenuItem("Hiển thị: Tất cả đơn hàng");
            itemTatCa.Click += (s, ev) => { currentFilterStatus = "Tất cả"; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            ToolStripMenuItem itemChoGiao = new ToolStripMenuItem("Lọc: Chờ giao hàng");
            itemChoGiao.Click += (s, ev) => { currentFilterStatus = "Chờ giao hàng"; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            ToolStripMenuItem itemYeuCauHuy = new ToolStripMenuItem("Lọc: Yêu cầu hủy");
            itemYeuCauHuy.Click += (s, ev) => { currentFilterStatus = "Yêu cầu hủy"; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            ToolStripMenuItem itemDangGiao = new ToolStripMenuItem("Lọc: Đang giao");
            itemDangGiao.Click += (s, ev) => { currentFilterStatus = "Đang giao"; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            ToolStripMenuItem itemDaNhan = new ToolStripMenuItem("Lọc: Đã nhận hàng");
            itemDaNhan.Click += (s, ev) => { currentFilterStatus = "Đã nhận hàng"; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            ToolStripMenuItem itemDaHuy = new ToolStripMenuItem("Lọc: Đã hủy");
            itemDaHuy.Click += (s, ev) => { currentFilterStatus = "Đã hủy"; LoadDanhSachDonHang(txt_tk.Text.Trim()); };

            // Thêm các mục vào Menu
            menuTuyChon.Items.Add(itemMoiNhat);
            menuTuyChon.Items.Add(itemCuNhat);
            menuTuyChon.Items.Add(new ToolStripSeparator());
            menuTuyChon.Items.Add(itemTatCa);
            menuTuyChon.Items.Add(itemChoGiao);
            menuTuyChon.Items.Add(itemYeuCauHuy);
            menuTuyChon.Items.Add(itemDangGiao);
            menuTuyChon.Items.Add(itemDaNhan);
            menuTuyChon.Items.Add(itemDaHuy);

            menuTuyChon.Show(bt_t, new Point(0, bt_t.Height));
        }

        // 6. NÚT TÌM KIẾM ĐƠN THEO MÃ (bt_tk)
        private void bt_tk_Click(object sender, EventArgs e)
        {
            string maCanTim = txt_tk.Text.Trim();
            LoadDanhSachDonHang(maCanTim);
        }

        // 7. NÚT TRỜ LẠI (bt_tl)
        private void bt_tl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}