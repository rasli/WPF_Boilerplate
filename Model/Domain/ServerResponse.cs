using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class ServerResponse
    {
        public bool Success { get; set; }
        public string Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
    public class ServerFileDownloadResponse
    {
        public string Status { get; set; }
        public string Code { get; set; }
    }
}
