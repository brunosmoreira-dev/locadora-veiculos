using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrabalhoC_.Models
{
    public class Fabricante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}
