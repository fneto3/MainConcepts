using Calculator.API.Model;
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
        public DbSet<CalculatorType> CalculatorTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
