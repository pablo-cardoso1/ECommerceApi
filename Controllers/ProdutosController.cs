using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ECommerceContext _context;

        public ProdutosController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var produtos = await _context.Produtos.ToListAsync();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter produtos: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto == null)
                    return NotFound();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter o produto: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto([FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a categoria existe no banco
            var categoriaExistente = await _context.Categorias.FindAsync(produto.CategoriaId);
            if (categoriaExistente == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            try
            {
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao criar produto: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao criar produto: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            try
            {
                var produtoBanco = await _context.Produtos.FindAsync(id);
                if (produtoBanco == null)
                    return NotFound();

                // Atualização das propriedades
                produtoBanco.Nome = produto.Nome;
                produtoBanco.Descricao = produto.Descricao;
                produtoBanco.Preco = produto.Preco;
                produtoBanco.Imagem = produto.Imagem;
                produtoBanco.CategoriaId = produto.CategoriaId;

                _context.Produtos.Update(produtoBanco);
                await _context.SaveChangesAsync();
                return Ok(produtoBanco);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao atualizar produto: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            try
            {
                var produtoBanco = await _context.Produtos.FindAsync(id);
                if (produtoBanco == null)
                    return NotFound();

                _context.Produtos.Remove(produtoBanco);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao deletar produto: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao deletar produto: " + ex.Message);
            }
        }
    }
}
