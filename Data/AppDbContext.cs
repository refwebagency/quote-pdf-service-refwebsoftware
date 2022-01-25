using Microsoft.EntityFrameworkCore;
using QuotePDFService.Models;

namespace QuotePDFService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<QuotePDF> QuotePDF { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<TodoTemplate> TodoTemplate { get; set; }
    }
}