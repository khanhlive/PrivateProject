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
    
    public partial class KhamBenh
    {
        public int KhamBenhID { get; set; }
        public int patientsID { get; set; }
        public System.DateTime NgayKham { get; set; }
        public Nullable<System.DateTime> NgayKetThucKham { get; set; }
        public string ChanDoanBD { get; set; }
        public Nullable<int> BenhChinh_MaICD { get; set; }
        public string BenhKemTheo_MaICDs { get; set; }
        public int DepartmentsID { get; set; }
        public int employeesID { get; set; }
        public Nullable<int> PhuongAn { get; set; }
        public string GuiKham_DepartmentsIDs { get; set; }
        public byte LoaiKham { get; set; }
        public string ChuyenKhoa_Code { get; set; }
        public bool deleted { get; set; }
        public int userIDCreated { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateUpdated { get; set; }
        public int userIDUpdated { get; set; }
    
        public virtual patient patient { get; set; }
    }
}
