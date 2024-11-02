using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Models;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItensPedidosController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public ItensPedidosController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: api/itempedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemPedido>>> GetItemPedidos()
        {
            try
            {
                var itensPedidos = await _context.ItensPedido.ToListAsync();
                return Ok(itensPedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter itens de pedido: " + ex.Message);
            }
        }

        // GET: api/itempedido/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPedido>> GetItemPedido(int id)
        {
            try
            {
                var itemPedido = await _context.ItensPedido.FindAsync(id);

                if (itemPedido == null)
                {
                    return NotFound();
                }

                return Ok(itemPedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter item de pedido: " + ex.Message);
            }
        }

        // POST: api/itempedido
        [HttpPost]
        public async Task<ActionResult<ItemPedido>> PostItemPedido(ItemPedido itemPedido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se o pedido existe
            var pedidoExistente = await _context.Pedidos.FindAsync(itemPedido.PedidoId);
            if (pedidoExistente == null)
            {
                return NotFound($"Pedido com ID {itemPedido.PedidoId} não encontrado.");
            }

            try
            {
                _context.ItensPedido.Add(itemPedido);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetItemPedido), new { id = itemPedido.Id }, itemPedido);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao criar item de pedido: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao criar item de pedido: " + ex.Message);
            }
        }

        // PUT: api/itempedido/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemPedido(int id, ItemPedido itemPedido)
        {
            if (id != itemPedido.Id)
            {
                return BadRequest();
            }

            // Verifica se o pedido existe
            var pedidoExistente = await _context.Pedidos.FindAsync(itemPedido.PedidoId);
            if (pedidoExistente == null)
            {
                return NotFound($"Pedido com ID {itemPedido.PedidoId} não encontrado.");
            }

            _context.Entry(itemPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItensPedidoExists(id))
                {
                    return NotFound();
                }
                return StatusCode(500, "Erro ao atualizar item de pedido.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar item de pedido: " + ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/itempedido/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemPedido(int id)
        {
            try
            {
                var itemPedido = await _context.ItensPedido.FindAsync(id);
                if (itemPedido == null)
                {
                    return NotFound();
                }

                _context.ItensPedido.Remove(itemPedido);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao deletar item de pedido: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar item de pedido: " + ex.Message);
            }
        }

        private bool ItensPedidoExists(int id)
        {
            return _context.ItensPedido.Any(e => e.Id == id);
        }
    }
}
