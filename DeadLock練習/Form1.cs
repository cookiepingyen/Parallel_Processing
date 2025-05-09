using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

        public List<int> intList = new List<int>();
        public ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();
        public List<Task> taskList = new List<Task>();
        public Form1()
        {
            InitializeComponent();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //Parallel.For(0, 100, (index) =>
            //{
            //    intList.Add(index);
            //});

            for (int i = 0; i < 10000; i++)
            {
                taskList.Add(Task.Run(() => { concurrentBag.Add(i); }));
            }

            await Task.WhenAll(taskList);
            Console.WriteLine(concurrentBag.Count);
        }
    }
}
