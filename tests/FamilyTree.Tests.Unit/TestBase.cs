using FamilyTree.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Tests.Unit;

public abstract class TestBase
{
    protected ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}