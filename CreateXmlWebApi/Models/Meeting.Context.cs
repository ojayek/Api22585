﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MoshanirMeetingEntities : DbContext
    {
        public MoshanirMeetingEntities()
            : base("name=MoshanirMeetingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MeetingFiles> MeetingFiles { get; set; }
        public virtual DbSet<Meetings> Meetings { get; set; }
        public virtual DbSet<MeetingSubjects> MeetingSubjects { get; set; }
        public virtual DbSet<Participators> Participators { get; set; }
        public virtual DbSet<MeetingParticipents> MeetingParticipents { get; set; }
    }
}