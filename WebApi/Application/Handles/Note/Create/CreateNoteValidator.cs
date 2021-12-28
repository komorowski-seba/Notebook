using FluentValidation;

namespace WebApi.Application.Handles.Note.Create
{
    public class CreateNoteValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteValidator()
        {
            RuleFor(n => n.Topic)
                .NotNull().WithMessage("Is null")
                .NotEmpty().WithMessage("Is empty");
            RuleFor(n => n.Description)
                .NotNull().WithMessage("Is null")
                .NotEmpty().WithMessage("Is empty");
        }
    }
}