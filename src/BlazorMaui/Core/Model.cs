using System.Drawing;
using System.Runtime.Versioning;

namespace TerrainPathIdentifier
{
    [SupportedOSPlatform("windows")]
    public class Model
    {
        public RouteLengths Selectedlength { get; set; } = RouteLengths.none;

        public CalucateCriteria SelectedCriteria { get; set; } = CalucateCriteria.none;

        public bool clicked;

        public int Row { get; set; }

        public int Colmn { get; set; }

        public int NumRoutes { get; set; } = 1;

        public string Color { get; set; }

        public FileResult File { get; set; }

        public ProcessFile Data { get; set; }

        public Bitmap Map { get; set; }

        public string Stream { get; set; } = "";

        public void CreateInstance()
        {
            //for unknown reason the method runs when the file picker is opened so
            //this check has to be done
            if (File == null || Row == 0 || Colmn == 0) return;
            Data = new ProcessFile(File, Row, Colmn);
            clicked = true;
        }

        public void MakeMap()
        {
            Map = Draw.DrawMap(Data);
            var bytestring = Convert.ToBase64String(Map.ImageToBytes());
            bytestring = string.Format("data:image/png;base64,{0}", bytestring);
            Stream = bytestring;
        }

        public void DrawPath()
        {
            //similar situation to CreateInstance, running on 1 input
            if (SelectedCriteria == 0 || Selectedlength == 0 || Color?.Length == 0) return;

            if (Selectedlength != RouteLengths.BetweenMinMax) NumRoutes = 1;
            Bitmap updatedmap = null;

            for (int i = 0; i < NumRoutes; i++)
            {
                var path = Data.FindRoute(Selectedlength, SelectedCriteria);
                updatedmap = Draw.DrawPath(path, Map, ColorTranslator.FromHtml(Color));
            }
            Map = updatedmap;
            var bytestring = Convert.ToBase64String(updatedmap.ImageToBytes());
            bytestring = string.Format("data:image/png;base64,{0}", bytestring);
            Stream = bytestring;
        }
    }
}
