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
    
    public partial class HealthRecipeKind
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HealthRecipeKind()
        {
            this.HealthRecipeProperties = new HashSet<HealthRecipeProperty>();
        }
    
        public System.Guid ObjectId { get; set; }
        public string RecipeDesc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HealthRecipeProperty> HealthRecipeProperties { get; set; }
    }
}
