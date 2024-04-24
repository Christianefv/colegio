using DB.DTO;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasMaestroController : ControllerBase
    {
        private ColegioContext _context;
        private ResultDto _result;
        public MateriasMaestroController(ColegioContext context)
        {
            _context = context;
            _result = new ResultDto();
        }

        [HttpPost]
        public ResultDto AddMateriaMaestro([FromBody] MateriaMaestro materiaMaestro)
        {
            try
            {
                //var maestro = _context.Maestros.FirstOrDefault(m => m.IdMaestro == materiaMaestro.idMaestro);
                //var materia = _context.Materias.FirstOrDefault(m => m.IdMateria == materiaMaestro.idMateria);

                //if (maestro == null)
                //{
                //    _result.value = false;
                //    _result.Message = "No se encontró el maestro seleccionado";
                //}

                //if (materia == null)
                //{
                //    _result.value = false;
                //    _result.Message = "No se encontró la materia seleccionada";
                //}

                //var materiaMaestro = new MateriaMaestro
                //{
                //    IdMaestro = idMaestro,
                //    IdMateria = idMateria
                //};

                _context.MateriaMaestros.Add(materiaMaestro);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpPut("{id}")]
        public ResultDto UpdateMateriaMaestro(int id, MateriaMaestro materiaMaestro)
        {
            try
            {

                var materiaMaestros = _context.MateriaMaestros.Find(id);

                if (materiaMaestros == null)
                {
                    _result.value = false;
                    _result.Message = ($"No se encontró la relación MateriaMaestro con ID {id}.");

                    return _result;
                }

                // Verificar si existen el maestro y la materia en la base de datos
                var maestro = _context.Maestros.FirstOrDefault(m => m.IdMaestro == materiaMaestro.IdMaestro);
                var materia = _context.Materias.FirstOrDefault(m => m.IdMateria == materiaMaestro.IdMateria);

                if (maestro == null)
                {
                    _result.value = false;
                    _result.Message = ($"No se encontró el maestro con ID {materiaMaestro.IdMaestro}.");
                    return _result;
                }

                if (materia == null)
                {
                    _result.value = false;
                    _result.Message = ($"No se encontró la materia con ID {materiaMaestro.IdMateria}.");
                    return _result;
                }

                // Actualizar la relación MateriaMaestro con los nuevos IDs
                materiaMaestro.IdMaestro = materiaMaestro.IdMaestro;
                materiaMaestro.IdMateria = materiaMaestro.IdMateria;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpDelete("{idMateria}/{idMaestro}")]
        public ResultDto DeleteMateriaMaestro(int idMateria, int idMaestro)
        {
            try
            {
                var materiaMaestro = _context.MateriaMaestros.FirstOrDefault(mm => mm.IdMateria == idMateria && mm.IdMaestro == idMaestro);

                if (materiaMaestro == null)
                {
                    _result.value = false;
                    _result.Message = $"No se encontró la relación MateriaMaestro con ID de Materia {idMateria} y ID de Maestro {idMaestro}.";
                    return _result;
                }

                _context.MateriaMaestros.Remove(materiaMaestro);
                _context.SaveChanges();

                _result.Message = ("Relación Materia-Maestro eliminada exitosamente.");
                
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpGet("materias-no-asignadas/{idMaestro}")]
        public ResultDto GetMateriasMaestro(int idMaestro)
        {
            try
            {
                var todasLasMaterias = _context.Materias.ToList();

                var materiasAsignadas = _context.MateriaMaestros
                    .Where(mm => mm.IdMaestro == idMaestro)
                    .Select(mm => mm.Materia)
                    .ToList();


                var materiasNoAsignadas = todasLasMaterias.Except(materiasAsignadas).ToList();

                if (materiasNoAsignadas != null && materiasNoAsignadas.Count > 0)
                {
                    _result.value = true;
                    _result.Data = materiasNoAsignadas;
                }
                else
                {
                    _result.value = false;
                    _result.Message = "No se encontraron materias no asignadas al maestro seleccionado.";
                }
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpGet("materias-asignadas/{idMaestro}")]
        public ResultDto GetMateriasNoAsignadasMaestro(int idMaestro)
        {
            try
            {
                var materiasAsignadas = _context.MateriaMaestros
                    .Where(mm => mm.IdMaestro == idMaestro)
                    .Select(mm => mm.Materia)
                    .ToList();

                if (materiasAsignadas != null && materiasAsignadas.Count > 0)
                {
                    _result.value = true;
                    _result.Data = materiasAsignadas;
                }
                else
                {
                    _result.Message = "No se encontraron materias asignadas al maestro seleccionado.";
                }
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }
    }
}
