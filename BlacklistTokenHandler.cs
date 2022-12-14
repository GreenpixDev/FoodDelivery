using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;

namespace FoodDelivery;

public class BlacklistTokenHandler : AuthorizationHandler<BlacklistTokenRequirement>
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _accessor;

    public BlacklistTokenHandler(IDistributedCache cache, IHttpContextAccessor accessor)
    {
        _cache = cache;
        _accessor = accessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BlacklistTokenRequirement requirement)
    {
        if (_accessor.HttpContext == null)
        {
            return;
        }
        
        string token = _accessor.HttpContext.Request.Headers[HeaderNames.Authorization]
            .ToString()
            .Replace("Bearer ", "");

        if (await _cache.GetStringAsync(token) == null)
        {
            context.Succeed(requirement);
        }
    }
}