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
    public class PedidoController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public PedidoController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var pedidos = await _context.Pedidos.ToListAsync();
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter pedidos: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido == null)
                    return NotFound();

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter pedido: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao criar pedido: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao criar pedido: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPedido(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            try
            {
                var pedidoBanco = await _context.Pedidos.FindAsync(id);
                if (pedidoBanco == null)
                {
                    return NotFound();
                }

                // Atualiza os itens do pedido
                pedidoBanco.Itens = pedido.Itens;

                _context.Pedidos.Update(pedidoBanco);
                await _context.SaveChangesAsync();
                return Ok(pedidoBanco);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar pedido: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null)
                {
                    return NotFound();
                }

                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao deletar pedido: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar pedido: " + ex.Message);
            }
        }
    }
}
