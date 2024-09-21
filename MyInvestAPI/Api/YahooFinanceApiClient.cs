using MyInvestAPI.Domain;
using YahooFinanceApi;
using MyInvestAPI.Extensions;

namespace MyInvestAPI.Api;

public class YahooFinanceApiClient
{
    public async static Task<ActiveReturn> GetActive(string active, string dYDesiredPercentage)
    {
        var search = await Yahoo.Symbols(active).Fields(
            Field.DividendDate,                 // data
            Field.Symbol,                       // ativo
            Field.LongName,                     // nome do ativo
            Field.QuoteType,                    // tipo do ativo
            Field.TrailingAnnualDividendYield,  // divident yield
            Field.RegularMarketPrice,           // preço atual
            Field.PriceToBook,                  // P/VPs
            Field.TrailingPE                    // P/L
                                                // Roe
        ).QueryAsync();

        if (search is null)
            throw new KeyNotFoundException();

        var result = search[$"{active}"];

        return await CreateActiveReturn(result, dYDesiredPercentage);
    }

    static async Task<ActiveReturn> CreateActiveReturn(Security result, string dYDesiredPercentage)
    {
        if (!decimal.TryParse(dYDesiredPercentage, out decimal dyDesired))
            throw new ArgumentException("Porcentagem inválida.");

        decimal dYCurrent = result[Field.TrailingAnnualDividendYield] != null ? Convert.ToDecimal(result[Field.TrailingAnnualDividendYield]) : 0;
        decimal currentPrice = result[Field.RegularMarketPrice] != null ? Convert.ToDecimal(result[Field.RegularMarketPrice]) : 0;

        decimal tetoPrice = CalculatePriceTeto(dYCurrent, currentPrice, dyDesired);
        string recomendation = Recomendation(currentPrice, tetoPrice);

        DateTime currentDate = DateTime.Now;

        ActiveReturn activeReturn = new();
        activeReturn.Data = currentDate.ToString("dd-MM-yyyy");
        activeReturn.Ativo = result.Symbol;
        activeReturn.NomeDoAtivo = result.LongName;
        activeReturn.Tipo = VerifyType(result.QuoteType);
        activeReturn.DividentYield = (dyDesired).ToString() + "%";
        activeReturn.P_VP = (result.PriceToBook).ToString("F1");
        activeReturn.PrecoAtual = $"R$ {result.RegularMarketPrice.ToString("F2")}";
        activeReturn.Preco_Teto = $"R$ {tetoPrice.ToString("F2")}";
        activeReturn.Indicacao = recomendation;
        activeReturn.P_L = (result.TrailingPE).ToString("F1");
        activeReturn.ROE = "Indisponível";
        activeReturn.Crecimento_De_Dividendos_5_anos = await CalculateDividendGrowth(result.Symbol);
        activeReturn.Proventos_pagos = $"{await CalculateProventosPagos(result.Symbol)}";

        return activeReturn;
    }

    //static async Task<string> CalculateRoe(string symbol)
    //{
    //    var stock = await Yahoo.GetStockAsync(ticker);
    //}

    static async Task<string> CalculateDividendGrowth(string symbol)
    {
        DateTime startDate = DateTime.Now.AddYears(-5);
        DateTime endDate = DateTime.Now;
        IEnumerable<DividendTick> history;

        try
        {
            history = await Yahoo.GetDividendsAsync(symbol, startDate, endDate);
        }
        catch (Exception)
        {
            return "Indisponível";
        }

        if (history == null || !history.Any())
        {
            return "Indisponível";
        }

        var dividends = history.Where(x => x.Dividend != null && x.Dividend > 0)
                               .Select(c => new
                               {
                                   c.DateTime,
                                   c.Dividend
                               })
                               .ToList();

        if (dividends.Count == 0)
        {
            return "Indisponível";
        }

        var dividendsPerYear = dividends.GroupBy(d => d.DateTime.Year)
                                        .Select(g => new
                                        {
                                            Year = g.Key,
                                            DividendsTotal = g.Sum(d => d.Dividend)
                                        })
                                        .ToList();

        double averageDividends = (double)dividendsPerYear.Average(d => d.DividendsTotal);

        return $"{(averageDividends * 100).ToString("0.##") + "%"} por ano.";
    }

    static async Task<string> CalculateProventosPagos(string ticker)
    {
        if (ticker is null)
        {
            throw new HttpResponseException(400, "O ticker não pode ser nulo!");
        }

        //Pega o último dia do ano passado, e a data de 5 anos atras referente a essa data, ignorando o ano atual
        DateTime lastDateLastYear = new DateTime(DateTime.Now.Year - 1, 12, 31);
        DateTime fiveYearsAgoDate = lastDateLastYear.AddDays(-5);

        try
        {
            var history = await Yahoo.GetDividendsAsync(ticker, new DateTime(lastDateLastYear.Year, lastDateLastYear.Month, lastDateLastYear.Day),
                                                                new DateTime(fiveYearsAgoDate.Year, fiveYearsAgoDate.Month, fiveYearsAgoDate.Day));

            if (history is null || history.Count() <= 0)
            {
                return "Dados indisponíveis";
            }

            decimal percentageForCalculate = 5 / 100;

            if (history.Count() < 5 && history.Count() >= 3)
            {
                percentageForCalculate = 3 / 100;
            }
            else
            {
                //Levando em consideração apenas dados maiores que 3 anos, se forem menos que 5, calculamos com base em 3 anos
                return "Dados indisponíveis";
            }

            decimal dividendAverage = 0;

            foreach (var candle in history)
            {
                dividendAverage += candle.Dividend;
            }

            dividendAverage /= percentageForCalculate;

            return $"{dividendAverage}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Houve um erro ao tentar buscar o historico de um dividendo. - ex: {ex.Message}");
            return "Dados indisponíveis";
        }
    }

    static decimal CalculatePriceTeto(decimal dYCurrent, decimal currentPrice, decimal dYDesiredPercentage)
    {
        dYDesiredPercentage /= 100;

        if (dYCurrent <= 0 || currentPrice <= 0)
            throw new InvalidOperationException("Data of Dividend Yield or Active price are invalid!");

        decimal AnnualDividends = dYCurrent * currentPrice;

        decimal priceTeto = AnnualDividends / dYDesiredPercentage;

        return priceTeto;
    }

    static string Recomendation(decimal currentPrice, decimal tetoPrice)
    {
        return tetoPrice < currentPrice ? "🟢 Comprar" : "🔴 Não-comprar";
    }

    static string VerifyType(string type)
    {
        if (type == "EQUITY")
        {
            return "Ação";
        }
        else if (type == "ETF" || type == "REIT")
        {
            return "FII";
        }
        return "Tipo não expecificado";
    }
}
