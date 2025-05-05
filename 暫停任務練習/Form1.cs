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

namespace 暫停任務練習
{
    public partial class Form1 : Form
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {

            int count = 0;
            Task task = Task.Run(() =>
            {
                while (true)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    count++;
                    Debug.WriteLine(count);
                    Thread.Sleep(500);
                }
            }, cts.Token);

        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
