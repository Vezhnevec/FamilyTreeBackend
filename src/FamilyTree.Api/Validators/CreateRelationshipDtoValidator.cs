using FluentValidation;
using FamilyTree.Api.Models;

namespace FamilyTree.Api.Validators;

public class CreateRelationshipDtoValidator : AbstractValidator<CreateRelationshipDto>
{
    public CreateRelationshipDtoValidator()
    {
        // 🔹 Нельзя создать связь с самим собой
        RuleFor(x => x.PersonId)
            .NotEqual(x => x.RelatedPersonId)
            .WithMessage("Человек не может быть связан сам с собой");

        // 🔹 Оба ID должны быть не пустыми
        RuleFor(x => x.RelatedPersonId)
            .NotEmpty().WithMessage("ID связанного человека обязателен");

        // 🔹 Тип связи должен быть валидным
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Недопустимый тип связи");
    }
}