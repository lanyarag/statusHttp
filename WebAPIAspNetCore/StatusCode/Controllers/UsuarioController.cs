using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatusCode.Models;

namespace StatusCode.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private SistemaContext DbSistema = new SistemaContext();

        [HttpGet]

        public ActionResult<List<Usuario>> RequererTodos()
        {
            return Ok();
        }

        [HttpGet("{Id}")]
        public ActionResult<Usuario> RequererUmPelaId(int Id)
        {
            Usuario usuario = DbSistema.Usuario.Find(Id);

            if (usuario == null)
            {
                return NotFound();
            }
            else { return Ok(usuario); }
        }

        [HttpPost]
        public ActionResult<Usuario> PublicarUm(Usuario Usuario)
        {
            //Usuario usuario = DbSistema.Usuario.Find(Usuario.Cpf);

            if (string.IsNullOrEmpty(Usuario.Cpf))
            {
                return Conflict();
            }
            else
            {
                return Created(Usuario.Cpf, Usuario);
            }
        }

        [HttpDelete("{Id}")]
        public ActionResult<Usuario> DeletarUmPelaId(int Id, Usuario Usuario)
        {
            if (Id == Usuario.Id)
            {
                return Unauthorized();
            }
            else { return NotFound(); }
        }

        [HttpPut("{Id}")]
        public ActionResult<Usuario> SubstituirUmPelaId(int Id, Usuario Usuario)
        {
            if (Id == Usuario.Id)
            {
                return Ok();
            }
            else if (Id == null)
            {
                return NotFound();
            }
            else if (Id != Usuario.Id)
            {
                return BadRequest();
            }
            else if (Usuario.Cpf == Usuario.Cpf)
            {
                return Conflict();
            }
            else { return BadRequest(); }
        }
    }
}
