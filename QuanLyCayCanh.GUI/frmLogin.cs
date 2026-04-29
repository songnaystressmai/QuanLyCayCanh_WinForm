using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCayCanh.BUS;
using System.Data;

namespace QuanLyCayCanh.GUI
{
    public partial class frmLogin : Form
    {
        private TextBox txtUser, txtPass;
        private Button btnLogin, btnExit;
        private NguoiDungBUS bus = new NguoiDungBUS();

        public frmLogin()
        {
            // Thiết lập Form Đăng nhập hiện đại
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeLoginUI();
        }

        private void InitializeLoginUI()
        {
            // Panel trang trí phía trên
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 150, BackColor = Color.FromArgb(33, 47, 61) };
            Label lblLogo = new Label
            {
                Text = "GARDEN LOGIN",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlTop.Controls.Add(lblLogo);

            // Ô nhập Tài khoản
            Label lblUser = new Label { Text = "Tên đăng nhập:", Location = new Point(50, 180), AutoSize = true };
            txtUser = new TextBox { Location = new Point(50, 210), Width = 300, Font = new Font("Segoe UI", 12) };

            // Ô nhập Mật khẩu
            Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(50, 260), AutoSize = true };
            txtPass = new TextBox { Location = new Point(50, 290), Width = 300, Font = new Font("Segoe UI", 12), PasswordChar = '*' };

            // Nút Đăng nhập
            btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Location = new Point(50, 350),
                Size = new Size(300, 45),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Nút Thoát
            btnExit = new Button
            {
                Text = "Thoát",
                Location = new Point(50, 410),
                Size = new Size(300, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gray
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();

            this.Controls.AddRange(new Control[] { pnlTop, lblUser, txtUser, lblPass, txtPass, btnLogin, btnExit });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            DataTable dt = bus.DangNhap(txtUser.Text, txtPass.Text);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo");
                this.Hide();
                frmMain main = new frmMain(); // Mở Form chính
                main.Show();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi");
            }
        }
    }
}