using DB.DTO;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private ColegioContext _context;
        private ResultDto _result;
        public MateriaController(ColegioContext context)
        {
            _context = context;
            _result = new ResultDto();
        }

        [HttpPost]
        public ResultDto AddMateria([FromBody] Materia materia)
        {
            try
            {
                _context.Materias.Add(materia);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpGet]
        public ResultDto GetMaterias()
        {
            try
            {
                IEnumerable<Materia> materias = _context.Materias.ToList();
                _result.Data = materias;
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpPut("{id}")]
        public ResultDto PutMateria(int id, [FromBody] Materia materia)
        {
            try
            {
                var materiaContext = _context.Materias.Find(id);

                if (materiaContext == null)
                {
                    _result.value = false;
                    _result.Message = "No se encontró la materia que se desea modificar";
                    return _result;
                }

                materiaContext.Descripcion = materia.Descripcion;
                _context.SaveChanges();
                _result.Message = "Se modificó el registro correctamente";
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpDelete("{id}")]
        public ResultDto DeleteMateria(int id)
        {

            try
            {
                var materia = _context.Materias.Find(id);

                if (materia == null)
                {
                    _result.value = false;
                    _result.Message = "No se encontró la materia que se desea eliminar";
                    return _result;
                }

                _context.Materias.Remove(materia);
                _context.SaveChanges();

                _result.Message = "Se eliminó la materia correctamente";
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
