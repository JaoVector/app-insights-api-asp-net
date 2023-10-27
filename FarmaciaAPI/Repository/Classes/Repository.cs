using Azure.Storage.Blobs;
using FarmaciaAPI.Context;
using FarmaciaAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FarmaciaAPI.Repository.Classes;

public class Repository<T> : IRepository<T> where T : class
{
    protected FContext _context;
    protected BlobServiceClient _blobServiceClient;

    public Repository(FContext context, BlobServiceClient blobService)
    {
        _context = context;
        _blobServiceClient = blobService;
    }


    public IQueryable<T> Get(int skip, int take)
    {
        return _context.Set<T>().AsNoTracking().Skip(skip).Take(take);
    }

    public async Task<T> BuscaPorID(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(expression);
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

}
