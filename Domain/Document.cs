namespace Domain
{
    public class Document
    {  
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public virtual Activity Activity { get; set; }
    }
}