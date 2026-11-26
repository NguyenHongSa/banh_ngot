using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace banh_ngot
{
    public partial class QUAN_LI_KHO : Form
    {
        private bool isUpdatingCombo = false;
        private string nguoiDangNhap = "Hệ thống";
        private List<BanhDTO> dsSanPham = new List<BanhDTO>();

        // Biến lưu mã phiếu đang được chọn để sửa (-1 nghĩa là không ở chế độ sửa)
        private int maPhieuDangSua = -1;

        public QUAN_LI_KHO()
        {
            InitializeComponent();
        }

        public QUAN_LI_KHO(string tenUser) : this()
        {
            if (!string.IsNullOrEmpty(tenUser))
            {
                this.nguoiDangNhap = tenUser;
            }
        }

        private void QUAN_LI_KHO_Load(object sender, EventArgs e)
        {
            // Ép buộc nối sự kiện bằng code (tránh lỗi Designer chưa gán)
            cb_ten.SelectedIndexChanged -= cb_ten_SelectedIndexChanged;
            cb_ten.SelectedIndexChanged += cb_ten_SelectedIndexChanged;

            cb_m.SelectedIndexChanged -= cb_m_SelectedIndexChanged;
            cb_m.SelectedIndexChanged += cb_m_SelectedIndexChanged;

            // Nối sự kiện cho 3 nút mới (nếu trong Designer chưa gán)
            if (bt_them != null) { bt_them.Click -= bt_them_Click; bt_them.Click += bt_them_Click; }
            if (bt_sua != null) { bt_sua.Click -= bt_sua_Click; bt_sua.Click += bt_sua_Click; }
            if (bt_xoa != null) { bt_xoa.Click -= bt_xoa_Click; bt_xoa.Click += bt_xoa_Click; }

            LoadComboBoxData();
            LoadDataHoaDonKho();
            GanSuKienTinhTien();
        }

        // Tải danh sách bánh & danh mục vào các ComboBox
        private void LoadComboBoxData()
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    dsSanPham = db.SanPhams.Select(s => new BanhDTO
                    {
                        MaBanh = s.MaBanh.Trim(),
                        TenBanh = s.TenBanh.Trim(),
                        DonViTinh = s.DonViTinh != null ? s.DonViTinh.Trim() : "",
                        MaDanhMuc = s.MaDanhMuc != null ? s.MaDanhMuc.Trim() : "",
                        DonGia = s.DonGiaNhap ?? s.DonGiaBan ?? 0
                    }).ToList();

                    isUpdatingCombo = true;

                    // Load Mã bánh
                    cb_m.DisplayMember = "MaBanh";
                    cb_m.ValueMember = "MaBanh";
                    cb_m.DataSource = dsSanPham.ToList();
                    cb_m.SelectedIndex = -1;

                    // Load Tên bánh
                    cb_ten.DisplayMember = "TenBanh";
                    cb_ten.ValueMember = "MaBanh";
                    cb_ten.DataSource = dsSanPham.ToList();
                    cb_ten.SelectedIndex = -1;

                    // Load Đơn vị tính
                    var dsDVT = dsSanPham.Select(s => s.DonViTinh).Where(d => !string.IsNullOrEmpty(d)).Distinct().ToList();
                    cb_dv.DataSource = dsDVT;
                    cb_dv.SelectedIndex = -1;

                    // Load Danh mục bánh
                    var dsDanhMuc = dsSanPham.Select(s => s.MaDanhMuc).Where(d => !string.IsNullOrEmpty(d)).Distinct().ToList();
                    cb_dm.DataSource = dsDanhMuc;
                    cb_dm.SelectedIndex = -1;

                    isUpdatingCombo = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách bánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cb_ten_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingCombo || cb_ten.SelectedIndex == -1) return;

            var selectedItem = cb_ten.SelectedItem as BanhDTO;
            if (selectedItem != null)
            {
                HienThiThongTinBanh(selectedItem, tuCbTen: true);
            }
        }

        private void cb_m_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingCombo || cb_m.SelectedIndex == -1) return;

            var selectedItem = cb_m.SelectedItem as BanhDTO;
            if (selectedItem != null)
            {
                HienThiThongTinBanh(selectedItem, tuCbTen: false);
            }
        }

        private void HienThiThongTinBanh(BanhDTO item, bool tuCbTen)
        {
            isUpdatingCombo = true;

            if (tuCbTen)
            {
                cb_m.SelectedValue = item.MaBanh;
            }
            else
            {
                cb_ten.SelectedValue = item.MaBanh;
            }

            cb_dv.Text = item.DonViTinh;
            cb_dm.Text = item.MaDanhMuc;
            txt_gia.Text = item.DonGia.ToString("G0");

            isUpdatingCombo = false;

            TinhTongTien(null, null);
        }

        public void LoadDataHoaDonKho()
        {
            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var dsHoaDon = (from hd in db.HoaDonKhoes
                                    join sp in db.SanPhams on hd.MaBanh.Trim() equals sp.MaBanh.Trim() into spGroup
                                    from sp in spGroup.DefaultIfEmpty()
                                    select new
                                    {
                                        MaPhieu = hd.MaPhieu,
                                        PhanLoai = hd.PhanLoai ?? hd.LoaiPhieu,
                                        NguoiTao = hd.NguoiTao ?? "N/A",
                                        MaBanh = hd.MaBanh,
                                        TenBanh = sp != null ? sp.TenBanh : "N/A",
                                        DanhMuc = hd.DanhMuc ?? (sp != null ? sp.MaDanhMuc : "N/A"),
                                        DonViTinh = hd.DonViTinh,
                                        SoLuong = hd.SoLuong,
                                        DonGia = hd.DonGia,
                                        TongTien = hd.TongTien,
                                        NgayNhapXuat = hd.NgayNhapXuat
                                    }).OrderByDescending(h => h.NgayNhapXuat).ToList();

                    dt_don.DataSource = null;
                    dt_don.DataSource = dsHoaDon;

                    if (dt_don.Columns["MaPhieu"] != null) dt_don.Columns["MaPhieu"].HeaderText = "Mã Phiếu";
                    if (dt_don.Columns["PhanLoai"] != null) dt_don.Columns["PhanLoai"].HeaderText = "Phân Loại";
                    if (dt_don.Columns["NguoiTao"] != null) dt_don.Columns["NguoiTao"].HeaderText = "Người Tạo";
                    if (dt_don.Columns["MaBanh"] != null) dt_don.Columns["MaBanh"].HeaderText = "Mã Bánh";
                    if (dt_don.Columns["TenBanh"] != null) dt_don.Columns["TenBanh"].HeaderText = "Tên Bánh";
                    if (dt_don.Columns["DanhMuc"] != null) dt_don.Columns["DanhMuc"].HeaderText = "Danh Mục";
                    if (dt_don.Columns["DonViTinh"] != null) dt_don.Columns["DonViTinh"].HeaderText = "Đơn Vị Tính";
                    if (dt_don.Columns["SoLuong"] != null) dt_don.Columns["SoLuong"].HeaderText = "Số Lượng";
                    if (dt_don.Columns["DonGia"] != null) dt_don.Columns["DonGia"].HeaderText = "Đơn Giá";
                    if (dt_don.Columns["TongTien"] != null) dt_don.Columns["TongTien"].HeaderText = "Tổng Tiền";
                    if (dt_don.Columns["NgayNhapXuat"] != null) dt_don.Columns["NgayNhapXuat"].HeaderText = "Ngày Thực Hiện";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GanSuKienTinhTien()
        {
            txt_sl.TextChanged -= TinhTongTien;
            txt_sl.TextChanged += TinhTongTien;

            txt_gia.TextChanged -= TinhTongTien;
            txt_gia.TextChanged += TinhTongTien;
        }

        private void TinhTongTien(object sender, EventArgs e)
        {
            if (int.TryParse(txt_sl.Text.Trim(), out int sl) && decimal.TryParse(txt_gia.Text.Trim(), out decimal gia))
            {
                txt_t.Text = (sl * gia).ToString("N0");
            }
            else
            {
                txt_t.Text = "0";
            }
        }

        private void bt_nhapkho_Click(object sender, EventArgs e)
        {
            XuLyKho("Nhập kho");
        }

        private void bt_xuatkho_Click(object sender, EventArgs e)
        {
            XuLyKho("Xuất kho");
        }

        private void XuLyKho(string loaiPhieu)
        {
            string maBanh = cb_m.Text.Trim();
            string tenBanh = cb_ten.Text.Trim();
            string dvt = cb_dv.Text.Trim();
            string danhMuc = cb_dm.Text.Trim();

            if (string.IsNullOrEmpty(maBanh))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập Mã Bánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txt_sl.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txt_gia.Text.Trim(), out decimal donGia) || donGia < 0)
            {
                MessageBox.Show("Giá sản phẩm không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tongTien = soLuong * donGia;

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == maBanh);
                    if (sp == null)
                    {
                        MessageBox.Show("Mã bánh chưa có trong CSDL! Vui lòng nhấn nút [THÊM SẢN PHẨM] để tạo sản phẩm mới trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (loaiPhieu == "Xuất kho" && (sp.SoLuongKho ?? 0) < soLuong)
                    {
                        MessageBox.Show($"Tồn kho không đủ! (Tồn hiện tại: {sp.SoLuongKho ?? 0})", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    HoaDonKho hd = new HoaDonKho
                    {
                        PhanLoai = loaiPhieu,
                        LoaiPhieu = loaiPhieu,
                        MaBanh = maBanh,
                        DonViTinh = dvt,
                        DanhMuc = string.IsNullOrEmpty(danhMuc) ? sp.MaDanhMuc : danhMuc,
                        SoLuong = soLuong,
                        DonGia = donGia,
                        TongTien = tongTien,
                        NgayNhapXuat = DateTime.Now,
                        NguoiTao = this.nguoiDangNhap
                    };
                    db.HoaDonKhoes.Add(hd);

                    if (loaiPhieu == "Nhập kho")
                    {
                        sp.SoLuongKho = (sp.SoLuongKho ?? 0) + soLuong;
                        sp.DaNhap = (sp.DaNhap ?? 0) + soLuong;
                    }
                    else if (loaiPhieu == "Xuất kho")
                    {
                        sp.SoLuongKho = (sp.SoLuongKho ?? 0) - soLuong;
                        sp.DaBan = (sp.DaBan ?? 0) + soLuong;
                    }

                    db.SaveChanges();

                    MessageBox.Show($"Đã {loaiPhieu.ToLower()} thành công bởi [{this.nguoiDangNhap}]!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                    LoadDataHoaDonKho();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu kho: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== 1. THÊM SẢN PHẨM MỚI VÀO CSDL ====================
        private void bt_them_Click(object sender, EventArgs e)
        {
            string maBanh = cb_m.Text.Trim();
            string tenBanh = cb_ten.Text.Trim();
            string dvt = cb_dv.Text.Trim();
            string danhMuc = cb_dm.Text.Trim();

            if (string.IsNullOrEmpty(maBanh) || string.IsNullOrEmpty(tenBanh))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Bánh và Tên Bánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Đọc số lượng và đơn giá từ giao diện
            int.TryParse(txt_sl.Text.Trim(), out int soLuong);
            decimal.TryParse(txt_gia.Text.Trim(), out decimal donGia);

            if (soLuong < 0) soLuong = 0;
            if (donGia < 0) donGia = 0;

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var existingSp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim().ToLower() == maBanh.ToLower());
                    if (existingSp != null)
                    {
                        MessageBox.Show("Mã bánh này đã tồn tại trong CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 1. Lưu SoLuongKho và DaNhap đúng với số lượng nhập từ Form
                    SanPham spMoi = new SanPham
                    {
                        MaBanh = maBanh,
                        TenBanh = tenBanh,
                        DonViTinh = dvt,
                        MaDanhMuc = danhMuc,
                        DonGiaNhap = donGia,
                        DonGiaBan = donGia,
                        SoLuongKho = soLuong, // Lưu đúng số lượng nhập vào tồn kho
                        DaNhap = soLuong,     // Lưu đúng số lượng nhập vào Đã nhập
                        DaBan = 0
                    };
                    db.SanPhams.Add(spMoi);

                    // 2. Tạo phiếu ghi nhận trong HoaDonKho
                    HoaDonKho hdMoi = new HoaDonKho
                    {
                        PhanLoai = "Sản phẩm mới",
                        LoaiPhieu = "Sản phẩm mới",
                        MaBanh = maBanh,
                        DonViTinh = dvt,
                        DanhMuc = danhMuc,
                        SoLuong = soLuong,
                        DonGia = donGia,
                        TongTien = soLuong * donGia,
                        NgayNhapXuat = DateTime.Now,
                        NguoiTao = this.nguoiDangNhap
                    };
                    db.HoaDonKhoes.Add(hdMoi);

                    db.SaveChanges();

                    MessageBox.Show($"Đã thêm sản phẩm mới [{tenBanh}] thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadComboBoxData();
                    LoadDataHoaDonKho();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ==================== 2. SỬA HÓA ĐƠN (CLICK LẦN 1: BẮT ĐẦU SỬA, CLICK LẦN 2: LƯU) ====================
        private void bt_sua_Click(object sender, EventArgs e)
        {
            // LẦN 1: Nếu chưa ở chế độ sửa -> Lấy dòng đang chọn trên DataGridView để chuẩn bị sửa
            if (maPhieuDangSua == -1)
            {
                if (dt_don.CurrentRow == null || dt_don.CurrentRow.Index < 0 || dt_don.CurrentRow.Cells["MaPhieu"].Value == null)
                {
                    MessageBox.Show("Vui lòng chọn một hóa đơn/phiếu từ danh sách bên phải để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                maPhieuDangSua = Convert.ToInt32(dt_don.CurrentRow.Cells["MaPhieu"].Value);
                bt_sua.Text = "XÁC NHẬN SỬA";
                MessageBox.Show($"Đang trong chế độ sửa Mã Phiếu: {maPhieuDangSua}.\nHãy thay đổi các thông tin bên trái rồi bấm lại nút [XÁC NHẬN SỬA] để hoàn tất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // LẦN 2: Đã ở chế độ sửa -> Lưu các thay đổi vào CSDL
            string maBanhMoi = cb_m.Text.Trim();
            string dvtMoi = cb_dv.Text.Trim();
            string danhMucMoi = cb_dm.Text.Trim();

            if (!int.TryParse(txt_sl.Text.Trim(), out int soLuongMoi) || soLuongMoi <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txt_gia.Text.Trim(), out decimal donGiaMoi) || donGiaMoi < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new QuanlytiembanhEntities())
                {
                    var hd = db.HoaDonKhoes.FirstOrDefault(h => h.MaPhieu == maPhieuDangSua);
                    if (hd == null)
                    {
                        MessageBox.Show("Không tìm thấy phiếu cần sửa trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetTrangThaiSua();
                        return;
                    }

                    // 1. Hoàn trả số lượng kho của phiếu CŨ trước khi sửa
                    var spCu = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == hd.MaBanh.Trim());
                    if (spCu != null)
                    {
                        if (hd.PhanLoai == "Nhập kho" || hd.LoaiPhieu == "Nhập kho")
                        {
                            spCu.SoLuongKho = (spCu.SoLuongKho ?? 0) - hd.SoLuong;
                            spCu.DaNhap = (spCu.DaNhap ?? 0) - hd.SoLuong;
                        }
                        else if (hd.PhanLoai == "Xuất kho" || hd.LoaiPhieu == "Xuất kho")
                        {
                            spCu.SoLuongKho = (spCu.SoLuongKho ?? 0) + hd.SoLuong;
                            spCu.DaBan = (spCu.DaBan ?? 0) - hd.SoLuong;
                        }
                    }

                    // 2. Kiểm tra và áp dụng số lượng kho MỚI
                    var spMoi = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == maBanhMoi);
                    if (spMoi == null)
                    {
                        MessageBox.Show("Mã bánh mới chọn không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if ((hd.PhanLoai == "Xuất kho" || hd.LoaiPhieu == "Xuất kho") && (spMoi.SoLuongKho ?? 0) < soLuongMoi)
                    {
                        MessageBox.Show($"Số lượng xuất mới vượt quá tồn kho khả dụng ({spMoi.SoLuongKho ?? 0})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cập nhật kho mới
                    if (hd.PhanLoai == "Nhập kho" || hd.LoaiPhieu == "Nhập kho")
                    {
                        spMoi.SoLuongKho = (spMoi.SoLuongKho ?? 0) + soLuongMoi;
                        spMoi.DaNhap = (spMoi.DaNhap ?? 0) + soLuongMoi;
                    }
                    else if (hd.PhanLoai == "Xuất kho" || hd.LoaiPhieu == "Xuất kho")
                    {
                        spMoi.SoLuongKho = (spMoi.SoLuongKho ?? 0) - soLuongMoi;
                        spMoi.DaBan = (spMoi.DaBan ?? 0) + soLuongMoi;
                    }

                    // 3. Cập nhật thông tin phiếu
                    hd.MaBanh = maBanhMoi;
                    hd.DonViTinh = dvtMoi;
                    hd.DanhMuc = danhMucMoi;
                    hd.SoLuong = soLuongMoi;
                    hd.DonGia = donGiaMoi;
                    hd.TongTien = soLuongMoi * donGiaMoi;

                    db.SaveChanges();

                    MessageBox.Show($"Sửa thành công phiếu [{maPhieuDangSua}]!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetTrangThaiSua();
                    ClearForm();
                    LoadDataHoaDonKho();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetTrangThaiSua()
        {
            maPhieuDangSua = -1;
            bt_sua.Text = "SỬA HÓA ĐƠN";
        }

        // ==================== 3. XÓA HÓA ĐƠN VÀ HUỶ BỎ SỐ LƯỢNG TỒN KHO ====================
        private void bt_xoa_Click(object sender, EventArgs e)
        {
            if (dt_don.CurrentRow == null || dt_don.CurrentRow.Index < 0 || dt_don.CurrentRow.Cells["MaPhieu"].Value == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn/phiếu từ bảng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPhieu = Convert.ToInt32(dt_don.CurrentRow.Cells["MaPhieu"].Value);

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa Phiếu [{maPhieu}] không?\n(Tồn kho của sản phẩm sẽ được tự động điều chỉnh lại!)",
                                                 "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (var db = new QuanlytiembanhEntities())
                    {
                        var hd = db.HoaDonKhoes.FirstOrDefault(h => h.MaPhieu == maPhieu);
                        if (hd != null)
                        {
                            // Trả lại tồn kho cho bảng SanPham
                            var sp = db.SanPhams.FirstOrDefault(s => s.MaBanh.Trim() == hd.MaBanh.Trim());
                            if (sp != null)
                            {
                                if (hd.PhanLoai == "Nhập kho" || hd.LoaiPhieu == "Nhập kho")
                                {
                                    sp.SoLuongKho = (sp.SoLuongKho ?? 0) - hd.SoLuong;
                                    sp.DaNhap = (sp.DaNhap ?? 0) - hd.SoLuong;
                                }
                                else if (hd.PhanLoai == "Xuất kho" || hd.LoaiPhieu == "Xuất kho")
                                {
                                    sp.SoLuongKho = (sp.SoLuongKho ?? 0) + hd.SoLuong;
                                    sp.DaBan = (sp.DaBan ?? 0) - hd.SoLuong;
                                }
                            }

                            db.HoaDonKhoes.Remove(hd);
                            db.SaveChanges();

                            MessageBox.Show($"Đã xóa thành công phiếu [{maPhieu}]!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ResetTrangThaiSua();
                            ClearForm();
                            LoadDataHoaDonKho();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dt_don_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dt_don.Rows[e.RowIndex].Cells["MaBanh"].Value != null)
            {
                DataGridViewRow row = dt_don.Rows[e.RowIndex];

                isUpdatingCombo = true;
                string maBanh = row.Cells["MaBanh"].Value?.ToString().Trim();
                cb_m.SelectedValue = maBanh;
                cb_ten.SelectedValue = maBanh;

                cb_dv.Text = row.Cells["DonViTinh"].Value?.ToString().Trim();

                if (dt_don.Columns["DanhMuc"] != null)
                {
                    cb_dm.Text = row.Cells["DanhMuc"].Value?.ToString().Trim();
                }
                isUpdatingCombo = false;

                txt_sl.Text = row.Cells["SoLuong"].Value?.ToString();
                txt_gia.Text = row.Cells["DonGia"].Value?.ToString();
                txt_t.Text = row.Cells["TongTien"].Value?.ToString();
            }
        }

        private void ClearForm()
        {
            isUpdatingCombo = true;
            cb_m.SelectedIndex = -1;
            cb_ten.SelectedIndex = -1;
            cb_dv.SelectedIndex = -1;
            cb_dm.SelectedIndex = -1;

            cb_m.Text = "";
            cb_ten.Text = "";
            cb_dv.Text = "";
            cb_dm.Text = "";
            isUpdatingCombo = false;

            txt_sl.Clear();
            txt_gia.Clear();
            txt_t.Clear();
        }

        private void bt_trolai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void cb_dv_SelectedIndexChanged(object sender, EventArgs e) { }

        private void bt_them_Click_1(object sender, EventArgs e)
        {

        }
    }

    public class BanhDTO
    {
        public string MaBanh { get; set; }
        public string TenBanh { get; set; }
        public string DonViTinh { get; set; }
        public string MaDanhMuc { get; set; }
        public decimal DonGia { get; set; }
    }
}