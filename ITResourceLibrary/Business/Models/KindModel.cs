using System;

namespace ITResourceLibrary.Business.Models
{
    public class KindModel
    {
        public string fTable { get; set; }
        public KindModel(string TabelName) { fTable = TabelName; }
        public KindModel() { }
        public string objectId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        
    }
    public class TreeModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
    }
}