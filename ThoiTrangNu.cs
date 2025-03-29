using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLon2
{
    public partial class ThoiTrangNu : Form
    {
        public ThoiTrangNu()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
        }

        string connectionString = "Data Source=MSI;Initial Catalog=QuanLyBanQuanAo;Integrated Security=True";

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaSanPham, TenSanPham, LoaiSanPham, KichCo, Gia, HangSanXuat FROM ThoiTrangNu";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
        }

        private void ThoiTrangNu_Load(object sender, EventArgs e)
        {
            // Thêm các loại sản phẩm vào comboBox1
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Áo");
            comboBox1.Items.Add("Quần");
            comboBox1.Items.Add("Váy");
            comboBox1.Items.Add("Giày");
            comboBox1.Items.Add("Phụ kiện");

            // Đặt giá trị mặc định
            comboBox1.SelectedIndex = 0;

            // Tải dữ liệu từ database
            LoadData();
        }

        private int selectedProductID = -1; // Biến lưu mã sản phẩm được chọn
        // Xử lý khi click vào một hàng trong DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Lấy giá trị MaSanPham từ cột đầu tiên
                selectedProductID = Convert.ToInt32(row.Cells["MaSanPham"].Value);

                textBox1.Text = row.Cells["TenSanPham"].Value.ToString();
                comboBox1.Text = row.Cells["LoaiSanPham"].Value.ToString();
                textBox3.Text = row.Cells["KichCo"].Value.ToString();
                textBox2.Text = row.Cells["Gia"].Value.ToString();
                textBox4.Text = row.Cells["HangSanXuat"].Value.ToString();
            }
        }

        // Xử lý khi nhấn button1 để thêm dữ liệu
        private void button1_Click(object sender, EventArgs e)
        {
            string tenSanPham = textBox1.Text.Trim();
            string loaiSanPham = comboBox1.Text.Trim();
            string kichCo = textBox3.Text.Trim();
            string gia = textBox2.Text.Trim();
            string hangSanXuat = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(tenSanPham) || string.IsNullOrEmpty(loaiSanPham) ||
                string.IsNullOrEmpty(kichCo) || string.IsNullOrEmpty(gia) || string.IsNullOrEmpty(hangSanXuat))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(gia, out decimal giaSanPham))
            {
                MessageBox.Show("Giá sản phẩm phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO ThoiTrangNu (TenSanPham, LoaiSanPham, KichCo, Gia, HangSanXuat) " +
                                   "VALUES (@TenSanPham, @LoaiSanPham, @KichCo, @Gia, @HangSanXuat)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenSanPham", tenSanPham);
                    cmd.Parameters.AddWithValue("@LoaiSanPham", loaiSanPham);
                    cmd.Parameters.AddWithValue("@KichCo", kichCo);
                    cmd.Parameters.AddWithValue("@Gia", giaSanPham);
                    cmd.Parameters.AddWithValue("@HangSanXuat", hangSanXuat);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Cập nhật lại DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedProductID == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenSanPham = textBox1.Text.Trim();
            string loaiSanPham = comboBox1.Text.Trim();
            string kichCo = textBox3.Text.Trim();
            string gia = textBox2.Text.Trim();
            string hangSanXuat = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(tenSanPham) || string.IsNullOrEmpty(loaiSanPham) ||
                string.IsNullOrEmpty(kichCo) || string.IsNullOrEmpty(gia) || string.IsNullOrEmpty(hangSanXuat))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(gia, out decimal giaSanPham))
            {
                MessageBox.Show("Giá sản phẩm phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE ThoiTrangNu SET TenSanPham=@TenSanPham, LoaiSanPham=@LoaiSanPham, " +
                                   "KichCo=@KichCo, Gia=@Gia, HangSanXuat=@HangSanXuat WHERE MaSanPham=@MaSanPham";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenSanPham", tenSanPham);
                    cmd.Parameters.AddWithValue("@LoaiSanPham", loaiSanPham);
                    cmd.Parameters.AddWithValue("@KichCo", kichCo);
                    cmd.Parameters.AddWithValue("@Gia", giaSanPham);
                    cmd.Parameters.AddWithValue("@HangSanXuat", hangSanXuat);
                    cmd.Parameters.AddWithValue("@MaSanPham", selectedProductID);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Cập nhật lại DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedProductID == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM ThoiTrangNu WHERE MaSanPham = @MaSanPham";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSanPham", selectedProductID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Cập nhật lại DataGridView
                        selectedProductID = -1; // Đặt lại ID sản phẩm về -1 sau khi xóa
                    }
                    else
                    {
                        MessageBox.Show("Xóa sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox5.Text.Trim(), out int productID))
            {
                MessageBox.Show("Vui lòng nhập một số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaSanPham, TenSanPham, LoaiSanPham, KichCo, Gia, HangSanXuat FROM ThoiTrangNu WHERE MaSanPham = @MaSanPham";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@MaSanPham", productID);

                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Nếu không tìm thấy, load lại toàn bộ dữ liệu
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}