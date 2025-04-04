using Libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 平行處理
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            CSVHelper helper = new CSVHelper();

            stopwatch.Start();

            List<CSVdata> list = helper.Read<CSVdata>("C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Input\\MOCK_DATA_1000000.csv");

            string outputPath = "C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output\\MOCK_DATA_1000000.csv";

            helper.WriteList(outputPath, list);

            stopwatch.Stop();


            Console.WriteLine(stopwatch.ElapsedMilliseconds / 1000.0 + "s");
            Console.ReadLine();

        }
    }
}
