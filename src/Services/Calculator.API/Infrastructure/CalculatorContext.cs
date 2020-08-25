using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;
using Model = Calculator.API.Model;

namespace Calculator.API.Infrastructure
{
    public class CalculatorContext : DbContext
    {
        public CalculatorContext(DbContextOptions<CalculatorContext> options) 
            : base(options)
        {
            
        }

        public DbSet<Model.Calculator> Calculators { get; set; }
        public DbSet<Model.CalculatorType> CalculatorTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

    public class CalculatorContextDesignFactory : IDesignTimeDbContextFactory<CalculatorContext>
    {
        public CalculatorContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CalculatorContext>()
                .UseSqlServer("Server=sqldata;Initial Catalog=Calculator;Integrated Security=true");

            return new CalculatorContext(optionsBuilder.Options);
        }
    }
}
