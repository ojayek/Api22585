using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using CreateXmlWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CreateXmlWebApi.Controllers
{
    public class TherapyController : ApiController
    {
        // GET: api/Contact

        private DarmanConnection db = new DarmanConnection();

        [HttpGet]
        [Route("api/Therapy/GetPersons")]
        //public IEnumerable<MeetingParticipent> GetPersons([FromBody] MeetingParticipent userInput)
        public TherapyResponseViewModel GetPersons()
        {
            var httpContext = HttpContext.Current;
            var data = db.Prs_GeneralView.Select(o => new { o.Prsnum, o.Nam, o.NamKhanevadegi, o.ShomarehBimeh, o.TarikhEstekhdam, o.Desc_Noe_EstekhDam, o.NoeEstekhdam, o.Proj_Name, o.Moavenat }).ToList();
            var Nam = httpContext.Request.Headers["Nam"];
            var NamKhanevadegi = httpContext.Request.Headers["NamKhanevadegi"];
            var CurrentPage = httpContext.Request.Headers["CurrentPage"];
            var PageSize = httpContext.Request.Headers["PageSize"];
            var lstTherapy = new List<TherapyViewModel>();
            if (string.IsNullOrEmpty(CurrentPage))
            {
                CurrentPage = "1";
            }
            if (string.IsNullOrEmpty(PageSize))
            {
                PageSize = "30";
            }
            if (!string.IsNullOrEmpty(Nam) || !string.IsNullOrEmpty(NamKhanevadegi))
            {
                data = data.Where(o => o.Nam.Contains(Nam) || o.NamKhanevadegi.Contains(NamKhanevadegi)).ToList();

            }
            var count = data.Count;
            var currentpage = int.Parse(CurrentPage);
            int pageSize = int.Parse(PageSize);
            if (currentpage == 0) { currentpage = 1; }
            if (pageSize == 0) { pageSize = 30; }
            int skip = (currentpage - 1) * pageSize;
            data = data.Skip(skip).Take(pageSize).ToList();
            foreach (var item in data)
            {
                var therapyData = new TherapyViewModel();
                therapyData.Prsnum = int.Parse(item.Prsnum.ToString());
                therapyData.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                therapyData.NameKhanevadeghi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                therapyData.Moavenat = (item.Moavenat != null) ? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                therapyData.VahehdSazemani = (item.Proj_Name != null) ? item.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                therapyData.TarikhEstekhdam = item.TarikhEstekhdam;
                therapyData.NoEstekhdam = (item.NoeEstekhdam != null) ? ((int)item.NoeEstekhdam) : -1;
                therapyData.Desc_Noe_Estekhdam = item.Desc_Noe_EstekhDam;
                therapyData.ShomarehBimeh = item.ShomarehBimeh;


                lstTherapy.Add(therapyData);
            }

            var lstResponseTherapy = new TherapyResponseViewModel();
            lstResponseTherapy.lstTherapy = lstTherapy;
            lstResponseTherapy.Count = count;
            lstResponseTherapy.CurrentPage = currentpage;
            lstResponseTherapy.TotalPage = count / int.Parse(PageSize);
            if (count > (lstResponseTherapy.TotalPage * int.Parse(PageSize)))
            {
                lstResponseTherapy.TotalPage = lstResponseTherapy.TotalPage + 1;
            }
            lstResponseTherapy.PageSize = int.Parse(PageSize);
            return lstResponseTherapy;
        }

        [HttpGet]
        [Route("api/Therapy/GetAllContract/")]

        public List<ContractDTO> GetAllContract()
        {
            var httpContext = HttpContext.Current;

            var contractDtos = new List<ContractDTO>();
            var contracts = db.HealthContracts.OrderByDescending(o => o.ContractDateAsOf).Take(5).ToList();
            foreach (var item in contracts)
            {
                var cdto = new ContractDTO();
                cdto.ObjectId = item.ObjectId;
                cdto.ContractNo = item.ContractNo;
                cdto.Contractor = item.Contractor;
                cdto.ContractType = item.ContractType;
                cdto.ContractDateTill = item.ContractDateTill;
                cdto.ContractDateAsOf = item.ContractDateAsOf;
                contractDtos.Add(cdto);

            }

            return contractDtos;

        }

        [HttpGet]
        [Route("api/Therapy/GetPackets/")]
        public List<HealthShoParBach> GetPackets()
        {
            var httpContext = HttpContext.Current;

            var packetData = new List<HealthShoParBach>();

            packetData = db.HealthShoParBaches.Where(o => o.Mah.Contains("1400")).OrderByDescending(o => o.Mah).ToList();

            return packetData;

        }

        [HttpGet]
        [Route("api/Therapy/GetPacketIsOpen/")]
        public List<HealthShoParBach> GetPacketIsOpen()
        {

            var packetData = new List<HealthShoParBach>();

            packetData = db.HealthShoParBaches.Where(o => o.Mah.Contains("1400") && !o.ToBimeAvalie && !o.TobimeNahaee).ToList();

            return packetData;

        }



        [HttpGet]
        [Route("api/Therapy/GetRelativeOfPerson/{Prsnum}")]
        public List<Prs_Aelemandi> GetRelativeOfPerson(int? Prsnum)
        {
            var httpContext = HttpContext.Current;

            var VisitData = new List<Prs_Aelemandi>();
            if (Prsnum != null)
            {
                VisitData = db.Prs_Aelemandi.Where(o => o.PrsNum == Prsnum).ToList();
                return VisitData;
            }
            else
            {
                return VisitData;
            }
        }

        [HttpGet]
        [Route("api/Therapy/GetAllRelative/")]
        public List<string> GetAllRelative()
        {
            var httpContext = HttpContext.Current;
            var VisitData = db.HealthVisitForWebs.DistinctBy(o => o.nesbat).Where(o => !String.IsNullOrEmpty(o.nesbat)).Select(o => o.nesbat).ToList();
            return VisitData;

        }

        [HttpGet]
        [Route("api/Therapy/GetAllVisit/{Prsnum:int?}")]
        public List<HealthVisitForWeb> GetAllVisit(int? Prsnum)
        {
            var httpContext = HttpContext.Current;
            var ContractNo = httpContext.Request.Headers["ContractNo"];
            var VisitData = new List<HealthVisitForWeb>();
            if (Prsnum != null)
            {

                if (string.IsNullOrEmpty(ContractNo))
                {
                    VisitData = db.HealthVisitForWebs.Where(o => o.Prsnum == Prsnum).OrderBy(o => o.VaziatBaste).ToList();
                    return VisitData;

                }
                else
                {
                    VisitData = db.HealthVisitForWebs.Where(o => o.Prsnum == Prsnum && o.ContractNo == ContractNo).OrderBy(o=>o.VaziatBaste).ToList();
                    return VisitData;
                }
            }
            else
            {
                VisitData = db.HealthVisitForWebs.Take(30).ToList();
                return VisitData;
            }


        }

        [HttpGet]
        [Route("api/Therapy/GetPacketData/{MahBachNo}")]
        public Object GetPacketData(string MahBachNo)
        {
            var httpContext = HttpContext.Current;           
            var VisitData = new List<Object>();
            var totalSum = 0;

            if (MahBachNo != null)
            {

                if (!string.IsNullOrEmpty(MahBachNo))
                {
                    var packetNo = int.Parse(MahBachNo);
                    var VisitDataTemp = db.HealthVisitViews.Where(o => o.ShomareBasteErsalBeBime == packetNo).ToList();
                    var rowNumber = 0;
                    var index = 1;
                  
                    var visitDataUniquePrsnum = VisitDataTemp.DistinctBy(o => o.Prsnum).Select(o => o.Prsnum);
                    foreach (var prsnum in visitDataUniquePrsnum)
                    {

                        Decimal explainedCost = 0;
                        Decimal confirmedCost = 0;
                        Decimal outOfContractCost = 0;
                        Decimal faranshizCost = 0;
                        Decimal paidValue = 0;
                      

                        foreach (var item in VisitDataTemp.Where(o=>o.Prsnum==prsnum))
                        {
                            rowNumber = rowNumber + 1;
                            index = index + 1;
                            var newObj = new
                            {
                                index = index,
                                rowNumber = rowNumber,
                                prsCode = item.Prsnum,
                                Nam = item.Nam,
                                nationalId = item.CodeMeli,
                                lastName = item.NamKhanevadegi,
                                patientName = item.NamBimar,
                                patientRelative = item.Nesbat,
                                prescriptionDate = item.Tarikh,
                                explainedCost = item.Khesarat,
                                confirmedCost = item.Taeed,
                                outOfContractCost = item.GairTaahod,
                                faranshizCost = item.Franshiz,
                                paidValue = item.pardakhti,
                                description = item.SharhDarman
                            };
                            explainedCost += item.Khesarat;
                            confirmedCost += item.Taeed!=null?item.Taeed.Value:0;
                            outOfContractCost += item.GairTaahod != null ? item.GairTaahod.Value : 0;
                            faranshizCost += item.Franshiz != null ? item.Franshiz.Value : 0;
                            paidValue += item.pardakhti != null ? item.pardakhti.Value : 0;

                            VisitData.Add(newObj);
                        }
                        var newTotalObj = new
                        {
                            index = index+1,
                            rowNumber = "جمع",
                            prsCode = prsnum,
                            Nam = "",
                            lastName = "",
                            patientName ="",
                            patientRelative = "",
                            prescriptionDate = "",
                            nationalId = "",
                            explainedCost = explainedCost,
                            confirmedCost = confirmedCost,
                            outOfContractCost = outOfContractCost,
                            faranshizCost = faranshizCost,
                            paidValue =paidValue,
                            description = ""
                        };
                        totalSum += int.Parse(paidValue.ToString());
                        VisitData.Add(newTotalObj);

                    }
                    return new { totalSum, VisitData };

                }
                else return new { totalSum, VisitData };

            }
            else
            {               
                return new { totalSum, VisitData };
            }


        }

        [HttpGet]
        [Route("api/Therapy/GetHealthRecipes/")]
        public List<HealthRecipeView> GetHealthRecipes()
        {
            var httpContext = HttpContext.Current;
            var data = db.HealthRecipeViews.ToList();
            return data;


        }


        [HttpPost]
        [Route("api/Therapy/DeleteRecipieKindById/")]
        public object DeleteRecipieKindById()
        {
            var httpContext = HttpContext.Current;
            try
            {
                var HealthRecipieKindId = httpContext.Request.Form["HealthRecipieKindId"];
                if (!string.IsNullOrEmpty(HealthRecipieKindId))
                {
                    var deletedItem = db.HealthRecipeKinds.Where(o => o.ObjectId == new Guid(HealthRecipieKindId)).SingleOrDefault();
                    if (deletedItem != null)
                    {
                        db.HealthRecipeKinds.Remove(deletedItem);
                        db.SaveChanges();
                        return new { Deleted = true, Description = "Item with this Guid:" + HealthRecipieKindId + "Removed" };
                    }
                    else
                    {
                        return new { Deleted = false, Description = "There is no Item with this Guid:" + HealthRecipieKindId + "in Database" };
                    }
                }
                else
                {
                    return new { Deleted = false, Description = "It should be Guid:" + HealthRecipieKindId };

                }
            }
            catch (Exception ex)
            {

                return new { Deleted = false, Description = "Error" + ex.InnerException.Message };
            }
        }

        [HttpGet]
        [Route("api/Therapy/GetHealthRecipesByContract/")]
        public List<RecipieKind> GetHealthRecipesByContract()
        {
            var httpContext = HttpContext.Current;
            var retData = new List<RecipieKind>();

            var ContractId = httpContext.Request.Headers["ContractId"];
            if (!string.IsNullOrEmpty(ContractId) && ContractId.ToLower() != "undefined")
            {
                var recipekindOfSpecificContract = db.HealthRecipeViews.Where(o => o.ContractId == new Guid(ContractId)).ToList();

                foreach (var item in recipekindOfSpecificContract)
                {
                    fillSomePropOfRecipeKind(retData, item.KindID.Value, item.RecipeDesc);
                }
                return retData;
            }
            else
            {
                var data = db.HealthRecipeKinds.ToList();
                foreach (var item in data)
                {
                    fillSomePropOfRecipeKind(retData, item.ObjectId, item.RecipeDesc);
                }
                return retData;
            }
        }

        private static void fillSomePropOfRecipeKind(List<RecipieKind> retData, Guid Id, String Desc)
        {
            var therapyData = new RecipieKind();
            therapyData.Id = Id;

            therapyData.Desc = Desc.Replace('ي', 'ی').Replace('ك', 'ک');

            retData.Add(therapyData);
        }

        [HttpGet]
        [Route("api/Therapy/GetSpecificPersonWithContracts/{Prsnum}")]
        public PersonDataWithContractList GetSpecificPersonWithContracts(int Prsnum)
        {
            var personDataWithContractList = new PersonDataWithContractList();
            var data = new List<ContractNoForPerson>();
            var httpContext = HttpContext.Current;


            data = db.HealthVisitForWebs.Where(o => o.Prsnum == Prsnum)
            .DistinctBy(o => o.ContractNo)
            .OrderByDescending(o => o.ContractDateAsOf)
            .Select(o => new ContractNoForPerson { value = o.RadifVorood.ToString(), label = o.ContractNo }).ToList();


            personDataWithContractList.lstContrtacts = data;
            var thisYearContract = db.HealthContracts.OrderByDescending(o => o.ContractDateAsOf).FirstOrDefault();
            if (thisYearContract != null)
            {
                var recipeProperties = from rp in db.HealthRecipeProperties
                                       join rk in db.HealthRecipeKinds
                                       on rp.KindID equals rk.ObjectId

                                       where rp.ContractId == thisYearContract.ObjectId
                                       select new DTORecipeProp { value = rp.ObjectId.ToString(), label = rk.RecipeDesc };
                personDataWithContractList.lstRecipeProperties = recipeProperties.ToList();
            }
            var relatives = db.Prs_Aelemandi.Where(o => o.PrsNum == Prsnum).ToList();
            foreach (var item in relatives)
            {
                var rItem = new DTORelative();
                rItem.value = item.ObjectId.ToString();
                rItem.label = item.Nam + " " + item.NamKhanevadegi;
                rItem.PersonId = item.MasterObjectId.ToString();
                personDataWithContractList.lstRelatives.Add(rItem);
            }

            var personData = db.Prs_GeneralView.Where(o => o.Prsnum == Prsnum).SingleOrDefault();
            if (personData != null)
            {
                personDataWithContractList.Prsnum = int.Parse(personData.Prsnum.ToString());
                personDataWithContractList.Nam = personData.Nam;
                personDataWithContractList.NameKhanevadeghi = personData.NamKhanevadegi;
                personDataWithContractList.ShomarehBimeh = personData.ShomarehBimeh;
                personDataWithContractList.Desc_Noe_Estekhdam = personData.Desc_Noe_EstekhDam;
                personDataWithContractList.TarikhEstekhdam = personData.TarikhEstekhdam;
                personDataWithContractList.Moavenat = personData.Moavenat;
                personDataWithContractList.VahehdSazemani = personData.Proj_Name;


            }

            return personDataWithContractList;
        }

        [HttpGet]
        [Route("api/Therapy/GetRecipePropByContractNo/")]
        public List<DTORecipeProp> GetRecipePropByContractNo()
        {

            var httpContext = HttpContext.Current;
            var retResponse = new List<DTORecipeProp>();
            var ContractNo = httpContext.Request.Headers["ContractNo"];
            if (!string.IsNullOrEmpty(ContractNo))
            {

                var thisSpecificContract = db.HealthContracts.Where(o => o.ContractNo == ContractNo).FirstOrDefault();
                if (thisSpecificContract != null)
                {
                    var recipeProperties = from rp in db.HealthRecipeProperties
                                           join rk in db.HealthRecipeKinds
                                           on rp.KindID equals rk.ObjectId

                                           where rp.ContractId == thisSpecificContract.ObjectId
                                           select new DTORecipeProp { value = rp.ObjectId.ToString(), label = rk.RecipeDesc };
                    retResponse.AddRange(recipeProperties.ToList());
                }
            }

            return retResponse;
        }

        [HttpPost]
        [Route("api/Therapy/CreateOrUpdateHealthVisit/{Id}")]
        public object CreateOrUpdateHealthVisit(string Id)
        {
            var hv = new HealthVisit();
            var req = HttpContext.Current.Request;
            try
            {
                if (Id == "0")
                {
                    hv.ObjectId = Guid.NewGuid();
                    FillPropOfHealthVisit(hv,req);
                    db.HealthVisits.Add(hv);
                    db.SaveChanges();
                    
                    return new { Update = false, Insert = true, Data = hv, Desc = "Inserted Successfully" };
                }
                else
                {
                    hv = db.HealthVisits.Where(o => o.ObjectId == new Guid(Id)).FirstOrDefault();
                    if (hv != null)
                    {
                        FillPropOfHealthVisit(hv,req);
                    }
                    db.SaveChanges();
                    return new { Update = true, Insert = false, Data = hv, Desc = "Updated Successfully" };

                }


            }
            catch (Exception ex)
            {

                return new { Update = false, Insert = false, Desc = "Error" + ex.Message };
            }

        }
        // test uis
        [HttpPost]
        [Route("api/Therapy/CreateOrUpdateRecipie/{Id}")]
        public object CreateOrUpdateRecipie(string Id)
        {
            var hrk = new HealthRecipeKind();
            try
            {
                if (Id == "0")
                {

                    hrk.ObjectId = Guid.NewGuid();

                    hrk.RecipeDesc = HttpContext.Current.Request.Form["RecipieDesc"];

                    db.HealthRecipeKinds.Add(hrk);
                    db.SaveChanges();
                    return new { Update = false, Insert = true, Data = hrk, Desc = "Inserted Successfully" };
                }
                else
                {
                    hrk = db.HealthRecipeKinds.Where(o => o.ObjectId == new Guid(Id)).FirstOrDefault();
                    if (hrk != null)
                    {
                        hrk.RecipeDesc = HttpContext.Current.Request.Form["RecipieDesc"];
                    }
                    db.SaveChanges();
                    return new { Update = true, Insert = false, Data = hrk, Desc = "Updated Successfully" };

                }


            }
            catch (Exception ex)
            {

                return new { Update = false, Insert = false, Desc = "Error" + ex.InnerException.Message };
            }

        }

        [HttpPost]
        [Route("api/Therapy/CreateOrUpdateHealthContract/{Id}")]
        public object CreateOrUpdateHealthContract(string Id)
        {
            var hc = new HealthContract();
            try
            {
                if (Id == "0")
                {

                    hc.ObjectId = Guid.NewGuid();

                    FillPropOfHealthContract(hc);

                    db.HealthContracts.Add(hc);
                    db.SaveChanges();
                    return new { Update = false, Insert = true, Data = hc, Desc = "Inserted Successfully" };
                }
                else
                {
                    hc = db.HealthContracts.Where(o => o.ObjectId == new Guid(Id)).FirstOrDefault();
                    if (hc != null)
                    {
                        FillPropOfHealthContract(hc);

                    }
                    db.SaveChanges();
                    return new { Update = true, Insert = false, Data = hc, Desc = "Updated Successfully" };

                }


            }
            catch (Exception ex)
            {

                return new { Update = false, Insert = false, Desc = "Error" + ex.Message };
            }

        }
        [HttpPost]
        [Route("api/Therapy/DeleteHealthContractById/")]
        public object DeleteHealthContractById()
        {
            var httpContext = HttpContext.Current;
            try
            {
                var HealthContractId = httpContext.Request.Form["ObjectId"];
                if (!string.IsNullOrEmpty(HealthContractId))
                {
                    var deletedItem = db.HealthContracts.Where(o => o.ObjectId == new Guid(HealthContractId)).SingleOrDefault();
                    if (deletedItem != null)
                    {
                        db.HealthContracts.Remove(deletedItem);
                        db.SaveChanges();
                        return new { Deleted = true, Description = "Item with this Guid:" + HealthContractId + "Removed" };
                    }
                    else
                    {
                        return new { Deleted = false, Description = "There is no Item with this Guid:" + HealthContractId + "in Database" };
                    }
                }
                else
                {
                    return new { Deleted = false, Description = "Id should be Guid:" + HealthContractId };

                }
            }
            catch (Exception ex)
            {

                return new { Deleted = false, Description = "Error" + ex.InnerException.Message };
            }
        }

        [HttpPost]
        [Route("api/Therapy/DeleteHealthVisitById/")]
        public object DeleteHealthVisitById()
        {
            var httpContext = HttpContext.Current;
            try
            {
                var HealthVisitId = httpContext.Request.Form["ObjectId"];
                if (!string.IsNullOrEmpty(HealthVisitId))
                {
                    var deletedItem = db.HealthVisits.Where(o => o.ObjectId == new Guid(HealthVisitId)).SingleOrDefault();
                    if (deletedItem != null)
                    {
                        db.HealthVisits.Remove(deletedItem);
                        db.SaveChanges();
                        return new { Deleted = true, Description = "Item with this Guid:" + HealthVisitId + "Removed" };
                    }
                    else
                    {
                        return new { Deleted = false, Description = "There is no Item with this Guid:" + HealthVisitId + "in Database" };
                    }
                }
                else
                {
                    return new { Deleted = false, Description = "Id should be Guid:" + HealthVisitId };

                }
            }
            catch (Exception ex)
            {

                return new { Deleted = false, Description = "Error" + ex.Message };
            }
        }


        [HttpPost]
        [Route("api/Therapy/CreateOrUpdateHealthShoParBach/{Id}")]
        public object CreateOrUpdateHealthShoParBach(string Id)
        {
            var hp = new HealthShoParBach();
            try
            {
                if (Id == "0")
                {

                    hp.ObjectId = Guid.NewGuid();

                    FillPropOfHealthShoParBach(hp, false);

                    db.HealthShoParBaches.Add(hp);
                    db.SaveChanges();
                    return new { Update = false, Insert = true, Data = hp, Desc = "Inserted Successfully" };
                }
                else
                {
                    hp = db.HealthShoParBaches.Where(o => o.ObjectId == new Guid(Id)).FirstOrDefault();
                    if (hp != null)
                    {
                        FillPropOfHealthShoParBach(hp, true);
                    }
                    db.SaveChanges();
                    return new { Update = true, Insert = false, Data = hp, Desc = "Updated Successfully" };

                }


            }
            catch (Exception ex)
            {

                return new { Update = false, Insert = false, Desc = "Error" + ex.Message };
            }

        }

        [HttpPost]
        [Route("api/Therapy/DeleteHealthShoParBachById/")]
        public object DeleteHealthShoParBachById()
        {
            var httpContext = HttpContext.Current;
            try
            {
                var HealthShoParBachId = httpContext.Request.Form["Id"];
                if (!string.IsNullOrEmpty(HealthShoParBachId))
                {
                    var deletedItem = db.HealthShoParBaches.Where(o => o.ObjectId == new Guid(HealthShoParBachId)).SingleOrDefault();
                    if (deletedItem != null)
                    {
                        db.HealthShoParBaches.Remove(deletedItem);
                        db.SaveChanges();
                        return new { Deleted = true, Description = "Item with this Guid:" + HealthShoParBachId + "Removed" };
                    }
                    else
                    {
                        return new { Deleted = false, Description = "There is no Item with this Guid:" + HealthShoParBachId + "in Database" };
                    }
                }
                else
                {
                    return new { Deleted = false, Description = "It should be Guid:" + HealthShoParBachId };

                }
            }
            catch (Exception ex)
            {

                return new { Deleted = false, Description = "Error" + ex.InnerException.Message };
            }
        }

        private void FillPropOfHealthShoParBach(HealthShoParBach hp, Boolean update)
        {
            hp.Mah = PersianToEnglish(HttpContext.Current.Request.Form["Mah"]);
            //var pdate = int.Parse(hp.Mah.Split('/')[0])+"/" + int.Parse(hp.Mah.Split('/')[1]) + "/" + int.Parse("0" +hp.Mah.Split('/')[2]);
            // hp.Mah = pdate;
            hp.MahBachNo = int.Parse(HttpContext.Current.Request.Form["MahBachNo"]);
            hp.ShomareParvande = HttpContext.Current.Request.Form["ShomareParvande"];
            if (!update)
            {
                var toBimeh = int.Parse(HttpContext.Current.Request.Form["ToBimeAvalie"]);
                if (toBimeh == 0)
                {
                    hp.ToBimeAvalie = false;
                    hp.TobimeNahaee = false;
                }


            }
            hp.VaziatBaste = int.Parse(HttpContext.Current.Request.Form["VaziatBaste"]);
        }

        private void FillPropOfHealthContract(HealthContract hc)
        {
            var newhc = hc;
            try
            {

                hc.ContractNo = (HttpContext.Current.Request.Form["ContractNo"]);
            }
            catch (Exception ex)
            {
                hc.ContractNo = newhc.ContractNo;

            }
            try
            {

                hc.Contractor = HttpContext.Current.Request.Form["Contractor"];
            }
            catch (Exception ex)
            {
                hc.Contractor = newhc.Contractor;

            }
            try
            {

                hc.ContractDateAsOf = PersianToEnglish(HttpContext.Current.Request.Form["ContractDateAsOf"]);
            }
            catch (Exception ex)
            {
                hc.ContractDateAsOf = newhc.ContractDateAsOf;

            }
            try
            {

                hc.ContractDateTill = PersianToEnglish(HttpContext.Current.Request.Form["ContractDateTill"]);
            }
            catch (Exception)
            {
                hc.ContractDateTill = newhc.ContractDateTill;

            }
            try
            {

                hc.ContractType = HttpContext.Current.Request.Form["ContractType"];
                if (hc.ContractType == "9")
                {
                    hc.ContractType = newhc.ContractType;
                }
            }
            catch (Exception ex)
            {
                hc.ContractType = newhc.ContractType;
            }
        }


        private void FillPropOfHealthVisit(HealthVisit hv,HttpRequest req)
        {
            var newhv = hv;
            try
            {

                hv.BachId =new Guid(req.Form["BatchId"]);
            }
            catch (Exception ex)
            {
                hv.BachId = newhv.BachId;

            }
            try
            {
                var prsnum =int.Parse(req.Form["PrsNum"]);
                

               var prsGuid=db.Prs_Aelemandi.Where(o=>o.PrsNum == prsnum && o.Nesbat==0).SingleOrDefault() ;
                if(prsGuid != null)
                {
                    hv.PersonelId = prsGuid.MasterObjectId;
                }
                else
                {
                    hv.PersonelId = newhv.PersonelId;
                }
            }
            catch (Exception ex)
            {
                hv.PersonelId = newhv.PersonelId;

            }
            try
            {

                hv.PatientId =new Guid(req.Form["PatientId"]);
            }
            catch (Exception ex)
            {
                hv.PatientId = newhv.PatientId;

            }
            try
            {

                hv.RecipeDate = PersianToEnglish(req.Form["RecipeDate"]);
            }
            catch (Exception)
            {
                hv.RecipeDate = newhv.RecipeDate;

            }
            try
            {

                hv.RecipeMoney = int.Parse(req.Form["RecipeMoney"]);
               
            }
            catch (Exception ex)
            {
                hv.RecipeMoney = newhv.RecipeMoney;
            }
            try
            {

                hv.RecipePropertiesId = new Guid (req.Form["RecipePropertiesId"]);

            }
            catch (Exception ex)
            {
                hv.RecipePropertiesId = newhv.RecipePropertiesId;
            }
            try
            {

                hv.Franshiz =int.Parse(req.Form["Franshiz"]);

            }
            catch (Exception ex)
            {
                hv.Franshiz = 0;
            }
            try
            {

                hv.OutofContractMoney = int.Parse(req.Form["OutofContractMoney"]);

            }
            catch (Exception ex)
            {
                hv.OutofContractMoney = 0;
            }
        }


        [HttpPost]
        [Route("api/Therapy/CreateInsuranceProcess/")]
        public HealthRecipeProperty CreateInsuranceProcess()
        {
            var hrp = new HealthRecipeProperty();
            hrp.ObjectId = Guid.NewGuid();

            hrp.DoctorId = HttpContext.Current.Request.Form["DoctorId"];
            hrp.KindID = new Guid(HttpContext.Current.Request.Form["KindID"]);
            hrp.MaxMoney4Pay = Convert.ToDecimal(HttpContext.Current.Request.Form["MaxMoney4Pay"]);
            hrp.MinMoney4Pay = Convert.ToDecimal(HttpContext.Current.Request.Form["MinMoney4Pay"]);
            hrp.ContractId = new Guid(HttpContext.Current.Request.Form["ContractId"]);
            hrp.Franshiz = Convert.ToInt32(HttpContext.Current.Request.Form["Faranshiz"]);
            hrp.Kardex = Convert.ToBoolean(HttpContext.Current.Request.Form["Kardex"]);
            hrp.ChandNafar = Convert.ToInt16(HttpContext.Current.Request.Form["ChandNafar"]);

            db.HealthRecipeProperties.Add(hrp);

            db.SaveChanges();
            return hrp;
        }

        private string ConvertToShamsi(DateTime miladi)
        {
            PersianCalendar pc = new PersianCalendar();

            DateTime dt = new DateTime(miladi.Year, miladi.Month, miladi.Day); // یا DateTime.Now

            string PersianDate = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
            return PersianDate;

        }
        private DateTime ConvertToMiladi(string persianDate)
        {
            PersianCalendar pc = new PersianCalendar();
            int Year = int.Parse(persianDate.Split('/')[0]);
            int Month = int.Parse(persianDate.Split('/')[1]);
            int Day = int.Parse(persianDate.Split('/')[2]);
            DateTime dt = new DateTime(Year, Month, Day, pc);
            return dt;
        }

        public byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
        public string PersianToEnglish(string persianStr)
        {
            Dictionary<string, string> LettersDictionary = new Dictionary<string, string>
            {
                ["۰"] = "0",
                ["۱"] = "1",
                ["۲"] = "2",
                ["۳"] = "3",
                ["۴"] = "4",
                ["۵"] = "5",
                ["۶"] = "6",
                ["۷"] = "7",
                ["۸"] = "8",
                ["۹"] = "9"
            };
            return LettersDictionary.Aggregate(persianStr, (current, item) =>
                         current.Replace(item.Key, item.Value));
        }


        //[HttpGet]
        //[Route("api/Meeting/SearchPersonByInputValue")]
        //public IEnumerable<MeetingParticipents> SearchPersonByInputValue()
        //{
        //    var httpContext = HttpContext.Current;
        //    var SearchKey = httpContext.Request.Headers["SearchKey"];
        //    var SearchKeyStr = Encoding.UTF8.GetString(Convert.FromBase64String(SearchKey));

        //    var data = db.MeetingParticipents.ToList();

        //    var lstContacts = new List<MeetingParticipents>();
        //    if (!string.IsNullOrEmpty(SearchKeyStr))
        //    {
        //        data = db.MeetingParticipents.Where(o => o.Nam.Contains(SearchKeyStr) || o.NamKhanevadegi.Contains(SearchKeyStr)).ToList();

        //    }
        //    foreach (var item in data)
        //    {
        //        var meetData = new MeetingParticipents();
        //        meetData.Prsnum = int.Parse(item.Prsnum.ToString());
        //        meetData.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك', 'ک') : null;
        //        meetData.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک') : null;
        //        meetData.Moavenat = (item.Moavenat != null) ? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک') : null;
        //        meetData.VahedSazmani = (item.VahedSazmani != null) ? item.VahedSazmani.Replace('ي', 'ی').Replace('ك', 'ک') : null;

        //        lstContacts.Add(meetData);
        //    }
        //    lstContacts = lstContacts.OrderBy(o => o.NamKhanevadegi).ToList();
        //    if (lstContacts.Count > 30)
        //    {
        //        return lstContacts.Take(30);
        //    }
        //    else return lstContacts;
        //}
        //[HttpGet]
        //[Route("api/Meeting/GetAllMeeting")]
        //public IEnumerable<MeetingData> GetAllMeeting()
        //{
        //    var data = db.Meetings.ToList();
        //    var lstContacts = new List<MeetingData>();

        //    foreach (var item in data)
        //    {
        //        var meetData = new MeetingData();
        //        meetData.Id = int.Parse(item.Id.ToString());
        //        meetData.Location = item.Location.ToString();
        //        meetData.Title = item.Title.ToString();
        //        meetData.InnerParticipators = (item.InnerParticipator != null) ? item.InnerParticipator.ToString() : "";
        //        meetData.MeetingDateStr = item.MeetingDate.HasValue ? ConvertToShamsi(item.MeetingDate.Value) : null;
        //        meetData.MeetingNumber = item.MeetingNumber.HasValue ? item.MeetingNumber.Value : 0;
        //        meetData.OuterParticipators = item.OuterParticipator;

        //        //prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
        //        //prsdata.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك','ک'):null;
        //        //prsdata.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک'):null;
        //        //prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
        //        //prsdata.Moavenat =(item.Moavenat !=null)? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک'):null;
        //        //prsdata.Email = item.Email;
        //        //prsdata.Proj_Name = (item.Proj_Name != null) ? item.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک'):null;
        //        //prsdata.NumBuild = (item.NumBuild != null) ? item.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک'):null;
        //        //prsdata.NamLatin = item.NamLatin;
        //        //prsdata.Sharh_Onvan = (item.Sharh_Onvan != null) ? item.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک'):null;
        //        //if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
        //        //{
        //        //    lstContacts.Add(prsdata);
        //        //}
        //        lstContacts.Add(meetData);
        //    }
        //    var teldatas = db.Meetings.Where(o => (!string.IsNullOrEmpty(o.Title) || !string.IsNullOrEmpty(o.Title)));
        //    //foreach (var item in teldatas)
        //    //{
        //    //    var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
        //    //    if (contact != null)
        //    //    {
        //    //        contact.Tel = item.Tel;
        //    //        contact.DirectPhoneNo = item.DirectPhoneNo;
        //    //    }
        //    //}
        //    return lstContacts;
        //}

        //[HttpGet]
        //[Route("api/Meeting/GetMeetingById/{Id}")]
        //public IEnumerable<MeetingData> GetMeetingById(int Id)
        //{
        //    var data = db.Meetings.Where(o => o.Id == Id).ToList();
        //    var participatorData = db.Participators.Where(o => o.MeetingId == Id);
        //    var lstContacts = new List<MeetingData>();

        //    foreach (var item in data)
        //    {
        //        var prsdata = new MeetingData();
        //        prsdata.lstSubjects = new List<MeetingSubject>();
        //        prsdata.Id = int.Parse(item.Id.ToString());
        //        prsdata.Title = (item.Title != null) ? item.Title.Replace('ي', 'ی').Replace('ك', 'ک') : null;
        //        prsdata.InnerParticipators = (item.InnerParticipator != null) ? item.InnerParticipator.Replace('ي', 'ی').Replace('ك', 'ک') : null;

        //        prsdata.OuterParticipators = (item.OuterParticipator != null) ? item.OuterParticipator.Replace('ي', 'ی').Replace('ك', 'ک') : null;

        //        prsdata.Location = (item.Location != null) ? item.Location.Replace('ي', 'ی').Replace('ك', 'ک') : null;
        //        prsdata.MeetingDateStr = ConvertToShamsi(item.MeetingDate.HasValue ? item.MeetingDate.Value : DateTime.Now);
        //        prsdata.MeetingNumber = int.Parse(item.MeetingNumber.ToString());



        //        if (!lstContacts.Where(o => o.Id == prsdata.Id).Any())
        //        {
        //            var meetingSubjects = db.MeetingSubjects.Where(o => o.MeetingId == item.Id).ToList();
        //            foreach (var meetSubject in meetingSubjects)
        //            {
        //                var ms = new MeetingSubject();
        //                ms.Id = meetSubject.Id;
        //                ms.Subject = meetSubject.SubTitle;
        //                ms.tracingResponsible = meetSubject.Responsible;
        //                ms.MeetingId = prsdata.Id;
        //                ms.endDateStr = meetSubject.DeadLine.HasValue ? ConvertToShamsi(meetSubject.DeadLine.Value) : null;
        //                prsdata.lstSubjects.Add(ms);
        //            }
        //            lstContacts.Add(prsdata);
        //        }
        //        var participatorsFullProp = from p in db.Participators
        //                                    join mp in db.MeetingParticipents
        //                                    on p.ParticipentId equals mp.Prsnum
        //                                    where p.MeetingId == Id
        //                                    select new ParticipatorSelect{ value = mp.Prsnum, label = mp.Nam + "-" + mp.NamKhanevadegi };
        //        prsdata.lstParticipators = participatorsFullProp.ToList();
        //    }
        //    return lstContacts;
        //}

        //[HttpPost]
        //[Route("api/Meeting/CreateMeeting")]
        //public Int32 CreateMeeting()
        //{
        //    var MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
        //    Meetings updateMeeting = db.Meetings.Where(o => o.MeetingNumber == MeetingNumber).SingleOrDefault();
        //    var createOrUpdate = (updateMeeting == null) ? true : false;

        //    var file1 = HttpContext.Current.Request.Files.Count > 0 ?   HttpContext.Current.Request.Files[0] : null;

        //    if (file1 != null && file1.ContentLength > 0)
        //    {
        //        var fileName = file1.FileName;
        //    }
        //    Meetings newMeeting = CreateOrUpdateMeetingInDB(createOrUpdate? new Meetings():updateMeeting, createOrUpdate); ;
        //    var afterInsert = db.Meetings.Where(o => o.MeetingNumber == newMeeting.MeetingNumber).SingleOrDefault();

        //    if (afterInsert != null)
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            // For add File To DB Table
        //            var docfiles = new List<string>();
        //            foreach (string file in httpRequest.Files)
        //            {
        //                CreateOrUpdateFileInDB(afterInsert, httpRequest, file,createOrUpdate);
        //            }
        //        }
        //        var selectedParticipators = JsonConvert.DeserializeObject < List < ParticipatorViewModel >> (HttpContext.Current.Request.Form["SelectedParticipators"]);


        //        List<SubjectViewModel> lstSubjects = JsonConvert.DeserializeObject<List<SubjectViewModel>>
        //                                                             (HttpContext.Current.Request.Form["lstSubjects"]);
        //        var oldsubjects = db.MeetingSubjects.Where(o => o.MeetingId == updateMeeting.Id);
        //        var oldparticipators = db.Participators.Where(o => o.MeetingId == updateMeeting.Id);
        //        if (createOrUpdate ==false)//means update
        //        {
        //            foreach (var item in oldsubjects)
        //            {
        //                db.MeetingSubjects.Remove(item);

        //            }
        //            foreach (var item in oldparticipators)
        //            {
        //                db.Participators.Remove(item);

        //            }
        //            db.SaveChanges();
        //        }

        //        foreach (var item in lstSubjects)
        //        {
        //            CreateSubjectInDB(afterInsert, item);
        //        }
        //        foreach (var item in selectedParticipators)
        //        {
        //            CreateParticipatorInDB(afterInsert, item);
        //        }
        //    }
        //    return afterInsert.Id;
        //}
        //private void CreateParticipatorInDB(Meetings afterInsert, ParticipatorViewModel item)
        //{

        //    var newParticipator = new Participators();
        //    newParticipator.MeetingId = afterInsert.Id;
        //    newParticipator.ParticipentId = item.Prsnum;

        //    db.Participators.Add(newParticipator);
        //    db.SaveChanges();
        //}


        //private void CreateSubjectInDB(Meetings afterInsert, SubjectViewModel item)
        //{

        //    var newSubject = new MeetingSubjects();
        //    newSubject.SubTitle = item.Subject;
        //    newSubject.Responsible = item.tracingResponsible;
        //    try
        //    {
        //        newSubject.DeadLine = ConvertToMiladi(item.endDateStr);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    newSubject.Number = item.SubjectNumber;

        //    newSubject.MeetingId = afterInsert.Id;
        //    db.MeetingSubjects.Add(newSubject);
        //    db.SaveChanges();
        //}

        //private void CreateOrUpdateFileInDB(Meetings afterInsert, HttpRequest httpRequest, string file, bool createOrUpdate)
        //{
        //    var postedFile = httpRequest.Files[file].InputStream;
        //    var updateFile = db.MeetingFiles.Where(o => o.MeetingId == afterInsert.Id).SingleOrDefault();
        //    if(updateFile == null) { createOrUpdate = true; }
        //    var newMeetingFile = createOrUpdate ? new MeetingFiles() : updateFile;

        //    newMeetingFile.MeetingFile = ReadToEnd(postedFile);
        //    newMeetingFile.MeetingId = afterInsert.Id;
        //    newMeetingFile.FileName = httpRequest.Files[file].FileName;
        //    newMeetingFile.FileType = httpRequest.Files[file].ContentType;
        //    newMeetingFile.FileSize = httpRequest.Files[file].ContentLength;

        //    if (createOrUpdate ) { db.MeetingFiles.Add(newMeetingFile); }            
        //    db.SaveChanges();
        //}

        //private Meetings CreateOrUpdateMeetingInDB(Meetings meeting,bool createOrUpdate)
        //{

        //    meeting.Title = HttpContext.Current.Request.Form["Title"];
        //    meeting.MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
        //    meeting.InnerParticipator = HttpContext.Current.Request.Form["InnerParticipators"];
        //    meeting.OuterParticipator = HttpContext.Current.Request.Form["OuterParticipators"];
        //    meeting.Location = HttpContext.Current.Request.Form["Location"];
        //    meeting.MeetingDate = ConvertToMiladi(HttpContext.Current.Request.Form["MeetingDateStr"]);
        //    if (createOrUpdate) { db.Meetings.Add(meeting); }

        //    db.SaveChanges();
        //    return meeting;
        //}

        //[HttpPost]
        //[Route("api/Meeting/UpdateMeeting")]
        //public Int32 UpdateMeeting()
        //{
        //    var file1 = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

        //    if (file1 != null && file1.ContentLength > 0)
        //    {
        //        var fileName = file1.FileName;
        //    }
        //    var MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
        //    Meetings updateMeeting = db.Meetings.Where(o => o.MeetingNumber == MeetingNumber).SingleOrDefault();
        //    CreateOrUpdateMeetingInDB(updateMeeting, false);

        //    if (updateMeeting != null)
        //    {
        //        var httpRequest = HttpContext.Current.Request;
        //        var updateFile=db.MeetingFiles.Where(o => o.MeetingId == updateMeeting.Id);
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            // For add File To DB Table
        //            var docfiles = new List<string>();
        //            foreach (string file in httpRequest.Files)
        //            {
        //                CreateOrUpdateFileInDB(updateMeeting, httpRequest, file,false);
        //            }
        //        }

        //        List<SubjectViewModel> lstSubjects = JsonConvert.DeserializeObject<List<SubjectViewModel>>
        //                                                             (HttpContext.Current.Request.Form["lstSubjects"]);

        //        var oldsubjects = db.MeetingSubjects.Where(o => o.MeetingId == updateMeeting.Id);
        //        foreach (var item in oldsubjects)
        //        {
        //            db.MeetingSubjects.Remove(item);
        //            db.SaveChanges();
        //        }

        //        foreach (var item in lstSubjects)
        //        {
        //            CreateSubjectInDB(updateMeeting, item);
        //        }
        //    }
        //    return (updateMeeting !=null)?updateMeeting.Id:0;
        //}
    }


}



