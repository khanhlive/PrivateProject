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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetsID { get; set; }
        public int AssetsCateID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public int DepartmentsID { get; set; }
        public System.DateTime NgayNhap { get; set; }
        public int TinhTrangNhap { get; set; }
        public decimal SoNamDaSD { get; set; }
        public Nullable<System.DateTime> NamSanXuat { get; set; }
        public Nullable<decimal> DonGiaNhap { get; set; }
        public bool deleted { get; set; }
        public int userIDCreated { get; set; }
        public System.DateTime dateCreated { get; set; }
        public Nullable<System.DateTime> dateUpdated { get; set; }
        public int userIDUpdated { get; set; }
        public byte NumberUpdated { get; set; }
    }
}