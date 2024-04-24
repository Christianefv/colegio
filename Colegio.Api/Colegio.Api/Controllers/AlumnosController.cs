using DB;
using DB.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Colegio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        public readonly string? con;
        private ResultDto _result;
        public AlumnosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("Conexion");
            _result = new ResultDto();
        }

        [HttpGet]
        public ResultDto GetAlumnos()
        {
            List<Alumnos> alumnos = new();
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcAlumnosCon", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Alumnos p = new Alumnos
                                {
                                    IdAlumno = Convert.ToInt32(reader["IdAlumno"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                                    ApellidoMaterno = reader["ApellidoMaterno"].ToString(),
                                    FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                    Sexo = reader["Sexo"].ToString(),
                                };

                                alumnos.Add(p);
                            }

                            _result.Data = alumnos;

                        }
                    }
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
        public ResultDto PostAlumno([FromBody] Alumnos p)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcAlumnosGuarda", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pNombre", p.Nombre);
                        cmd.Parameters.AddWithValue("@pApellidoPaterno", p.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@pApellidoMaterno", p.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@pFechaNacimiento", p.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@pSexo", p.Sexo);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;
            
        }

        [HttpPut("{id}")]
        public ResultDto PutAlumno([FromBody] Alumnos p, int id)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcAlumnosModificar", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pIdAlumno", id);
                        cmd.Parameters.AddWithValue("@pNombre", p.Nombre);
                        cmd.Parameters.AddWithValue("@pApellidoPaterno", p.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@pApellidoMaterno", p.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@pFechaNacimiento", p.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@pSexo", p.Sexo);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _result.value = false;
                _result.Message = ex.Message;
            }

            return _result;

        }

        [HttpDelete("{id}")]
        public ResultDto DeleteAlumno(int id)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcAlumnosEliminar", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pIdAlumno", id);
                        cmd.ExecuteNonQuery();
                    }
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
