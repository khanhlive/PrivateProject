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
    
    public partial class Feature
    {
        public int FeatureID { get; set; }
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }
        public string FeatureDescription { get; set; }
        public byte Levels { get; set; }
        public string RedirectURL { get; set; }
        public Nullable<int> FeatureID_TrucThuoc { get; set; }
        public string FeatureIcon { get; set; }
        public bool Status { get; set; }
    }
}
