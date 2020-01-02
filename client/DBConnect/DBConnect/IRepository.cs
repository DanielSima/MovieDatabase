namespace DBConnect
{
    /// <summary>
    /// A repository pattern interface.
    /// </summary>
    /// <typeparam name="T">Any object inhereted from EntityBase.</typeparam>
    public interface IRepository<T> where T : EntityBase
    {
        void Create(T entity);

        void Delete(T entity);

        T GetById(int id);

        void Update(T entity);
    }

    /// <summary>
    /// Base class for every class that wants to implement IRepository<T>.
    /// </summary>
    public abstract class EntityBase
    {
        protected readonly int id;

        protected EntityBase(int id)
        {
            this.id = id;
        }
        protected EntityBase()
        {
            this.id = -1;
        }
        public int Id
        {
            get { return id; }
        }
    }
}