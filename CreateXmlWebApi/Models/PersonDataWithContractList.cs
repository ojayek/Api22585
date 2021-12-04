using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class PersonDataWithContractList
    {
        public PersonDataWithContractList()
        {
            lstContrtacts = new List<ContractNoForPerson>();
            lstRecipeProperties = new List<DTORecipeProp>();
            lstRelatives = new List<DTORelative>();
        }
        public List<ContractNoForPerson> lstContrtacts { get; set; }

        public List<DTORecipeProp> lstRecipeProperties { get; set; }
        public List<DTORelative> lstRelatives { get; set; }

        public int Prsnum { get; set; }
        public string ShomarehBimeh { get; set; }
        public string Nam { get; set; }
        public string NameKhanevadeghi { get; set; }
        public string Desc_Noe_Estekhdam { get; set; }
        public string TarikhEstekhdam { get; set; }
        public string VahehdSazemani { get; set; }
        public string Moavenat { get; set; }

    }
}