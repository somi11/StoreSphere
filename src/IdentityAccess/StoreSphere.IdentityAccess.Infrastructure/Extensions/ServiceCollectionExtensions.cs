using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StoreSphere.IdentityAccess.Application.Contracts;
using StoreSphere.IdentityAccess.Infrastructure.Authentication;
using StoreSphere.IdentityAccess.Infrastructure.Common;
using StoreSphere.IdentityAccess.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreSphere.IdentityAccess.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityAccessInfrastructure(this IServiceCollection services)
        {

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IIdentityUserService, IdentityUserService>();
            services.AddScoped<IUserStore<IdentityUser>>(sp =>
            {
                var context = sp.GetRequiredService<AppIdentityDbContext>();
                var store = new UserStore<IdentityUser, IdentityRole, AppIdentityDbContext, string>(context)
                {
                    AutoSaveChanges = false // 🚀 key point
                };
                return store;
            });

            services.AddScoped<IRoleStore<IdentityRole>>(sp =>
            {
                var context = sp.GetRequiredService<AppIdentityDbContext>();
                var store = new RoleStore<IdentityRole, AppIdentityDbContext, string>(context)
                {
                    AutoSaveChanges = false // 🚀 key point
                };
                return store;
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            return services;
        }
    }
}
