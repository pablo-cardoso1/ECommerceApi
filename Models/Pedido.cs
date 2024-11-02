using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Relacionamento com o usuário

        public DateTime Data { get; set; }

        [Required]
        public StatusPedido Status { get; set; }
        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        public void AdicionarItem(ItemPedido item)
        {
            Itens.Add(item);
        }

        public void RemoverItem(int itemId)
        {
            var item = Itens.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                Itens.Remove(item);
            }
        }

        public override string ToString()
        {
            return $"Pedido {Id}: {Status} em {Data}, Total de Itens: {Itens.Count}";
        }
    }

    public enum StatusPedido
    {
        Pendente,
        Concluído,
        Enviado,
        Cancelado
    }
}
