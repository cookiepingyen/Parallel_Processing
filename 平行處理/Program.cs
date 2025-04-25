using Libraries;
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
            Stopwatch stopwatch = new Stopwatch();
            CSVHelper helper = new CSVHelper();

            int ROW_COUNT = 40_000_000;
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

            for (int i = 0; i < times; i++)
            {
                int index = i;
                Task task = Task.Run(async () =>
                {
                    string outputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output\\MOCK_DATA_{ROW_COUNT}_{index}.csv";
                    Stopwatch subStopwatch = new Stopwatch();
                    subStopwatch.Start();
                    List<CSVdata> list = helper.OptimizeRead<CSVdata>(inputPath, index * Batch_COUNT, Batch_COUNT);
                    subReadTime = Math.Round((subStopwatch.ElapsedMilliseconds / 1000f), 2);
                    subReadTimes.Add(subReadTime);

                    subStopwatch.Restart();

                    int batchCount = 1_000_000;
                    int batchSize = Batch_COUNT / batchCount;
                    List<Task> writeTasks = new List<Task>();
                    for (int j = 0; j < batchSize; j++)
                    {
                        int taskIndex = j;
                        var rawData = list.Skip(taskIndex * batchCount).Take(batchCount).ToList();

                        Task writeTask = Task.Run(() =>
                        {
                            helper.OptimizeWriteList($@"C:\Users\user\source\repos\C#基礎專案\平行處理\Output\task_{index}_{taskIndex}.csv", rawData);
                        });

                        writeTasks.Add(writeTask);
                    }

                    await Task.WhenAll(writeTasks);

                    helper.OptimizeWriteList(outputPath, list);
                    subWriteTime = Math.Round((subStopwatch.ElapsedMilliseconds / 1000f), 2);
                    subWriteTimes.Add(subWriteTime);
                    Console.WriteLine($"第{index + 1}任務完成! 第{index * Batch_COUNT}~{index * Batch_COUNT + Batch_COUNT}筆。 讀取時間:{subReadTime} | 寫入時間:{subWriteTime} | 任務完成時間:{Math.Round(readTime + writeTime, 2)} s");
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"平均讀取時間: {subReadTimes.Median()} | 平均寫入時間: {subWriteTimes.Median()} | 總完成時間: {Math.Round((stopwatch.ElapsedMilliseconds / 1000f), 2)} s");

            Console.ReadLine();





            // 單一紀錄讀取寫入時間
            //string inputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Input\\MOCK_DATA_{ROW_COUNT}.csv";
            //string outputPath = $"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output\\MOCK_DATA_{ROW_COUNT}.csv";
            //if (Directory.Exists($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output"))
            //{
            //    Directory.Delete($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output", true);
            //    Directory.CreateDirectory($"C:\\Users\\user\\source\\repos\\C#基礎專案\\平行處理\\Output");
            //}

            //stopwatch.Start();
            //List<CSVdata> list = helper.Read<CSVdata>(inputPath);
            //stopwatch.Stop();
            //readTime = Math.Round(stopwatch.ElapsedMilliseconds / 1000.0, 2);
            //Console.WriteLine("讀取: " + readTime + "s");


            //stopwatch.Restart();
            //helper.WriteList(outputPath, list);
            //stopwatch.Stop();
            //writeTime = Math.Round(stopwatch.ElapsedMilliseconds / 1000.0, 2);

            //Console.WriteLine("寫入: " + stopwatch.ElapsedMilliseconds / 1000.0 + "s");
            //Console.WriteLine("任務完成，總耗時:" + (readTime + writeTime) + "s");
            //Console.ReadLine();

        }
    }
}
