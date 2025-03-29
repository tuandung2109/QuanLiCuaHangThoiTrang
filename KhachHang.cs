using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BaiTapLon2
{
    public partial class KhachHang : Form
    {
        private string connectionString = "Server=MSI;Database=QuanLyBanQuanAo;Integrated Security=True;";

        public KhachHang()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            LoadData();

            // Gán sự kiện CellClick
            dataGridView1.CellClick += dataGridView1_CellClick;

            // Gán sự kiện cho các button
            button1.Click += button1_Click; // Thêm
            button2.Click += button2_Click; // Sửa
            button3.Click += button3_Click; // Xóa
            button4.Click += button4_Click; // Tìm kiếm
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaKH, HoTen, SoDienThoai, Email, DiaChi, NgaySinh FROM KhachHang";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra xem có dòng nào được chọn không
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Gán giá trị từ DataGridView vào các điều khiển tương ứng
                textBox1.Text = row.Cells["HoTen"].Value?.ToString();
                textBox2.Text = row.Cells["SoDienThoai"].Value?.ToString();
                textBox3.Text = row.Cells["Email"].Value?.ToString();
                textBox4.Text = row.Cells["DiaChi"].Value?.ToString();

                // Kiểm tra nếu cột NgaySinh có dữ liệu thì gán vào DateTimePicker
                if (row.Cells["NgaySinh"].Value != DBNull.Value && row.Cells["NgaySinh"].Value != null)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                }
                else
                {
                    dateTimePicker1.Value = DateTime.Now; // Nếu trống thì đặt mặc định là ngày hiện tại
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và Số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiaChi, NgaySinh) " +
                                   "VALUES (@HoTen, @SoDienThoai, @Email, @DiaChi, @NgaySinh)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@HoTen", textBox1.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", dateTimePicker1.Value == DateTime.Now ? (object)DBNull.Value : dateTimePicker1.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData(); // Tải lại dữ liệu
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maKH = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaKH"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE KhachHang SET HoTen = @HoTen, SoDienThoai = @SoDienThoai, Email = @Email, " +
                                   "DiaChi = @DiaChi, NgaySinh = @NgaySinh WHERE MaKH = @MaKH";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        cmd.Parameters.AddWithValue("@HoTen", textBox1.Text);
                        cmd.Parameters.AddWithValue("@SoDienThoai", textBox2.Text);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(textBox3.Text) ? (object)DBNull.Value : textBox3.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text);
                        cmd.Parameters.AddWithValue("@NgaySinh", dateTimePicker1.Value == DateTime.Now ? (object)DBNull.Value : dateTimePicker1.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Sửa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData(); // Tải lại dữ liệu
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maKH = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaKH"].Value);
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM KhachHang WHERE MaKH = @MaKH";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", maKH);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData(); // Tải lại dữ liệu
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                LoadData(); // Nếu ô tìm kiếm trống thì hiển thị lại toàn bộ dữ liệu
                return;
            }

            if (!int.TryParse(textBox5.Text, out int maKH))
            {
                MessageBox.Show("Mã khách hàng phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaKH, HoTen, SoDienThoai, Email, DiaChi, NgaySinh FROM KhachHang WHERE MaKH = @MaKH";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@MaKH", maKH);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng với mã này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}