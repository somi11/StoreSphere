using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Features.Commands.Users.UpdateUser
{
    public record UpdateUserCommand(Guid UserId, string? Email, string? UserType, bool? ActivateUser) 
        : IRequest<Unit>;
}

