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
    
    public partial class Department
    {
        public int DepartmentsID { get; set; }
        public string DepartmentsName { get; set; }
        public string DepartmentsAddress { get; set; }
        public string DepartmentsCode { get; set; }
        public Nullable<byte> ChuyenKhoa_Code { get; set; }
        public string MaBenhVien_TrucThuoc { get; set; }
        public Nullable<int> DepartmentsID_TrucThuoc { get; set; }
        public byte ChucNang { get; set; }
        public byte Levels { get; set; }
        public bool Status { get; set; }
        public bool deleted { get; set; }
        public int userIDCreated { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateUpdated { get; set; }
        public Nullable<int> userIDUpdated { get; set; }
        public byte NumberUpdated { get; set; }
    }
}
