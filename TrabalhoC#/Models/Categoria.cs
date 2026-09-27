using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrabalhoC_.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}
