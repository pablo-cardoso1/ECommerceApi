using System.ComponentModel.DataAnnotations;
using ECommerceApi.Models;

public class Produto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    public string Descricao { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }
    public string Imagem { get; set; }
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; } // Deixe como nullable
}
