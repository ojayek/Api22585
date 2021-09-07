using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class SubjectViewModel
    {
        public string Subject { get; set; }
        public string tracingResponsible { get; set; }
        public string endDateStr { get; set; }
        public int id { get; set; }
        public string SubjectNumber { get; set; }       

    }
}