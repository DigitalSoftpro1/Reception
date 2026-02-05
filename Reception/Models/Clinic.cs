namespace Reception.Models
{
    public class Clinic
    {
     
        public int Id { get; set; }
        public string? Name { set; get; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Treatment>? Treatment { get; set; }

    }
}
