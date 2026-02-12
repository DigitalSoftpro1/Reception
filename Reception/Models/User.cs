namespace Reception.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // لاحقًا نعمل Hash
        public bool CanEditPayment { get; set; } // ⭐ الصلاحية
    }

}
