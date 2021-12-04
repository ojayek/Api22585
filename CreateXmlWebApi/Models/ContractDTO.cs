using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class ContractDTO
    {
        public System.Guid ObjectId { get; set; }
        public string ContractNo { get; set; }
        public string Contractor { get; set; }
        public string ContractDateAsOf { get; set; }
        public string ContractDateTill { get; set; }
        public string ContractType { get; set; }
    }
}