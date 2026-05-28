using AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aluno>().Property(a => a.Nome).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Professor>().Property(p => p.Nome).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Disciplina>().Property(d => d.Nome).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Usuario>().Property(u => u.Email).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Usuario>().Property(u => u.SenhaHash).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Nota>()
                .HasOne(n => n.Aluno)
                .WithMany(a => a.Notas)
                .HasForeignKey(n => n.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Nota>()
                .HasOne(n => n.Disciplina)
                .WithMany(d => d.Notas)
                .HasForeignKey(n => n.DisciplinaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
