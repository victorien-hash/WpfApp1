using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.Core.Entites;

namespace VisionnementFilm.Infrastructure
{
    public class VisionnementFilmDbContext: DbContext
    {

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Film> Films { get; set; }
        public VisionnementFilmDbContext(DbContextOptions options) :
                            base(options)
        { }

        public VisionnementFilmDbContext() : base(new
               DbContextOptionsBuilder<VisionnementFilmDbContext>()
               .UseSqlServer(@"Server=.;Database=SolutionGestionClientsDB;Trusted_Connection=True;")
               .Options)
        { }
    }
}
