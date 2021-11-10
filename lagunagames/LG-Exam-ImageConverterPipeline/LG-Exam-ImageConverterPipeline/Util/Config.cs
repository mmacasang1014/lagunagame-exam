using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LG_Exam_ImageConverterPipeline.Util
{
    public static class Config
    {
        public const string TexConfigUrl = "https://hv4gxzchk24cqfezebn3ujjz6oy2kbtztv5vghn6kpbkjc3vg4rq.arweave.net/DR21A6A-H0SSfW2DpSa1O9duWYWjx41g6TIh8UPJ6T4";

        public static string InputFileDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\input\"));
        public static string OutputFileDirecotry = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\LG-AssetBundle\Assets\textures\"));//  @"E:\kaspo\exam\lagunagames\LG-AssetBundle\";

        public static string ProjectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"));
    }
}
