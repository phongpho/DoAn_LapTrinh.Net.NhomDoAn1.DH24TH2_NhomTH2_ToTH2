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
    public partial class frmQuanLyMonHoc : Form
    {
        Database db;
        string a = "";

        public frmQuanLyMonHoc()
        {
            InitializeComponent();
            db = new Database();

        }

        private void LoadData()
        {
            try
            {
                DataTable dtMonHoc = db.GetData("SELECT * FROM MonHoc");

                dgvMonHoc.DataSource = dtMonHoc;

                dgvMonHoc.Columns[0].HeaderText = "Mã Môn học";
                dgvMonHoc.Columns[1].HeaderText = "Tên Môn học";
                dgvMonHoc.Columns[2].HeaderText = "Số tín chỉ";
                dgvMonHoc.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách môn học: " + ex.Message);
            }
        }
        private void frmQuanLyMonHoc_Load(object sender, EventArgs e)
        {
            txtMaMH.Enabled = false;
            txtTenMH.Enabled = false;
            numSoTinChi.Enabled = false;
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMonHoc.Rows[e.RowIndex];
                txtMaMH.Text = row.Cells[0].Value.ToString();
                txtTenMH.Text = row.Cells[1].Value.ToString();
                numSoTinChi.Value = Convert.ToDecimal(row.Cells["SoTinChi"].Value);
            }
            txtMaMH.Enabled = false;
            txtTenMH.Enabled = false;
            numSoTinChi.Enabled = false;
            btnXacNhan.Enabled = false;
            a = ""; 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaMH.Enabled = true;
            txtTenMH.Enabled = true;
            numSoTinChi.Enabled = true;

            txtMaMH.Text = "";
            txtTenMH.Text = "";
            numSoTinChi.Value = 0; 

            txtMaMH.Focus();
            btnXacNhan.Enabled = true;

            a = "them";
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMaMH.Text == "")
            {
                MessageBox.Show("Vui lòng chọn một môn học từ danh sách trước khi sửa!");
                return;
            }

            txtMaMH.Enabled = false; 
            txtTenMH.Enabled = true;
            numSoTinChi.Enabled = true; 
            btnXacNhan.Enabled = true;

            txtTenMH.Focus();

            a = "sua";
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string maMH = txtMaMH.Text.Trim();
            string tenMH = txtTenMH.Text.Trim();
            int soTinChi = (int)numSoTinChi.Value;

            if (maMH == "" || tenMH == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên môn học!");
                return;
            }
            if (soTinChi <= 0)
            {
                MessageBox.Show("Số tín chỉ phải lớn hơn 0!");
                return;
            }

            if(a== "them")
            {
                try
                {
                    string sqlInsert = $"INSERT INTO MonHoc (MaMH, TenMH, SoTinChi) VALUES ('{maMH}', N'{tenMH}', {soTinChi})";
                    db.Execute(sqlInsert);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm (Mã Môn học có thể đã tồn tại): " + ex.Message);
                }
            }
            else if (a == "sua") 
            {
                try
                {
                    string sqlUpdate = $"UPDATE MonHoc SET TenMH = N'{tenMH}', SoTinChi = {soTinChi} WHERE MaMH = '{maMH}'";
                    db.Execute(sqlUpdate);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa: " + ex.Message);
                }
            }

            txtMaMH.Text = "";
            txtTenMH.Text = "";
            numSoTinChi.Value = 0;
            txtMaMH.Enabled = false;
            txtTenMH.Enabled = false;
            numSoTinChi.Enabled = false;
            btnXacNhan.Enabled = false;
            a = "";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maMH = txtMaMH.Text.Trim();
            if (maMH == "")
            {
                MessageBox.Show("Vui lòng chọn môn học muốn xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa môn học " + maMH + "?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sqlDelete = $"DELETE FROM MonHoc WHERE MaMH = '{maMH}'";
                    db.Execute(sqlDelete);
                    LoadData();

                    txtMaMH.Text = "";
                    txtTenMH.Text = "";
                    numSoTinChi.Value = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: Môn học này có thể đang có Sinh viên học. Bạn phải xóa điểm của môn này trước.\n\nChi tiết lỗi: " + ex.Message, "Lỗi ràng buộc dữ liệu");
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            LoadData(); 
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaMH.Text = "";
            txtTenMH.Text = "";
            numSoTinChi.Value = 0;

            txtMaMH.Enabled = false;
            txtTenMH.Enabled = false;
            numSoTinChi.Enabled = false;
            btnXacNhan.Enabled = false;

            a = "";
        }
    }
}
