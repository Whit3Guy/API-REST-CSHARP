using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCrudDotNET.Estudantes;
using Microsoft.EntityFrameworkCore;

namespace ApiCrudDotNET.database
{
    public class AppDbContext : DbContext
    {
        // Tabela de banco de dados
        public DbSet<Estudante> Estudantes { get; set;}

        //definição do contexto de AddEs
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}