using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Application.Interfaces;
using WebApi.Domain.Common;
using WebApi.Domain.Entity;

namespace WebApi.Infrastructure.Persistence.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IApplicationDbContext _dbContext;

        public NoteRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Note> CreateAsync(Note note, CancellationToken cancellToken)
        {
            await _dbContext.Notes.AddAsync(note, cancellToken);
            return note;
        }

        public async Task<IEnumerable<Note>> GetListAsync(Guid userId, int position, int size, CancellationToken cancellToken)
        {
            var result = await _dbContext.Notes
                .AsNoTracking()
                .Where(note => note.CreatedBy.Equals(userId))
                .OrderByDescending(note => note.Created)
                .Skip(position * size)
                .Take(size)
                .ToListAsync(cancellationToken: cancellToken);
            return result;
        }
    }
}