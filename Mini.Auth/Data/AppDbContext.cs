
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mini.AuthApi.Models;



namespace Mini.AuthApi.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        // Removed the invalid constructor with string parameter
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Corrected property name to match standard naming conventions
        public DbSet<User> Users { get; set; }
    }
}
