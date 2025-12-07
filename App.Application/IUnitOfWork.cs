using App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Application
{
    public interface IUnitOfWork : IDisposable
    {
        //IBaseRepository<T> BaseRepository<T>() where T : class; 

        //IBaseService<Author> Authors { get; }
        //IBookService Books { get; }

        //IBaseRepository<Author> Authors { get; }






        int SaveChanges();
        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}