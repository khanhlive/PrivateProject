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
    
    public partial class DMICD10
    {
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ICD_ID { get; set; }
        public string ICD_Code { get; set; }
        public string ICD_NameWHOvn { get; set; }
        public string ICD_NameWHOen { get; set; }
        public string ICD_Name { get; set; }
        public bool Status { get; set; }
        public bool deleted { get; set; }
    }
}
