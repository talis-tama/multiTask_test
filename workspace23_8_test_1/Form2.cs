using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workspace23_8_test_1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1("進行状況ダイアログのテスト", new DoWorkEventHandler(Form1_DoWork), 100);
            DialogResult result = f1.ShowDialog(this);
            if (result == DialogResult.Cancel) { MessageBox.Show("キャンセルされました"); }
            else if (result == DialogResult.Abort)
            {
                Exception ex = f1.Error;
                MessageBox.Show("エラー:" + ex.Message);
            }
            else if (result == DialogResult.OK)
            {
                int stopTime = (int)f1.Result;
                MessageBox.Show("成功しました:" + stopTime.ToString());
            }
            f1.Dispose();
        }
        private void Form1_DoWork(object sender,DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            int stopTime = (int)e.Argument;
            for (int i = 1; i<= 100; i++)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                System.Threading.Thread.Sleep(stopTime);
                bw.ReportProgress(i, i.ToString() + "%終了しました");
            }
            e.Result = stopTime * 100;
        }
    }
}
