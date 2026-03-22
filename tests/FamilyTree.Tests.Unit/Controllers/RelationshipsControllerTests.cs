using FamilyTree.Api.Controllers;
using FamilyTree.Api.Models;
using FamilyTree.Domain.Entities;
using FamilyTree.Infrastructure.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FamilyTree.Tests.Unit.Controllers;

public class RelationshipsControllerTests
{
    [Fact]
    public async Task CreateRelationship_WithValidDto_ReturnsCreatedAtAction()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var person1 = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Иван",
            LastName = "Иванов",
            Gender = Gender.Male,
            CreatedAt = DateTime.UtcNow
        };
        var person2 = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Пётр",
            LastName = "Петров",
            Gender = Gender.Male,
            CreatedAt = DateTime.UtcNow
        };

        context.Persons.AddRange(person1, person2);
        await context.SaveChangesAsync();

        var controller = new RelationshipsController(context);

        var dto = new CreateRelationshipDto
        {
            PersonId = person1.Id,
            RelatedPersonId = person2.Id,
            Type = RelationshipType.Parent
        };

        var result = await controller.CreateRelationship(dto);

        var createdAtResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;

        var relationship = createdAtResult.Value.Should().BeOfType<Relationship>().Subject;
        relationship.PersonId.Should().Be(dto.PersonId);
        relationship.RelatedPersonId.Should().Be(dto.RelatedPersonId);
        relationship.Type.Should().Be(dto.Type);
    }

    [Fact]
    public async Task CreateRelationship_WithSamePersonId_ReturnsBadRequest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var controller = new RelationshipsController(context);

        var sameId = Guid.NewGuid();
        var dto = new CreateRelationshipDto
        {
            PersonId = sameId,
            RelatedPersonId = sameId,
            Type = RelationshipType.Parent
        };

        var result = await controller.CreateRelationship(dto);

        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetRelationshipsByPerson_WithExistingId_ReturnsRelationships()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var person1 = new Person { Id = Guid.NewGuid(), FirstName = "Иван", LastName = "Иванов", Gender = Gender.Male, CreatedAt = DateTime.UtcNow };
        var person2 = new Person { Id = Guid.NewGuid(), FirstName = "Пётр", LastName = "Петров", Gender = Gender.Male, CreatedAt = DateTime.UtcNow };
        context.Persons.AddRange(person1, person2);
        await context.SaveChangesAsync();

        var relationship = new Relationship
        {
            Id = Guid.NewGuid(),
            PersonId = person1.Id,
            RelatedPersonId = person2.Id,
            Type = RelationshipType.Parent,
            CreatedAt = DateTime.UtcNow
        };

        context.Relationships.Add(relationship);
        await context.SaveChangesAsync();

        var controller = new RelationshipsController(context);

        var result = await controller.GetRelationshipsByPerson(person1.Id);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;

        var relationships = okResult.Value.Should().BeAssignableTo<IEnumerable<RelationshipResponseDto>>().Subject;

        relationships.Should().HaveCount(1);
        relationships.First().PersonId.Should().Be(person1.Id);
        relationships.First().RelatedPersonId.Should().Be(person2.Id);
        relationships.First().Type.Should().Be(RelationshipType.Parent);
        relationships.First().PersonName.Should().Be("Иван Иванов");
        relationships.First().RelatedPersonName.Should().Be("Пётр Петров");
    }
}