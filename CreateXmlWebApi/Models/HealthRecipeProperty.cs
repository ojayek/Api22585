//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CreateXmlWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HealthRecipeProperty
    {
        public System.Guid ObjectId { get; set; }
        public Nullable<System.Guid> KindID { get; set; }
        public string DoctorId { get; set; }
        public decimal MaxMoney4Pay { get; set; }
        public Nullable<decimal> MinMoney4Pay { get; set; }
        public Nullable<System.Guid> ContractId { get; set; }
        public Nullable<int> Franshiz { get; set; }
        public Nullable<bool> Kardex { get; set; }
        public Nullable<int> ChandNafar { get; set; }
    
        public virtual HealthContract HealthContract { get; set; }
        public virtual HealthRecipeKind HealthRecipeKind { get; set; }
    }
}
