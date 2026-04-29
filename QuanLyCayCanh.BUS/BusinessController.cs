using System;
using System.Data;
using QuanLyCayCanh.DAL;
using QuanLyCayCanh.DTO;

namespace QuanLyCayCanh.BUS
{
    // Lớp xử lý nghiệp vụ cho Cây Cảnh
    public class CayCanhBUS
    {
        DataService ds = new DataService();

        public DataTable LayTatCaCay()
        {
            return ds.GetTable("SELECT * FROM CayCanh");
        }

        public bool ThemCayCanh(CayCanh cay)
        {
            // Lưu ý: Đảm bảo tên hàm trong DataService là Execute hoặc ExecuteQuery cho khớp
            string sql = $"INSERT INTO CayCanh VALUES(N'{cay.TenCay}', {cay.Gia}, {cay.SoLuong}, {cay.DanhMucId})";
            return ds.ExecuteQuery(sql);
        }

        public bool XoaCay(int id)
        {
            string sql = "DELETE FROM CayCanh WHERE Id = " + id;
            return ds.ExecuteQuery(sql);
        }
    }

    // Lớp xử lý nghiệp vụ cho Đơn Hàng
    public class DonHangBUS
    {
        DataService ds = new DataService();

        public void TaoDonHang(int khId, int ndId)
        {
            // Gọi stored procedure từ database
            string sql = $"EXEC sp_TaoDonHang {khId}, {ndId}";
            ds.ExecuteQuery(sql);
        }
    }

    // Bạn có thể thêm các class BUS khác (KhachHangBUS, NhanVienBUS) vào đây
}