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
    public partial class frmQuanLyLop : Form
    {
        Database db;
        string a = "";
        public frmQuanLyLop()
        {
            InitializeComponent();
            db = new Database();
        }

        private void LoadComBoBox()
        {
            try
            {
                DataTable dtKhoa = db.GetData("SELECT MaKhoa, TenKhoa FROM Khoa");

                cboKhoa.DataSource = dtKhoa;

                cboKhoa.DisplayMember = "TenKhoa";
                cboKhoa.ValueMember = "MaKhoa";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Khoa: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                string sql = " select Lop.MaLop, Lop.TenLop, Khoa.TenKhoa " +
                             "from Lop " +
                             "inner join Khoa on Lop.MaKhoa = Khoa.MaKhoa";
                DataTable dtLop = db.GetData(sql);

                dgvLop.DataSource = dtLop;

                dgvLop.Columns[0].HeaderText = "Mã Lớp";
                dgvLop.Columns[1].HeaderText = "Tên Lớp";
                dgvLop.Columns[2].HeaderText = "Tên Khoa";
                dgvLop.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvLop.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Lớp: " + ex.Message);
            }
        }
        private void frmQuanLyLop_Load(object sender, EventArgs e)
        {
            LoadComBoBox();
            //LoadData();

            txtMaLop.Enabled = false;
            txtTenLop.Enabled = false;
            btnXacNhan.Enabled = false;
            //cboKhoa.Enabled = false;
        }

        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLop.Rows[e.RowIndex];
                txtMaLop.Text = row.Cells["MaLop"].Value.ToString();
                txtTenLop.Text = row.Cells["TenLop"].Value.ToString();

                cboKhoa.Text = row.Cells["TenKhoa"].Value.ToString();
            }
            txtMaLop.Enabled = false;
            txtTenLop.Enabled = false;
            //cboKhoa.Enabled = false;
            btnXacNhan.Enabled = false;
            a = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaLop.Enabled = true;
            txtTenLop.Enabled = true;
            cboKhoa.Enabled = true;

            txtMaLop.Text = "";
            txtTenLop.Text = "";
            cboKhoa.SelectedIndex = 0;

            txtMaLop.Focus();
            btnXacNhan.Enabled = true;

            a = "them";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Text == "")
            {
                MessageBox.Show("Vui lòng chọn một lớp từ danh sách trước khi sửa!");
                return;
            }

            txtMaLop.Enabled = false; 
            txtTenLop.Enabled = true;
            cboKhoa.Enabled = true; 
            btnXacNhan.Enabled = true;

            txtTenLop.Focus();

            a = "sua";
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            string tenLop = txtTenLop.Text.Trim();

            string maKhoa = "";
            if (cboKhoa.SelectedValue != null)
            {
                maKhoa = cboKhoa.SelectedValue.ToString();
            }

            if (maLop == "" || tenLop == "" || maKhoa == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Lớp, Tên Lớp và chọn Khoa!");
                return;
            }

            if (a == "them")
            {
                try
                {
                    string sqlInsert = $"INSERT INTO Lop (MaLop, TenLop, MaKhoa) VALUES ('{maLop}', N'{tenLop}', '{maKhoa}')";
                    db.Execute(sqlInsert);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm: " + ex.Message);
                }
            }
            else if (a == "sua")
            {
                try
                {
                    string sqlUpdate = $"UPDATE Lop SET TenLop = N'{tenLop}', MaKhoa = '{maKhoa}' WHERE MaLop = '{maLop}'";
                    db.Execute(sqlUpdate);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message);
                }
            }

            txtMaLop.Text = "";
            txtTenLop.Text = "";
            cboKhoa.SelectedIndex = 0;
            txtMaLop.Enabled = false;
            txtTenLop.Enabled = false;
            cboKhoa.Enabled = false;
            btnXacNhan.Enabled = false;
            a = "";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maLop = txtMaLop.Text.Trim();
            if (maLop == "")
            {
                MessageBox.Show("Vui lòng chọn lớp muốn xóa!");
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa lớp " + maLop + "?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = $"DELETE FROM Lop WHERE MaLop = '{maLop}'";
                    db.Execute(sqlDelete);
                    LoadData();
                    txtMaLop.Text = "";
                    txtTenLop.Text = "";
                    cboKhoa.SelectedIndex = 0;
                    txtMaLop.Enabled = false;
                    txtTenLop.Enabled = false;
                    cboKhoa.Enabled = false;
                    btnXacNhan.Enabled = false;
                    a = ""; 
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: Lớp này có thể đang chứa Sinh viên. Bạn phải xóa Sinh viên thuộc lớp này trước.\n\nChi tiết lỗi: " + ex.Message, "Lỗi ràng buộc dữ liệu");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaLop.Text = "";
            txtTenLop.Text = "";
            cboKhoa.SelectedIndex = 0;

            txtMaLop.Enabled = false;
            txtTenLop.Enabled = false;
            cboKhoa.Enabled = false;
            btnXacNhan.Enabled = false;

            a = "";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
