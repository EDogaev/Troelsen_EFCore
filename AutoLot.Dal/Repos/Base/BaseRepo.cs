using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Exceptions;
using AutoLot.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Dal.Repos.Base;

public abstract class BaseRepo<T> : IRepo<T> where T : BaseEntity, new()
{
    private readonly bool _disposeContext;
    private bool _isDisposed;

    public ApplicationDbContext Context { get; }

    public DbSet<T> Table { get; }

    protected BaseRepo(ApplicationDbContext context)
    {
        Context = context;
        Table = Context.Set<T>();
        _disposeContext = false;
    }

    protected BaseRepo(DbContextOptions<ApplicationDbContext> options) : this(new ApplicationDbContext(options))
    {
        _disposeContext = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            if (_disposeContext)
                Context.Dispose();
        }

        _isDisposed = true;
    }

    ~BaseRepo()
    {
        Dispose(false);
    }

    public int Add(T entity, bool persist = true)
    {
        Table.Add(entity);
        return persist ? SaveChanges() : 0;
    }

    public int AddRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.AddRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public int Update(T entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? SaveChanges() : 0;
    }

    public int UpdateRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.UpdateRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public int Delete(int id, byte[] timeStamp, bool persist = true)
    {
        var entity = new T { Id = id, TimeStamp = timeStamp };
        Context.Entry(entity).State = EntityState.Deleted;
        return persist ? SaveChanges() : 0;
    }

    public int Delete(T entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? SaveChanges() : 0;
    }

    public int DeleteRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.RemoveRange(entities);
        return persist ? SaveChanges() : 0;
    }

    /// <summary>
    /// Если сущность уже отслеживается, тогда возвращается отслеживаемый экземпляр, иначе запись извлекается из базы данных.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T? Find(int? id) => Table.Find(id);

    /// <summary>
    /// Извлечение записи, но без ее добавления в ChangeTracker, используя AsNoTrackingWithldentityResolution().
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T? FindAsNoTracking(int? id) => Table.AsNoTrackingWithIdentityResolution().FirstOrDefault(x => x.Id == id);

    public T? FindIgnoreQueryFilters(int? id) => Table.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);

    public virtual IEnumerable<T> GetAll() => Table;

    public virtual IEnumerable<T> GetAllIgnoreQueryFilters() => Table.IgnoreQueryFilters();

    public void ExecuteQuery(string sql, object[] sqlParametersObjects) => Context.Database.ExecuteSqlRaw(sql, sqlParametersObjects);

    public int SaveChanges()
    {
        try
        {
            return Context.SaveChanges();
        }
        catch (CustomException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CustomException("An error occured updating the database", ex);
        }
    }
}