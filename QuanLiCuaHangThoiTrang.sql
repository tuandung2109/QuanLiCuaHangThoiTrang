    
	   /* ĐÂY LÀ FILE GỐC   */
		  

	-- Tạo cơ sở dữ liệu
	CREATE DATABASE QuanLyBanQuanAo;

	ALTER DATABASE QuanLyBanQuanAo SET MULTI_USER;

	-- Sử dụng cơ sở dữ liệu
	USE QuanLyBanQuanAo;
	GO

	-- Tạo bảng Tài Khoản
	CREATE TABLE TaiKhoan (
		TenDangNhap NVARCHAR(50) PRIMARY KEY,
		MatKhau NVARCHAR(50) NOT NULL
	);

	INSERT INTO TaiKhoan (TenDangNhap, MatKhau)
	VALUES ('tuandung', '123');
	GO

	-- Tạo bảng Khách Hàng
	CREATE TABLE KhachHang (
		MaKH INT IDENTITY(1,1) PRIMARY KEY,
		HoTen NVARCHAR(100) NOT NULL,
		SoDienThoai VARCHAR(15) UNIQUE NOT NULL,
		Email NVARCHAR(100) UNIQUE NULL,
		DiaChi NVARCHAR(255) NULL,
		NgaySinh DATE NULL
	);
	GO

	-- Tạo bảng Nhân Viên
	CREATE TABLE NhanVien (
		MaNV INT IDENTITY(1,1) PRIMARY KEY,
		HoTen NVARCHAR(100) NOT NULL,
		ChucVu NVARCHAR(50) NOT NULL,
		SoDienThoai VARCHAR(15) UNIQUE NOT NULL,
		Email NVARCHAR(100) UNIQUE NULL,
		DiaChi NVARCHAR(255) NULL
	);
	GO

	-- Tạo bảng Thời Trang Nam
	CREATE TABLE ThoiTrangNam (
		MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
		TenSanPham NVARCHAR(150) NOT NULL,
		LoaiSanPham NVARCHAR(50) NOT NULL,
		KichCo NVARCHAR(10) NOT NULL,
		Gia DECIMAL(18,2) NOT NULL,
		HangSanXuat NVARCHAR(100) NOT NULL
	);
	GO

	-- Tạo bảng Thời Trang Nữ
	CREATE TABLE ThoiTrangNu (
		MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
		TenSanPham NVARCHAR(150) NOT NULL,
		LoaiSanPham NVARCHAR(50) NOT NULL,
		KichCo NVARCHAR(10) NOT NULL,
		Gia DECIMAL(18,2) NOT NULL,
		HangSanXuat NVARCHAR(100) NOT NULL
	);
	GO

	-- Tạo bảng Hóa Đơn
	CREATE TABLE HoaDon (
		MaHD INT IDENTITY(1,1) PRIMARY KEY,
		MaKH INT NOT NULL,
		MaNV INT NOT NULL,
		NgayLap DATE NOT NULL,
		TongTien DECIMAL(18,2) NOT NULL CHECK (TongTien >= 0),
		FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
		FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
	);
	GO

	-- Tạo bảng Chi Tiết Hóa Đơn
	CREATE TABLE ChiTietHoaDon (
		MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
		MaHD INT NOT NULL,
		MaSanPham INT NOT NULL,
		LoaiSanPham NVARCHAR(10) CHECK (LoaiSanPham IN ('Nam', 'Nữ')) NOT NULL,
		SoLuong INT NOT NULL CHECK (SoLuong > 0),
		DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
		ThanhTien AS (SoLuong * DonGia) PERSISTED,
		FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD)
	);
	GO

	-- Chèn dữ liệu vào bảng Khách Hàng
	INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiaChi)
	VALUES (N'Nguyễn Văn A', '0987654321', 'nguyenvana@example.com', N'123 Đường ABC, Hà Nội'),
		   (N'Trần Thị B', '0978123456', 'tranthib@example.com', N'456 Đường XYZ, TP. Hồ Chí Minh'),
		   (N'Lê Văn C', '0912345678', 'levanc@example.com', N'789 Đường MNP, Đà Nẵng'),
		   (N'Phạm Thị D', '0933456789', 'phamthid@example.com', N'12 Đường QRS, Hải Phòng'),
		   (N'Hoàng Văn E', '0965678901', 'hoangvane@example.com', N'34 Đường UVW, Cần Thơ'),
		   (N'Đỗ Thị F', '0986789012', 'dothif@example.com', N'56 Đường LMN, Nha Trang'),
		   (N'Bùi Văn G', '0947890123', 'buivang@example.com', N'78 Đường OPQ, Huế'),
		   (N'Ngô Thị H', '0928901234', 'ngothih@example.com', N'90 Đường RST, Đồng Nai'),
		   (N'Vũ Văn I', '0919012345', 'vuvani@example.com', N'23 Đường GHI, Bình Dương'),
		   (N'Phan Thị J', '0970123456', 'phanthij@example.com', N'45 Đường KLM, Vũng Tàu');
	GO

	-- Cập nhật cột NgaySinh cho các khách hàng hiện có
UPDATE KhachHang
SET NgaySinh = CASE MaKH
    WHEN 2 THEN '1995-08-22' -- Trần Thị B
    WHEN 3 THEN '1988-12-10' -- Lê Văn C
    WHEN 4 THEN '1993-03-25' -- Phạm Thị D
    WHEN 5 THEN '1992-07-18' -- Hoàng Văn E
    WHEN 6 THEN '1997-01-30' -- Đỗ Thị F
    WHEN 7 THEN '1991-09-05' -- Bùi Văn G
    WHEN 8 THEN '1994-11-12' -- Ngô Thị H
    WHEN 9 THEN '1989-06-20' -- Vũ Văn I
    WHEN 10 THEN '1996-04-08' -- Phan Thị J
    END
WHERE MaKH BETWEEN 1 AND 10;

-- Kiểm tra dữ liệu sau khi cập nhật
SELECT MaKH, HoTen, SoDienThoai, Email, DiaChi, NgaySinh
FROM KhachHang
ORDER BY MaKH;
GO


	-- Chèn dữ liệu vào bảng Nhân Viên
	INSERT INTO NhanVien (HoTen, ChucVu, SoDienThoai, Email, DiaChi)
	VALUES (N'Trần Thị B', N'Quản lý', '0912345678', 'tranthib@example.com', N'456 Đường XYZ, TP.HCM'),
		   (N'Lê Văn C', N'Bán hàng', '0923456789', 'levanc@example.com', N'789 Đường MNP, Đà Nẵng'),
		   (N'Phạm Thị D', N'Thu ngân', '0934567890', 'phamthid@example.com', N'12 Đường QRS, Hải Phòng'),
		   (N'Hoàng Văn E', N'Bộ phận kho', '0945678901', 'hoangvane@example.com', N'34 Đường UVW, Cần Thơ'),
		   (N'Đỗ Thị F', N'Bán hàng', '0956789012', 'dothif@example.com', N'56 Đường LMN, Nha Trang'),
		   (N'Bùi Văn G', N'Bộ phận kho', '0967890123', 'buivang@example.com', N'78 Đường OPQ, Huế'),
		   (N'Ngô Thị H', N'Thu ngân', '0978901234', 'ngothih@example.com', N'90 Đường RST, Đồng Nai'),
		   (N'Vũ Văn I', N'Bán hàng', '0989012345', 'vuvani@example.com', N'23 Đường GHI, Bình Dương'),
		   (N'Phan Thị J', N'Bộ phận kho', '0990123456', 'phanthij@example.com', N'45 Đường KLM, Vũng Tàu'),
		   (N'Nguyễn Minh K', N'Thu ngân', '0901234567', 'nguyenminhk@example.com', N'67 Đường XYZ, TP. Hà Nội');
	GO

	-- Chèn dữ liệu vào bảng Thời Trang Nam
	INSERT INTO ThoiTrangNam (TenSanPham, LoaiSanPham, KichCo, Gia, HangSanXuat)
	VALUES (N'Áo sơ mi nam', N'Áo', N'L', 350000, N'Việt Tiến'),
		   (N'Quần jeans nam', N'Quần', N'32', 450000, N'Levi''s'),
		   (N'Áo thun nam cổ tròn', N'Áo', N'M', 250000, N'Uniqlo'),
		   (N'Áo khoác da nam', N'Áo khoác', N'XL', 1200000, N'Zara'),
		   (N'Quần tây nam', N'Quần', N'30', 500000, N'An Phước'),
		   (N'Giày sneaker nam', N'Giày', N'42', 900000, N'Nike'),
		   (N'Áo hoodie nam', N'Áo khoác', N'L', 600000, N'Adidas'),
		   (N'Quần short nam', N'Quần', N'M', 300000, N'H&M'),
		   (N'Áo polo nam', N'Áo', N'L', 400000, N'Lacoste'),
		   (N'Giày tây nam', N'Giày', N'41', 1100000, N'Gucci');
	GO

	-- Chèn dữ liệu vào bảng Thời Trang Nữ
	INSERT INTO ThoiTrangNu (TenSanPham, LoaiSanPham, KichCo, Gia, HangSanXuat)
	VALUES (N'Váy đầm nữ', N'Váy', N'M', 550000, N'Elise'),
		   (N'Áo sơ mi nữ', N'Áo', N'S', 320000, N'IVY Moda'),
		   (N'Quần jeans nữ', N'Quần', N'28', 480000, N'Levi''s'),
		   (N'Chân váy bút chì', N'Váy', N'M', 600000, N'GUMAC'),
		   (N'Đầm maxi nữ', N'Váy', N'L', 750000, N'Elise'),
		   (N'Áo khoác dạ nữ', N'Áo khoác', N'XL', 1300000, N'Mango'),
		   (N'Giày cao gót nữ', N'Giày', N'37', 990000, N'Charles & Keith'),
		   (N'Quần culottes nữ', N'Quần', N'M', 450000, N'Zara'),
		   (N'Áo thun nữ basic', N'Áo', N'L', 280000, N'Uniqlo'),
		   (N'Giày sneaker nữ', N'Giày', N'38', 850000, N'Adidas');
	GO

	-- Tạo trigger TRG_SetDonGia
	CREATE TRIGGER TRG_SetDonGia
	ON ChiTietHoaDon
	INSTEAD OF INSERT
	AS
	BEGIN
		INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong, DonGia)
		SELECT 
			i.MaHD,
			i.MaSanPham,
			i.LoaiSanPham,
			i.SoLuong,
			CASE 
				WHEN i.LoaiSanPham = 'Nam' THEN (SELECT Gia FROM ThoiTrangNam WHERE MaSanPham = i.MaSanPham)
				WHEN i.LoaiSanPham = 'Nữ' THEN (SELECT Gia FROM ThoiTrangNu WHERE MaSanPham = i.MaSanPham)
				ELSE NULL
			END
		FROM inserted i
		WHERE 
			(i.LoaiSanPham = 'Nam' AND EXISTS (SELECT 1 FROM ThoiTrangNam WHERE MaSanPham = i.MaSanPham))
			OR (i.LoaiSanPham = 'Nữ' AND EXISTS (SELECT 1 FROM ThoiTrangNu WHERE MaSanPham = i.MaSanPham));
	END;
	GO

	-- Tạo trigger TRG_UpdateTongTien
	CREATE TRIGGER TRG_UpdateTongTien
	ON ChiTietHoaDon
	AFTER INSERT
	AS
	BEGIN
		UPDATE HoaDon
		SET TongTien = (
			SELECT SUM(ThanhTien)
			FROM ChiTietHoaDon
			WHERE ChiTietHoaDon.MaHD = HoaDon.MaHD
		)
		FROM HoaDon
		INNER JOIN inserted i ON HoaDon.MaHD = i.MaHD;
	END;
	GO

	-- Chèn dữ liệu vào bảng Hóa Đơn
	INSERT INTO HoaDon (MaKH, MaNV, NgayLap, TongTien)
	VALUES (1, 1, '2025-03-19', 0),
		   (2, 2, '2025-03-19', 0),
		   (3, 3, '2025-03-20', 0); -- Thêm hóa đơn cho MaHD = 3
	GO

	-- Chèn dữ liệu vào bảng ChiTietHoaDon cho hóa đơn đầu tiên (MaHD = 1)
	INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong)
	VALUES (1, 1, 'Nam', 2),  -- 2 Áo sơ mi nam
		   (1, 2, 'Nam', 1),  -- 1 Quần jeans nam
		   (1, 3, 'Nam', 3);  -- 3 Áo thun nam cổ tròn
	GO

	-- Chèn dữ liệu vào bảng ChiTietHoaDon cho hóa đơn thứ hai (MaHD = 2)
	INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong)
	VALUES (2, 1, 'Nữ', 1),  -- 1 Váy đầm nữ
		   (2, 5, 'Nữ', 2),  -- 2 Đầm maxi nữ
		   (2, 6, 'Nữ', 1);  -- 1 Áo khoác dạ nữ
	GO

	-- Chèn dữ liệu vào bảng ChiTietHoaDon cho hóa đơn thứ ba (MaHD = 3)
	INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong)
	VALUES (3, 1, 'Nam', 2),  -- 2 Áo sơ mi nam
		   (3, 6, 'Nữ', 1);  -- 1 Áo khoác dạ nữ
	GO

	INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong)
	VALUES (3, 1, 'Nữ', 1);   -- 1 Váy đầm nữ (Gia = 550000 từ ThoiTrangNu)
	GO
	INSERT INTO ChiTietHoaDon (MaHD, MaSanPham, LoaiSanPham, SoLuong)
	VALUES(3, 2, 'Nam', 1);  -- 1 Quần jeans nam (Gia = 450000 từ ThoiTrangNam)
	GO


	-- Kiểm tra dữ liệu
	SELECT * FROM ChiTietHoaDon;
	SELECT * FROM HoaDon;




-- Kiểm tra dữ liệu sau khi thêm
SELECT MaNV, HoTen, ChucVu, SoDienThoai, Email, DiaChi, MaSoThue
FROM NhanVien
ORDER BY MaNV;


ALTER TABLE NhanVien
ADD MaSoThue CHAR(10);


UPDATE NhanVien
SET MaSoThue = CASE MaNV
    WHEN 1 THEN '1234567890'
    WHEN 2 THEN '2345678901'
    WHEN 3 THEN '3456789012'
    WHEN 4 THEN '4567890123'
    WHEN 5 THEN '5678901234'
    WHEN 6 THEN '6789012345'
    WHEN 7 THEN '7890123456'
    WHEN 8 THEN '8901234567'
    WHEN 9 THEN '9012345678'
    WHEN 10 THEN '0123456789'
    END
WHERE MaNV BETWEEN 1 AND 10;

