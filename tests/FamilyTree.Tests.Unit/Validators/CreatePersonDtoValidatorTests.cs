using FamilyTree.Api.Models;
using FamilyTree.Api.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace FamilyTree.Tests.Unit.Validators;

public class CreatePersonDtoValidatorTests
{
    private readonly CreatePersonDtoValidator _validator;

    public CreatePersonDtoValidatorTests()
    {
        _validator = new CreatePersonDtoValidator();
    }

    [Fact]
    public void Validate_WithValidData_ShouldNotHaveErrors()
    {
        var dto = new CreatePersonDto
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateOnly(1970, 1, 1),
            Gender = Domain.Entities.Gender.Male,
            Bio = "Тест"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithEmptyFirstName_ShouldHaveError()
    {
        var dto = new CreatePersonDto
        {
            FirstName = "",  
            LastName = "Иванов",
            Gender = Domain.Entities.Gender.Male
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FirstName)
              .WithErrorMessage("Имя обязательно");
    }

    [Fact]
    public void Validate_WithFutureBirthDate_ShouldHaveError()
    {
        var dto = new CreatePersonDto
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
            Gender = Domain.Entities.Gender.Male
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
              .WithErrorMessage("Дата рождения не может быть в будущем");
    }

    [Fact]
    public void Validate_WithDeathDateBeforeBirthDate_ShouldHaveError()
    {
        var dto = new CreatePersonDto
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateOnly(1990, 1, 1),
            DeathDate = new DateOnly(1980, 1, 1),  
            Gender = Domain.Entities.Gender.Male
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.DeathDate)
              .WithErrorMessage("Дата смерти не может быть раньше даты рождения");
    }
}