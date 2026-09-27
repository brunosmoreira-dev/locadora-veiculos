using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrabalhoC_.Models
{
    public class Veiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        public string Modelo { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Ano de fabricação inválido.")]
        public int AnoFabricacao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A quilometragem não pode ser negativa.")]
        public decimal Quilometragem { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe um fabricante válido.")]
        public int FabricanteId { get; set; }
        [JsonIgnore]
        public Fabricante? Fabricante { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe uma categoria válida.")]
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        [JsonIgnore]
        public List<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    }
}
