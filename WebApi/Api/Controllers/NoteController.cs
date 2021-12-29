using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Application.Handles.Note.Create;
using WebApi.Application.Handles.Note.GetList;
using WebApi.Application.Models;

namespace Api.Controllers
{
    [ApiController, Route("Note")]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NoteController> _logger;

        public NoteController(IMediator mediator, ILogger<NoteController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost, Authorize]
        public async Task<Guid> Create([FromBody] NoteModel model)
        {
            try
            {
                var result = await _mediator.Send(new CreateNoteCommand
                {
                    Topic = model.Topic,
                    Description = model.Desc
                });
                return result.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }

        }

        [HttpGet, Authorize]
        public async Task<IList<NoteModel>> GetList(int position, int size)
        {
            var result = await _mediator.Send(new GetListNoteQuery
            {
                Position = position,
                Size = size
            });
            return result.ToList();
        }
    }
}