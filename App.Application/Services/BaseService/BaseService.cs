using App.Application.Interfaces.Repositories;
using App.Common.Dtos.PaginaionModel;
using App.Domain.Consts;
using App.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Services.BaseService
{
    public class BaseService<T> : IBaseService<T> where T : class, IBaseEntity
    {
        // protected ApplicationDbContext _context;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<T> _baseRepository;

        public BaseService(IUnitOfWork unitOfWork, IBaseRepository<T> baseRepository)
        {
            //_context = context;
            _unitOfWork = unitOfWork;
            _baseRepository = baseRepository;
        }
        public void DetachEntity<T>(T entity)
        {
            _baseRepository.DetachEntity(entity);

        }

        public virtual IEnumerable<T> GetAll()
        {
            return _baseRepository.GetAll();
        }

        public virtual IEnumerable<T> GetAll(string[] includes)
        {
            return _baseRepository.GetAll(includes);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool withNoTracking = true, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetAllAsync(withNoTracking, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] includes, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetAllAsync(includes, false, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[]? includes = null, bool withNoTracking = true,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Descending, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetAllAsync(includes, withNoTracking, orderBy, orderByDirection, cancellationToken);
        }

        public T? GetById(Guid id, string[] includes = null)
        {
            return _baseRepository.GetById(id, includes);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetByIdAsync(id, null, cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Guid id, string[] includes = null, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.GetByIdAsync(id, includes, cancellationToken);
        }

        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            T query = _baseRepository.Find(criteria, includes);
            return query;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null, CancellationToken cancellationToken = default)
        {
            T query = await _baseRepository.FindAsync(criteria, includes, cancellationToken);
            return query;
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IEnumerable<T> data = _baseRepository.FindAll(criteria, includes);
            return data;
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            return _baseRepository.FindAll(criteria, skip, take);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IEnumerable<T> data = _baseRepository.FindAll(criteria, skip, take, orderBy, orderByDirection);
            return data;
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IEnumerable<T> data = await _baseRepository.FindAllAsync(criteria, includes);
            return data;
        }

        public async Task<IEnumerable<T>> FindAllAsyncWithTracking(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IEnumerable<T> data = await _baseRepository.FindAllAsyncWithTracking(criteria, includes);
            return data;
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.FindAllAsync(criteria, take, skip, cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, CancellationToken cancellationToken = default)
        {
            IEnumerable<T> data = await _baseRepository.FindAllAsync(criteria, take, skip, orderBy, orderByDirection, cancellationToken);
            return data;
        }
        public IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> data = _baseRepository.FindAllByQuarable(criteria, includes);
            return data;
        }
        public IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, int take, int skip, string[] includes = null)
        {
            IQueryable<T> data = _baseRepository.FindAllByQuarable(criteria, take, skip, includes);
            return data;
        }
        public IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, int take, int skip, string[] includes = null,
                                Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> data = _baseRepository.FindAllByQuarable(criteria, take, skip, includes, orderBy, orderByDirection);
            return data;
        }
        public async Task<PagainationModel<IEnumerable<T>>> FindAllAsyncByPagination(Expression<Func<T, bool>>? criteria = null,
            int pageNumber = 1, int pageSize = 10, string[] includes = null, CancellationToken cancellationToken = default)
        {
            var data = await _baseRepository.FindAllAsyncByPagination(criteria, pageNumber, pageSize, includes, cancellationToken);
            return data;

        }
        public T Add(T entity, string userId)
        {
            _baseRepository.Add(entity, userId);
            _unitOfWork.SaveChanges();
            return entity;
        }

        public async Task<T> AddAsync(T entity, string userId, CancellationToken cancellationToken = default)
        {
            await _baseRepository.AddAsync(entity, userId, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities, string userId)
        {
            _baseRepository.AddRange(entities, userId);
            _unitOfWork.SaveChanges();
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string userId, CancellationToken cancellationToken = default)
        {
            await _baseRepository.AddRangeAsync(entities, userId, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return entities;
        }

        public T Update(T entity, string userId)
        {
            _baseRepository.Update(entity, userId);
            _unitOfWork.SaveChanges();
            return entity;
        }


        public T UpdateIsDeleted(T entity, string userId)
        {
            _baseRepository.UpdateIsDeleted(entity, userId);
            _unitOfWork.SaveChanges();
            return entity;
        }
        public IEnumerable<T> UpdateIsDeletedRange(IEnumerable<T> entities, string userId)
        {
            _baseRepository.UpdateIsDeletedRange(entities, userId);
            _unitOfWork.SaveChanges();
            return entities;
        }
        public IEnumerable<T> UpdateRange(IEnumerable<T> entities, string userId)
        {
            _baseRepository.UpdateRange(entities, userId);
            _unitOfWork.SaveChanges();
            return entities;
        }

        public void Delete(T entity)
        {
            _baseRepository.Delete(entity);
            _unitOfWork.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _baseRepository.DeleteRange(entities);
            _unitOfWork.SaveChanges();
        }

        public void Attach(T entity)
        {
            _baseRepository.Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _baseRepository.AttachRange(entities);
        }

        public int Count()
        {
            return _baseRepository.Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _baseRepository.Count(criteria);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _baseRepository.CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.CountAsync(criteria, cancellationToken);
        }


    }
}
