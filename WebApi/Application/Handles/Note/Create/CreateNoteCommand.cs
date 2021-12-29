using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Application.Interfaces;
using WebApi.Domain.Common;

namespace WebApi.Application.Handles.Note.Create
{
    public class CreateNoteCommand : IRequest<Domain.Entity.Note>
    {
        public string Topic { get; init; }
        public string Description { get; init; }
    }

    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Domain.Entity.Note>
    {
        private readonly INoteRepository _noteRepository;
        private readonly IApplicationDbContext _dbContext;
        
        public CreateNoteCommandHandler(INoteRepository noteRepository, IApplicationDbContext dbContext)
        {
            _noteRepository = noteRepository;
            _dbContext = dbContext;
        }

        public async Task<Domain.Entity.Note> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = new Domain.Entity.Note(request.Topic, request.Description);
            var result = await _noteRepository.CreateAsync(note, cancellationToken);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}