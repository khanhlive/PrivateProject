//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Moss.Hospital.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class KBVaoVien
    {
        public int KBVaoVienID { get; set; }
        public int patientsID { get; set; }
        public int KhamBenhID { get; set; }
        public System.DateTime NgayNhapVien { get; set; }
        public string MaICDs { get; set; }
        public string BenhLy { get; set; }
        public string TienSuBanThan { get; set; }
        public string TienSuGiaDinh { get; set; }
        public string KhamToanThan { get; set; }
        public string KhamBoPhan { get; set; }
        public string KetQuaLamSang { get; set; }
        public string DaXuLy { get; set; }
        public Nullable<decimal> Mach { get; set; }
        public Nullable<decimal> NhietDo { get; set; }
        public string HuyetAp { get; set; }
        public Nullable<decimal> NhipTho { get; set; }
        public string KhamTaiMuiHong { get; set; }
        public string KhamMat { get; set; }
        public string KhamNhanAp { get; set; }
        public string KhamRangHamMat { get; set; }
        public int DepartmentsID_DieuTri { get; set; }
        public string SoVaoVien { get; set; }
        public string SoBenhAn { get; set; }
    }
}
