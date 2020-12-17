using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CreateXmlWebApi.Models;

namespace CreateXmlWebApi.Controllers
{
    public class ContactController : ApiController
    {
        // GET: api/Contact

        private IntranetDatabaseString db = new IntranetDatabaseString();
        [HttpGet]
        [Route("api/Contact/GetContactList")]
        public IEnumerable<Person> GetContactList()
        {
            var data = db.View_ContactList.ToList();
            var lstContacts = new List<Person>();

            foreach (var item in data)
            {
                var prsdata = new Person();
                prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
                prsdata.Nam = item.Nam;
                prsdata.NamKhanevadegi = item.NamKhanevadegi;
                prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
                prsdata.Moavenat = item.Moavenat;
                prsdata.Email = item.Email;
                prsdata.Proj_Name = item.Proj_Name;
                prsdata.NumBuild = item.NumBuild;
                prsdata.NamLatin = item.NamLatin;
                prsdata.Sharh_Onvan = item.Sharh_Onvan;
                if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
                {
                    lstContacts.Add(prsdata);
                }
            }
            var teldatas = db.contactnew.Where(o => (!string.IsNullOrEmpty(o.Tel) || !string.IsNullOrEmpty(o.DirectPhoneNo)));
            foreach (var item in teldatas)
            {
                var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
                if (contact != null)
                {
                    contact.Tel = item.Tel;
                    contact.DirectPhoneNo = item.DirectPhoneNo;
                }
            }
            return lstContacts;
        }

        [HttpGet]
        [Route("api/Contact/GetContactByPrsNum/{PrsNum}")]
        public IEnumerable<Person> GetContactByPrsNum(int prsnum)
        {
            var data = db.View_ContactList.Where(o=>o.Prsnum==prsnum).ToList();
            var lstContacts = new List<Person>();

            foreach (var item in data)
            {
                var prsdata = new Person();
                prsdata.Prsnum = int.Parse(item.Prsnum.ToString());
                prsdata.Nam = item.Nam;
                prsdata.NamKhanevadegi = item.NamKhanevadegi;
                prsdata.NamKhanevadegiLatin = item.NamKhanevadegiLatin;
                prsdata.Moavenat = item.Moavenat;
                prsdata.Email = item.Email;
                prsdata.Proj_Name = item.Proj_Name;
                prsdata.NumBuild = item.NumBuild;
                prsdata.NamLatin = item.NamLatin;
                prsdata.Sharh_Onvan = item.Sharh_Onvan;
                if (!lstContacts.Where(o => o.Prsnum == prsdata.Prsnum).Any())
                {
                    lstContacts.Add(prsdata);
                }
            }
            var teldatas = db.contactnew.Where(o => o.prsnum==prsnum);
            foreach (var item in teldatas)
            {
                var contact = lstContacts.Where(o => o.Prsnum == item.prsnum).SingleOrDefault();
                if (contact != null)
                {
                    contact.Tel = item.Tel;
                    contact.DirectPhoneNo = item.DirectPhoneNo;
                }
            }
            return lstContacts;
        }

        [HttpPost]
        [Route("api/Contact/CreateContact")]
        public contactnew CreateContact([FromBody] ContactData data)
        {
            var contact = new contactnew();
            var insertdBefor= db.contactnew.Where(o => o.prsnum == data.Prsnum).SingleOrDefault();
            var MainData = db.View_ContactList.Where(o => o.Prsnum == data.Prsnum).SingleOrDefault();

            if (insertdBefor != null)
            {
                insertdBefor.DirectPhoneNo = data.DirectPhoneNo;
                insertdBefor.Tel = data.Tel;
                insertdBefor.Name= MainData.Nam + " " + MainData.NamKhanevadegi;
                insertdBefor.OfficePosition = MainData.Proj_Name;
                insertdBefor.Title = MainData.Sharh_Onvan;
                insertdBefor.Building = MainData.NumBuild;
                insertdBefor.Deputy = MainData.Moavenat;
                insertdBefor.Pic = MainData.Prsnum + ".jpg";
                insertdBefor.Enabled = true;
             
                db.SaveChanges();
                
                return insertdBefor;
            }
            else
            {
                contact.DirectPhoneNo = data.DirectPhoneNo;
                contact.Tel = data.Tel;
                contact.Name = MainData.Nam + " " + MainData.NamKhanevadegi;
                contact.OfficePosition = MainData.Proj_Name;               
                contact.Title = MainData.Sharh_Onvan;
                contact.Building = MainData.NumBuild;
                contact.Deputy = MainData.Moavenat;
                contact.prsnum = MainData.Prsnum;
                contact.Pic = MainData.Prsnum + ".jpg";
                contact.Enabled = true;

                db.contactnew.Add(contact);                
                db.SaveChanges();
                return contact;
            }                                 
        }

    }
}
