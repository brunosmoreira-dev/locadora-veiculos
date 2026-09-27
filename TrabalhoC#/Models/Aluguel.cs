using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrabalhoC_.Models
{
    public class Aluguel
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataDevolucao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A quilometragem inicial não pode ser negativa.")]
        public decimal QuilometragemInicial { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A quilometragem final não pode ser negativa.")]
        public decimal QuilometragemFinal { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da diária deve ser maior que zero.")]
        public decimal ValorDiaria { get; set; }
        public decimal ValorTotal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe um cliente válido.")]
        public int ClienteId { get; set; }
        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Informe um veículo válido.")]
        public int VeiculoId { get; set; }
        [JsonIgnore]
        public Veiculo? Veiculo { get; set; }
    }
}
