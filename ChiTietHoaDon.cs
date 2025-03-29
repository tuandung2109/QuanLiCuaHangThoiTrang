using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace QuanLyBanQuanAo
{
    public partial class ChiTietHoaDon : Form
    {
        private string connectionString = "Server=MSI;Database=QuanLyBanQuanAo;Trusted_Connection=True;";
        private int maHD;

        public ChiTietHoaDon(int maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }

        private void ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            txtMaHD.Text = maHD.ToString();
            LoadChiTietHoaDon();
            cboLoaiSanPham.SelectedIndex = 0; // Mặc định chọn "Nam"
            LoadSanPham("Nam");
        }

        // Load chi tiết hóa đơn
        private void LoadChiTietHoaDon()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Sử dụng LEFT JOIN để lấy TenSanPham từ ThoiTrangNam hoặc ThoiTrangNu
                    string query = @"
                        SELECT 
                            c.MaChiTiet, 
                            c.MaHD, 
                            c.MaSanPham, 
                            CASE 
                                WHEN c.LoaiSanPham = 'Nam' THEN (SELECT TenSanPham FROM ThoiTrangNam WHERE MaSanPham = c.MaSanPham)
                                WHEN c.LoaiSanPham = 'Nữ' THEN (SELECT TenSanPham FROM ThoiTrangNu WHERE MaSanPham = c.MaSanPham)
                            END AS TenSanPham, 
                            c.LoaiSanPham, 
                            c.SoLuong, 
                            c.DonGia, 
                            c.ThanhTien
                        FROM ChiTietHoaDon c
                        WHERE c.MaHD = @MaHD";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewChiTiet.DataSource = dt;

                    // Tùy chỉnh tiêu đề cột
                    dataGridViewChiTiet.Columns["MaChiTiet"].HeaderText = "Mã Chi Tiết";
                    dataGridViewChiTiet.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
                    dataGridViewChiTiet.Columns["MaSanPham"].HeaderText = "Mã Sản Phẩm";
                    dataGridViewChiTiet.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                    dataGridViewChiTiet.Columns["LoaiSanPham"].HeaderText = "Loại Sản Phẩm";
                    dataGridViewChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
                    dataGridViewChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                    dataGridViewChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";

                    // Đặt thứ tự cột
                    dataGridViewChiTiet.Columns["MaChiTiet"].DisplayIndex = 0;
                    dataGridViewChiTiet.Columns["MaHD"].DisplayIndex = 1;
                    dataGridViewChiTiet.Columns["MaSanPham"].DisplayIndex = 2;
                    dataGridViewChiTiet.Columns["TenSanPham"].DisplayIndex = 3;
                    dataGridViewChiTiet.Columns["LoaiSanPham"].DisplayIndex = 4;
                    dataGridViewChiTiet.Columns["SoLuong"].DisplayIndex = 5;
                    dataGridViewChiTiet.Columns["DonGia"].DisplayIndex = 6;
                    dataGridViewChiTiet.Columns["ThanhTien"].DisplayIndex = 7;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message);
            }
        }

        // Load sản phẩm dựa trên loại (Nam hoặc Nữ)
        private void LoadSanPham(string loaiSanPham)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = loaiSanPham == "Nam"
                        ? "SELECT MaSanPham, TenSanPham FROM ThoiTrangNam"
                        : "SELECT MaSanPham, TenSanPham FROM ThoiTrangNu";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cboMaSanPham.DataSource = null; // Xóa datasource cũ trước khi gán mới
                    cboMaSanPham.DataSource = dt;
                    cboMaSanPham.DisplayMember = "TenSanPham";
                    cboMaSanPham.ValueMember = "MaSanPham";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message);
            }
        }

        // Khi thay đổi loại sản phẩm
        private void cboLoaiSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiSanPham.SelectedItem != null)
            {
                LoadSanPham(cboLoaiSanPham.SelectedItem.ToString());
            }
        }

        // Khi chọn sản phẩm, hiển thị đơn giá
        private void cboMaSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMaSanPham.SelectedValue != null && cboLoaiSanPham.SelectedItem != null)
                {
                    if (cboMaSanPham.SelectedValue is int maSanPham)
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = cboLoaiSanPham.SelectedItem.ToString() == "Nam"
                                ? "SELECT Gia FROM ThoiTrangNam WHERE MaSanPham = @MaSanPham"
                                : "SELECT Gia FROM ThoiTrangNu WHERE MaSanPham = @MaSanPham";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@MaSanPham", maSanPham);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                decimal donGia = Convert.ToDecimal(result);
                                txtDonGia.Text = donGia.ToString();
                                TinhThanhTien();
                            }
                            else
                            {
                                txtDonGia.Text = "0";
                                txtThanhTien.Text = "0";
                                MessageBox.Show("Không tìm thấy giá của sản phẩm này!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy đơn giá: " + ex.Message);
            }
        }

        // Tính thành tiền khi số lượng thay đổi
        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTien();
        }

        private void TinhThanhTien()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSoLuong.Text) && !string.IsNullOrEmpty(txtDonGia.Text))
                {
                    if (int.TryParse(txtSoLuong.Text, out int soLuong) && decimal.TryParse(txtDonGia.Text, out decimal donGia))
                    {
                        txtThanhTien.Text = (soLuong * donGia).ToString();
                    }
                    else
                    {
                        txtThanhTien.Text = "0";
                    }
                }
                else
                {
                    txtThanhTien.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính thành tiền: " + ex.Message);
            }
        }

        // Thêm chi tiết hóa đơn
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSoLuong.Text) || !int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!");
                    return;
                }

                if (cboMaSanPham.SelectedValue == null || cboLoaiSanPham.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong, DonGia) " +
                                   "VALUES (@MaHD, @MaSanPham, @LoaiSanPham, @SoLuong, @DonGia)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    cmd.Parameters.AddWithValue("@MaSanPham", cboMaSanPham.SelectedValue);
                    cmd.Parameters.AddWithValue("@LoaiSanPham", cboLoaiSanPham.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmd.Parameters.AddWithValue("@DonGia", decimal.Parse(txtDonGia.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm chi tiết hóa đơn thành công!");
                    LoadChiTietHoaDon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message);
            }
        }

        // Xóa chi tiết hóa đơn
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewChiTiet.SelectedRows.Count > 0)
                {
                    int maChiTiet = Convert.ToInt32(dataGridViewChiTiet.SelectedRows[0].Cells["MaChiTiet"].Value);
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM ChiTietHoaDon WHERE MaChiTiet = @MaChiTiet";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaChiTiet", maChiTiet);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa chi tiết hóa đơn thành công!");
                        LoadChiTietHoaDon();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một chi tiết để xóa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa chi tiết hóa đơn: " + ex.Message);
            }
        }

        // Cập nhật tổng tiền hóa đơn
        private void btnCapNhatTongTien_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE HoaDon SET TongTien = (SELECT ISNULL(SUM(ThanhTien), 0) FROM ChiTietHoaDon WHERE MaHD = @MaHD) WHERE MaHD = @MaHD";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật tổng tiền thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tổng tiền: " + ex.Message);
            }
        }

        // Sự kiện in báo cáo khi nhấn nút Print (button1)
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo DataSet2
                BaiTapLon2.DataSet2 ds = new BaiTapLon2.DataSet2();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn JOIN để lấy dữ liệu từ ChiTietHoaDon, HoaDon, KhachHang, và ThoiTrangNam/ThoiTrangNu
                    string query = @"
                        SELECT 
                            c.MaChiTiet, 
                            c.MaHD, 
                            c.MaSanPham, 
                            c.LoaiSanPham, 
                            c.SoLuong, 
                            c.DonGia, 
                            c.ThanhTien, 
                            h.MaKH, 
                            h.MaNV, 
                            h.NgayLap, 
                            h.TongTien,
                            k.HoTen,
                            CASE 
                                WHEN c.LoaiSanPham = 'Nam' THEN (SELECT TenSanPham FROM ThoiTrangNam WHERE MaSanPham = c.MaSanPham)
                                WHEN c.LoaiSanPham = 'Nữ' THEN (SELECT TenSanPham FROM ThoiTrangNu WHERE MaSanPham = c.MaSanPham)
                            END AS TenSanPham
                        FROM ChiTietHoaDon c
                        INNER JOIN HoaDon h ON c.MaHD = h.MaHD
                        LEFT JOIN KhachHang k ON h.MaKH = k.MaKH
                        WHERE c.MaHD = @MaHD";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@MaHD", maHD);
                    adapter.Fill(ds, "ChiTietHoaDon");

                    // Debug: Kiểm tra dữ liệu
                    if (ds.ChiTietHoaDon.Rows.Count > 0)
                    {
                        MessageBox.Show($"Số dòng: {ds.ChiTietHoaDon.Rows.Count}\n" +
                                        $"MaHD: {ds.ChiTietHoaDon.Rows[0]["MaHD"]}\n" +
                                        $"MaKH: {ds.ChiTietHoaDon.Rows[0]["MaKH"]}\n" +
                                        $"HoTen: {ds.ChiTietHoaDon.Rows[0]["HoTen"]}\n" +
                                        $"TenSanPham: {ds.ChiTietHoaDon.Rows[0]["TenSanPham"]}\n" +
                                        $"TongTien: {ds.ChiTietHoaDon.Rows[0]["TongTien"]}");
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu!");
                    }
                }

                // Tạo báo cáo CrystalReport2
                BaiTapLon2.CrystalReport2 report = new BaiTapLon2.CrystalReport2();
                report.SetDataSource(ds);

                // Mở FormPrint và hiển thị báo cáo
                BaiTapLon2.FormPrint formPrint = new BaiTapLon2.FormPrint();
                formPrint.CrystalReportViewer.ReportSource = report;
                formPrint.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in hóa đơn: " + ex.Message);
            }
        }
    }
}