using MediatR;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Features.Commands.Users.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserHandler(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(
            new UserId(Guid.NewGuid()),
            Enum.Parse<UserType>(request.UserType),
            Email.Create(request.Email),
            request.TenantId.HasValue ? new TenantId(request.TenantId.Value) : null
        );
           await _repository.Add(user, request.Password);

            // Persist + dispatch domain events
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id.Value;
        }
    }
}
