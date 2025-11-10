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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            bool trangthai;

            if (username == "admin" && password == "1")
            {
                trangthai = true; //admin
                MessageBox.Show("Đăng nhập thành công với quyền Admin!");
            }
            else if (username == "sv" && password == "1") 
            {
                trangthai = false; //sinhvien
                MessageBox.Show("Đăng nhập thành công với quyền Sinh Viên!");
            }
            else
            {
    
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

  
            this.Hide();
            frmMain fMain = new frmMain(trangthai);
            fMain.Show();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
