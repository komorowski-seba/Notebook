using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entity;

namespace WebApi.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Note> Notes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
