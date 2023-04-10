using Spectre.Console;
using System.Drawing;

namespace TerrainPathIdentifier
{
    public class TPI
    {
        public static void Main()
        {
            Console.WriteLine("any images created will be saved in the current directory");

            while (true)
            {
                ProcessFile data = Input();

                Bitmap map = Draw.DrawMap(data);
                Console.WriteLine("saved original");

                map.Save($"original.png");

                bool loop = true;

                while (loop)
                {
                    RouteLengths routechoice = AnsiConsole.Prompt(new SelectionPrompt<RouteLengths>()
                    {
                        Title = "Choose your route length"
                    }.AddChoices((RouteLengths[])Enum.GetValues(typeof(RouteLengths))));

                    CalucateCriteria Calculatechoice = AnsiConsole.Prompt(new SelectionPrompt<CalucateCriteria>()
                    {
                        Title = "Choose your path finding criteria"
                    }.AddChoices((CalucateCriteria[])Enum.GetValues(typeof(CalucateCriteria))));

                    KnownColor Colorchoice = AnsiConsole.Prompt(new SelectionPrompt<KnownColor>()
                    {
                        Title = "Choose your path color"
                    }.AddChoices((KnownColor[])Enum.GetValues(typeof(KnownColor))));


                    Indices[] path = data.FindRoute(routechoice, Calculatechoice);
                    Bitmap newmap = Draw.DrawPath(path, map, System.Drawing.Color.FromKnownColor(Colorchoice));

                    Console.WriteLine("saved updated");

                    newmap.Save($"updated.png");

                    loop = AnsiConsole.Confirm("continue with the currentfile?");
                }
            }
        }

        public static ProcessFile Input()
        {
            string path = AnsiConsole.Ask<string>("enter the path to the file:");
            int row = AnsiConsole.Ask<int>("how many rows is the file:");
            int colmn = AnsiConsole.Ask<int>("how many colmns the file:");

            return new(path, row, colmn);
        }
    }
}
