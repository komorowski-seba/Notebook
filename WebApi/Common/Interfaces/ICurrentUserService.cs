using System;

namespace WebApi.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid UserId { get; }
    }
}
