//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataFlow.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    [DataContract]
    public partial class Region : IAuditedEntity
    {
        public Region()
        {
            this.Territories = new HashSet<Territory>();
        }
    
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public string RegionDescription { get; set; }
    
        public virtual ICollection<Territory> Territories { get; set; }
    }
}