/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SAPlificationForm
{
	[XmlRoot(ElementName = "Job_Details")]
	public class Job_Details
	{
		[XmlElement(ElementName = "Job_No")]
		public string Job_No { get; set; }
		[XmlElement(ElementName = "User")]
		public string User { get; set; }
		[XmlElement(ElementName = "SAP_Location")]
		public string SAP_Location { get; set; }
		[XmlElement(ElementName = "Short_Desc")]
		public string Short_Desc { get; set; }
		[XmlElement(ElementName = "Long_Desc")]
		public string Long_Desc { get; set; }
		[XmlElement(ElementName = "Hours_Worked")]
		public string Hours_Worked { get; set; }
		[XmlElement(ElementName = "Add_Worker_1")]
		public string Add_Worker_1 { get; set; }
		[XmlElement(ElementName = "Add_Worker_2")]
		public string Add_Worker_2 { get; set; }
		[XmlElement(ElementName = "Part_Number_1")]
		public string Part_Number_1 { get; set; }
		[XmlElement(ElementName = "Qty_1")]
		public string Qty_1 { get; set; }
		[XmlElement(ElementName = "JSEA_Number")]
		public string JSEA_Number { get; set; }
		[XmlElement(ElementName = "Part_Number_2")]
		public string Part_Number_2 { get; set; }
		[XmlElement(ElementName = "Qty_2")]
		public string Qty_2 { get; set; }
	}

	[XmlRoot(ElementName = "SAP_LOG")]
	public class SAP_LOG
	{
		[XmlElement(ElementName = "Job_Details")]
		public List<Job_Details> Job_Details { get; set; }
	}
}
