using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Security;

namespace SAPlificationForm
{
    class UpdateXML
    {
        private string file;
        public UpdateXML(string file)
        {                        
            this.file = file;            
        }

        public void UpdateXMLFile(SAPJobDetails jobDetails)
        {
            XmlDocument doc = new XmlDocument();

            if (!File.Exists(file))
            {
                // Create the XmlDocument.

                doc.LoadXml("<SAP_LOG><Job_Details><Job_No></Job_No><User></User><SAP_Location></SAP_Location><Short_Desc></Short_Desc><Long_Desc></Long_Desc><Hours_Worked></Hours_Worked><Add_Worker_1></Add_Worker_1><Add_Worker_2></Add_Worker_2><Part_Number_1></Part_Number_1><Qty_1></Qty_1><JSEA_Number></JSEA_Number><Part_Number_2></Part_Number_2><Qty_2></Qty_2></Job_Details></SAP_LOG>"); //Your string here

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer = new XmlTextWriter(file, null);
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
                writer.Close();
            }

            string shortDesc = "";
            string longDesc = "";

            if (jobDetails.ShortDesc != null)
            {             
                shortDesc = SecurityElement.Escape(jobDetails.ShortDesc);
            }
            if(jobDetails.LongDesc != null)
            {
                longDesc = SecurityElement.Escape(jobDetails.LongDesc.ToString());
            }
            

            doc.Load(file);
            XmlNode nl = doc.SelectSingleNode("//SAP_LOG");
            XmlDocument xmlDoc2 = new XmlDocument();
            
            xmlDoc2.LoadXml("<Job_Details><Job_No>" + jobDetails.ID + "</Job_No><User>" + jobDetails.Title + "</User><SAP_Location>" + jobDetails.SAPAreaCode + "</SAP_Location><Short_Desc>" + shortDesc + "</Short_Desc><Long_Desc>" + longDesc + "</Long_Desc><Hours_Worked>" + jobDetails.Time + "</Hours_Worked><Add_Worker_1>" + jobDetails.AdditionalWorkerOne + "</Add_Worker_1><Add_Worker_2>" + jobDetails.AdditionalWorkerTwo + "</Add_Worker_2><Part_Number_1>" + jobDetails.PartNumOne + "</Part_Number_1><Qty_1>" + jobDetails.PartQtyOne + "</Qty_1><JSEA_Number>" + jobDetails.JseaID + "</JSEA_Number><Part_Number_2>" + jobDetails.PartNumTwo + "</Part_Number_2><Qty_2>" + jobDetails.PartQtyTwo + "</Qty_2></Job_Details>");
            XmlNode n = doc.ImportNode(xmlDoc2.FirstChild, true);
            nl.AppendChild(n);
            
            doc.Save(file);
        }

        public int GetLastJobID()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SAP_LOG));
            SAP_LOG sl;
            int biggerNum = 0;

            try
            {
                using (Stream reader = new FileStream(file, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    sl = (SAP_LOG)serializer.Deserialize(reader);
                }
                
                for (int i = 0; i < sl.Job_Details.Count; i++)
                {
                    if (!sl.Job_Details[i].Job_No.Equals(""))
                    {
                        if (Int32.Parse(sl.Job_Details[i].Job_No) > biggerNum)
                        {
                            biggerNum = Int32.Parse(sl.Job_Details[i].Job_No);
                        }
                    }
                }
            }
            catch
            {
                
            }            

            return biggerNum + 1;
        }

    }
}
