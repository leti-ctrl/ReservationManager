using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReservationManager.Cache.Helper;
using ReservationManager.Cache.Redis;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories;

namespace ReservationManager.Cache.Repositories;

public class UserCachedRepository : CrudBaseEntityCacheRepository<User>, IUserRepository
{
    private readonly UserRepository _repository;
    private readonly IRedisService _redisService;
    
    public UserCachedRepository(UserRepository repository, IRedisService redisService) : base(repository, redisService)
    {
        _repository = repository;
        _redisService = redisService;
    }

    public async Task<User> UpdateUserRolesAsync(User user, Role[] roles)
    {
        return await _repository.UpdateUserRolesAsync(user, roles);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _repository.GetUserByEmailAsync(email);
    }

    public async Task<User?> GetUserByIdWithRoleAsync(int userId)
    {
        var redisKey = BuildKeyHelper.BuildKeyByTypeAndId(typeof(User), userId);
        var redisUser = await _redisService.GetAsync(redisKey);
        if (redisUser != null)
            return JsonConvert.DeserializeObject<User>(redisUser, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        
        var user = await _repository.GetUserByIdWithRoleAsync(userId);
        await _redisService.SetAsync(redisKey, JsonConvert.SerializeObject(user));
        return user;
    }
}