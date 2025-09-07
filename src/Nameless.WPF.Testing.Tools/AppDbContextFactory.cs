using System.IO;
using Microsoft.EntityFrameworkCore;
using Nameless.WPF.Data;

namespace Nameless.WPF.Testing.Tools;

public static class AppDbContextFactory {
    public static readonly string BaseDirectoryPath = Path.Combine(typeof(AppDbContextFactory).Assembly.GetDirectoryPath(), "databases");

    public static AppDbContext Create(Action<AppDbContextFactoryOptions>? configure = null) {
        var innerConfigure = configure ?? (_ => { });
        var options = new AppDbContextFactoryOptions();

        innerConfigure(options);

        var builder = new DbContextOptionsBuilder<AppDbContext>();

        builder = options.UseInMemory
            ? builder.ConfigureInMemoryDatabase()
            : builder.ConfigureFileDatabase();

        builder.AddInterceptors(options.Interceptors)
               .UseSeeding(options.Seeding ?? ((_, _) => { }))
               .UseAsyncSeeding(options.SeedingAsync ?? ((_, _, _) => Task.CompletedTask));

        var dbContext = new AppDbContext(builder.Options);

        // It will trigger the seeding process.
        if (options.UseInMemory) { dbContext.Database.EnsureCreated(); }
        else { dbContext.Database.Migrate(); }

        return dbContext;
    }

    private static DbContextOptionsBuilder<AppDbContext> ConfigureFileDatabase(this DbContextOptionsBuilder<AppDbContext> self) {
        Directory.CreateDirectory(BaseDirectoryPath);

        var databaseFilePath = Path.Combine(BaseDirectoryPath, $"{Guid.NewGuid():N}.db");
        var connectionString = string.Format(Constants.Database.CONN_STR_PATTERN, databaseFilePath);

        return self.UseSqlite(connectionString);
    }

    private static DbContextOptionsBuilder<AppDbContext> ConfigureInMemoryDatabase(this DbContextOptionsBuilder<AppDbContext> self) {
        return self.UseInMemoryDatabase($"{Guid.NewGuid():N}.db");
    }
}