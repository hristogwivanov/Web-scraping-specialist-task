using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

string htmlData = File.ReadAllText("../../../htmlToParse.html");

try
{

    List<Product> products = ParseProductsFromHtml(htmlData);

    string jsonOutput = JsonSerializer.Serialize(products, new JsonSerializerOptions
    {
        WriteIndented = true
    });

    Console.WriteLine(jsonOutput);

}
catch (Exception e)
{
    Console.WriteLine("An error occurred: " + e.Message);
}


static List<Product> ParseProductsFromHtml(string html)
{
    List<Product> products = new List<Product>();

    var productNameRegex = new Regex(@"<h4><a[^>]*>([^<]*)</a></h4>");
    var priceRegex = new Regex(@"<span[^>]*>\$([^<]*)</span>");
    var ratingRegex = new Regex(@"rating\s*=\s*""([^""]*)");

    var productNameMatches = productNameRegex.Matches(html);
    var priceMatches = priceRegex.Matches(html);
    var ratingMatches = ratingRegex.Matches(html);

    for (int i = 0; i < productNameMatches.Count; i++)
    {
        string productName = WebUtility.HtmlDecode(productNameMatches[i].Groups[1].Value);
        string price = priceMatches[i].Groups[1].Value.Replace(",", "");
        string rating = ratingMatches[i].Groups[1].Value;

        products.Add(new Product
        {
            productName = productName,
            price = price,
            rating = rating
        });
    }

    return products;
}


class Product
{
    public string productName { get; set; }
    public string price { get; set; }
    public string rating { get; set; }
}
