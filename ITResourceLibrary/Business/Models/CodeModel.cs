using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.Business.Models
{
    public class CodeModel
    {
        public string ObjectId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Author { get; set; }
        public string Visible { get; set; }
    }
}