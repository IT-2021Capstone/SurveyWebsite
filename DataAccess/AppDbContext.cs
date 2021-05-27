using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace DataAccess
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceResponse> MultipleChoiceResponses { get; set; }
        public DbSet<MultipleChoiceText> MultipleChoiceTexts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
