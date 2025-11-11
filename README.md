# 🪟 ĐỒ ÁN LẬP TRÌNH .NET - QUẢN LÝ SINH VIÊN

## 👨‍💻 Thông tin nhóm

| Họ và tên        | MSSV       | Lớp      |
|------------------|------------|-----------|
| **Phó Bảo Phong** | **DTH235731** | **DH24TH2** |
| **Tống Nhựt Nam** | **DTH2357**   | **DH24TH2** |


---

## 📘 Môn học
- **Tên môn:** Lập trình .NET  
- **Đề tài:** Xây dựng ứng dụng **Quản lý Sinh viên** bằng **Windows Forms**  
- **Ngôn ngữ:** C# (.NET Framework)  

---

## 🎯 Mục tiêu đồ án
- Xây dựng **ứng dụng quản lý sinh viên** có giao diện trực quan, thân thiện và dễ sử dụng.  
- Áp dụng các kỹ năng:
  - Lập trình hướng đối tượng (OOP)  
  - Thiết kế giao diện với **Windows Forms**  
  - Kết nối và thao tác dữ liệu từ **SQL Server**  
  - Xử lý CRUD và tra cứu dữ liệu trực quan  

---

## ✨ Tính năng chính
Ứng dụng cho phép **Admin** thực hiện quản lý khoa, lớp, môn học và sinh viên

Ứng dụng cho phép **Sinh viên** tra cứu thông tin cá nhân và điểm số các môn học cũng như điểm rèn luyện các học kỳ

### 🔐 Đăng nhập
- Hệ thống phân quyền và **phân tách giao diện** dành cho Admin.  

### 🗂️ Quản lý Danh mục (CRUD)
- **Quản lý Khoa** – Form: `frmQuanLyKhoa`  
- **Quản lý Lớp** – Form: `frmQuanLyLop` *(có ComboBox liên kết Khoa)*  
- **Quản lý Môn học** – Form: `frmQuanLyMonHoc` *(có NumericUpDown cho Tín chỉ)*  

### 🛠️ Quản lý Nghiệp vụ (CRUD)
- **Quản lý Sinh viên** – Form: `frmQuanLySinhVien` *(Form phức tạp nhất, có nhiều điều khiển và liên kết dữ liệu)*  
- **Quản lý Điểm Tích Lũy** – Form: `frmDiemTichLuy`  
- **Quản lý Điểm Rèn Luyện** – Form: `frmDiemRenLuyen`  

### 📊 Tra cứu
- **Form:** `frmTraCuuDiem`  
- Chức năng tra cứu **bảng điểm chi tiết của từng sinh viên**  

---

## ⚙️ Công nghệ sử dụng
- **Ngôn ngữ:** C#  
- **Môi trường phát triển:** Visual Studio  
- **Cơ sở dữ liệu:** Microsoft SQL Server  
- **Kiến trúc:** Windows Forms + ADO.NET  
- **Kết nối:** DataSet / SqlConnection / SqlCommand  
