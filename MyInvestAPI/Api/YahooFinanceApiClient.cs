using Flurl.Util;
using YahooFinanceApi;

namespace MyInvestAPI.Api;

public class YahooFinanceApiClient
{
    public async static Task<string> Operations()
    {
        var securities = await Yahoo.Symbols("AAPL", "MSFT").Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketChange).QueryAsync();

        var apple = securities["AAPL"];
        var microsoft = securities["MSFT"];


        string result = $"Apple: {apple[Field.RegularMarketPrice]} (Change: {apple[Field.RegularMarketChange]})\n" +
                     $"Microsoft: {microsoft[Field.RegularMarketPrice]} (Change: {microsoft[Field.RegularMarketChange]})";

        // Retorne o resultado
        return result;
    }
}
