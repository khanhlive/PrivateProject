using System.Linq;

namespace Moss.Hospital.Data.Dao
{
    public static class Helper
    {
        public static string TO_LOWER(string b_param)
        {
            if (!string.IsNullOrEmpty(b_param)) return b_param.Trim().ToLower();
            return "";
        }
        public static string TO_UPPER(string b_param)
        {
            if (!string.IsNullOrEmpty(b_param)) return b_param.Trim().ToUpper();
            return "";
        }
        public static string TO_STRING(int b_param)
        {
            return b_param.ToString();
        }
        public static string TO_STATUS(bool b_param)
        {
            string b_kq = "";
            switch (b_param)
            {
                case true: b_kq = "Đang sử dụng"; break;
                case false: b_kq = "Không sử dụng"; break;
            }
            return b_kq;
        }
        public static string TO_CHUCNANG(byte b_param)
        {
            var item = (from i in Library.ConstDictionary.dicChucNangDepartment where i.Key == b_param select new { i.Value }).FirstOrDefault();
            if (item != null) return item.Value;
            return "";
        }
        public static string TO_CHUYENKHOA_CODE(byte b_param)
        {
            var item = (from i in Library.ConstDictionary.dicMaChuyenKhoa_TT43 where i.Key == b_param select new { i.Value }).FirstOrDefault();
            if (item != null) return item.Value;
            return "";
        }
    }
}