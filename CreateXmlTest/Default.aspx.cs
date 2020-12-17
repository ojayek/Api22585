using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CreateXmlTest
{
    public partial class Default : System.Web.UI.Page
    {
       /// private string yourfile;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);






            //doc.LoadXml("<Letter><Protocol>wrench</Protocol></Letter>");
            XmlElement root = doc.CreateElement("Letter");
            XmlElement id = doc.CreateElement("Protocol");
            root.AppendChild(id);
            doc.AppendChild(root);
            id.SetAttribute("NAME", "ECE");
            id.SetAttribute("version", "1.0.1");
            XmlElement id2 = doc.CreateElement("Software");
            id2.SetAttribute("GUID", "{0200414A-0500-6F46-726D-73000053568B}");
            id2.SetAttribute("SoftwareDeveloper", "MiiroFiler");
            id2.SetAttribute("Version", "1");
            XmlElement id3 = doc.CreateElement("Sender");

            id3.SetAttribute("Code", "15560000");
            id3.SetAttribute("Department", "شرکت سهامی خدمات  مهندسی مشانیر");
            id3.SetAttribute("Name", "مدیر سیستم System Manager");
            id3.SetAttribute("Organization", "شرکت سهامی خدمات  مهندسی مشانیر");
            id3.SetAttribute("Position", "مدیر  سیستم");
            XmlElement id4 = doc.CreateElement("Receiver");
            id4.SetAttribute("Code", TextBox1.Text);
            id4.SetAttribute("Department", TextBox2.Text);
            id4.SetAttribute("Name", "");
            id4.SetAttribute("Organization", TextBox3.Text);
            id4.SetAttribute("Position", TextBox4.Text);
            id4.SetAttribute("ReceiveType", "Origin");
            XmlElement id5 = doc.CreateElement("OtherReceivers");
            XmlElement otrs = doc.CreateElement("OtherReceiver");
            otrs.SetAttribute("Code", "");
            otrs.SetAttribute("Department", "");
            otrs.SetAttribute("Name", "");
            otrs.SetAttribute("Organization", "");
            otrs.SetAttribute("Position", "");
            otrs.SetAttribute("ReceiveType", "");
            XmlElement id6 = doc.CreateElement("LetterNo");
            id6.InnerText = TextBox5.Text;
            XmlElement id7 = doc.CreateElement("LetterDateTime");
            id7.SetAttribute("ShowAs", "gregorian");
            id7.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            XmlElement id8 = doc.CreateElement("RelatedLetters");
            XmlElement id9 = doc.CreateElement("Subject");
            id9.InnerText = TextBox6.Text;
            XmlElement id10 = doc.CreateElement("Priority");
            id10.SetAttribute("Code", "1");
            id10.SetAttribute("Name", "عادی");
            XmlElement id11 = doc.CreateElement("Classification");
            id11.SetAttribute("Code", "1");
            id11.SetAttribute("Name", "");
            string imagePath = @"E:\sample.jpg";
           /// string imagePath = yourfile;
            string imgBase64String;
            imgBase64String = Convert.ToBase64String(File.ReadAllBytes(imagePath));
            XmlElement id12_1 = doc.CreateElement("Origins");
            XmlElement id12 = doc.CreateElement("Origin");
            id12.SetAttribute("ContentType", "image/tiff");
            id12.SetAttribute("Extension", "tiff");
            id12.SetAttribute("Description", "با امضای مشانیر");
            id12.InnerText = imgBase64String;


            root.AppendChild(id2);
            root.AppendChild(id3);
            root.AppendChild(id4);
            root.AppendChild(id5);
            id5.AppendChild(otrs);
            root.AppendChild(id6);
            root.AppendChild(id7);
            root.AppendChild(id8);
            root.AppendChild(id9);
            root.AppendChild(id10);
            root.AppendChild(id11);
            id12_1.AppendChild(id12);
            root.AppendChild(id12_1);





            // Add a price element.
            // >>>>  XmlElement newElem = doc.CreateElement("Receiver");
            // >>>>>  newElem.InnerText = TextBox1.Text;
            //  doc.DocumentElement.AppendChild(newElem);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            // Save the document to a file and auto-indent the output.
            XmlWriter writer = XmlWriter.Create("d:\\data.xml", settings);
            doc.Save(writer);


            Response.Redirect("YourPage.aspx");




        }
    }
}