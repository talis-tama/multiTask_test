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
    public partial class Form1 : Form
    {
        public Form1(string caption, DoWorkEventHandler doWork, object argument)
        {
            InitializeComponent();
            workerArgument = argument;
            Text = caption;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ControlBox = false;
            CancelButton = cancelAsyncButton;
            messageLabel.Text = "";
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            cancelAsyncButton.Text = "キャンセル";
            cancelAsyncButton.Enabled = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            Shown += new EventHandler(ProgressDialog_Shown);
            cancelAsyncButton.Click += new EventHandler(cancelAsyncButton_Click);
            backgroundWorker1.DoWork += doWork;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }
        public Form1(string formTitle,DoWorkEventHandler doWorkHandler) : this(formTitle, doWorkHandler, null) { }
        private object workerArgument = null;
        private object _result = null;
        public object Result { get { return _result; } }
        private Exception _error = null;
        public Exception Error { get { return _error; } }
        public BackgroundWorker BackgroundWorker { get { return backgroundWorker1; } }
        private void ProgressDialog_Shown(object sender,EventArgs e) { backgroundWorker1.RunWorkerAsync(workerArgument); }
        private void cancelAsyncButton_Click(object sender,EventArgs e)
        {
            cancelAsyncButton.Enabled = false;
            backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker1_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < progressBar1.Minimum) { progressBar1.Value = progressBar1.Minimum; }
            else if (progressBar1.Maximum < e.ProgressPercentage) { progressBar1.Value = progressBar1.Maximum; }
            else { progressBar1.Value = e.ProgressPercentage; }
            messageLabel.Text = (string)e.UserState;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("エラーが発生しました。", e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _error = e.Error;
                DialogResult = DialogResult.Abort;
            }
            else if (e.Cancelled) { DialogResult = DialogResult.Cancel; }
            else
            {
                _result = e.Result;
                DialogResult = DialogResult.OK;
            }
            Close();
        }
    }
}
