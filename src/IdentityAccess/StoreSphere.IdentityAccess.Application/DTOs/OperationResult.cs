using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.DTOs
{
    public record OperationResult(bool Success, string? ErrorMessage = null)
    {
        public static OperationResult Ok() => new(true);
        public static OperationResult Fail(string error) => new(false, error);
    }
}
