using System.Drawing;
using System.Runtime.Versioning;

namespace TerrainPathIdentifier
{
    [SupportedOSPlatform("windows")]
    public static class Extensions
    {
        public static int Min(this int[,] array, Indices MapBounds)
        {
            int smallest = int.MaxValue;
            for (int i = 0; i < MapBounds.Row; i++)
            {
                for (int j = 0; j < MapBounds.Colmn; j++)
                {
                    if (array[i, j] < smallest)
                    {
                        smallest = array[i, j];
                    }
                }
            }
            return smallest;
        }

        public static int Max(this int[,] array, Indices MapBounds)
        {
            int biggest = 0;
            for (int i = 0; i < MapBounds.Row; i++)
            {
                for (int j = 0; j < MapBounds.Colmn; j++)
                {
                    if (array[i, j] > biggest)
                    {
                        biggest = array[i, j];
                    }
                }
            }
            return biggest;
        }

        public static Dictionary<Indices, int> Shuffle(this Dictionary<Indices, int> dict)
        {
            return dict.OrderBy(_ => new Random((int)DateTimeOffset.Now.Ticks).Next()).ToDictionary(x => x.Key, x => x.Value);
        }

        public static byte[] ImageToBytes(this Bitmap bmp)
        {
            using var stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}