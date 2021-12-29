using System;

namespace WebApi.Application.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }
    }
}
