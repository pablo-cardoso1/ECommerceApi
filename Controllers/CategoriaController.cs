using Microsoft.AspNetCore.Mvc;
using ECommerceApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public CategoriasController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            try
            {
                var categorias = _context.Categorias.ToList();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                // Logar a exceção (se você estiver usando um logger)
                return StatusCode(500, "Erro ao obter categorias: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            try
            {
                var categoria = _context.Categorias.Find(id);
                if (categoria == null)
                    return NotFound();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter a categoria: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarCategoria([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Add(categoria);
                _context.SaveChanges();
                return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, categoria);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao criar categoria: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao criar categoria: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            try
            {
                var categoriaBanco = _context.Categorias.Find(id);
                if (categoriaBanco == null)
                    return NotFound();

                categoriaBanco.Nome = categoria.Nome;

                _context.Categorias.Update(categoriaBanco);
                _context.SaveChanges();
                return Ok(categoriaBanco);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar categoria: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarCategoria(int id)
        {
            try
            {
                var categoriaBanco = _context.Categorias.Find(id);
                if (categoriaBanco == null)
                    return NotFound();

                _context.Categorias.Remove(categoriaBanco);
                _context.SaveChanges();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao deletar categoria: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar categoria: " + ex.Message);
            }
        }
    }
}
