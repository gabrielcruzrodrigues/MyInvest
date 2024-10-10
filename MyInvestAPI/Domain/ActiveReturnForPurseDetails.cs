namespace MyInvestAPI.Domain
{
    public class ActiveReturnForPurseDetails
    {
        public int Id {get; set; }
        public string? Ativo { get; set; }
        public string? Tipo { get; set; }
        public string? DividentYield { get; set; }
        public string? PrecoAtual { get; set; }
        public string? Proventos_pagos { get; set; }
        public string? Preco_Teto { get; set; }
        public string? Indicacao { get; set; }
    }
}
