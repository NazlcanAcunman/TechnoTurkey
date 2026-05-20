using EventTicket.Data.Context;
using EventTicket.Data.Repositories.Abstract;

namespace EventTicket.Data.Repositories;
public class GenericRepository<T> : RepositoryBase<T> where T : class
{
    public GenericRepository(AppDbContext context) : base(context) { }
}