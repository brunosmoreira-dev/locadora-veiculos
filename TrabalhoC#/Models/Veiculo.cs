namespace TrabalhoC_.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public int AnoFabricacao { get; set; }
        public decimal Quilometragem { get; set; }

        public int FabricanteId { get; set; }
        public Fabricante Fabricante { get; set; } = null!;

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public List<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    }
}
