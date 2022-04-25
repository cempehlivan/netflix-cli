using HtmlAgilityPack;
using NetflixCli.Models;
using NetflixCli.Helpers;

string url = "https://www.netflix.com/tr/browse/genre/34399";
string html = string.Empty;
int categoryPage = 1;
int moviePage = 1;

using (HttpClient client = new())
    html = await client.GetStringAsync(url);

if (string.IsNullOrEmpty(html))
{
    Console.WriteLine("Netflix could not be reached.");
    return;
}

html = System.Net.WebUtility.HtmlDecode(html);

HtmlDocument document = new();
document.LoadHtml(html);

List<CategoryModel> categories = document.DocumentNode
    .SelectNodes("//main[starts-with(@class, \"nm-collections-container\")]/section[@class=\"nm-collections-row\"]")
    //.Take(options.categoryCount)
    .Select(section =>
    {
        CategoryModel category = new();
        HtmlNode h2 = section.SelectSingleNode("./h2[@class=\"nm-collections-row-name\"]");
        category.CategoryName = h2?.SelectSingleNode(".//span[@class=\"nm-collections-row-name\"]")?.InnerText ?? h2.InnerText;
        category.Movies = section.SelectNodes(".//span[@class=\"nm-collections-title-name\"]").Select(s => new MovieModel { MovieName = s.InnerText }).ToList();

        return category;
    }).Where(s => string.IsNullOrWhiteSpace(s.CategoryName) == false && s.Movies?.Any() == true).ToList();

categories.Print(categoryPage, moviePage);


while (true)
{
    Console.Write("Use the arrow keys: ");
    var key = Console.ReadKey();

    switch (key.Key)
    {
        case ConsoleKey.LeftArrow:
            categoryPage -= 1;
            categoryPage = categoryPage < 1 ? 1 : categoryPage;
            break;
        case ConsoleKey.UpArrow:
            moviePage -= 1;
            moviePage = moviePage < 1 ? 1 : moviePage;
            break;
        case ConsoleKey.RightArrow:
            categoryPage += 1;
            break;
        case ConsoleKey.DownArrow:
            moviePage += 1;
            break;
        default:
            break;
    }

    Console.Clear();
    categories.Print(categoryPage, moviePage);
}