using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Features.Commands.Users.RemoveUser
{
    public record RemoveUserCommand(Guid UserId) : IRequest<Unit>;

}
