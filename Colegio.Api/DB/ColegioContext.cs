using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class ColegioContext : DbContext
    {
        public ColegioContext( DbContextOptions<ColegioContext> options ) : base( options ) 
        { 

        }

        public DbSet<Maestro> Maestros { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<MateriaMaestro> MateriaMaestros { get; set; }


    }
}
