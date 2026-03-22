using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Domain.Entities;
public class Person  
{
    public Guid Id { get; set; }
    private string _firstName = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Relationship> Relationships { get; set; } = new List<Relationship>();

}

public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}