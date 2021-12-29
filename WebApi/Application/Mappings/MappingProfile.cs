using AutoMapper;
using WebApi.Application.Models;
using WebApi.Domain.Entity;

namespace WebApi.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Note, NoteModel>()
                .ForMember(dto => dto.Topic, opt => opt.MapFrom(note => note.Topic))
                .ForMember(dto => dto.Desc, opt => opt.MapFrom(note => note.Description));
        }
    }
}