using FamilyTree.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FamilyTree.Tests.Unit.Tests;

public class PersonTests
{
    [Fact]
    public void Person_Create_WithValidData_ShouldHaveCorrectProperties()
    {
        var person = new Person
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111001"),
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateOnly(1970, 1, 1),
            Gender = Gender.Male,
            Bio = "Тестовый пользователь",
            CreatedAt = DateTime.UtcNow
        };

        person.FirstName.Should().Be("Иван");
        person.LastName.Should().Be("Иванов");
        person.BirthDate.Should().Be(new DateOnly(1970, 1, 1));
        person.Gender.Should().Be(Gender.Male);
        person.Id.Should().NotBeEmpty();
        person.CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public void Person_Create_WithNullFirstName_ShouldBeNull()
    {
        var person = new Person
        {
            FirstName = null,
            LastName = "Иванов"
        };

        person.FirstName.Should().BeNull();
    }

    [Fact]
    public void Person_Create_WithNullLastName_ShouldBeNull()
    {
        var person = new Person
        {
            FirstName = "Иван",
            LastName = null
        };

        person.LastName.Should().BeNull();
    }

    [Fact]
    public void Person_Create_WithGender_ShouldHaveCorrectValue()
    {
        var person = new Person { Gender = Gender.Male };

        person.Gender.Should().Be(Gender.Male);  
    }

    [Fact]
    public void Person_Create_ShouldHaveCreatedAtSet()
    {
        var before = DateTime.UtcNow;

        var person = new Person
        {
            FirstName = "Иван",
            LastName = "Иванов",
            CreatedAt = DateTime.UtcNow
        };

        person.CreatedAt.Should().BeAfter(before.AddSeconds(-1));
        person.CreatedAt.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
    }
}