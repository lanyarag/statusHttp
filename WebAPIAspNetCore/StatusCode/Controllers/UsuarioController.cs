using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var Usuarios = DbSistema.Usuario?.ToList();
            return Ok(Usuarios);
        }

        [HttpGet("{Id}")]
        public ActionResult<Usuario> RequererUmPelaId(int Id)
        {
            //Procurar o usuário pelo Id
            Usuario usuario = DbSistema.Usuario.Find(Id);

            //Se o user for null, retorna not found
            if (usuario == null)
            {
                return NotFound();
            }
            //Se encontrar o usuário, força o status 200 e retorna a instância usuario
            else { return Ok(usuario); }
        }

        [HttpPost]
        public ActionResult PublicarUm(Usuario Usuario)
        {

            if (!ExisteCpf(Usuario.Cpf))
            {
                return Conflict();
            }
            else
            {
                DbSistema.Usuario.Add(Usuario);
                DbSistema.SaveChanges();
                return CreatedAtAction("Publicado um usuario",new { id = Usuario.Id });
            }
        }

        [HttpDelete("{Id}")]
        public ActionResult DeletarUmPelaId(int Id, Usuario Usuario)
        {
            if (!ExisteUser(Id))
            {
                return NotFound();
            }
            else 
            { 
                return Unauthorized(); 
            }
        }

        [HttpPut("{Id}")]
        public ActionResult<Usuario> SubstituirUmPelaId(int Id, Usuario Usuario)
        {
            //Se o Id da instância do usuário for diferente do Id do usuario pesquisado, retorna BadRequest
            if (Id != Usuario.Id)
            {
                return BadRequest();
            }

            //Substitui o valor da instância no banco de dados

            DbSistema.Entry(Usuario).State = EntityState.Modified;

            try
            {
                //Salvar alteração no Banco de dados
                DbSistema.SaveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                //Caso não consiga salvar, verifico se o usuario existe chamando o método criado
                if (ExisteUser(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                return 
            }
        }

        private bool ExisteCpf(string Cpf)
        {
            return DbSistema.Usuario.Any(Coluna => Coluna.Cpf == Cpf);
        }

        private bool ExisteUser(int Id)
        {
            return DbSistema.Usuario.Any(Coluna => Coluna.Id == Id);
        }

    }
}
