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
    
    public partial class DMTinhThanh
    {
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int TinhThanhID { get; set; }
        public string TinhThanh_Code { get; set; }
        public string TinhThanh_Name { get; set; }
        public bool TinhThanh_Status { get; set; }
        public byte TinhThanh_Cap { get; set; }
    }
}
