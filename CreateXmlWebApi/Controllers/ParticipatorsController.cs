using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using CreateXmlWebApi.Models;
using Newtonsoft.Json;

namespace CreateXmlWebApi.Controllers
{
    public class ParticipatorsController : ApiController
    {
        private MoshanirMeetingEntities db = new MoshanirMeetingEntities();

        [HttpGet]
        [Route("api/Participator/GetParticipators")]
        //public IEnumerable<MeetingParticipent> GetPersons([FromBody] MeetingParticipent userInput)
        public IEnumerable<MeetingParticipents> GetParticipators()
        {
            var lstContacts = new List<MeetingParticipents>();
            var data = db.MeetingParticipents.ToList();          
            foreach (var item in data)
            {
                var mp = new MeetingParticipents();
                SetParticipentProps(item, mp);

                lstContacts.Add(mp);
            }
            return lstContacts;
        }

        private static void SetParticipentProps(MeetingParticipents item, MeetingParticipents mp)
        {
            mp.Prsnum = int.Parse(item.Prsnum.ToString());
            mp.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك', 'ک') : null;
            mp.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک') : null;
            // Moavenat Means CompanyName
            mp.Moavenat = (item.Moavenat != null) ? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک') : null;
            // VahedSazmani Means Job Position In Company
            mp.VahedSazmani = (item.VahedSazmani != null) ? item.VahedSazmani.Replace('ي', 'ی').Replace('ك', 'ک') : null;
            mp.Noe_Eshteghal = item.Noe_Eshteghal;
            mp.Noe_Sherkat = item.Noe_Sherkat;
            mp.MobileNumber = item.MobileNumber;
            mp.Email = item.Email;
        }

        [HttpGet]
        [Route("api/Participator/SearchPersonByInputValue")]
        public IEnumerable<MeetingParticipents> SearchPersonByInputValue()
        {
            var httpContext = HttpContext.Current;
            var SearchKey = httpContext.Request.Headers["SearchKey"];
            var SearchKeyStr = Encoding.UTF8.GetString(Convert.FromBase64String(SearchKey));

            var data = db.MeetingParticipents.ToList();

            var lstContacts = new List<MeetingParticipents>();
            if (!string.IsNullOrEmpty(SearchKeyStr))
            {
                data = db.MeetingParticipents.Where(o => o.Nam.Contains(SearchKeyStr) || o.NamKhanevadegi.Contains(SearchKeyStr)).ToList();

            }
            foreach (var item in data)
            {
                var mp = new MeetingParticipents();
                SetParticipentProps(item, mp);

                lstContacts.Add(mp);
            }
            lstContacts = lstContacts.OrderBy(o => o.NamKhanevadegi).ToList();
            if (lstContacts.Count > 30)
            {
                return lstContacts.Take(30);
            }
            else return lstContacts;
        }
      
        [HttpGet]
        [Route("api/Participator/GetParticipatorById/{Id}")]
        public MeetingParticipents GetParticipatorById(int Id)
        {
            var data = db.MeetingParticipents.Where(o => o.Prsnum == Id).ToList();          
            var mp = new MeetingParticipents();

            foreach (var item in data)
            {
                SetParticipentProps(item, mp);
            }
            return mp;
        }

        [HttpPost]
        [Route("api/Participator/CreateParticipator")]
        public Int32 CreateParticipator()
        {
            return 1;
            //var MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
            //Meetings updateMeeting = db.Meetings.Where(o => o.MeetingNumber == MeetingNumber).SingleOrDefault();
            //var createOrUpdate = (updateMeeting == null) ? true : false;

            //var file1 = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            //if (file1 != null && file1.ContentLength > 0)
            //{
            //    var fileName = file1.FileName;
            //}
            //Meetings newMeeting = CreateOrUpdateMeetingInDB(createOrUpdate ? new Meetings() : updateMeeting, createOrUpdate); ;
            //var afterInsert = db.Meetings.Where(o => o.MeetingNumber == newMeeting.MeetingNumber).SingleOrDefault();

            //if (afterInsert != null)
            //{
            //    var httpRequest = HttpContext.Current.Request;
            //    if (httpRequest.Files.Count > 0)
            //    {
            //        // For add File To DB Table
            //        var docfiles = new List<string>();
            //        foreach (string file in httpRequest.Files)
            //        {
            //            CreateOrUpdateFileInDB(afterInsert, httpRequest, file, createOrUpdate);
            //        }
            //    }
            //    var selectedParticipators = JsonConvert.DeserializeObject<List<ParticipatorViewModel>>(HttpContext.Current.Request.Form["SelectedParticipators"]);


            //    List<SubjectViewModel> lstSubjects = JsonConvert.DeserializeObject<List<SubjectViewModel>>
            //                                                         (HttpContext.Current.Request.Form["lstSubjects"]);
            //    var oldsubjects = db.MeetingSubjects.Where(o => o.MeetingId == updateMeeting.Id);
            //    var oldparticipators = db.Participators.Where(o => o.MeetingId == updateMeeting.Id);
            //    if (createOrUpdate == false)//means update
            //    {
            //        foreach (var item in oldsubjects)
            //        {
            //            db.MeetingSubjects.Remove(item);

            //        }
            //        foreach (var item in oldparticipators)
            //        {
            //            db.Participators.Remove(item);

            //        }
            //        db.SaveChanges();
            //    }

            //    foreach (var item in lstSubjects)
            //    {
            //        CreateSubjectInDB(afterInsert, item);
            //    }
            //    foreach (var item in selectedParticipators)
            //    {
            //        CreateParticipatorInDB(afterInsert, item);
            //    }
            //}
            //return afterInsert.Id;
        }
    }
}