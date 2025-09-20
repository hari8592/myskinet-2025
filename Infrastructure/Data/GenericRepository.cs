using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T :BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepository(StoreContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public bool Exists(int id)
        {
            return _context.Set<T>().Any(x => x.Id == id);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {

            // it will filter based on spec and will return first item in the list that matches
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            //it will filter based on spec and will return List
            return await ApplySpecification(spec).ToListAsync();
        }

        //below method required to call ApplySpecification 
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            //we have to call SpecificationEvaluator's method GetQuery
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        //below method required to call ApplySpecification 
        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
        {
            //we have to call SpecificationEvaluator's method GetQuery
            return SpecificationEvaluator<T>.GetQuery<T,TResult>(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
        {
            // it will filter based on spec and will return first item in the list that matches
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
    }
}
