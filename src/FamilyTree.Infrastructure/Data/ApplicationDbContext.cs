using FamilyTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public virtual DbSet<Person> Persons => Set<Person>();
    public virtual DbSet<Relationship> Relationships => Set<Relationship>();  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Bio)
                  .HasMaxLength(2000);

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Relationship>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Связь с Person (кто)
            entity.HasOne(e => e.Person)
                  .WithMany(p => p.Relationships)
                  .HasForeignKey(e => e.PersonId)
                  .OnDelete(DeleteBehavior.Restrict); 

            // Связь с Person (с кем)
            entity.HasOne(e => e.RelatedPerson)
                  .WithMany()
                  .HasForeignKey(e => e.RelatedPersonId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PersonId, e.RelatedPersonId, e.Type })
                  .IsUnique();
        });
        // 🔹 Сидирование тестовых данных
        modelBuilder.Entity<Person>().HasData(
            new Person
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111001"),
                FirstName = "Иван",
                LastName = "Иванов",
                BirthDate = new DateOnly(1970, 1, 1),
                DeathDate = null,
                Gender = Gender.Male,
                Bio = "Основатель семейства",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Person
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111002"),
                FirstName = "Петр",
                LastName = "Иванов",
                BirthDate = new DateOnly(1995, 5, 15),
                DeathDate = null,
                Gender = Gender.Male,
                Bio = "Сын Ивана",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Person
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111003"),
                FirstName = "Анна",
                LastName = "Иванова",
                BirthDate = new DateOnly(1998, 8, 20),
                DeathDate = null,
                Gender = Gender.Female,
                Bio = "Дочь Ивана",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Relationship>().HasData(
            new Relationship
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222001"),
                PersonId = Guid.Parse("11111111-1111-1111-1111-111111111001"), // Иван
                RelatedPersonId = Guid.Parse("11111111-1111-1111-1111-111111111002"), // Петр
                Type = RelationshipType.Parent,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Relationship
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222002"),
                PersonId = Guid.Parse("11111111-1111-1111-1111-111111111001"), // Иван
                RelatedPersonId = Guid.Parse("11111111-1111-1111-1111-111111111003"), // Анна
                Type = RelationshipType.Parent,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}