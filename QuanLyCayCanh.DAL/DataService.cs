using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyCayCanh.DAL
{
    public class DataService
    {
        // Thay chuỗi kết nối phù hợp với máy của bạn
        private string strConn = @"Data Source=EXCALIBUR\SQLEXPRESS;Initial Catalog=QuanLyCayCanh;Integrated Security=True;TrustServerCertificate=True";

        public DataTable GetTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool ExecuteQuery(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch { return false; }
        }
    }
}
