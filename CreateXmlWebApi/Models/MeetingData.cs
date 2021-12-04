using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class MeetingData
    {
        public String Title { get; set; }
        public int Id { get; set; }
        public int MeetingNumber { get; set; }

        public string InnerParticipators { get; set; }
        public string OuterParticipators { get; set; }
        public string Location { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingDateStr { get; set; }
        public List<MeetingSubject> lstSubjects { get; set; }
        public List<ParticipatorSelect> lstParticipators { get; set; }



    }
    public class ParticipatorSelect
    {
        public int value { get; set; }
        
        public String label { get; set; }

    }
    public class MeetingSubject
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string tracingResponsible { get; set; }
        public int MeetingId { get; set; }
        public DateTime endDate { get; set; }
        public String endDateStr { get; set; }

    }
}