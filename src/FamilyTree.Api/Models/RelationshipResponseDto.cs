namespace FamilyTree.Api.Models;

public class RelationshipResponseDto
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;  // ← Готовое имя
    public Guid RelatedPersonId { get; set; }
    public string RelatedPersonName { get; set; } = string.Empty;  // ← Готовое имя
    public Domain.Entities.RelationshipType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}