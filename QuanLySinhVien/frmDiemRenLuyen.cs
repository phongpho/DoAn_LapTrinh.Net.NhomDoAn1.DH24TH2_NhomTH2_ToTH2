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
    public partial class frmDiemRenLuyen : Form
    {
        Database db;
        string currentMaLop = "";
        string currentHocKy = "";


        bool isUpdateMode = false;

        public frmDiemRenLuyen()
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

  
        private void LoadComboBoxHocKy()
        {

            cboHocKy.Items.Add("HK1");
            cboHocKy.Items.Add("HK2");
            cboHocKy.Items.Add("HK3");
            cboHocKy.Items.Add("HK4");
            cboHocKy.Items.Add("HK5");
            cboHocKy.Items.Add("HK6");
            cboHocKy.Items.Add("HK7");
            cboHocKy.Items.Add("HK8");


            cboHocKy.SelectedIndex = 0;

 
            cboHocKy.DropDownStyle = ComboBoxStyle.DropDownList;
        }


        private void SetInputState(bool enabled)
        {
            numDRL.Enabled = enabled; 
            btnLuuDiem.Enabled = enabled; 
            btnXoa.Enabled = enabled; 
        }

        private void LoadDataGridView()
        {
 
            string sql = $@"
                SELECT 
                    sv.MaSV, 
                    sv.HoTen, 
                    drl.DRL 
                FROM 
                    SinhVien sv
                LEFT JOIN 
                    DiemRenLuyen drl ON sv.MaSV = drl.MaSV AND drl.HocKy = '{currentHocKy}'
                WHERE 
                    sv.MaLop = '{currentMaLop}'
                ORDER BY
                    sv.HoTen";

            DataTable dtDiem = db.GetData(sql);

            dgvDiemRL.DataSource = dtDiem;

            dgvDiemRL.Columns["MaSV"].HeaderText = "Mã SV";
            dgvDiemRL.Columns["HoTen"].HeaderText = "Họ Tên";
            dgvDiemRL.Columns["DRL"].HeaderText = "Điểm Rèn Luyện";
            dgvDiemRL.Columns["HoTen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }


        private void btnLoadData_Click(object sender, EventArgs e) 
        {
            if (cboLop.SelectedValue == null || cboHocKy.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Lớp VÀ Học kỳ trước khi tải!");
                return;
            }

 
            currentMaLop = cboLop.SelectedValue.ToString();
            currentHocKy = cboHocKy.SelectedItem.ToString();

            LoadDataGridView();


            SetInputState(true);

            txtMaSV.Text = "";
            txtHoTen.Text = "";
            numDRL.Value = 0;
            isUpdateMode = false;
        }

        private void dgvDiemRL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDiemRL.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();

                if (row.Cells["DRL"].Value == DBNull.Value) 
                {
                    numDRL.Value = 0;
                    isUpdateMode = false;
                }
                else
                {
                    numDRL.Value = Convert.ToDecimal(row.Cells["DRL"].Value);
                    isUpdateMode = true;
                }
            }
        }

        private void btnLuuDiem_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text;
            int diemDRL = (int)numDRL.Value; 

            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách.");
                return;
            }


            string sql = "";

            if (isUpdateMode) 
            {

                sql = $"UPDATE DiemRenLuyen SET DRL = {diemDRL} WHERE MaSV = '{maSV}' AND HocKy = '{currentHocKy}'";
            }
            else 
            {
                sql = $"INSERT INTO DiemRenLuyen (MaSV, HocKy, DRL) VALUES ('{maSV}', '{currentHocKy}', {diemDRL})";
            }

            try
            {
                db.Execute(sql);
                LoadDataGridView();

                txtMaSV.Text = "";
                txtHoTen.Text = "";
                numDRL.Value = 0;
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

            if (MessageBox.Show($"Bạn có chắc muốn XÓA điểm rèn luyện {currentHocKy} của SV {maSV}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sql = $"DELETE FROM DiemRenLuyen WHERE MaSV = '{maSV}' AND HocKy = '{currentHocKy}'";
                    db.Execute(sql);

                    LoadDataGridView();

                    txtMaSV.Text = "";
                    txtHoTen.Text = "";
                    numDRL.Value = 0;
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

        private void frmDiemRenLuyen_Load_1(object sender, EventArgs e)
        {
            LoadComboBoxLop();
            LoadComboBoxHocKy();

            SetInputState(false);
            txtMaSV.Enabled = false;
            txtHoTen.Enabled = false;

            dgvDiemRL.ReadOnly = true;
        }
    }
}