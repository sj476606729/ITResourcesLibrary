using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.Business.Models
{
    public class TreeModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
    }
}