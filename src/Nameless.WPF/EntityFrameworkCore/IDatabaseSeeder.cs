using Microsoft.EntityFrameworkCore;

namespace Nameless.WPF.EntityFrameworkCore;

public interface IDatabaseSeeder {
    Task ExecuteAsync(DbContext dbContext, bool storeManagementOperation, CancellationToken cancellationToken);

    void Execute(DbContext dbContext, bool storeManagementOperation);
}