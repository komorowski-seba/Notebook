using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using WebApi.Common.Interfaces;
using WebApi.Common.Models;
using WebApi.Domain.Common;

namespace WebApi.Application.Handles.Note.GetList
{
    public class GetListNoteQuery : IRequest<IEnumerable<NoteModel>>
    {
        public int Position { get; init; }
        public int Size { get; init; }
    }

    public class GetListNoteHandler : IRequestHandler<GetListNoteQuery, IEnumerable<NoteModel>>
    {
        private readonly IMapper _mapper;
        private readonly INoteRepository _noteRepository;
        private readonly ICurrentUserService _currentUserService;
        
        public GetListNoteHandler(IMapper mapper, 
            INoteRepository noteRepository, 
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<NoteModel>> Handle(GetListNoteQuery request, CancellationToken cancellationToken)
        {
            var nodes = await  _noteRepository.GetListAsync(_currentUserService.UserId,
                request.Position,
                request.Size,
                cancellationToken);
            var result = _mapper.Map<IEnumerable<NoteModel>>(nodes);
            return result;
        }
    }
}