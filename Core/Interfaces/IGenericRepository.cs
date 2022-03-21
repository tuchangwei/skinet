using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<T> GetByIdAsync(int Id);
        public Task<IReadOnlyList<T>> ListAllAsync();

        public Task<T> GetEntityWithSpec(ISpecification<T> spec);
        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    }
}