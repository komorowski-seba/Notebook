using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserId { get; }
        
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = Guid
                .TryParse(httpContextAccessor.HttpContext?
                        .User
                        .FindFirstValue(ClaimTypes.NameIdentifier), 
                    out var userId)
                ? userId
                : Guid.Empty;
        }
    }        
}