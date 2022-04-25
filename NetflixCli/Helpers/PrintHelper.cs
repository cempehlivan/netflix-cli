using NetflixCli.Models;
using NetflixCli.Services;

namespace NetflixCli.Helpers;

internal static class PrintHelper
{
    internal static void Print(this List<CategoryModel> categories, int categoryPage, int moviePage)
    {
        NetflixLogo();

        int rowCount = 10; //categories.Max(s => s.Movies.Count);
        int categoryCount = 5;

        int categorySkip = (categoryPage * categoryCount) - categoryCount;
        int rowSkip = (moviePage * rowCount) - rowCount;


        var printList = categories.Skip(categorySkip).Take(categoryCount).ToList();

        TablePrinterService t = new(printList.Select(s => (s?.CategoryName ?? "").ToUpper()).ToArray());

        int rowNumber = rowSkip;

        for (int i = 0; i < rowCount; i++)
        {
            string[] columns = new string[printList.Count];

            for (int a = 0; a < printList.Count; a++)
                columns[a] = $"{rowNumber + 1} - {(printList[a]?.Movies?.Count > rowNumber ? printList[a]?.Movies[rowNumber]?.MovieName ?? "" : "")}";

            t.AddRow(columns);
            rowNumber++;
        }

        t.Print();
    }


    private static void NetflixLogo()
    {
        string logo = @"
  _   _        _     __  _  _       
 | \ | |  ___ | |_  / _|| |(_)__  __
 |  \| | / _ \| __|| |_ | || |\ \/ /
 | |\  ||  __/| |_ |  _|| || | >  < 
 |_| \_| \___| \__||_|  |_||_|/_/\_\
                                    
";

        Console.WriteLine(logo);
    }
}
