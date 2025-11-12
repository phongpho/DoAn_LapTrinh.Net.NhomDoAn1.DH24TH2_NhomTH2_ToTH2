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
    public partial class frmTraCuuThongTin : Form
    {
        Database db;

        public frmTraCuuThongTin()
        {
            InitializeComponent();
            db = new Database();
        }

        private void frmTraCuuThongTin_Load(object sender, EventArgs e)
        {
            txtHoTen.ReadOnly = true;
            txtLop.ReadOnly = true;
            txtKhoa.ReadOnly = true;
            txtNgaySinh.ReadOnly = true;
            txtGioiTinh.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtGPA.ReadOnly = true;
            txtAvgDRL.ReadOnly = true;
            txtHocBong.ReadOnly = true;
            txtXepLoai.ReadOnly = true;

            txtHoTen.Enabled = true;
            txtLop.Enabled = true;
            txtKhoa.Enabled = true;
            txtNgaySinh.Enabled = true;

            dgvDiemTichLuy.ReadOnly = true;
            dgvDiemRenLuyen.ReadOnly = true;
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            string mssv = txtMssv.Text.Trim();

            if (string.IsNullOrEmpty(mssv))
            {
                MessageBox.Show("Vui lòng nhập Mã số sinh viên (MSSV) để tra cứu.");
                return;
            }

            ResetForm();
            LoadThongTinCaNhan(mssv);
            LoadDiemRenLuyen(mssv);
            LoadDiemTichLuy(mssv);
            LoadTongKet(mssv);
        }

        private void LoadThongTinCaNhan(string mssv)
        {
            try
            {
                string sql = $@"
                    SELECT 
                        sv.HoTen, 
                        sv.NgaySinh, 
                        sv.GioiTinh, 
                        sv.Email, 
                        l.TenLop, 
                        k.TenKhoa 
                    FROM 
                        SinhVien sv
                    JOIN 
                        Lop l ON sv.MaLop = l.MaLop
                    JOIN 
                        Khoa k ON l.MaKhoa = k.MaKhoa
                    WHERE 
                        sv.MaSV = '{mssv}'";

                DataTable dtInfo = db.GetData(sql);

                if (dtInfo.Rows.Count > 0)
                {
                    DataRow row = dtInfo.Rows[0];        
                    txtHoTen.Text = row["HoTen"].ToString();
                    txtLop.Text = row["TenLop"].ToString();
                    txtKhoa.Text = row["TenKhoa"].ToString();
                    txtNgaySinh.Text = Convert.ToDateTime(row["NgaySinh"]).ToString("dd/MM/yyyy");
                    txtGioiTinh.Text = row["GioiTinh"].ToString();
                    txtEmail.Text = row["Email"].ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên có mã " + mssv, "Không tìm thấy");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin cá nhân: " + ex.Message);
            }
        }

        private void LoadDiemTichLuy(string mssv)
        {
            try
            {
                string sql = $@"
                    SELECT 
                        mh.TenMH, 
                        mh.SoTinChi, 
                        kq.DTL 
                    FROM 
                        KetQuaHocTap kq
                    JOIN 
                        MonHoc mh ON kq.MaMH = mh.MaMH
                    WHERE 
                        kq.MaSV = '{mssv}'";

                DataTable dtDiem = db.GetData(sql);

                dgvDiemTichLuy.DataSource = dtDiem;

                dgvDiemTichLuy.Columns["TenMH"].HeaderText = "Tên Môn học";
                dgvDiemTichLuy.Columns["SoTinChi"].HeaderText = "Số Tín chỉ";
                dgvDiemTichLuy.Columns["DTL"].HeaderText = "Điểm Tích Lũy";
                dgvDiemTichLuy.Columns["TenMH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng điểm tích lũy: " + ex.Message);
            }
        }

        private void LoadDiemRenLuyen(string mssv)
        {
            try {

                string sql = $@"
                    SELECT 
                        drl.HocKy, 
                        drl.DRL 
                    FROM 
                        DiemRenLuyen drl 
                    WHERE 
                        drl.MaSV = '{mssv}'
                    ORDER BY
                        drl.HocKy";

                DataTable dtDiemRL = db.GetData(sql);

                dgvDiemRenLuyen.DataSource = dtDiemRL;

                dgvDiemRenLuyen.Columns["HocKy"].HeaderText = "Học Kỳ";
                dgvDiemRenLuyen.Columns["DRL"].HeaderText = "Điểm Rèn Luyện";
                dgvDiemRenLuyen.Columns["HocKy"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng điểm rèn luyện: " + ex.Message);
            }
        }

        private void LoadTongKet(string mssv)
        {
            double gpa = 0.0;
            double avgDRL = 0.0;

            try
            {
                string sqlGPA = $"SELECT AVG(DTL) FROM KetQuaHocTap WHERE MaSV = '{mssv}'";
                DataTable dtGPA = db.GetData(sqlGPA);
                if (dtGPA.Rows.Count > 0 && dtGPA.Rows[0][0] != DBNull.Value)
                {
                    gpa = Convert.ToDouble(dtGPA.Rows[0][0]);
                    txtGPA.Text = gpa.ToString("F2"); 
                }
                else
                {
                    txtGPA.Text = "N/A";
                }
            }
            catch
            {
                txtGPA.Text = "Lỗi";
            }

            try
            {
                string sqlDRL = $"SELECT AVG(CAST(DRL AS FLOAT)) FROM DiemRenLuyen WHERE MaSV = '{mssv}'";
                DataTable dtDRL = db.GetData(sqlDRL);
                if (dtDRL.Rows.Count > 0 && dtDRL.Rows[0][0] != DBNull.Value)
                {
                    avgDRL = Convert.ToDouble(dtDRL.Rows[0][0]);
                    txtAvgDRL.Text = avgDRL.ToString("F2"); 
                }
                else
                {
                    txtAvgDRL.Text = "N/A";
                }
            }
            catch
            {
                txtAvgDRL.Text = "Lỗi";
            }


            if (gpa >= 9.0 && avgDRL >= 90)
            {
                txtXepLoai.Text = "Xuất sắc";
            }
            else if (gpa >= 8.0 && avgDRL >= 80)
            {
                txtXepLoai.Text = "Giỏi";
            }
            else if (gpa >= 6.5 && avgDRL >= 65)
            {
                txtXepLoai.Text = "Khá";
            }
            else if (gpa >= 5.0 && avgDRL >= 50)
            {
                txtXepLoai.Text = "Trung bình";
            }
            else if (gpa == 0 || avgDRL == 0)
            {
                txtXepLoai.Text = "N/A (Thiếu điểm)";
            }
            else
            {
                txtXepLoai.Text = "Yếu";
            }

 
            if ((txtXepLoai.Text == "Giỏi" || txtXepLoai.Text == "Xuất sắc"))
            {
                txtHocBong.Text = "Có";
            }
            else
            {
                txtHocBong.Text = "Không";
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMssv.Text = "";
            ResetForm();
        }
        private void ResetForm()
        {
            txtHoTen.Text = "";
            txtLop.Text = "";
            txtKhoa.Text = "";
            txtNgaySinh.Text = "";
            txtGioiTinh.Text = "";
            txtEmail.Text = "";
            txtGPA.Text = "";
            txtAvgDRL.Text = "";
            txtXepLoai.Text = "";
            txtHocBong.Text = "";

            dgvDiemTichLuy.DataSource = null;
            dgvDiemRenLuyen.DataSource = null;

        
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
