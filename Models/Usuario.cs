using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public string Tipo { get; set; } // Cliente ou Administrador

        // Método para gerar o hash da senha
        public void SetSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            SenhaHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        // Método para validar a senha
        public bool ValidarSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash == SenhaHash;
        }
    }
}
