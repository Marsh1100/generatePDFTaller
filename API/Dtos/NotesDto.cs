namespace API.Dtos
{
    public class NotesDto
    {
        public int Id {get;set;}
        public string SubjectName { get; set; }
        public double Note1 { get; set; }
        public double Note2 { get; set; }
        public double Note3 { get; set; }
        public double Average { get; set; }
    }
}