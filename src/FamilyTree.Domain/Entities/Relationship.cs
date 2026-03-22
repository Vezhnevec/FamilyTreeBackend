namespace FamilyTree.Domain.Entities;

public class Relationship
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public Guid RelatedPersonId { get; set; }
    public Person RelatedPerson { get; set; } = null!;

    public RelationshipType Type { get; set; }

    public DateTime CreatedAt { get; set; }
}

public enum RelationshipType
{
    Parent = 1,      // Родитель
    Child = 2,       // Ребёнок
    Spouse = 3,      // Супруг
    Sibling = 4      // Брат/Сестра
}