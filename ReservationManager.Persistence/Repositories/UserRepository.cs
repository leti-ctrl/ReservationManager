﻿using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class UserRepository : CrudBaseEntityRepository<User>, IUserRepository
    {
        public UserRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> UpdateUserRolesAsync(User user, Role[] roles)
        {
            var deletable = await Context.Set<RoleUser>().Where(x => x.UserId == user.Id)
                .ToListAsync();
            Context.Set<RoleUser>().RemoveRange(deletable);
            var newRolesUser = roles.Select(x => new RoleUser
            {
                UserId = user.Id,
                RolesId = x.Id
            });
            await Context.Set<RoleUser>().AddRangeAsync(newRolesUser);
            await Context.SaveChangesAsync();
            user.Roles = roles;
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await Context.Set<User>()
                                .Include(r => r.Roles)
                                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetUserByIdWithRoleAsync(int userId)
        {
            return await Context.Set<User>()
                .Include(r => r.Roles)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
