using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class ScrapingService
{
    private readonly HttpClient _httpClient;

    public ScrapingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(string Name, int? Price)> ScrapePhoneInfo(string url)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);

        var nameNode = doc.DocumentNode.SelectSingleNode("//h1[@class='name']");
        var priceNode = doc.DocumentNode.SelectSingleNode("//div[@class='price']");

        if (nameNode != null && priceNode != null)
        {
            var name = nameNode.InnerText.Trim();
            var priceText = priceNode.InnerText.Trim();
            var price = ExtractPrice(priceText);

            return (name, price);
        }
        else
        {
            return (null, null);
        }
    }

    private int? ExtractPrice(string priceText)
    {
        var digitsOnly = new string(priceText.Where(char.IsDigit).ToArray());
        if (int.TryParse(digitsOnly, out int price))
        {
            return price;
        }
        return null;
    }
}