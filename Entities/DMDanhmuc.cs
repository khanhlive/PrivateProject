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
    
    public partial class DMDanhmuc
    {
        public int DanhMucID { get; set; }
        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public int LoaiDanhMuc { get; set; }
        public bool Status { get; set; }
        public Nullable<bool> deleted { get; set; }
        public int userIDCreated { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateUpdated { get; set; }
        public Nullable<int> userIDUpdated { get; set; }
        public byte NumberUpdated { get; set; }
    }
}
