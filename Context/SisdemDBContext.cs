using Microsoft.EntityFrameworkCore;
using Sisdem.Models;

namespace Sisdem.Context
{
    public class SisdemDBContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

        public SisdemDBContext(DbContextOptions<SisdemDBContext> opts)  
            :  base (opts)
        {

        }
    }
}