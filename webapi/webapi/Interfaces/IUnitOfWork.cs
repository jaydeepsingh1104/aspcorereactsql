using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPICore.Interfaces ;

namespace WebAPICore.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
         Task<bool> SaveAsync();
         IUserRepositry UserRepositry {get; }
    }
}