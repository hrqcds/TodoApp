using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class AppContextDb : DbContext
    {
        public AppContextDb(DbContextOptions<AppContextDb> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Dizendo que chave e.Id é a chave primária da minha migration
            modelBuilder.Entity<Todo>().HasKey(t => t.Id);

            // Modelando a coluna do tipo todo com valor obrigatório e com 100 caracteres
            modelBuilder.Entity<Todo>()
                .Property(t => t.Title)
                .IsRequired()
                .HasColumnType("varchar(100)");

            modelBuilder.Entity<Todo>()
                .Property(t => t.Value)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // Setando as mudanças para a tabela com o nome fornecido
            modelBuilder.Entity<Todo>()
                .ToTable("Todos");

            // Executando as mudanças selecionadas
            base.OnModelCreating(modelBuilder);
        }
    }
}
