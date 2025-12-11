using Microsoft.EntityFrameworkCore;
using Mini.AuthApi.Data;

namespace Mini.AuthApi.Extensions
{
    public static class BuildMigration
    {
       public static void  ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }
    }
}
