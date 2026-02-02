namespace Reception.Models
{
    public interface IRepository <Entity>
    {
        void Add(Entity entity);
        void Update(Entity entity);
        void Delete(int Id);
        void ChangeStatus(int Id);
        Entity Find(int Id);
        List<Entity> View();
        List<Entity> ViewClient();

    }
}
