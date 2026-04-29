using QuanLyCayCanh.DAL;
using System.Data;

namespace QuanLyCayCanh.BUS
{
    public class NguoiDungBUS
    {
        DataService da = new DataService();

        public DataTable DangNhap(string user, string pass)
        {
            // Gọi Procedure sp_DangNhap mà bạn đã tạo trong Database
            string sql = $"EXEC sp_DangNhap N'{user}', N'{pass}'";
            return da.GetTable(sql);
        }
    }
}