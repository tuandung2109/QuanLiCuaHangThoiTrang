namespace QuanLyBanQuanAo
{
    partial class ChiTietHoaDon
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblMaHD = new System.Windows.Forms.Label();
            this.txtMaHD = new System.Windows.Forms.TextBox();
            this.dataGridViewChiTiet = new System.Windows.Forms.DataGridView();
            this.cboLoaiSanPham = new System.Windows.Forms.ComboBox();
            this.cboMaSanPham = new System.Windows.Forms.ComboBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.txtThanhTien = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnCapNhatTongTien = new System.Windows.Forms.Button();
            this.lblLoaiSanPham = new System.Windows.Forms.Label();
            this.lblMaSanPham = new System.Windows.Forms.Label();
            this.lblSoLuong = new System.Windows.Forms.Label();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.lblThanhTien = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChiTiet)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMaHD
            // 
            this.lblMaHD.AutoSize = true;
            this.lblMaHD.Location = new System.Drawing.Point(30, 20);
            this.lblMaHD.Name = "lblMaHD";
            this.lblMaHD.Size = new System.Drawing.Size(44, 13);
            this.lblMaHD.TabIndex = 0;
            this.lblMaHD.Text = "Mã HD:";
            // 
            // txtMaHD
            // 
            this.txtMaHD.Location = new System.Drawing.Point(100, 17);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.ReadOnly = true;
            this.txtMaHD.Size = new System.Drawing.Size(100, 20);
            this.txtMaHD.TabIndex = 1;
            // 
            // dataGridViewChiTiet
            // 
            this.dataGridViewChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChiTiet.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewChiTiet.Name = "dataGridViewChiTiet";
            this.dataGridViewChiTiet.RowHeadersWidth = 51;
            this.dataGridViewChiTiet.Size = new System.Drawing.Size(760, 150);
            this.dataGridViewChiTiet.TabIndex = 2;
            // 
            // cboLoaiSanPham
            // 
            this.cboLoaiSanPham.FormattingEnabled = true;
            this.cboLoaiSanPham.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
            this.cboLoaiSanPham.Location = new System.Drawing.Point(100, 220);
            this.cboLoaiSanPham.Name = "cboLoaiSanPham";
            this.cboLoaiSanPham.Size = new System.Drawing.Size(100, 21);
            this.cboLoaiSanPham.TabIndex = 3;
            this.cboLoaiSanPham.SelectedIndexChanged += new System.EventHandler(this.cboLoaiSanPham_SelectedIndexChanged);
            // 
            // cboMaSanPham
            // 
            this.cboMaSanPham.FormattingEnabled = true;
            this.cboMaSanPham.Location = new System.Drawing.Point(100, 250);
            this.cboMaSanPham.Name = "cboMaSanPham";
            this.cboMaSanPham.Size = new System.Drawing.Size(150, 21);
            this.cboMaSanPham.TabIndex = 4;
            this.cboMaSanPham.SelectedIndexChanged += new System.EventHandler(this.cboMaSanPham_SelectedIndexChanged);
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(100, 280);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(100, 20);
            this.txtSoLuong.TabIndex = 5;
            this.txtSoLuong.TextChanged += new System.EventHandler(this.txtSoLuong_TextChanged);
            // 
            // txtDonGia
            // 
            this.txtDonGia.Location = new System.Drawing.Point(100, 310);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.ReadOnly = true;
            this.txtDonGia.Size = new System.Drawing.Size(100, 20);
            this.txtDonGia.TabIndex = 6;
            // 
            // txtThanhTien
            // 
            this.txtThanhTien.Location = new System.Drawing.Point(100, 340);
            this.txtThanhTien.Name = "txtThanhTien";
            this.txtThanhTien.ReadOnly = true;
            this.txtThanhTien.Size = new System.Drawing.Size(100, 20);
            this.txtThanhTien.TabIndex = 7;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(300, 220);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 23);
            this.btnThem.TabIndex = 8;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(300, 250);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 23);
            this.btnXoa.TabIndex = 9;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnCapNhatTongTien
            // 
            this.btnCapNhatTongTien.Location = new System.Drawing.Point(300, 280);
            this.btnCapNhatTongTien.Name = "btnCapNhatTongTien";
            this.btnCapNhatTongTien.Size = new System.Drawing.Size(75, 23);
            this.btnCapNhatTongTien.TabIndex = 10;
            this.btnCapNhatTongTien.Text = "Cập nhật TT";
            this.btnCapNhatTongTien.UseVisualStyleBackColor = true;
            this.btnCapNhatTongTien.Click += new System.EventHandler(this.btnCapNhatTongTien_Click);
            // 
            // lblLoaiSanPham
            // 
            this.lblLoaiSanPham.AutoSize = true;
            this.lblLoaiSanPham.Location = new System.Drawing.Point(30, 223);
            this.lblLoaiSanPham.Name = "lblLoaiSanPham";
            this.lblLoaiSanPham.Size = new System.Drawing.Size(47, 13);
            this.lblLoaiSanPham.TabIndex = 11;
            this.lblLoaiSanPham.Text = "Loại SP:";
            // 
            // lblMaSanPham
            // 
            this.lblMaSanPham.AutoSize = true;
            this.lblMaSanPham.Location = new System.Drawing.Point(30, 253);
            this.lblMaSanPham.Name = "lblMaSanPham";
            this.lblMaSanPham.Size = new System.Drawing.Size(42, 13);
            this.lblMaSanPham.TabIndex = 12;
            this.lblMaSanPham.Text = "Mã SP:";
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.AutoSize = true;
            this.lblSoLuong.Location = new System.Drawing.Point(30, 283);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(52, 13);
            this.lblSoLuong.TabIndex = 13;
            this.lblSoLuong.Text = "Số lượng:";
            // 
            // lblDonGia
            // 
            this.lblDonGia.AutoSize = true;
            this.lblDonGia.Location = new System.Drawing.Point(30, 313);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(47, 13);
            this.lblDonGia.TabIndex = 14;
            this.lblDonGia.Text = "Đơn giá:";
            // 
            // lblThanhTien
            // 
            this.lblThanhTien.AutoSize = true;
            this.lblThanhTien.Location = new System.Drawing.Point(30, 343);
            this.lblThanhTien.Name = "lblThanhTien";
            this.lblThanhTien.Size = new System.Drawing.Size(61, 13);
            this.lblThanhTien.TabIndex = 15;
            this.lblThanhTien.Text = "Thành tiền:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(292, 309);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 29);
            this.button1.TabIndex = 16;
            this.button1.Text = "Print";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChiTietHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 381);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblThanhTien);
            this.Controls.Add(this.lblDonGia);
            this.Controls.Add(this.lblSoLuong);
            this.Controls.Add(this.lblMaSanPham);
            this.Controls.Add(this.lblLoaiSanPham);
            this.Controls.Add(this.btnCapNhatTongTien);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.txtThanhTien);
            this.Controls.Add(this.txtDonGia);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.cboMaSanPham);
            this.Controls.Add(this.cboLoaiSanPham);
            this.Controls.Add(this.dataGridViewChiTiet);
            this.Controls.Add(this.txtMaHD);
            this.Controls.Add(this.lblMaHD);
            this.Name = "ChiTietHoaDon";
            this.Text = "Chi Tiết Hóa Đơn";
            this.Load += new System.EventHandler(this.ChiTietHoaDon_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChiTiet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMaHD;
        private System.Windows.Forms.TextBox txtMaHD;
        private System.Windows.Forms.DataGridView dataGridViewChiTiet;
        private System.Windows.Forms.ComboBox cboLoaiSanPham;
        private System.Windows.Forms.ComboBox cboMaSanPham;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.TextBox txtThanhTien;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnCapNhatTongTien;
        private System.Windows.Forms.Label lblLoaiSanPham;
        private System.Windows.Forms.Label lblMaSanPham;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.Label lblThanhTien;
        private System.Windows.Forms.Button button1;
    }
}