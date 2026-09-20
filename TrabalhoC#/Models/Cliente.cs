namespace TrabalhoC_.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    }
}
