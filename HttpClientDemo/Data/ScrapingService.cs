using System;
using System.Collections.Generic;
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

    public async Task<List<(string Name, int? Price)>> ScrapePhoneInfo(string url)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);

        var phoneNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'box browsingitem')]");

        if (phoneNodes != null && phoneNodes.Any())
        {
            var phones = new List<(string Name, int? Price)>();

            foreach (var node in phoneNodes)
            {
                var nameNode = node.SelectSingleNode(".//a[@class='name browsinglink']");
                var priceNode = node.SelectSingleNode(".//span[@class='price-box__price']");

                if (nameNode != null && priceNode != null)
                {
                    var name = nameNode.InnerText.Trim();
                    var priceText = priceNode.InnerText.Trim();
                    var price = ExtractPrice(priceText);

                    phones.Add((name, price));
                }
            }

            return phones;
        }
        else
        {
            return new List<(string Name, int? Price)>();
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