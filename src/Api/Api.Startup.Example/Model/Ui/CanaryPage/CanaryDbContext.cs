using Microsoft.EntityFrameworkCore;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

/// <summary>
/// Class CanaryDbContext.
/// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
public class CanaryDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CanaryDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public CanaryDbContext(DbContextOptions<CanaryDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the test entities.
    /// </summary>
    /// <value>The test entities.</value>
    public DbSet<TestEntity> TestEntities { get; set; }
}