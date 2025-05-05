using Libraries;
using ParallelProcessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 平行處理
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // concurrentBag/concurrentQueue => 裝載容器
            // lock / mutex / ReadWriteSlim / SemaphoreSlim(紅綠燈機制)





            Stopwatch stopwatch = new Stopwatch();
            CSVHelper helper = new CSVHelper();

            int ROW_COUNT = 24_000_000;
            //int Batch_COUNT = 2_500_000;
            int Batch_COUNT = 3_000_000;
            double readTime = 0;
            double writeTime = 0;
            double subReadTime = 0;
            double subWriteTime = 0;
            List<double> subReadTimes = new List<double>();
            List<double> subWriteTimes = new List<double>();

            Console.WriteLine($"筆數: {ROW_COUNT}");

            string inputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Input\\MOCK_DATA_{ROW_COUNT}.csv";
            //string outputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output\\MOCK_DATA_{ROW_COUNT}.csv";

            if (Directory.Exists($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output"))
            {
                Directory.Delete($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output", true);
                Directory.CreateDirectory($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output");
            }

            stopwatch.Start();

            List<Task> tasks = new List<Task>();

            int times = (ROW_COUNT % Batch_COUNT == 0) ? ROW_COUNT / Batch_COUNT : (ROW_COUNT / Batch_COUNT) + 1;


            await Parallel.ForAsync(0, times, async (index, token) =>
            {
                string outputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output\\MOCK_DATA_{ROW_COUNT}_{index}.csv";
                Stopwatch subStopwatch = new Stopwatch();
                subStopwatch.Start();
                List<CSVdata> list = helper.OptimizeRead<CSVdata>(inputPath, index * Batch_COUNT, Batch_COUNT);
                subReadTime = Math.Round((subStopwatch.ElapsedMilliseconds / 1000f), 2);
                subReadTimes.Add(subReadTime);

                subStopwatch.Restart();
                helper.OptimizeWriteList(outputPath, list);
                subWriteTime = Math.Round((subStopwatch.ElapsedMilliseconds / 1000f), 2);
                subWriteTimes.Add(subWriteTime);
                Console.WriteLine($"第{index + 1}任務完成! 第{index * Batch_COUNT}~{index * Batch_COUNT + Batch_COUNT}筆。 讀取時間:{subReadTime} | 寫入時間:{subWriteTime} | 任務完成時間:{Math.Round(readTime + writeTime, 2)} s");

            });

            await Task.WhenAll(tasks);
            Console.WriteLine($"平均讀取時間: {subReadTimes.Median()} | 平均寫入時間: {subWriteTimes.Median()} | 總完成時間: {Math.Round((stopwatch.ElapsedMilliseconds / 1000f), 2)} s");

            Console.ReadLine();


            Console.WriteLine("任務完成");
            Console.ReadKey();


        }
    }
}