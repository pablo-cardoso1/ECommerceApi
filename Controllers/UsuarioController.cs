using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public UsuarioController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterUsuarios()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter usuários: " + ex.Message);
            }
        }

        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObterUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                return usuario;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter usuário: " + ex.Message);
            }
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> CriarUsuario(Usuario usuario, string senha)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            usuario.SetSenha(senha);

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuario.Id }, usuario);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao criar usuário: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao criar usuário: " + ex.Message);
            }
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, Usuario usuario, string senha = null)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            try
            {
                var usuarioBanco = await _context.Usuarios.FindAsync(id);
                if (usuarioBanco == null)
                {
                    return NotFound();
                }

                usuarioBanco.Nome = usuario.Nome;
                usuarioBanco.Email = usuario.Email;

                // Atualiza a senha apenas se uma nova senha for fornecida
                if (!string.IsNullOrEmpty(senha))
                {
                    usuarioBanco.SetSenha(senha);
                }

                usuarioBanco.Tipo = usuario.Tipo;

                _context.Usuarios.Update(usuarioBanco);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar usuário: " + ex.Message);
            }
        }

        // DELETE: api/usuario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao deletar usuário: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar usuário: " + ex.Message);
            }
        }
    }
}
