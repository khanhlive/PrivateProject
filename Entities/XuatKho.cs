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
    
    public partial class XuatKho
    {
        public int XuatKhoID { get; set; }
        public int DepartmentsID_KhoX { get; set; }
        public byte LoaiPhieuXuat { get; set; }
        public Nullable<int> DepartmentsID_NoiNhan { get; set; }
        public Nullable<int> patientsID { get; set; }
        public string NguoiNhan { get; set; }
        public string NoiDungXuat { get; set; }
        public System.DateTime NgayXuat { get; set; }
        public Nullable<System.DateTime> NgayCT { get; set; }
        public string SoCT { get; set; }
        public byte Duyet_status { get; set; }
        public Nullable<int> Duyet_employeesID { get; set; }
        public Nullable<System.DateTime> Duyet_Date { get; set; }
        public int userIDCreated { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateUpdated { get; set; }
        public int userIDUpdated { get; set; }
        public byte NumberUpdated { get; set; }
    }
}
