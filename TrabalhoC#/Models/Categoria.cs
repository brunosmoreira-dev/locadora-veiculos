namespace TrabalhoC_.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}
