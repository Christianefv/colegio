using DB;
using DB.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroController : ControllerBase
    {
        private ColegioContext _context;
        private ResultDto _result;
        public MaestroController(ColegioContext context)
        {
            _context = context;
            _result = new ResultDto();
        }

        [HttpGet]
        public ResultDto GetMaestros()
        {
            try
            {
                IEnumerable<Maestro> maestros = _context.Maestros.ToList();
                _result.Data = maestros;
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpGet("{id}")]
        public ResultDto GetMaestro(int id)
        {
            try
            {
                if (_context.Maestros.Any(e => e.IdMaestro == id))
                {
                    _result.value = true;
                    _result.Data = _context.Maestros.Where(e => e.IdMaestro == id);
                }
                else
                {
                    _result.value = false;
                    _result.Message = "No se encontró registro del maestro.";
                }
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
        }

        [HttpPost]
        public ResultDto PostMaestro(Maestro maestro)
        {
            var result = new ResultDto();

            try
            {
                _context.Maestros.Add(maestro);
                _context.SaveChanges();

                result.Message = "Se agregó el maestro correctamente";
            }
            catch (Exception ex)
            {
                result.value = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpPut("{id}")]
        public ResultDto PutMaestro(int id,[FromBody] Maestro maestro)
        {
            var result = new ResultDto();

            try
            {
                var maestroExistente = _context.Maestros.Find(id);
                if (maestroExistente == null)
                {
                    result.value = false;
                    result.Message = "Maestro no encontrado";
                    return result;
                }

                maestroExistente.Nombre = maestro.Nombre;
                maestroExistente.ApellidoPaterno = maestro.ApellidoPaterno;
                maestroExistente.ApellidoMaterno = maestro.ApellidoMaterno;
                maestroExistente.Sexo = maestro.Sexo;
                _context.Maestros.Update(maestroExistente);
                _context.SaveChanges();

                result.Message = "Se actualizó el maestro correctamente";
            }
            catch (Exception ex)
            {
                result.value = false;
                result.Message = ex.Message;
            }

            return result;
        }

        [HttpDelete("{id}")]
        public ResultDto DeleteMaestro(int id)
        {
            var result = new ResultDto();

            try
            {
                var maestro = _context.Maestros.Find(id);

                if (maestro == null)
                {
                    result.value = false;
                    result.Message = "No se encontró el maestro que se desea eliminar";
                    return result;
                }

                _context.Maestros.Remove(maestro);
                _context.SaveChanges();

                result.Message = "Se eliminó el maestro correctamente";
            }
            catch (Exception ex)
            {
                result.value = false;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
