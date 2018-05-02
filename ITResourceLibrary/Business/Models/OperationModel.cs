namespace ITResourceLibrary.Business.Models
{
    public class OperationModel
    {
        public string fTable { get; set; }
        public OperationModel(string TabelName) { fTable = TabelName; }
        public OperationModel() { }
        public string User { get; set; }
        public string Operate { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public string createdAt { get; set; }
    }
}