using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.Business.Models
{
    public class AccountModel
    {
        public string fTable { get; set; }
        public AccountModel(string TabelName) { fTable = TabelName; }
        public AccountModel() { }
        public string Name { get; set; }
        public string Password { get; set; }
        public string username { get; set; }
    }
}