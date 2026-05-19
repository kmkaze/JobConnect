using Microsoft.EntityFrameworkCore;
using JobConnectAPI.Models;

namespace JobConnectAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Postulacion> Postulaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cedula).HasColumnName("cedula").IsRequired();
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
                entity.Property(e => e.Apellido).HasColumnName("apellido").IsRequired();
                entity.Property(e => e.Celular).HasColumnName("celular").IsRequired();
                entity.Property(e => e.Correo).HasColumnName("correo").IsRequired();
                entity.Property(e => e.Contrasena).HasColumnName("contrasena").IsRequired();
                entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
                entity.Property(e => e.Rol).HasColumnName("rol"); // ← debe estar
                entity.HasIndex(e => e.Correo).IsUnique();
                entity.HasIndex(e => e.Cedula).IsUnique();
            });

            // Ofertas
            modelBuilder.Entity<Oferta>(entity =>
            {
                entity.ToTable("ofertas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Titulo).HasColumnName("titulo").IsRequired();
                entity.Property(e => e.Empresa).HasColumnName("empresa").IsRequired();
                entity.Property(e => e.Ubicacion).HasColumnName("ubicacion").IsRequired();
                entity.Property(e => e.Salario).HasColumnName("salario");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion").IsRequired();
                entity.Property(e => e.FechaPublicacion).HasColumnName("fecha_publicacion");
            });

            // Postulaciones
            modelBuilder.Entity<Postulacion>(entity =>
            {
                entity.ToTable("postulaciones");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
                entity.Property(e => e.OfertaId).HasColumnName("oferta_id");
                entity.Property(e => e.FechaPostulacion).HasColumnName("fecha_postulacion");
                entity.HasIndex(e => new { e.UsuarioId, e.OfertaId }).IsUnique();
            });
        }
    }
}