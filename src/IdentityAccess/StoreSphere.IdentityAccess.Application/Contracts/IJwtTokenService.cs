using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Contracts
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(string userId);
    }
}
