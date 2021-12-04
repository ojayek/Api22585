using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreateXmlWebApi.Models
{
    public class TherapyResponseViewModel
    {
        public TherapyResponseViewModel()
        {
            lstTherapy= new List<TherapyViewModel>();
        }
        public List<TherapyViewModel> lstTherapy { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }


    }
}