using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPlificationForm
{
    public class ErrorMessage
    {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class ErrorError
    {
        public string code { get; set; }
        public ErrorMessage message { get; set; }
    }

    public class ErrorRoot
    {
        public ErrorError error { get; set; }
    }
    
}
