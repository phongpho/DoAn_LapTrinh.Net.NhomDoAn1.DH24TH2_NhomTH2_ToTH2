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
    public partial class frmDiemTichLuy : Form
    {
        Database db;
        string currentMaLop = "";
        string currentMaMH = "";

        bool isUpdateMode = false;

        public frmDiemTichLuy()
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
                cboLop.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Lớp: " + ex.Message);
            }
        }
        private void LoadComboBoxMonHoc()
        {
            try
            {
                DataTable dtMH = db.GetData("SELECT MaMH, TenMH FROM MonHoc");
                cboMonHoc.DataSource = dtMH;
                cboMonHoc.DisplayMember = "TenMH";
                cboMonHoc.ValueMember = "MaMH";
                cboMonHoc.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách Môn học: " + ex.Message);
            }
        }

        private void frmDiemTichLuy_Load(object sender, EventArgs e)
        {
            LoadComboBoxLop();
            LoadComboBoxMonHoc();
            SetInputState(false);
            txtMaSV.Enabled = false;
            txtHoTen.Enabled = false;
        }

        private void SetInputState(bool enabled)
        {
            numDiemSo.Enabled = enabled;
            btnLuuDiem.Enabled = enabled; 
            btnXoa.Enabled = enabled; 
        }

        private void LoadDataGridView()
        {
            string sql = $@"
                SELECT sv.MaSV, sv.HoTen, kq.DTL 
                FROM SinhVien sv
                LEFT JOIN KetQuaHocTap kq ON sv.MaSV = kq.MaSV AND kq.MaMH = '{currentMaMH}'
                WHERE sv.MaLop = '{currentMaLop}'
                ORDER BY sv.HoTen";

            DataTable dtDiem = db.GetData(sql);
            dgvDiem.DataSource = dtDiem;

            dgvDiem.Columns["MaSV"].HeaderText = "Mã SV";
            dgvDiem.Columns["HoTen"].HeaderText = "Họ Tên";
            dgvDiem.Columns["DTL"].HeaderText = "Điểm Tích Lũy";
            dgvDiem.Columns["HoTen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            if (cboLop.SelectedValue == null || cboMonHoc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Lớp VÀ Môn học trước khi tải!");
                return;
            }

            currentMaLop = cboLop.SelectedValue.ToString();
            currentMaMH = cboMonHoc.SelectedValue.ToString();
            LoadDataGridView();
            SetInputState(true);

            
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            numDiemSo.Value = 0;
            isUpdateMode = false; 
        }

        private void dgvDiem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDiem.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();

                if (row.Cells["DTL"].Value == DBNull.Value)
                {
                    numDiemSo.Value = 0;
                    isUpdateMode = false; 
                }
                else
                {
                    numDiemSo.Value = Convert.ToDecimal(row.Cells["DTL"].Value);
                    isUpdateMode = true;
                }
            }
        }

        private void btnLuuDiem_Click(object sender, EventArgs e) 
        {
            string maSV = txtMaSV.Text;
            decimal diemSo = numDiemSo.Value;

            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách.");
                return;
            }

            string sql = "";

            if (isUpdateMode) 
            {

                sql = $"UPDATE KetQuaHocTap SET DTL = {diemSo} WHERE MaSV = '{maSV}' AND MaMH = '{currentMaMH}'";
            }
            else 
            {

                sql = $"INSERT INTO KetQuaHocTap (MaSV, MaMH, DTL) VALUES ('{maSV}', '{currentMaMH}', {diemSo})";
            }

            try
            {
                db.Execute(sql);
                LoadDataGridView();

                txtMaSV.Text = "";
                txtHoTen.Text = "";
                numDiemSo.Value = 0;
                isUpdateMode = false; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu điểm: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e) 
        {
            string maSV = txtMaSV.Text;
            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa điểm.");
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn XÓA điểm môn {cboMonHoc.Text} của SV {maSV}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sql = $"DELETE FROM KetQuaHocTap WHERE MaSV = '{maSV}' AND MaMH = '{currentMaMH}'";
                    db.Execute(sql);

                    LoadDataGridView();

                    txtMaSV.Text = "";
                    txtHoTen.Text = "";
                    numDiemSo.Value = 0;
                    isUpdateMode = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa điểm: " + ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}