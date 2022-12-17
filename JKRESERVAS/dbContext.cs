
using JKRESERVAS.Entity;
using Microsoft.EntityFrameworkCore;

namespace JKRESERVAS
{
        public class SampleContext : DbContext
        {
        public SampleContext(DbContextOptions options) : base(options)
        {
        }

        private readonly string? _connectionString;
        public SampleContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString != null)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Doc_vinculados>().HasKey(a => a.Serie_doc);
            modelBuilder.Entity<Reservas>().HasMany(b => b.doc_vinculados).WithOne().HasForeignKey(p => p.Reservasid);
            modelBuilder.Entity<Doc_vinculados>().HasMany(b => b.documentoDetalles).WithOne().HasForeignKey(p => p.serieNumero);
        }

        public DbSet<Reservas> reservas { get; set; }
        public DbSet<Usuario> usuario { get; set; }
        public DbSet<Local> local { get; set; }
        public DbSet<empresa> empresa { get; set; }
        public DbSet<Usuario_local> usuario_local { get; set; }
        public DbSet<estado_mesa> estado_mesa { get; set; }
        public DbSet<Doc_vinculados> doc_vinculados { get; set; }
        public DbSet<documentoDetalles> documentoDetalles { get; set; }
    }

}
