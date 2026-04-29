using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QuanLyCayCanh.BUS;
using QuanLyCayCanh.DTO;

namespace QuanLyCayCanh.GUI
{
    public class frmCayCanh : frmBase
    {
        // --- Control chính ---
        private DataGridView dgv;
        private TextBox txtId, txtTen, txtGia, txtSoLuong;
        private PictureBox pbCay;
        private Button btnThem, btnSua, btnXoa, btnChonAnh, btnReset;

        // --- Biến nghiệp vụ ---
        private CayCanhBUS bus = new CayCanhBUS();
        private string duongDanAnh = ""; // Lưu đường dẫn ảnh tạm thời

        public frmCayCanh()
        {
            this.lblHeaderTitle.Text = "🌿 QUẢN LÝ THÔNG TIN CÂY CẢNH";
            SetupModernUI();
            SetupEvents();
            LoadDataToGrid();
        }

        // --- 1. Vẽ Giao diện (Rất chi tiết) ---
        private void SetupModernUI()
        {
            TableLayoutPanel mainTlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(15) };
            mainTlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350f)); // Cột trái: Nhập liệu
            mainTlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // Cột phải: Bảng dữ liệu

            // >>> PANEL NHẬP LIỆU (Trái) >>>
            Panel pnlInput = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(15), BorderStyle = BorderStyle.FixedSingle };

            // -- Input ID (Ẩn, dùng để sửa/xóa) --
            txtId = new TextBox { Visible = false }; pnlInput.Controls.Add(txtId);

            // -- Khu vực Ảnh cây --
            pbCay = new PictureBox { Size = new Size(180, 180), Location = new Point(70, 15), BackColor = Color.FromArgb(240, 240, 240), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };
            pnlInput.Controls.Add(pbCay);

            btnChonAnh = new Button { Text = "Chọn ảnh...", Location = new Point(100, 200), Size = new Size(120, 30), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(230, 230, 230), Cursor = Cursors.Hand };
            pnlInput.Controls.Add(btnChonAnh);

            // -- Các ô nhập liệu --
            int labelY = 250; int inputY = 275; int spacing = 60;
            Font fLabel = new Font("Segoe UI", 9, FontStyle.Bold);
            Font fInput = new Font("Segoe UI", 11);

            pnlInput.Controls.Add(new Label { Text = "Tên cây:", Location = new Point(15, labelY), Font = fLabel });
            txtTen = new TextBox { Location = new Point(15, inputY), Width = 300, Font = fInput };
            pnlInput.Controls.Add(txtTen);

            pnlInput.Controls.Add(new Label { Text = "Giá bán (VNĐ):", Location = new Point(15, labelY + spacing), Font = fLabel });
            txtGia = new TextBox { Location = new Point(15, inputY + spacing), Width = 300, Font = fInput };
            pnlInput.Controls.Add(txtGia);

            pnlInput.Controls.Add(new Label { Text = "Số lượng tồn:", Location = new Point(15, labelY + spacing * 2), Font = fLabel });
            txtSoLuong = new TextBox { Location = new Point(15, inputY + spacing * 2), Width = 300, Font = fInput };
            pnlInput.Controls.Add(txtSoLuong);

            // -- Nhóm nút bấm --
            int btnY = 480; int btnWidth = 145; int btnHeight = 40;
            Font fBtn = new Font("Segoe UI", 9, FontStyle.Bold);

            btnThem = CreateButton("✅ Thêm", Color.FromArgb(46, 204, 113), new Point(15, btnY), btnWidth, btnHeight);
            btnSua = CreateButton("✏️ Sửa", Color.FromArgb(52, 152, 219), new Point(170, btnY), btnWidth, btnHeight);
            btnXoa = CreateButton("❌ Xóa", Color.FromArgb(231, 76, 60), new Point(15, btnY + spacing - 10), btnWidth, btnHeight);
            btnReset = CreateButton("🔄 Làm mới", Color.FromArgb(149, 165, 166), new Point(170, btnY + spacing - 10), btnWidth, btnHeight);

            pnlInput.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnReset });

            // >>> BẢNG DỮ LIỆU (Phải) >>>
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(15, 0, 0, 0),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 45 }
            };
            // Style header dgv
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 47, 61);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            // Xen kẽ màu dòng cho đẹp
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 249);

            mainTlp.Controls.Add(pnlInput, 0, 0);
            mainTlp.Controls.Add(dgv, 1, 0);
            this.Controls.Add(mainTlp);
        }

        // --- 2. Cài đặt Logic & Sự kiện ---
        private void SetupEvents()
        {
            // Sự kiện click bảng
            dgv.CellClick += Dgv_CellClick;

            // Sự kiện các nút
            btnChonAnh.Click += BtnChonAnh_Click;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnReset.Click += (s, e) => ResetForm();
        }

        private void LoadDataToGrid()
        {
            dgv.DataSource = bus.LayTatCaCay();
            // Đặt tên tiếng Việt cho các cột nếu cần (tùy thuộc vào dữ liệu trả về từ BUS)
            if (dgv.Columns.Count > 0)
            {
                // ví dụ dgv.Columns[0].HeaderText = "Mã số"; ...
            }
        }

        // >>> LOGIC: Bấm vào dòng, hiện thông tin + ảnh >>>
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                txtId.Text = row.Cells["Id"].Value.ToString();
                txtTen.Text = row.Cells["TenCay"].Value.ToString();
                txtGia.Text = row.Cells["Gia"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();

                // Xử lý ảnh cây: Giả định Database lưu tên ảnh (ví dụ "xuong_rong.jpg")
                try
                {
                    string folderAnh = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Images\"; // Đường dẫn thư mục ảnh
                    object val = row.Cells["HinhAnh"].Value; // Đổi "HinhAnh" thành tên cột chứa đường dẫn/tên ảnh của bạn

                    if (val != null && val != DBNull.Value && !string.IsNullOrEmpty(val.ToString()))
                    {
                        string tenAnh = val.ToString();
                        string pathComple = Path.Combine(folderAnh, tenAnh);
                        if (File.Exists(pathComple))
                        {
                            pbCay.Image = Image.FromFile(pathComple);
                            duongDanAnh = tenAnh; // Lưu tên ảnh hiện tại
                        }
                        else
                        {
                            pbCay.Image = null; pbCay.BackColor = Color.FromArgb(250, 219, 216); // Báo đỏ nhẹ nếu ko tìm thấy
                        }
                    }
                    else { pbCay.Image = null; pbCay.BackColor = Color.FromArgb(240, 240, 240); }
                }
                catch { pbCay.Image = null; }
            }
        }

        // >>> LOGIC: Chọn ảnh từ máy tính >>>
        private void BtnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbCay.Image = Image.FromFile(ofd.FileName);
                // Ở Đồ án 1, để đơn giản: Ta chỉ lấy TÊN file ảnh, giả định bạn copy ảnh vào thư mục Images của project.
                duongDanAnh = Path.GetFileName(ofd.FileName);
            }
        }

        // >>> LOGIC: Thêm mới >>>
        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTen.Text)) { MessageBox.Show("Vui lòng nhập tên cây!"); return; }
            CayCanh cay = GetDataFromForm();
            if (bus.ThemCayCanh(cay)) { MessageBox.Show("Thêm thành công!"); ResetForm(); LoadDataToGrid(); }
            else { MessageBox.Show("Thêm thất bại!"); }
        }

        // >>> LOGIC: Sửa >>>
        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) { MessageBox.Show("Vui lòng chọn cây cần sửa từ bảng!"); return; }
            CayCanh cay = GetDataFromForm();
            cay.Id = int.Parse(txtId.Text); // Cần ID để sửa
            //if (bus.CapNhatCay(cay)) { MessageBox.Show("Cập nhật thành công!"); ResetForm(); LoadDataToGrid(); }
            //else { MessageBox.Show("Cập nhật thất bại!"); }
        }

        // >>> LOGIC: Xóa >>>
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) { MessageBox.Show("Vui lòng chọn cây cần xóa từ bảng!"); return; }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa cây: " + txtTen.Text + " ?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (bus.XoaCay(int.Parse(txtId.Text))) { MessageBox.Show("Xóa thành công!"); ResetForm(); LoadDataToGrid(); }
                else { MessageBox.Show("Xóa thất bại!"); }
            }
        }

        // --- Hàm hỗ trợ khác ---
        private void ResetForm()
        {
            txtId.Text = ""; txtTen.Text = ""; txtGia.Text = "0"; txtSoLuong.Text = "0"; pbCay.Image = null; duongDanAnh = ""; pbCay.BackColor = Color.FromArgb(240, 240, 240);
        }

        private CayCanh GetDataFromForm()
        {
            // Đồ án 1: Giả lập lấy ID Danh mục mặc định = 1. Bạn cần thêm ComboBox chọn danh mục sau.
            decimal gia; decimal.TryParse(txtGia.Text, out gia);
            int sl; int.TryParse(txtSoLuong.Text, out sl);
            return new CayCanh { TenCay = txtTen.Text, Gia = gia, SoLuong = sl, DanhMucId = 1 }; // Bổ sung HinhAnh = duongDanAnh nếu Database hỗ trợ
        }

        private Button CreateButton(string text, Color backColor, Point loc, int w, int h)
        {
            return new Button { Text = text, Location = loc, Size = new Size(w, h), FlatStyle = FlatStyle.Flat, BackColor = backColor, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Cursor = Cursors.Hand, TextAlign = ContentAlignment.MiddleCenter };
        }
    }
}