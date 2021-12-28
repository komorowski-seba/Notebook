using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Domain.Entity;

namespace WebApi.Domain.Common
{
    public interface INoteRepository
    {
        Task<Note> CreateAsync(Note note, CancellationToken cancellToken);
        Task<IEnumerable<Note>> GetListAsync(Guid userId, int position, int size, CancellationToken cancellToken);
    }
}