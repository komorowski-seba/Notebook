using System;

namespace WebApi.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
