using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBanQuanAo
{
    public partial class HoaDon : Form
    {
        private string connectionString = "Server=MSI;Database=QuanLyBanQuanAo;Trusted_Connection=True;";

        public HoaDon()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
            LoadComboBox();
            dtpNgayLap.Value = DateTime.Now; // Mặc định ngày hiện tại
            txtTongTien.Text = "0";
        }

        // Load danh sách hóa đơn

        /*
        private void LoadHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM HoaDon";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewHoaDon.DataSource = dt;
            }
        }

        */


        private void LoadHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Sử dụng JOIN để lấy thêm HoTen từ bảng KhachHang
                string query = @"
            SELECT 
                h.MaHD, 
                h.MaKH, 
                k.HoTen AS TenKhachHang, 
                h.MaNV, 
                h.NgayLap, 
                h.TongTien 
            FROM HoaDon h
            LEFT JOIN KhachHang k ON h.MaKH = k.MaKH";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewHoaDon.DataSource = dt;

                // Tùy chỉnh tiêu đề cột (nếu cần)
                dataGridViewHoaDon.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
                dataGridViewHoaDon.Columns["MaKH"].HeaderText = "Mã Khách Hàng";
                dataGridViewHoaDon.Columns["TenKhachHang"].HeaderText = "Tên Khách Hàng";
                dataGridViewHoaDon.Columns["MaNV"].HeaderText = "Mã Nhân Viên";
                dataGridViewHoaDon.Columns["NgayLap"].HeaderText = "Ngày Lập";
                dataGridViewHoaDon.Columns["TongTien"].HeaderText = "Tổng Tiền";
            }
        }


        // Load dữ liệu cho ComboBox
        private void LoadComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Load MaKH
                SqlDataAdapter adapterKH = new SqlDataAdapter("SELECT MaKH, HoTen FROM KhachHang", conn);
                DataTable dtKH = new DataTable();
                adapterKH.Fill(dtKH);
                cboMaKH.DataSource = dtKH;
                cboMaKH.DisplayMember = "HoTen";
                cboMaKH.ValueMember = "MaKH";

                // Load MaNV
                SqlDataAdapter adapterNV = new SqlDataAdapter("SELECT MaNV, HoTen FROM NhanVien", conn);
                DataTable dtNV = new DataTable();
                adapterNV.Fill(dtNV);
                cboMaNV.DataSource = dtNV;
                cboMaNV.DisplayMember = "HoTen";
                cboMaNV.ValueMember = "MaNV";
            }
        }

        // Thêm hóa đơn
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO HoaDon (MaKH, MaNV, NgayLap, TongTien) VALUES (@MaKH, @MaNV, @NgayLap, @TongTien)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKH", cboMaKH.SelectedValue);
                cmd.Parameters.AddWithValue("@MaNV", cboMaNV.SelectedValue);
                cmd.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                cmd.Parameters.AddWithValue("@TongTien", 0); // Tổng tiền mặc định là 0, sẽ cập nhật sau khi thêm chi tiết
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm hóa đơn thành công!");
                LoadHoaDon();
            }
        }

        // Sửa hóa đơn
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridViewHoaDon.SelectedRows.Count > 0)
            {
                int maHD = Convert.ToInt32(dataGridViewHoaDon.SelectedRows[0].Cells["MaHD"].Value);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE HoaDon SET MaKH = @MaKH, MaNV = @MaNV, NgayLap = @NgayLap WHERE MaHD = @MaHD";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    cmd.Parameters.AddWithValue("@MaKH", cboMaKH.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaNV", cboMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@NgayLap", dtpNgayLap.Value);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa hóa đơn thành công!");
                    LoadHoaDon();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để sửa!");
            }
        }

        // Xóa hóa đơn
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridViewHoaDon.SelectedRows.Count > 0)
            {
                int maHD = Convert.ToInt32(dataGridViewHoaDon.SelectedRows[0].Cells["MaHD"].Value);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM HoaDon WHERE MaHD = @MaHD";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa hóa đơn thành công!");
                    LoadHoaDon();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa!");
            }
        }

        // Mở form Chi Tiết Hóa Đơn
        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (dataGridViewHoaDon.SelectedRows.Count > 0)
            {
                int maHD = Convert.ToInt32(dataGridViewHoaDon.SelectedRows[0].Cells["MaHD"].Value);
                ChiTietHoaDon formChiTiet = new ChiTietHoaDon(maHD);
                formChiTiet.ShowDialog();
                LoadHoaDon(); // Cập nhật lại tổng tiền sau khi chỉnh sửa chi tiết
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xem chi tiết!");
            }
        }

        // Hiển thị thông tin hóa đơn khi chọn trên DataGridView
        private void dataGridViewHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewHoaDon.SelectedRows.Count > 0)
            {
                cboMaKH.SelectedValue = dataGridViewHoaDon.SelectedRows[0].Cells["MaKH"].Value;
                cboMaNV.SelectedValue = dataGridViewHoaDon.SelectedRows[0].Cells["MaNV"].Value;
                dtpNgayLap.Value = Convert.ToDateTime(dataGridViewHoaDon.SelectedRows[0].Cells["NgayLap"].Value);
                txtTongTien.Text = dataGridViewHoaDon.SelectedRows[0].Cells["TongTien"].Value.ToString();
            }
        }
    }
}