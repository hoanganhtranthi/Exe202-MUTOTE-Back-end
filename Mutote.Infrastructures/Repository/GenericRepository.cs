﻿

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MuTote.Application.Repository;
using MuTote.Infrastructures.DBContext;
using System.Linq.Expressions;

namespace MuTote.Infrastructures.Repository
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private static MutoteContext Context;
        private static DbSet<T> Table { get; set; }
        public GenericRepository(MutoteContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await Context.AddAsync(entity);
        }

        public async Task RemoveAsync(T entity)
        {
            Context.Remove(entity);
        }

        public async Task<List<T>> GetWhere(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = Table;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null)
        {
             IQueryable<T> query = Table;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }


        public EntityEntry<T> Delete(T entity)
        {
            return Context.Remove(entity);
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            return Table.Where(predicate).AsQueryable();
        }

        public T Find(Func<T, bool> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Table.SingleOrDefaultAsync(predicate);
        }

        public async Task<T> GetById(int id)
        {
            return await Table.FindAsync(id);
        }
        public async Task Update(T entity, int Id)
        {
            var existEntity = await GetById(Id);
            Context.Entry(existEntity).CurrentValues.SetValues(entity);
            Table.Update(existEntity);
        }

        public DbSet<T> GetAll()
        {
            return Table;
        }
    }
}
