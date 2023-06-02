using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace WebApplication2.Models
{
    public class AppDbContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL("server=localhost;port=3306;username=root;password=;database=animal_shelter_test;convert zero datetime=True");
        //}
    }
}
