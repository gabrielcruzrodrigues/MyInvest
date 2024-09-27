using MyInvestAPI.Domain;
using YahooFinanceApi;
using MyInvestAPI.Extensions;
using Newtonsoft.Json.Linq;

namespace MyInvestAPI.Api;

public class YahooFinanceApiClient
{
    public async static Task<Security> GetActive(string active, string dYDesiredPercentage)
    {
        if (string.IsNullOrEmpty(active))
        {
            throw new HttpResponseException(500, "O ativo para a pesquisa não pode ser nulo ou vazio");
        }

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

        return search[$"{active}"];
    }

    public static async Task<ActiveReturnForPurseDetails> CreateActiveReturnForPurseDetails(string ticker, string dYDesiredPercentage, int activeId)
    {
        Security result = await GetActive(ticker, dYDesiredPercentage);

        if (!decimal.TryParse(dYDesiredPercentage, out decimal dyDesired))
            throw new ArgumentException("Porcentagem inválida.");

        decimal dYCurrent = result[Field.TrailingAnnualDividendYield] != null ? Convert.ToDecimal(result[Field.TrailingAnnualDividendYield]) : 0;
        decimal currentPrice = result[Field.RegularMarketPrice] != null ? Convert.ToDecimal(result[Field.RegularMarketPrice]) : 0;

        //obtendo os dividendos e calculando o total e a media dos proventos pagos
        JToken dividendosDaAcao = await ObterDividendosAte5Anos(result.Symbol);
        string mediaDosProventosPagos = CalculateMediaDosProventosPagos(dividendosDaAcao);
        string totalDosProventosPagos = CalculateTotalDosDividendosPagos(dividendosDaAcao);

        decimal tetoPrice = CalculatePriceTeto(mediaDosProventosPagos, currentPrice, dyDesired);
        string recomendation = Recomendation(currentPrice, tetoPrice);

        DateTime currentDate = DateTime.Now;

        ActiveReturnForPurseDetails activeReturnForPurseDetails = new ();
        activeReturnForPurseDetails.Id = activeId;
        activeReturnForPurseDetails.Ativo = result.Symbol;
        activeReturnForPurseDetails.Tipo = VerifyType(result.QuoteType);
        activeReturnForPurseDetails.DividentYield = (dyDesired).ToString() + "%";
        activeReturnForPurseDetails.PrecoAtual = $"R$ {result.RegularMarketPrice.ToString("F2")}";
        activeReturnForPurseDetails.Preco_Teto = $"R$ {tetoPrice.ToString("F2")}";
        activeReturnForPurseDetails.Indicacao = recomendation;

        activeReturnForPurseDetails.Proventos_pagos = "Dados indisponíveis";
        if (mediaDosProventosPagos != "Dados indisponíveis")
        {
            activeReturnForPurseDetails.Proventos_pagos = $"Total: R$ {totalDosProventosPagos} | Média: R$ {mediaDosProventosPagos}";
        }

        return activeReturnForPurseDetails;
    }

    public static async Task<ActiveReturn> CreateActiveReturn(string ticker, string dYDesiredPercentage)
    {
        Security result = await GetActive(ticker, dYDesiredPercentage);

        if (!decimal.TryParse(dYDesiredPercentage, out decimal dyDesired))
            throw new ArgumentException("Porcentagem inválida.");

        decimal dYCurrent = result[Field.TrailingAnnualDividendYield] != null ? Convert.ToDecimal(result[Field.TrailingAnnualDividendYield]) : 0;
        decimal currentPrice = result[Field.RegularMarketPrice] != null ? Convert.ToDecimal(result[Field.RegularMarketPrice]) : 0;

        //obtendo os dividendos e calculando o total e a media dos proventos pagos
        JToken dividendosDaAcao = await ObterDividendosAte5Anos(result.Symbol);
        string mediaDosProventosPagos = CalculateMediaDosProventosPagos(dividendosDaAcao);
        string totalDosProventosPagos = CalculateTotalDosDividendosPagos(dividendosDaAcao);

        decimal tetoPrice = CalculatePriceTeto(mediaDosProventosPagos, currentPrice, dyDesired);
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

        activeReturn.Proventos_pagos = "Dados indisponíveis";
        if (mediaDosProventosPagos != "Dados indisponíveis")
        {
            activeReturn.Proventos_pagos = $"Total: R$ {totalDosProventosPagos} | Média: R$ {mediaDosProventosPagos}";
        }

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

    static async Task<JToken> ObterDividendosAte5Anos(string ticker)
    {
        if (ticker is null)
        {
            throw new HttpResponseException(400, "O ticker não pode ser nulo!");
        }

        //Pega o último dia do ano passado, e o primeiro dia da data de 5 anos atras referente a essa data, ignorando o ano atual
        DateOnly endDate = new DateOnly(DateTime.Now.Year - 1, 12, 31);
        DateTime startFullDate = new DateTime(endDate.Year, 1, 1).AddYears(-4);
        DateOnly startDate = new DateOnly(startFullDate.Year, startFullDate.Month, startFullDate.Day);

        long startTimestamp = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
        long endTimestamp = new DateTimeOffset(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

        var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}?interval=1d&period1={startTimestamp}&period2={endTimestamp}&events=div";

        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                var response = await client.GetStringAsync(url);

                var data = JObject.Parse(response);

                if (data["chart"]?["result"] == null || !data["chart"]["result"].HasValues)
                {
                    Console.WriteLine("Erro: Não foi possível recuperar os dados.");
                    return "Dados indisponíveis";
                }

                var events = data["chart"]["result"][0]["events"];

                if (events is null || events["dividends"] is null)
                {
                    Console.WriteLine("Nenhum dividendo encontrado no período especificado.");
                    return "Dados indisponíveis";
                }

                return events;
            }
        }
        catch(Exception ex)
        {
            throw new Exception($"Houve um problema ao tentar buscar os dividendos por ticker. - ex: {ex.Message}");
        }
    }

    static string CalculateTotalDosDividendosPagos(JToken dividends)
    {
        try
        {
                decimal totalDividends = 0;
                foreach (var dividend in dividends["dividends"])
                {
                    totalDividends += (decimal)dividend.First["amount"];
                }

                return $"{totalDividends.ToString("F2")}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Houve um erro ao tentar calcular o total dos dividendos pagos de uma ação. - ex: {ex.Message}");
            return "Dados indisponíveis";
        }
    }

    static string CalculateMediaDosProventosPagos(JToken dividends)
    {
        if (dividends is null || dividends.Count() <= 0)
        {
            return "Dados indisponíveis";
        }

        //Dicionário para armazenar os dividendos por ano, separando os vários dividendos retornados pelo ano
        Dictionary<int, List<decimal>> dividendsByYear = new Dictionary<int, List<decimal>>();

        foreach (var dividend in dividends["dividends"])
        {
            var amount = (decimal)dividend.First["amount"];
            var date = (long)dividend.First["date"];
            var dividendDate = DateTimeOffset.FromUnixTimeSeconds(date).DateTime;

            int year = dividendDate.Year;

            // Adiciona o dividendo na lista correspondente ao ano
            if (!dividendsByYear.ContainsKey(year))
            {
                dividendsByYear[year] = new List<decimal>();
            }

            dividendsByYear[year].Add(amount);
        }

        int quantityDividendHistoryYears = dividendsByYear.Keys.Count();
        decimal total = 0;

        //soma todos os valores dentro de cada ano
        foreach (var year in dividendsByYear.Keys)
        {
            total += dividendsByYear[year].Sum();
            Console.WriteLine($"Ano: {year} - valor total: {total}");
        }

        return $"{(total / quantityDividendHistoryYears).ToString("F2")}";
    }

    static decimal CalculatePriceTeto(string mediaProventosPagos, decimal currentPrice, decimal dYDesiredPercentage)
    {
        if (!decimal.TryParse(mediaProventosPagos, out decimal mediaProventosPagosDecimal))
        {
            throw new Exception("Valor do total dos proventos pagos é inválido!");
        }

        dYDesiredPercentage /= 100;

        if (mediaProventosPagosDecimal <= 0 || currentPrice <= 0)
            throw new InvalidOperationException("Data of Dividend Yield or Active price are invalid!");

        decimal priceTeto = mediaProventosPagosDecimal / dYDesiredPercentage;

        return priceTeto;
    }

    static string Recomendation(decimal currentPrice, decimal tetoPrice)
    {
        return currentPrice < tetoPrice ? "🟢 Comprar" : "🔴 Não-comprar";
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
