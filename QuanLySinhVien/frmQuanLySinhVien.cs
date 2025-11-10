using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frmQuanLySinhVien : Form
    {
        Database db;
        string a = "";
        public frmQuanLySinhVien()
        {
            InitializeComponent();
            db = new Database();
        }

        private void LoadComboBoxLop()
        {
            try
            {
                DataTable dtLop = db.GetData("SELECT MaLop, TenLop FROM Lop");
                cboLop.DataSource = dtLop;
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Lớp: " + ex.Message);
            }
        }

        private void LoadData(string maLop)
        {
            try
            {
                string sql = $"SELECT * FROM SinhVien WHERE MaLop = '{maLop}'";
                DataTable dtSinhVien = db.GetData(sql);

                dgvSinhVien.DataSource = dtSinhVien;

                dgvSinhVien.Columns["NgaySinh"].Visible = false;
                dgvSinhVien.Columns["DiaChi"].Visible = false;
                dgvSinhVien.Columns["SoDienThoai"].Visible = false;
                dgvSinhVien.Columns["Email"].Visible = false;
                dgvSinhVien.Columns["MaLop"].Visible = false;

                dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
                dgvSinhVien.Columns["HoTen"].HeaderText = "Họ Tên";
                dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới tính";

                dgvSinhVien.Columns["HoTen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sinh viên: " + ex.Message);
            }
        }
        private void frmQuanLySinhVien_Load(object sender, EventArgs e)
        {
            LoadComboBoxLop();

            txtMaSV.Enabled = false;
            txtHoTen.Enabled = false;
            dtpNgaySinh.Enabled = false;
            radNam.Enabled = false;
            radNu.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSoDienThoai.Enabled = false;
            txtEmail.Enabled = false;

            btnXacNhan.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboLop.SelectedValue != null)
            {
                string maLop = cboLop.SelectedValue.ToString();
                LoadData(maLop);
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (a == "them") return;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();

                if (row.Cells["GioiTinh"].Value.ToString() == "Nam")
                {
                    radNam.Checked = true;
                }
                else
                {
                    radNu.Checked = true;
                }
            }

            txtMaSV.Enabled = false;
            txtHoTen.Enabled = false;
            dtpNgaySinh.Enabled = false;
            radNam.Enabled = false;
            radNu.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSoDienThoai.Enabled = false;
            txtEmail.Enabled = false;

            btnXacNhan.Enabled = false;
            a = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboLop.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Lớp trước khi thêm sinh viên!");
                return;
            }

            txtMaSV.Enabled = true;
            txtHoTen.Enabled = true;
            dtpNgaySinh.Enabled = true;
            radNam.Enabled = true;
            radNu.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSoDienThoai.Enabled = true;
            txtEmail.Enabled = true;

            txtMaSV.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            radNam.Checked = true;
            txtDiaChi.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";

            txtMaSV.Focus();
            btnXacNhan.Enabled = true;
            cboLop.Enabled = false;
            btnLuu.Enabled = false;

            a = "them";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách trước khi sửa!");
                return;
            }

        
            txtMaSV.Enabled = false;
            txtHoTen.Enabled = true;
            dtpNgaySinh.Enabled = true;
            radNam.Enabled = true;
            radNu.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSoDienThoai.Enabled = true;
            txtEmail.Enabled = true;

            btnXacNhan.Enabled = true;
            cboLop.Enabled = false;
            btnLuu.Enabled = false; 

            txtHoTen.Focus();

            a = "sua";
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string ngaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd");
            string gioiTinh = (radNam.Checked) ? "Nam" : "Nữ";
            string diaChi = txtDiaChi.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string maLop = cboLop.SelectedValue.ToString(); 

            if (maSV == "" || hoTen == "")
            {
                MessageBox.Show("Mã SV và Họ Tên không được để trống!");
                return;
            }

            if (a == "them")
            {
                try
                {
                    string sqlInsert = $"INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaLop) " +
                                       $"VALUES ('{maSV}', N'{hoTen}', '{ngaySinh}', N'{gioiTinh}', N'{diaChi}', '{sdt}', '{email}', '{maLop}')";
                    db.Execute(sqlInsert);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm (Mã SV có thể đã tồn tại): " + ex.Message);
                }
            }
            else if (a == "sua") 
            {
                try
                {
                
                    string sqlUpdate = $"UPDATE SinhVien SET HoTen = N'{hoTen}', NgaySinh = '{ngaySinh}', GioiTinh = N'{gioiTinh}', " +
                                       $"DiaChi = N'{diaChi}', SoDienThoai = '{sdt}', Email = '{email}' " +
                                       $"WHERE MaSV = '{maSV}'";
                    db.Execute(sqlUpdate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message);
                }
            }

            LoadData(maLop);
            btnHuy_Click(sender, e); 
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text.Trim();
            if (maSV == "")
            {
                MessageBox.Show("Vui lòng chọn sinh viên muốn xóa!");
                return;
            }

            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên {maSV}?\n\n(CẢNH BÁO: Toàn bộ điểm rèn luyện và điểm học tập của sinh viên này cũng sẽ bị XÓA VĨNH VIỄN!)",
                                "Xác nhận xóa",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sqlDeleteDiemRL = $"DELETE FROM DiemRenLuyen WHERE MaSV = '{maSV}'";
                    string sqlDeleteDiemHT = $"DELETE FROM KetQuaHocTap WHERE MaSV = '{maSV}'";
                    string sqlDeleteSV = $"DELETE FROM SinhVien WHERE MaSV = '{maSV}'";

                    db.Execute(sqlDeleteDiemRL);  
                    db.Execute(sqlDeleteDiemHT); 
                    db.Execute(sqlDeleteSV);     

                    MessageBox.Show($"Đã xóa thành công sinh viên {maSV} và toàn bộ điểm liên quan.");

                    LoadData(cboLop.SelectedValue.ToString());
                    btnHuy_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi nghiêm trọng");
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            radNam.Checked = true;
            txtDiaChi.Text = "";
            txtSoDienThoai.Text = "";
            txtEmail.Text = "";

            txtMaSV.Enabled = false;
            txtHoTen.Enabled = false;
            dtpNgaySinh.Enabled = false;
            radNam.Enabled = false;
            radNu.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSoDienThoai.Enabled = false;
            txtEmail.Enabled = false;

            btnXacNhan.Enabled = false;
            cboLop.Enabled = true; 
            btnLuu.Enabled = true; 


            a = "";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
