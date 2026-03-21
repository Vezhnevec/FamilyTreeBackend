using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FamilyTree.Infrastructure.Data;

// 🔹 Эта фабрика нужна ТОЛЬКО для миграций и инструментов EF Core
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Настраиваем опции для design-time (миграции)
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Используем ту же строку подключения, что и в приложении
        // Или можно захардкодить для миграций:
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=familytree;Username=postgres;Password=postgres123");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}