using System.Runtime.Versioning;

namespace TerrainPathIdentifier
{
    [SupportedOSPlatform("windows")]
    public sealed class ProcessFile
    {
        public readonly int[,] Map;

        public readonly Indices MapBounds;

        public ProcessFile(string file, int rows, int colmns)
        {
            Map = CreateArray(file, rows, colmns);
            MapBounds = new Indices(rows, colmns);
        }

        private static int[,] CreateArray(string file, int rows, int colmns)
        {
            try
            {
                string[] text = File.ReadAllText(file).Split("   ", StringSplitOptions.RemoveEmptyEntries);

                int[,] Map = new int[rows, colmns];

                int index = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < colmns; j++)
                    {
                        Map[i, j] = int.Parse(text[index], System.Globalization.NumberStyles.Any);
                        index++;
                    }
                }

                return Map;
            }
            catch (FormatException)
            {
                string[] text = File.ReadAllLines(file);

                int[,] Map = new int[rows, colmns];
                Console.WriteLine($"{rows} - {colmns}");
                Console.WriteLine($"{text.Length}");

                for (int i = 0; i < rows; i++)
                {
                    string[] numbers = text[i].Split("   ", StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < colmns; j++)
                    {
                        Console.WriteLine($"{i} - {j}");
                        Map[i, j] = int.Parse(numbers[j], System.Globalization.NumberStyles.Any);
                    }
                }

                return Map;
            }
        }

        public Indices[] FindRoute(RouteLengths routeLength, CalucateCriteria criteria)
        {
            Indices[] Coordinates = Array.Empty<Indices>();

            Indices start = FindNextPosition(0, new Range(0, MapBounds.Row), routeLength);
            Coordinates = Coordinates.Append(start).ToArray();
            for (int i = 1; i < MapBounds.Colmn; i++)
            {
                Range range;
                Indices next;

                int LastCheckedRow = Coordinates.Last().Row;
                int outofrange = MapBounds.Row - 1;

                range = CheckNeededRows(LastCheckedRow, outofrange);
                next = ByElevationOrDifference(routeLength, criteria, Coordinates, i, range);

                Coordinates = Coordinates.Append(next).ToArray();
            }
            return Coordinates;
        }

        private Indices ByElevationOrDifference(RouteLengths routeLength, CalucateCriteria criteria, Indices[] Coordinates, int i, Range range)
        {
            return criteria == CalucateCriteria.ByActualValue
                ? FindNextPosition(i, range, routeLength)
                : FindNextPosition(i, range, Map[Coordinates.Last().Row, Coordinates.Last().Colmn]);
        }

        private static Range CheckNeededRows(int LastCheckedRow, int outofrange)
        {
            if (LastCheckedRow == 0)
            {
                return new(LastCheckedRow, LastCheckedRow + 2);
            }
            else
            {
                return LastCheckedRow == outofrange ? new(LastCheckedRow - 1, LastCheckedRow + 1)
                                                    : new(LastCheckedRow - 1, LastCheckedRow + 2);
            }
        }

        private Indices FindNextPosition(int ColmnToCheck, Range RowsToCheck, RouteLengths length)
        {
            Dictionary<Indices, int> ValuePositionPairs = new();
            for (int i = RowsToCheck.Start.Value; i < RowsToCheck.End.Value; i++)
            {
                ValuePositionPairs.Add(new Indices(i, ColmnToCheck), Map[i, ColmnToCheck]);
            }

            int small = ValuePositionPairs.Values.Min();
            int large = ValuePositionPairs.Values.Max();
            return length switch
            {
                RouteLengths.Shortest => ValuePositionPairs.FirstOrDefault(x => x.Value == small).Key,
                RouteLengths.Longest => ValuePositionPairs.FirstOrDefault(x => x.Value == large).Key,
                RouteLengths.BetweenMinMax => ValuePositionPairs.Shuffle().FirstOrDefault().Key,
                _ => throw new NotImplementedException("route length parameter is not in enum")
            };
        }

        private Indices FindNextPosition(int ColmnToCheck, Range RowsToCheck, int lastvalue)
        {
            Dictionary<Indices, int> ValuePositionPairs = new();
            for (int i = RowsToCheck.Start.Value; i < RowsToCheck.End.Value; i++)
            {
                ValuePositionPairs.Add(new Indices(i, ColmnToCheck), Map[i, ColmnToCheck]);
            }
            List<int> lowest = ValuePositionPairs.Values.OrderBy(x => Math.Abs(x - lastvalue)).ToList();
            return ValuePositionPairs.FirstOrDefault(x => x.Value == lowest[0]).Key;
        }
    }
}