namespace TrabalhoC_.Models
{
    public class Aluguel
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataDevolucao { get; set; }
        public decimal QuilometragemInicial { get; set; }
        public decimal QuilometragemFinal { get; set; }
        public decimal ValorDiaria { get; set; }
        public decimal ValorTotal { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; } = null!;
    }
}
