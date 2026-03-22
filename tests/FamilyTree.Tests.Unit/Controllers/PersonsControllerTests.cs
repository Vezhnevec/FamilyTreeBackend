using FamilyTree.Api.Controllers;
using FamilyTree.Api.Models;
using FamilyTree.Domain.Entities;
using FamilyTree.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace FamilyTree.Tests.Unit.Controllers;

public class PersonsControllerTests : TestBase
{
    [Fact]
    public async Task CreatePerson_WithValidDto_ReturnsCreatedAtAction()
    {
        using var context = CreateContext();  // ← Используем метод из базового класса
        var controller = new PersonsController(context);

        var dto = new CreatePersonDto
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateOnly(1970, 1, 1),
            Gender = Gender.Male,
            Bio = "Тест"
        };

        var result = await controller.CreatePerson(dto);

        var createdAtResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;

        var person = createdAtResult.Value.Should().BeOfType<Person>().Subject;
        person.FirstName.Should().Be("Иван");
        person.LastName.Should().Be("Иванов");
        person.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetPerson_WithNonExistentId_ReturnsNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var controller = new PersonsController(context);

        var testId = Guid.NewGuid();

        var result = await controller.GetPerson(testId);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetPerson_WithExistingId_ReturnsPerson()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var expectedPerson = new Person
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111001"),
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateOnly(1970, 1, 1),
            Gender = Gender.Male,
            CreatedAt = DateTime.UtcNow
        };

        context.Persons.Add(expectedPerson);
        await context.SaveChangesAsync();

        var controller = new PersonsController(context);

        var result = await controller.GetPerson(expectedPerson.Id);

        var person = result.Value.Should().BeOfType<Person>().Subject;
        person.FirstName.Should().Be("Иван");
        person.Id.Should().Be(expectedPerson.Id);
    }


}