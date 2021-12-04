using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class RecipieKind
    {
        public Guid Id { get; set; }
        public string Desc { get; set; }
        public string ContractNo { get; set; }
        public string StartOfContract { get; set; }
        public string EndOfContract { get; set; }
        public string RKD { get; set; }

    }
}