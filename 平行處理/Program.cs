using Libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            int ROW_COUNT = 5000;
            double readTime = 0;
            double writeTime = 0;

            Console.WriteLine($"筆數: {ROW_COUNT}");

            string inputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Input\\MOCK_DATA_{ROW_COUNT}.csv";
            string outputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output\\MOCK_DATA_{ROW_COUNT}.csv";

            if (Directory.Exists($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output"))
            {
                Directory.Delete($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output", true);
                Directory.CreateDirectory($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output");
            }

            stopwatch.Start();
            List<CSVdata> list = helper.Read<CSVdata>(inputPath);
            stopwatch.Stop();
            readTime = stopwatch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("讀取: " + readTime + "s");


            stopwatch.Restart();
            helper.WriteList(outputPath, list);
            stopwatch.Stop();
            writeTime = stopwatch.ElapsedMilliseconds / 1000.0;

            Console.WriteLine("寫入: " + stopwatch.ElapsedMilliseconds / 1000.0 + "s");


            Console.WriteLine("任務完成，總耗時:" + (readTime + writeTime) + "s");
            Console.ReadLine();

        }
    }
}
