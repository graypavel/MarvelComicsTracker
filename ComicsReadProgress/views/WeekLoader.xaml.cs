using System;
using System.Collections.Generic;
using System.ComponentModel;
using ComicsReadProgress.code;

namespace ComicsReadProgress.views
{
    public partial class WeekLoader
    {
        public readonly List<Issue> Issues;
        private readonly MarvelWikiaParser parser;

        public WeekLoader(MarvelWikiaParser parser)
        {
            InitializeComponent();
            Issues = new List<Issue>();
            this.parser = parser;
            var worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var count = parser.GetIssuesCount();
            if (count == 0)
                return;
            var step = Convert.ToDouble(100/count);
            double progress = 0;
            var worker = sender as BackgroundWorker;
            for (var i = 0; i < count; i++)
            {
                var issue = parser.GetIssue(i);
                Issues.Add(issue);
                progress += step;
                worker?.ReportProgress((int) progress);
            }
        }
    }
}
