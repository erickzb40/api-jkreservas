using JKRESERVAS.Entity;
using Microsoft.EntityFrameworkCore;

namespace JKRESERVAS
{
        public class SampleContext : DbContext
        {
            public SampleContext(DbContextOptions<SampleContext> options) : base(options)
            {
            }

        public DbSet<reservas> reservas { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        }

}
