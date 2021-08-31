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
    public class MeetingController : ApiController
    {
        // GET: api/Contact

        private MoshanirMeetingEntities db = new MoshanirMeetingEntities();

        [HttpGet]
        [Route("api/Meeting/GetPersons")]
        //public IEnumerable<MeetingParticipent> GetPersons([FromBody] MeetingParticipent userInput)
        public IEnumerable<MeetingParticipents> GetPersons()
        {
            var httpContext = HttpContext.Current;
            var data = db.MeetingParticipents.ToList();
            var Nam = httpContext.Request.Headers["Nam"];
            var NamKhanevadegi = httpContext.Request.Headers["NamKhanevadegi"];
            var lstContacts = new List<MeetingParticipents>();
            if (!string.IsNullOrEmpty(Nam) || !string.IsNullOrEmpty(NamKhanevadegi))
            {
                data = data.Where(o => o.Nam.Contains(Nam) || o.NamKhanevadegi.Contains(NamKhanevadegi)).ToList();

            }
            foreach (var item in data)
            {
                var meetData = new MeetingParticipents();
                meetData.Prsnum = int.Parse(item.Prsnum.ToString());
                meetData.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                meetData.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                meetData.Moavenat = (item.Moavenat != null) ? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                meetData.VahedSazmani = (item.VahedSazmani != null) ? item.VahedSazmani.Replace('ي', 'ی').Replace('ك', 'ک') : null;

                lstContacts.Add(meetData);
            }

            return lstContacts.Take(30);
        }
        [HttpGet]
        [Route("api/Meeting/SearchPersonByInputValue")]
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
                var meetData = new MeetingParticipents();
                meetData.Prsnum = int.Parse(item.Prsnum.ToString());
                meetData.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                meetData.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                meetData.Moavenat = (item.Moavenat != null) ? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                meetData.VahedSazmani = (item.VahedSazmani != null) ? item.VahedSazmani.Replace('ي', 'ی').Replace('ك', 'ک') : null;

                lstContacts.Add(meetData);
            }
            lstContacts = lstContacts.OrderBy(o => o.NamKhanevadegi).ToList();
            if (lstContacts.Count > 30)
            {
                return lstContacts.Take(30);
            }
            else return lstContacts;
        }
        [HttpGet]
        [Route("api/Meeting/GetAllMeeting")]
        public IEnumerable<MeetingData> GetAllMeeting()
        {
            var data = db.Meetings.ToList();
            var lstContacts = new List<MeetingData>();

            foreach (var item in data)
            {
                var meetData = new MeetingData();
                meetData.Id = int.Parse(item.Id.ToString());
                meetData.Location = item.Location.ToString();
                meetData.Title = item.Title.ToString();
                meetData.InnerParticipators = (item.InnerParticipator != null) ? item.InnerParticipator.ToString() : "";
                meetData.MeetingDateStr = item.MeetingDate.HasValue ? ConvertToShamsi(item.MeetingDate.Value) : null;
                meetData.MeetingNumber = item.MeetingNumber.HasValue ? item.MeetingNumber.Value : 0;
                meetData.OuterParticipators = item.OuterParticipator;

                //prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
                //prsdata.Nam = (item.Nam != null) ? item.Nam.Replace('ي', 'ی').Replace('ك','ک'):null;
                //prsdata.NamKhanevadegi = (item.NamKhanevadegi != null) ? item.NamKhanevadegi.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
                //prsdata.Moavenat =(item.Moavenat !=null)? item.Moavenat.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.Email = item.Email;
                //prsdata.Proj_Name = (item.Proj_Name != null) ? item.Proj_Name.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.NumBuild = (item.NumBuild != null) ? item.NumBuild.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //prsdata.NamLatin = item.NamLatin;
                //prsdata.Sharh_Onvan = (item.Sharh_Onvan != null) ? item.Sharh_Onvan.Replace('ي', 'ی').Replace('ك', 'ک'):null;
                //if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
                //{
                //    lstContacts.Add(prsdata);
                //}
                lstContacts.Add(meetData);
            }
            var teldatas = db.Meetings.Where(o => (!string.IsNullOrEmpty(o.Title) || !string.IsNullOrEmpty(o.Title)));
            //foreach (var item in teldatas)
            //{
            //    var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
            //    if (contact != null)
            //    {
            //        contact.Tel = item.Tel;
            //        contact.DirectPhoneNo = item.DirectPhoneNo;
            //    }
            //}
            return lstContacts;
        }

        [HttpGet]
        [Route("api/Meeting/GetMeetingById/{Id}")]
        public IEnumerable<MeetingData> GetMeetingById(int Id)
        {
            var data = db.Meetings.Where(o => o.Id == Id).ToList();
            var lstContacts = new List<MeetingData>();

            foreach (var item in data)
            {
                var prsdata = new MeetingData();
                prsdata.lstSubjects = new List<MeetingSubject>();
                prsdata.Id = int.Parse(item.Id.ToString());
                prsdata.Title = (item.Title != null) ? item.Title.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                prsdata.InnerParticipators = (item.InnerParticipator != null) ? item.InnerParticipator.Replace('ي', 'ی').Replace('ك', 'ک') : null;

                prsdata.OuterParticipators = (item.OuterParticipator != null) ? item.OuterParticipator.Replace('ي', 'ی').Replace('ك', 'ک') : null;

                prsdata.Location = (item.Location != null) ? item.Location.Replace('ي', 'ی').Replace('ك', 'ک') : null;
                prsdata.MeetingDateStr = ConvertToShamsi(item.MeetingDate.HasValue ? item.MeetingDate.Value : DateTime.Now);
                prsdata.MeetingNumber = int.Parse(item.MeetingNumber.ToString());



                if (!lstContacts.Where(o => o.Id == prsdata.Id).Any())
                {
                    var meetingSubjects = db.MeetingSubjects.Where(o => o.MeetingId == item.Id).ToList();
                    foreach (var meetSubject in meetingSubjects)
                    {
                        var ms = new MeetingSubject();
                        ms.Id = meetSubject.Id;
                        ms.Subject = meetSubject.SubTitle;
                        ms.tracingResponsible = meetSubject.Responsible;
                        ms.MeetingId = prsdata.Id;
                        ms.endDateStr = meetSubject.DeadLine.HasValue ? ConvertToShamsi(meetSubject.DeadLine.Value) : null;
                        prsdata.lstSubjects.Add(ms);
                    }
                    lstContacts.Add(prsdata);
                }
            }
            return lstContacts;
        }

        [HttpPost]
        [Route("api/Meeting/CreateMeeting")]
        public Int32 CreateMeeting()
        {
            var MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
            Meetings updateMeeting = db.Meetings.Where(o => o.MeetingNumber == MeetingNumber).SingleOrDefault();
            var createOrUpdate = (updateMeeting == null) ? true : false;

            var file1 = HttpContext.Current.Request.Files.Count > 0 ?   HttpContext.Current.Request.Files[0] : null;

            if (file1 != null && file1.ContentLength > 0)
            {
                var fileName = file1.FileName;
            }
            Meetings newMeeting = CreateOrUpdateMeetingInDB(createOrUpdate? new Meetings():updateMeeting, createOrUpdate); ;
            var afterInsert = db.Meetings.Where(o => o.MeetingNumber == newMeeting.MeetingNumber).SingleOrDefault();
           
            if (afterInsert != null)
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    // For add File To DB Table
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        CreateOrUpdateFileInDB(afterInsert, httpRequest, file,createOrUpdate);
                    }
                }
              
                List<SubjectViewModel> lstSubjects = JsonConvert.DeserializeObject<List<SubjectViewModel>>
                                                                     (HttpContext.Current.Request.Form["lstSubjects"]);
                var oldsubjects = db.MeetingSubjects.Where(o => o.MeetingId == updateMeeting.Id);
                if(createOrUpdate ==false)
                {
                    foreach (var item in oldsubjects)
                    {
                        db.MeetingSubjects.Remove(item);
                       
                    }
                    db.SaveChanges();
                }
               
                foreach (var item in lstSubjects)
                {
                    CreateSubjectInDB(afterInsert, item);
                }
            }
            return afterInsert.Id;
        }

        private void CreateSubjectInDB(Meetings afterInsert, SubjectViewModel item)
        {

            var newSubject = new MeetingSubjects();
            newSubject.SubTitle = item.Subject;
            newSubject.Responsible = item.tracingResponsible;
            try
            {
                newSubject.DeadLine = ConvertToMiladi(item.endDateStr);
            }
            catch (Exception ex)
            {

            }
            newSubject.Number = item.SubjectNumber;

            newSubject.MeetingId = afterInsert.Id;
            db.MeetingSubjects.Add(newSubject);
            db.SaveChanges();
        }

        private void CreateOrUpdateFileInDB(Meetings afterInsert, HttpRequest httpRequest, string file, bool createOrUpdate)
        {
            var postedFile = httpRequest.Files[file].InputStream;
            var updateFile = db.MeetingFiles.Where(o => o.MeetingId == afterInsert.Id).SingleOrDefault();
            if(updateFile == null) { createOrUpdate = true; }
            var newMeetingFile = createOrUpdate ? new MeetingFiles() : updateFile;

            newMeetingFile.MeetingFile = ReadToEnd(postedFile);
            newMeetingFile.MeetingId = afterInsert.Id;
            newMeetingFile.FileName = httpRequest.Files[file].FileName;
            newMeetingFile.FileType = httpRequest.Files[file].ContentType;
            newMeetingFile.FileSize = httpRequest.Files[file].ContentLength;

            if (createOrUpdate ) { db.MeetingFiles.Add(newMeetingFile); }            
            db.SaveChanges();
        }

        private Meetings CreateOrUpdateMeetingInDB(Meetings meeting,bool createOrUpdate)
        {

            meeting.Title = HttpContext.Current.Request.Form["Title"];
            meeting.MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
            meeting.InnerParticipator = HttpContext.Current.Request.Form["InnerParticipators"];
            meeting.OuterParticipator = HttpContext.Current.Request.Form["OuterParticipators"];
            meeting.Location = HttpContext.Current.Request.Form["Location"];
            meeting.MeetingDate = ConvertToMiladi(HttpContext.Current.Request.Form["MeetingDateStr"]);
            if (createOrUpdate) { db.Meetings.Add(meeting); }

            db.SaveChanges();
            return meeting;
        }

        [HttpPost]
        [Route("api/Meeting/UpdateMeeting")]
        public Int32 UpdateMeeting()
        {
            var file1 = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            if (file1 != null && file1.ContentLength > 0)
            {
                var fileName = file1.FileName;
            }
            var MeetingNumber = int.Parse(HttpContext.Current.Request.Form["MeetingNumber"]);
            Meetings updateMeeting = db.Meetings.Where(o => o.MeetingNumber == MeetingNumber).SingleOrDefault();
            CreateOrUpdateMeetingInDB(updateMeeting, false);

            if (updateMeeting != null)
            {
                var httpRequest = HttpContext.Current.Request;
                var updateFile=db.MeetingFiles.Where(o => o.MeetingId == updateMeeting.Id);
                if (httpRequest.Files.Count > 0)
                {
                    // For add File To DB Table
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        CreateOrUpdateFileInDB(updateMeeting, httpRequest, file,false);
                    }
                }

                List<SubjectViewModel> lstSubjects = JsonConvert.DeserializeObject<List<SubjectViewModel>>
                                                                     (HttpContext.Current.Request.Form["lstSubjects"]);

                var oldsubjects = db.MeetingSubjects.Where(o => o.MeetingId == updateMeeting.Id);
                foreach (var item in oldsubjects)
                {
                    db.MeetingSubjects.Remove(item);
                    db.SaveChanges();
                }

                foreach (var item in lstSubjects)
                {
                    CreateSubjectInDB(updateMeeting, item);
                }
            }
            return (updateMeeting !=null)?updateMeeting.Id:0;
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

    }


}



