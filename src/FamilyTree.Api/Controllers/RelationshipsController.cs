using FamilyTree.Api.Models;
using FamilyTree.Domain.Entities;
using FamilyTree.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelationshipsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RelationshipsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/relationships
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RelationshipResponseDto>>> GetRelationships()
    {
        var relationships = await _context.Relationships
            .Include(r => r.Person)
            .Include(r => r.RelatedPerson)
            .Select(r => new RelationshipResponseDto
            {
                Id = r.Id,
                PersonId = r.PersonId,
                PersonName = $"{r.Person.FirstName} {r.Person.LastName}", 
                RelatedPersonId = r.RelatedPersonId,
                RelatedPersonName = $"{r.RelatedPerson.FirstName} {r.RelatedPerson.LastName}",  
                Type = r.Type,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(relationships);
    }

    // GET: api/relationships/person/{personId}
    [HttpGet("person/{personId}")]
    public async Task<ActionResult<IEnumerable<RelationshipResponseDto>>> GetRelationshipsByPerson(Guid personId)
    {
        var relationships = await _context.Relationships
            .Include(r => r.Person)
            .Include(r => r.RelatedPerson)
            .Where(r => r.PersonId == personId || r.RelatedPersonId == personId)
            .Select(r => new RelationshipResponseDto
            {
                Id = r.Id,
                PersonId = r.PersonId,
                PersonName = $"{r.Person.FirstName} {r.Person.LastName}",
                RelatedPersonId = r.RelatedPersonId,
                RelatedPersonName = $"{r.RelatedPerson.FirstName} {r.RelatedPerson.LastName}",
                Type = r.Type,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(relationships);
    }

    // POST: api/relationships
    [HttpPost]
    public async Task<ActionResult<Relationship>> CreateRelationship(CreateRelationshipDto dto)
    {
        if (dto.PersonId == dto.RelatedPersonId)
            return BadRequest("Человек не может быть связан сам с собой");

        var personExists = await _context.Persons.AnyAsync(p => p.Id == dto.PersonId);
        var relatedPersonExists = await _context.Persons.AnyAsync(p => p.Id == dto.RelatedPersonId);

        if (!personExists || !relatedPersonExists)
            return NotFound("Один или оба человека не найдены");

        var existing = await _context.Relationships
            .AnyAsync(r => r.PersonId == dto.PersonId
                        && r.RelatedPersonId == dto.RelatedPersonId
                        && r.Type == dto.Type);

        if (existing)
            return BadRequest("Такая связь уже существует");

        var relationship = new Relationship
        {
            Id = Guid.NewGuid(),
            PersonId = dto.PersonId,
            RelatedPersonId = dto.RelatedPersonId,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow
        };

        _context.Relationships.Add(relationship);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRelationships), new { id = relationship.Id }, relationship);
    }

    // DELETE: api/relationships/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRelationship(Guid id)
    {
        var relationship = await _context.Relationships.FindAsync(id);
        if (relationship == null) return NotFound();

        _context.Relationships.Remove(relationship);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}