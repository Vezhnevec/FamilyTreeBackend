using FluentValidation;
using FamilyTree.Api.Models;

namespace FamilyTree.Api.Validators;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    public CreatePersonDtoValidator()
    {
        // 🔹 Имя
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(100).WithMessage("Имя не может быть длиннее 100 символов")
            .Matches(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$").WithMessage("Имя содержит недопустимые символы");

        // 🔹 Фамилия
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Фамилия обязательна")
            .MaximumLength(100).WithMessage("Фамилия не может быть длиннее 100 символов");

        // 🔹 Дата рождения (не в будущем)
        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(x => x.BirthDate.HasValue)
            .WithMessage("Дата рождения не может быть в будущем");

        // 🔹 Дата смерти (не раньше рождения)
        RuleFor(x => x.DeathDate)
            .GreaterThanOrEqualTo(x => x.BirthDate)
            .When(x => x.BirthDate.HasValue && x.DeathDate.HasValue)
            .WithMessage("Дата смерти не может быть раньше даты рождения");

        // 🔹 Биография (опционально, но с ограничением)
        RuleFor(x => x.Bio)
            .MaximumLength(2000).WithMessage("Биография не может быть длиннее 2000 символов");
    }
}