using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPlificationForm
{
    class ListResponse
    {
        public class Metadata
        {
            public string id { get; set; }
            public string uri { get; set; }
            public string etag { get; set; }
            public string type { get; set; }
        }

        public class Deferred
        {
            public string uri { get; set; }
        }

        public class FirstUniqueAncestorSecurableObject
        {
            public Deferred __deferred { get; set; }
        }

        public class RoleAssignments
        {
            public Deferred __deferred { get; set; }
        }

        public class AttachmentFiles
        {
            public Deferred __deferred { get; set; }
        }

        public class ContentType
        {
            public Deferred __deferred { get; set; }
        }

        public class GetDlpPolicyTip
        {
            public Deferred __deferred { get; set; }
        }

        public class FieldValuesAsHtml
        {
            public Deferred __deferred { get; set; }
        }

        public class FieldValuesAsText
        {
            public Deferred __deferred { get; set; }
        }

        public class FieldValuesForEdit
        {
            public Deferred __deferred { get; set; }
        }

        public class File
        {
            public Deferred __deferred { get; set; }
        }

        public class Folder
        {
            public Deferred __deferred { get; set; }
        }

        public class LikedByInformation
        {
            public Deferred __deferred { get; set; }
        }

        public class ParentList
        {
            public Deferred __deferred { get; set; }
        }

        public class Properties
        {
            public Deferred __deferred { get; set; }
        }

        public class Versions
        {
            public Deferred __deferred { get; set; }
        }

        public class D
        {
            public Metadata __metadata { get; set; }
            public FirstUniqueAncestorSecurableObject FirstUniqueAncestorSecurableObject { get; set; }
            public RoleAssignments RoleAssignments { get; set; }
            public AttachmentFiles AttachmentFiles { get; set; }
            public ContentType ContentType { get; set; }
            public GetDlpPolicyTip GetDlpPolicyTip { get; set; }
            public FieldValuesAsHtml FieldValuesAsHtml { get; set; }
            public FieldValuesAsText FieldValuesAsText { get; set; }
            public FieldValuesForEdit FieldValuesForEdit { get; set; }
            public File File { get; set; }
            public Folder Folder { get; set; }
            public LikedByInformation LikedByInformation { get; set; }
            public ParentList ParentList { get; set; }
            public Properties Properties { get; set; }
            public Versions Versions { get; set; }
            public int FileSystemObjectType { get; set; }
            public int Id { get; set; }
            public object ServerRedirectedEmbedUri { get; set; }
            public string ServerRedirectedEmbedUrl { get; set; }
            public int ID { get; set; }
            public string ContentTypeId { get; set; }
            public string Title { get; set; }
            public DateTime Modified { get; set; }
            public DateTime Created { get; set; }
            public int AuthorId { get; set; }
            public int EditorId { get; set; }
            public string OData__UIVersionString { get; set; }
            public bool Attachments { get; set; }
            public string GUID { get; set; }
            public object ComplianceAssetId { get; set; }
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
        }

        public class GoodRoot
        {
            public D d { get; set; }
        }
    }
}
