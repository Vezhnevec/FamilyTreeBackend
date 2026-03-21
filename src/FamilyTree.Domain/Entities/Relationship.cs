namespace FamilyTree.Domain.Entities;

public class Relationship
{
    public Guid Id { get; set; }

    // Кто является участником связи
    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;

    // С кем связана связь
    public Guid RelatedPersonId { get; set; }
    public Person RelatedPerson { get; set; } = null!;

    // Тип связи
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