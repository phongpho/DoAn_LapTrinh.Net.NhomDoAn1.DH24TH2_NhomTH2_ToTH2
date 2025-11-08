using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public partial class frmQuanLyKhoa : Form
    {
        Database db;

        string a = "";
        public frmQuanLyKhoa()
        {
            InitializeComponent();
            db = new Database();
        }

        private void LoadData()
        {
            try
            {
              
                DataTable dtKhoa = db.GetData("SELECT * FROM Khoa");

              
                dgvKhoa.DataSource = dtKhoa; 

                dgvKhoa.Columns[0].HeaderText = "Mã Khoa";
                dgvKhoa.Columns[1].HeaderText = "Tên Khoa";
                dgvKhoa.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }
        private void frmQuanLyKhoa_Load(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void dgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhoa.Rows[e.RowIndex];
     
                txtMaKhoa.Text = row.Cells["MaKhoa"].Value.ToString();
                txtTenKhoa.Text = row.Cells["TenKhoa"].Value.ToString();
            }
            txtMaKhoa.Enabled = false;
            txtTenKhoa.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaKhoa.Enabled = true;
            txtTenKhoa.Enabled = true;
            txtMaKhoa.Text = "";
            txtTenKhoa.Text = "";
            txtMaKhoa.Focus();
            btnXacNhan.Enabled = true;

            a = "them";
        }
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string maKhoa = txtMaKhoa.Text.Trim();
            string tenKhoa = txtTenKhoa.Text.Trim();

            if (a == "them")
            {
                if (maKhoa == "" || tenKhoa == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã Khoa và Tên Khoa!");
                    return;
                }

                try
                {

                    string sqlInsert = $"INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES ('{maKhoa}', N'{tenKhoa}')";

                    db.Execute(sqlInsert);

                    LoadData();

                    txtMaKhoa.Text = "";
                    txtTenKhoa.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm: " + ex.Message);
                }
            }
            else if (a == "sua")
            {
                if (maKhoa == "" || tenKhoa == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã Khoa và Tên Khoa!");
                    return;
                }

                try
                {
                    string sqlUpdate = $"UPDATE Khoa SET TenKhoa = N'{tenKhoa}' WHERE MaKhoa = '{maKhoa}'";
                    db.Execute(sqlUpdate);

                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message);
                }
            }
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMaKhoa.Enabled = false;
            txtTenKhoa.Enabled = true;
            btnXacNhan.Enabled = true;
            txtTenKhoa.Focus();
            a = "sua";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maKhoa = txtMaKhoa.Text.Trim();
            if (maKhoa == "")
            {
                MessageBox.Show("Vui lòng chọn khoa muốn xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khoa " + maKhoa + "?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = $"DELETE FROM Khoa WHERE MaKhoa = '{maKhoa}'";

                    db.Execute(sqlDelete);
                    LoadData();

                    txtMaKhoa.Text = "";
                    txtTenKhoa.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: Khoa này có thể đang chứa các Lớp. Bạn phải xóa các Lớp thuộc khoa này trước.\n\nChi tiết lỗi: " + ex.Message, "Lỗi ràng buộc dữ liệu");
                }
            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaKhoa.Text = "";
            txtTenKhoa.Text = "";

            txtMaKhoa.Enabled = false;
            txtTenKhoa.Enabled = false;
            btnXacNhan.Enabled = false;


            a = "";
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
