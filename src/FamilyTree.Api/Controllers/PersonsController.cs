using FamilyTree.Api.Models;           // ← Для CreatePersonDto, UpdatePersonDto
using FamilyTree.Domain.Entities;      // ← Для Person, Gender
using FamilyTree.Infrastructure.Data;  // ← Для ApplicationDbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Api.Controllers;  // ← Важно: правильный namespace

[ApiController]  // ← Обязательно!
[Route("api/[controller]")]  // ← Обязательно!
public class PersonsController : ControllerBase  // ← public класс!
{
    private readonly ApplicationDbContext _context;

    public PersonsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/persons
    [HttpGet]  // ← Атрибут метода!
    public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
    {
        return await _context.Persons.ToListAsync();
    }

    // GET: api/persons/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(Guid id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();
        return person;
    }

    // POST: api/persons
    [HttpPost]
    public async Task<ActionResult<Person>> CreatePerson(CreatePersonDto dto)
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            DeathDate = dto.DeathDate,
            Gender = dto.Gender,
            Bio = dto.Bio,
            CreatedAt = DateTime.UtcNow
        };

        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
    }
    // PUT: api/persons/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] UpdatePersonDto dto)  // ← Changed!
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();

        person.FirstName = dto.FirstName;      // ← Теперь работает!
        person.LastName = dto.LastName;        // ← Теперь работает!
        person.BirthDate = dto.BirthDate;
        person.DeathDate = dto.DeathDate;
        person.Gender = dto.Gender;
        person.Bio = dto.Bio;
        person.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/persons/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}