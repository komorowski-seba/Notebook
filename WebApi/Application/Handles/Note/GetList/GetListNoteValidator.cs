using FluentValidation;

namespace WebApi.Application.Handles.Note.GetList
{
    public class GetListNoteValidator : AbstractValidator<GetListNoteQuery>
    {
        public GetListNoteValidator()
        {
            RuleFor(n => n.Position)
                .Must(m => m >= 0).WithMessage("Cannot be less than zero");
            RuleFor(n => n.Size)
                .Must(m => m > 0).WithMessage("Cannot be less than one");
        }
    }
}