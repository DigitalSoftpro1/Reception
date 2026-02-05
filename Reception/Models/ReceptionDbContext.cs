using Microsoft.EntityFrameworkCore;

namespace Reception.Models
{
    public class ReceptionDbContext : DbContext
    {
        public ReceptionDbContext(DbContextOptions<ReceptionDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Visit> Visits { get; set; }
        
        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Treatment> Treatments { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }


        public virtual DbSet<VisitTreatment> VisitTreatments { get; set; }
    }
}
