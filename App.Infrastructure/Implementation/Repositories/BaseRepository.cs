using System.Data;
using System.Linq.Expressions;
using App.Application.Interfaces.Repositories;
using App.Common.Dtos.PaginaionModel;
using App.Domain.Consts;
using App.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Infrastructure.Implementation.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
	{
		public readonly ApplicationDbContext _context;

		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public void DetachEntity<T>(T entity)
		{
			var entry = _context.Entry(entity);
			if (entry != null)
			{
				entry.State = EntityState.Detached;
			}
		}


		public virtual IEnumerable<T> GetAll(bool withNoTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();

			if (withNoTracking)
				query = query.AsNoTracking();

			query = query.Where(x => !x.IsDeleted);
			query = query.OrderByDescending(x => x.CreatedOn);
			return query.ToList();
		}

		public virtual IEnumerable<T> GetAll(string[] includes, bool withNoTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();

			if (withNoTracking)
				query = query.AsNoTracking();

			query = query.Where(x => !x.IsDeleted);

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return query.ToList();
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync(bool withNoTracking = true, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>();

			if (withNoTracking)
				query = query.AsNoTracking();

			query = query.Where(x => !x.IsDeleted);
			query = query.OrderByDescending(x => x.CreatedOn);

			return await query.ToListAsync(cancellationToken);
		}
		public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>();

			query = query.Where(x => !x.IsDeleted);
			query = query.OrderByDescending(x => x.CreatedOn);

			return await query.ToListAsync(cancellationToken);
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync(string[] includes, bool withNoTracking = true, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>();

			if (withNoTracking)
				query = query.AsNoTracking();

			query = query.Where(x => !x.IsDeleted);

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return await query.ToListAsync();
		}
		public virtual async Task<IEnumerable<T>> GetAllAsync(string[]? includes = null, bool withNoTracking = true,
													Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Descending,
													CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>();

			if (withNoTracking)
				query = query.AsNoTracking();

			query = query.Where(x => !x.IsDeleted);

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);
			if (orderBy != null)
			{
				if (orderByDirection == OrderBy.Ascending)
					query = query.OrderBy(orderBy);
				else
					query = query.OrderByDescending(orderBy);
			}
			return await query.ToListAsync();
		}


		public IQueryable<T> GetQueryable()
		{
			return _context.Set<T>();
		}


		public T? GetById(Guid id, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();
			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return query.Where(o => o.Id == id).FirstOrDefault();
		}

		public async Task<T> GetByIdAsync(Guid id, string[] includes = null, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>();
			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return await query.Where(o => o.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return query.SingleOrDefault(criteria);
		}

		public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>();

			if (includes != null)
				foreach (var incluse in includes)
					query = query.Include(incluse);

			return await query.SingleOrDefaultAsync(criteria, cancellationToken);
		}

		public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			query = query.Where(x => !x.IsDeleted);
			query = query.OrderByDescending(x => x.CreatedOn);

			return query.Where(criteria).ToList();
		}

		public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
		{
			return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
		}

		public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
			Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Descending)
		{
			IQueryable<T> query = _context.Set<T>().Where(criteria);

			if (skip.HasValue)
				query = query.Skip(skip.Value);

			if (take.HasValue)
				query = query.Take(take.Value);

			if (orderBy != null)
			{
				if (orderByDirection == OrderBy.Ascending)
					query = query.OrderBy(orderBy);
				else
					query = query.OrderByDescending(orderBy);
			}

			return query.ToList();
		}

		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			return await query.Where(criteria).ToListAsync();
		}

		public async Task<IEnumerable<T>> FindAllAsyncWithTracking(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>().AsTracking();

			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			return await query.Where(criteria).ToListAsync();
		}
		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip, CancellationToken cancellationToken)
		{
			return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync(cancellationToken);
		}

		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
			Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, CancellationToken cancellationToken = default)
		{
			IQueryable<T> query = _context.Set<T>().Where(criteria);

			if (take.HasValue)
				query = query.Take(take.Value);

			if (skip.HasValue)
				query = query.Skip(skip.Value);

			if (orderBy != null)
			{
				if (orderByDirection == OrderBy.Ascending)
					query = query.OrderBy(orderBy);
				else
					query = query.OrderByDescending(orderBy);
			}

			return await query.ToListAsync(cancellationToken);
		}
		public IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>().AsNoTrackingWithIdentityResolution();

			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			query = query.Where(x => !x.IsDeleted);
			query = query.OrderByDescending(x => x.CreatedOn);

			return query.Where(criteria);
		}
		public IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria, int take, int skip, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>().AsNoTracking();
			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);
			query = query.Where(criteria).Skip(take * (skip - 1)).Take(take);

			return query;
		}
		public IQueryable<T> FindAllByQuarable(Expression<Func<T, bool>> criteria,
											   int take,
											   int skip,
											   string[] includes = null,
											   Expression<Func<T, object>> orderBy = null,
											   string orderByDirection = OrderBy.Ascending)
		{
			IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(criteria);

			if (includes != null)
			{
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
			}

			if (orderBy != null)
			{
				query = orderByDirection == OrderBy.Ascending
					? query.OrderBy(orderBy)
					: query.OrderByDescending(orderBy);
			}
			query = query.Skip(take * (skip - 1)).Take(take);
			return query;
		}

		public async Task<PagainationModel<IEnumerable<T>>> FindAllAsyncByPagination(Expression<Func<T, bool>>? criteria = null, int pageNumber = 1, int pageSize = 10,
																						string[] includes = null, CancellationToken cancellationToken = default)
		{
			int totalCount = 0;
			IQueryable<T> query = _context.Set<T>().AsNoTracking();

			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			if (criteria != null)
				query = query.Where(criteria);

			totalCount = query.Count();

			query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
			var data = await query.ToListAsync(cancellationToken);
			return new PagainationModel<IEnumerable<T>>()
			{
				Data = data,
				PageNumber = pageNumber,
				PageSize = pageSize,
				TotalCount = totalCount
			};
		}

		public T Add(T entity, string userId)
		{
			entity.CreatedBy = userId;
			entity.CreatedOn = DateTime.UtcNow;
			_context.Set<T>().Add(entity);
			return entity;
		}

		public async Task<T> AddAsync(T entity, string userId, CancellationToken cancellationToken = default)
		{
			entity.CreatedBy = userId;
			entity.CreatedOn = DateTime.UtcNow;
			await _context.Set<T>().AddAsync(entity, cancellationToken);
			return entity;
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities, string userId)
		{
			foreach (var entity in entities)
			{
				entity.CreatedBy = userId;
				entity.CreatedOn = DateTime.UtcNow;
			}
			_context.Set<T>().AddRange(entities);
			return entities;
		}

		public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, string userId, CancellationToken cancellationToken = default)
		{
			foreach (var entity in entities)
			{
				entity.CreatedBy = userId;
				entity.CreatedOn = DateTime.UtcNow;
			}
			await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
			return entities;
		}


		public T Update(T entity, string userId)
		{
			entity.UpdatedBy = userId;
			entity.UpdatedOn = DateTime.UtcNow;
			_context.Update(entity);
			return entity;
		}

		public T UpdateIsDeleted(T entity, string userId)
		{
			entity.IsDeleted = true;
			entity.UpdatedBy = userId;
			entity.UpdatedOn = DateTime.UtcNow;
			_context.Update(entity);
			return entity;
		}
		public IEnumerable<T> UpdateIsDeletedRange(IEnumerable<T> entities, string userId)
		{
			foreach (var entity in entities)
			{
				entity.IsDeleted = true;
				entity.UpdatedBy = userId;
				entity.UpdatedOn = DateTime.UtcNow;
			}
			_context.UpdateRange(entities);
			return entities;
		}

		public IEnumerable<T> UpdateRange(IEnumerable<T> entities, string userId)
		{
			foreach (var entity in entities)
			{
				entity.UpdatedBy = userId;
				entity.UpdatedOn = DateTime.UtcNow;
			}
			_context.UpdateRange(entities);
			_context.SaveChanges();
			return entities;
		}
		public void Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public void DeleteRange(IEnumerable<T> entities)
		{
			_context.Set<T>().RemoveRange(entities);
		}

		public void Attach(T entity)
		{
			_context.Set<T>().Attach(entity);
		}

		public void AttachRange(IEnumerable<T> entities)
		{
			_context.Set<T>().AttachRange(entities);
		}

		public int Count()
		{
			return _context.Set<T>().Count();
		}

		public int Count(Expression<Func<T, bool>> criteria)
		{
			return _context.Set<T>().Count(criteria);
		}

		public async Task<int> CountAsync(CancellationToken cancellationToken = default)
		{
			return await _context.Set<T>().CountAsync(cancellationToken);
		}

		public async Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
		{
			return await _context.Set<T>().CountAsync(criteria, cancellationToken);
		}
		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return await _context.Database.BeginTransactionAsync();
		}


	}
}
