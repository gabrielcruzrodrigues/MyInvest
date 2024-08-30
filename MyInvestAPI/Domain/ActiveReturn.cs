namespace MyInvestAPI.Domain
{
    public class ActiveReturn
    {
        public DateOnly Data { get; set; }
        public string? Ativo { get; set; }
        public string? NomeDoAtivo { get; set; }
        public TypeEnum Tipo { get; set; }
        public double DividentYield { get; set; }
        public decimal PrecoAtual { get; set; }
        public double P_VP { get; set; }
        public decimal Preco_Teto { get; set; }
        public bool Indicacao { get; set; }
        public double P_L { get; set; }
        public double ROE { get; set; }
        public double CrecimentoDeDividendos { get; set; }
    }
}
