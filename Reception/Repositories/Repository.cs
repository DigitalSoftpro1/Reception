using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reception.Models
{
    public class Repository<Entity> : IRepository<Entity> where Entity : BaseEntity
    {
        private readonly ReceptionDbContext _context;
        private readonly DbSet<Entity> _dbSet;

        public Repository(ReceptionDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Entity>();
        }

        public void Add(Entity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Entity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int Id)
        {
            var entity = _dbSet.Find(Id);
            if (entity != null)
            {
                entity.IsDeleted = true; // Soft delete
                _context.SaveChanges();
            }
        }

        public void ChangeStatus(int Id)
        {
            var entity = _dbSet.Find(Id);
            if (entity != null)
            {
                entity.IsActive = !entity.IsActive;
                _context.SaveChanges();
            }
        }

        public Entity Find(int Id)
        {
            return _dbSet.FirstOrDefault(e => e.Id == Id && !e.IsDeleted);
        }

        public List<Entity> View()
        {
            return _dbSet.Where(e => !e.IsDeleted).ToList();
        }

        public List<Entity> ViewClient()
        {
            return _dbSet.Where(e => !e.IsDeleted && e.IsActive).ToList();
        }
    }
}
