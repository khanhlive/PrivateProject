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
    
    public partial class patientsExt
    {
        public int patientsID { get; set; }
        public Nullable<int> TinhThanhID { get; set; }
        public Nullable<int> QuanHuyenID { get; set; }
        public Nullable<int> XaPhuongID { get; set; }
        public string NguoiThan_Ten { get; set; }
        public string NguoiThan_SoDT { get; set; }
        public Nullable<int> QuocTichID { get; set; }
        public Nullable<int> NgheNgiepID { get; set; }
        public byte[] partientsPicture { get; set; }
        public Nullable<decimal> CanNang { get; set; }
    }
}
