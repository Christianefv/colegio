using DB;
using DB.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Colegio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        public readonly string? con;
        private ResultDto _result;
        public GruposController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("Conexion");
            _result = new ResultDto();
        }

        [HttpGet]
        public ResultDto GetGrupos()
        {
            List<Grupo> grupos = new();
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcGruposCon", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Grupo p = new Grupo
                                {
                                    IdGrupo = Convert.ToInt32(reader["IdGrupo"]),
                                    NombreGrupo = reader["NombreGrupo"].ToString()
                                };

                                grupos.Add(p);
                            }

                            _result.Data = grupos;

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
        public ResultDto PostGrupo([FromBody] Grupo p)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcGruposGuarda", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pNombreGrupo", p.NombreGrupo);
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
        public ResultDto PutGrupo(int id, [FromBody] Grupo p)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcGruposModifica", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pIdGrupo", id);
                        cmd.Parameters.AddWithValue("@pNombreGrupo", p.NombreGrupo);
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
        public ResultDto DeleteGrupo(int id)
        {
            try
            {
                using (SqlConnection connection = new(con))
                {
                    connection.Open();
                    using (SqlCommand cmd = new("ProcGruposEliminar", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pIdGrupo", id);
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
