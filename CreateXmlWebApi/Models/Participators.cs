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
    
    public partial class Participators
    {
        public int Id { get; set; }
        public Nullable<int> MeetingId { get; set; }
        public Nullable<int> ParticipentId { get; set; }
        public string Description { get; set; }
    
        public virtual Meetings Meetings { get; set; }
        public virtual MeetingParticipents MeetingParticipents { get; set; }
    }
}