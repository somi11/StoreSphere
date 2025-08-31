using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
    public interface IUnitOfWork
    {
        /// <summary>Save changes and dispatch domain events (in Infrastructure implementation)</summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
