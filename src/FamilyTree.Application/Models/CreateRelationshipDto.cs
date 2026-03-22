namespace FamilyTree.Api.Models;

public class CreateRelationshipDto
{
    public Guid PersonId { get; set; }
    public Guid RelatedPersonId { get; set; }
    public Domain.Entities.RelationshipType Type { get; set; }
}