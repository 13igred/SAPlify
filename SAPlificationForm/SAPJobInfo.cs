using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SAPlificationForm.ListResponse;

namespace SAPlificationForm
{    
    class SAPJobDetails
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string SAPAreaCode { get; set; }
        public string ShortDesc { get; set; }
        public object LongDesc { get; set; }
        public string Time { get; set; }
        public object AdditionalWorkerOne { get; set; }
        public object AdditionalWorkerTwo { get; set; }
        public string WorkType { get; set; }
        public object PartNumOne { get; set; }
        public object PartQtyOne { get; set; }
        public object JobType { get; set; }
        public object WorkCenter { get; set; }
        public object Priority { get; set; }
        public object TypeZ2 { get; set; }
        public object JseaID { get; set; }
        public object PartNumTwo { get; set; }
        public object PartQtyTwo { get; set; }
        public SAPJobDetails(GoodRoot gr)
        {
            ID = gr.d.ID;
            Title = gr.d.Title;
            Created = gr.d.Created;
            SAPAreaCode = gr.d.SAPAreaCode;
            ShortDesc = gr.d.ShortDesc;
            LongDesc = gr.d.LongDesc;
            Time = gr.d.Time;
            AdditionalWorkerOne = gr.d.AdditionalWorkerOne;
            AdditionalWorkerTwo = gr.d.AdditionalWorkerTwo;
            WorkType = gr.d.WorkType;
            PartNumOne = gr.d.PartNumOne;
            PartQtyOne = gr.d.PartQtyOne;
            JobType = gr.d.JobType;
            WorkCenter = gr.d.WorkCenter;
            Priority = gr.d.Priority;
            TypeZ2 = gr.d.TypeZ2;
            JseaID = gr.d.JseaID;
            PartNumTwo = gr.d.PartNumTwo;
            PartQtyTwo = gr.d.PartQtyTwo;
        }
    }
}
