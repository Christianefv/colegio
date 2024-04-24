using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class MateriaMaestro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? IdMateriaMaestro { get; set; }
        public int? IdMaestro { get; set; }
        public int? IdMateria { get; set; }
        [ForeignKey("IdMaestro")]
        public virtual Maestro? Maestro { get; set; }
        [ForeignKey("IdMateria")]
        public virtual Materia? Materia { get; set; }
    }
}
