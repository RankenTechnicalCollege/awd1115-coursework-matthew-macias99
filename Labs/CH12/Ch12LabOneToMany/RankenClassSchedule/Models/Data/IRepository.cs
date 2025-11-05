namespace RankenClassSchedule.Models.Data
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List(QueryOptions<T> options);

        //get type by ida
        T? Get(int id);

        //get type with linq query
        T? Get(QueryOptions<T> options);

        //create
        void Insert(T entity);

        //update
        void Update(T entity);

        //delete
        void Delete(T entity);

        //save
        void Save();
    }
}
