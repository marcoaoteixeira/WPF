using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nameless.WPF.Data;

/// <summary>
///     Represents the application <see cref="DbContext"/>.
/// </summary>
public class AppDbContext : DbContext {
    /// <summary>
    ///     Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">
    ///     The database context options.
    /// </param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}

// This is necessary to use the EF Core CLI tool
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
    public AppDbContext CreateDbContext(string[] args) {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                      .UseSqlite("Data Source=database.db")
                      .Options;

        return new AppDbContext(options);
    }
}