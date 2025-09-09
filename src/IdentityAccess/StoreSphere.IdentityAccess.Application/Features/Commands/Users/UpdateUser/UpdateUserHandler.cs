using MediatR;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Domain.Aggregates;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Accounts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using StoreSphere.IdentityAccess.Domain.ValueObjects.UserType;

namespace StoreSphere.IdentityAccess.Application.Features.Commands.Users.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserHandler(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Load aggregate
            var user = await _repository.GetByIdAsync(new UserId(request.UserId), cancellationToken)
                       ?? throw new Exception("User not found");

            // 2. Domain-driven changes
            if (!string.IsNullOrEmpty(request.Email))
                user.ChangeEmail(Email.Create(request.Email));

            if (!string.IsNullOrEmpty(request.UserType) &&
                Enum.TryParse<UserType>(request.UserType, out var newType))
                user.ChangeUserType(newType);

            if (request.ActivateUser.HasValue)
            {
                if (request.ActivateUser.Value) user.Activate();
                else user.Deactivate();
            }

            // 3. Persist in both Domain + Identity
            await _repository.Update(user, request.CurrentPassword, request.NewPassword);

            // 4. Save domain changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
