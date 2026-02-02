using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Reception.Models;
using Reception.Data.Intercptors; // <-- هذا ضروري الآن


namespace Reception.Data.Intercptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // cutoff request ==> Modify
            var dbContext = eventData.Context;
            if (dbContext is null)
            {
                return new InterceptionResult<int>();
            }

            var data = dbContext.ChangeTracker.Entries<BaseEntity>();
            foreach (var entity in data)
            {
                if (entity.State is EntityState.Deleted)
                {
                    entity.State = EntityState.Modified;
                    entity.Entity.IsDeleted = true;
                }
            }

            return base.SavingChanges(eventData, result);
        }

    }
}
