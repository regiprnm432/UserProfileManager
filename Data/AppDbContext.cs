using Microsoft.EntityFrameworkCore;
using UserProfileManager.Models;

namespace UserProfileManager.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
