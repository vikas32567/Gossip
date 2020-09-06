using Gossip.Models;
using System.Collections.Generic;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace Gossip.Helpers
{
    public static class TokenClient
    {
        public static ClaimsPrincipal GenerateToken(LoginRequest request)
        {
            var claims = new List<Claim>();
            var randomId = GenerateRandomNumber();
            
            claims.Add(new Claim(ClaimTypes.Sid, randomId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, request.Name));

            var principle = new ClaimsPrincipal();
            principle.AddIdentity(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            return principle;
        }

        private static int GenerateRandomNumber()
        {
            var rand = new Random();
            var number = rand.Next(1, 100);

            return number;
        }
        
    }
}