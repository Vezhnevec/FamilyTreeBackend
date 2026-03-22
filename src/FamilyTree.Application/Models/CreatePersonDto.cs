namespace FamilyTree.Api.Models;

public class CreatePersonDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public Domain.Entities.Gender Gender { get; set; }
    public string? Bio { get; set; }
}