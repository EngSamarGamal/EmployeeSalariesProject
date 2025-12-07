using App.Common.Dtos.PaginaionModel;
using App.Domain.Consts;
using App.Domain.Models.Base;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        public void DetachEntity<T>(T entity);

        IEnumerable<T> GetAll(bool withNoTracking = true);
        IEnumerable<T> GetAll(string[] includes, bool withNoTracking = true);

        Task<IEnumerable<T>> GetAllAsync(bool withNoTracking = true, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(string[] includes, bool withNoTracking = true, CancellationToken cancellation = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(string[]? includes = null, bool withNoTracking = true,
                                                    Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Descending,
                                                    CancellationToken cancellationToken = default);
        IQueryable<T> GetQueryable();
        T? GetById(Guid id, string[] includes = null);
        Task<T> GetByIdAsync(Guid id, string[] includes = null, CancellationToken cancellationToken = default);

        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null, CancellationToken cancellationToken = default);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsyncWithTracking(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, CancellationToken cancellationToken = default);
        T Add(T entity, string userId);
        Task<T> AddAsync(T entity, string userId, CancellationToken cancellationToken = default);
        IEnumerable<T> AddRange(IEnumerable<T> entities, string userId);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string userId, CancellationToken cancellationToken = default);
        T Update(T entity, string userId);
        T UpdateIsDeleted(T entity, string userId);
        IEnumerable<T> UpdateIsDeletedRange(IEnumerable<T> entities, string userId);

        IEnumerable<T> UpdateRange(IEnumerable<T> entities, string userId);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default);

        IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, string[] includes = null);
        IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, int take, int skip, string[] includes = null);
        IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, int take, int skip, string[] includes = null,
                                Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<PagainationModel<IEnumerable<T>>> FindAllAsyncByPagination(Expression<Func<T, bool>>? criteria = null,
            int pageNumber = 1, int pageSize = 10, string[] includes = null, CancellationToken cancellationToken = default);

        public Task<IDbContextTransaction> BeginTransactionAsync();



    }
}
