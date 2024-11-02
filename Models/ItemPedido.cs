using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } // Relacionamento com o pedido

        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } // Relacionamento com o produto

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantidade { get; set; }

        public override string ToString()
        {
            return $"ItemPedido {Id}: Produto {ProdutoId}, Quantidade: {Quantidade}";
        }
    }
}
