using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DeadLock練習
{
    public partial class Form1 : Form
    {
        private static readonly object key = new object();
        private static readonly object key2 = new object();

        public static int StaticNum;

        public static int A = 0;
        public static int B = 0;
        public Form1()
        {
            InitializeComponent();

            StaticNum = 0;



            Parallel.For(0, 100, (index, token) =>
            {
                //lock (key)
                //{
                //    NumAdd(ref StaticNum);
                //}
                NumAdd(ref StaticNum);
            });

            Console.WriteLine($"StaticNum: {StaticNum}");
            Console.WriteLine($"A: {A}, B: {B}");

        }

        public int NumAdd(ref int num)
        {
            int temp = num;

            Thread.Sleep(5);

            lock (key)
            {
                if (temp % 3 == 0)
                {
                    temp += 2;
                    A++;
                }
                else
                {
                    temp += 1;
                    B++;
                }

            }

            num = temp;
            return num;
        }


        public async Task ParallelTask()
        {
            Task task = Task.Run(async () =>
            {

            });
            await task;
        }
    }
}
