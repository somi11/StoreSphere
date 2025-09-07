using MediatR;
using StoreShpere.SharedKernel.Events;
using StoreShpere.SharedKernel.UserIntegrationEvents.UserRemoved;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Domain.ValueObjects.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Application.Features.Commands.Users.RemoveUser
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public RemoveUserHandler(
            IUserRepository userRepository,
            ITenantRepository tenantRepository,
            IStoreRepository storeRepository,
            IUnitOfWork unitOfWork,
            IIntegrationEventPublisher publisher
            )
        {
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _storeRepository = storeRepository;
            _unitOfWork = unitOfWork;
            _integrationEventPublisher = publisher;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // Remove user from tenant
            if (user.TenantId != null)
            {
                var tenant = await _tenantRepository.GetByIdAsync(user.TenantId, cancellationToken);
                tenant?.RemoveUser(user.Id);
            }

            // Remove user from all stores
            var stores = await _storeRepository.GetByUserIdAsync(user.Id, cancellationToken);
            foreach (var store in stores)
            {
                var assignments = store.Assignments.Where(a => a.UserId == user.Id).ToList();
                foreach (var assignment in assignments)
                {
                    store.RemoveUser(user.Id, assignment.RoleId);
                }
            }
            // deactivate user
            if (user.IsActive)
            {
                user.Deactivate();
            }

            // Remove user aggregate
            await _userRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var integrationEvent = new UserRemovedIntegrationEvent(user.Id.Value);
            await _integrationEventPublisher.PublishAsync(integrationEvent, cancellationToken);


            return Unit.Value;
        }
    }
}
