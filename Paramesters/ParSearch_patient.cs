using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moss.Hospital.Data.Paramesters
{
    public class Patient_ParameterSearch
    {
        public DateTime NgayTu { set; get; }
        public DateTime NgayDen{ set; get; }
        public List<byte> DoituongBenhnhan { set; get; }
        public List<int> Bophan { set; get; }
        public List<byte> LoaiBenhAn { set; get; }
        public List<byte> TrangthaiBenhnhan { set; get; }
        /// <summary>
        /// Ưu tiên
        /// </summary>
        public List<byte> Uutien { set; get; }
        /// <summary>
        /// Cấp cứu
        /// </summary>
        public bool? CapCuu { set; get; }
        public string MaBenhVienKCB { set; get; }
        public List<string> BuongGiuong { get; set; }
        public bool? NgoaigioHanhchinh { get; set; }
    }
}
