using System;
using System.ComponentModel.DataAnnotations;

namespace ITResourceLibrary.Business.Models
{
   
    public class CodeModel
    {
        public string fTable { get; set; }
        public CodeModel(string TabelName) { fTable = TabelName; }
        public CodeModel() { }
        public string objectId { get; set; }
        [Required]
        [MinLength(length:2)]
        public string Title { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Visible { get; set; }
        public string KindObjectId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        
    }
}