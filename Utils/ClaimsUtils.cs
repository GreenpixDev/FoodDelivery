﻿using System.Security.Claims;
using FoodDelivery.Configuration;

namespace FoodDelivery.Utils;

public class ClaimsUtils
{
    private const string ShortTypeName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/ShortTypeName";
    
    public static string getEmail(ClaimsPrincipal principal)
    {
        return principal.Claims
            .First(e => e.Properties[ShortTypeName] == JwtConfiguration.EmailClaim)
            .Value;
    }
}