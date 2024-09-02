using Flurl.Util;
using MyInvestAPI.Domain;
using System.Runtime.CompilerServices;
using YahooFinanceApi;

namespace MyInvestAPI.Api;

public class YahooFinanceApiClient
{
    public async static Task<ActiveReturn> GetActive(string active)
    {
        var securitie = await Yahoo.Symbols(active).Fields(
                Field.DividendDate, //data
                Field.Symbol, //ativo
                Field.LongName, //nome do ativo
                Field.QuoteType, //tipo do ativo
                Field.TrailingAnnualDividendYield, //divident yield
                Field.RegularMarketPrice, //preço atual
                Field.PriceToBook, // P/VP
                Field.TrailingPE  // P/L
                                  // Roe
                                  //Crecimento de dividendos
            ).QueryAsync();

        var result = securitie[$"{active}"];

        return CreateActiveReturn(result);
    }

    static ActiveReturn CreateActiveReturn(Security result)
    {
        decimal tetoPrice = CalculatePriceTeto((decimal)result.TrailingAnnualDividendYield, (decimal)result.RegularMarketPrice);
        string recomendation = Recomendation((decimal)result.RegularMarketPrice, tetoPrice);

        DateTime currentDate = DateTime.Now;

        ActiveReturn activeReturn = new();
        activeReturn.Data = currentDate.ToString("dd-MM-yyyy");
        activeReturn.Ativo = result.Symbol;
        activeReturn.NomeDoAtivo = result.LongName;
        activeReturn.Tipo = result.QuoteType;
        activeReturn.DividentYield = (result.TrailingAnnualDividendYield * 100).ToString("F2") + "%";
        activeReturn.PrecoAtual = $"R$ {result.RegularMarketPrice.ToString("F2")}";
        activeReturn.P_VP = (result.PriceToBook).ToString("F1");
        activeReturn.Preco_Teto = $"R$ {tetoPrice.ToString("F2")}";
        activeReturn.Indicacao = recomendation;
        activeReturn.P_L = (result.TrailingPE).ToString("F1");

        return activeReturn;
    }

    static decimal CalculatePriceTeto(decimal dividendYield, decimal RegularMarketPrice
        )
    {
        decimal desiredReturnRate = 0.06M;

        //calculates the average dividend per share
        decimal dividendPerShare = RegularMarketPrice * dividendYield;

        return dividendPerShare / desiredReturnRate;
    }

    static string Recomendation(decimal regularMarketPrice, decimal tetoPrice)
    {
        return regularMarketPrice <= tetoPrice ? "🟢 Comprar" : "🔴 Não-comprar";
    }
}
