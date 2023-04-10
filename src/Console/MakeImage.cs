using System.Drawing;
using System.Runtime.Versioning;
using Color = System.Drawing.Color;

namespace TerrainPathIdentifier
{
    [SupportedOSPlatform("windows")]
    public sealed class Draw
    {
        public static Bitmap DrawMap(ProcessFile data)
        {
            Bitmap bmp = new(data.MapBounds.Colmn, data.MapBounds.Row);
            decimal Percentage = data.Map.Max(data.MapBounds) / (decimal)data.Map.Min(data.MapBounds) / 100;

            for (int i = 0; i < data.MapBounds.Row; i++)
            {
                for (int j = 0; j < data.MapBounds.Colmn; j++)
                {
                    decimal num = data.Map[i, j] * Percentage;
                    bmp.SetPixel(j, i, Color.FromArgb((int)num, (int)num, (int)num));
                }
            }
            return bmp;
        }

        public static Bitmap DrawPath(Indices[] coordinates, Bitmap bmp, Color color)
        {
            foreach (Indices coordinate in coordinates)
            {
                bmp.SetPixel(coordinate.Colmn, coordinate.Row, color);
            }
            return bmp;
        }
    }
}