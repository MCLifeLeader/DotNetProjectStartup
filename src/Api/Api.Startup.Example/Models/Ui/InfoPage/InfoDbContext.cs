using Microsoft.EntityFrameworkCore;

namespace Startup.Api.Models.Ui.InfoPage;

/// <summary>
/// Class CanaryDbContext.
/// Implements the <see cref="Microsoft.EntityFrameworkCore.DbContext" />
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
public class InfoDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InfoDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public InfoDbContext(DbContextOptions<InfoDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the test entities.
    /// </summary>
    /// <value>The test entities.</value>
    public DbSet<TestEntity> TestEntities { get; set; }
}