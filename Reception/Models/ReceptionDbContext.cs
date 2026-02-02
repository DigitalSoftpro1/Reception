using Microsoft.EntityFrameworkCore;

namespace Reception.Models
{
    public class ReceptionDbContext : DbContext
    {
        public ReceptionDbContext(DbContextOptions<ReceptionDbContext> options) : base(options)
        {
        }

        public virtual DbSet<TransactionVisitTreatment> TransactionVisitTreatment { get; set; }

        public virtual DbSet<TransactionVisitPayment> TransactionVisitPayment { get; set; }

        public virtual DbSet<TransactionVisit> TransactionVisit { get; set; }
    }
}
