using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Data.Entities;
using WebAPICore.Interfaces;
namespace WebAPICore.Data.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReactJSDemoContext _Context;



        public UnitOfWork(ReactJSDemoContext context)
        {
            this._Context = context;
        }
          public void Dispose()
        {
            _Context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IUserRepositry UserRepositry => new UserRepositry(_Context);

  
        public async Task<bool> SaveAsync()
        {
            return await _Context.SaveChangesAsync() > 0;
        }
    }
}