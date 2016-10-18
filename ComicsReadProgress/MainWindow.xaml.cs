using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using ComicsReadProgress.code;
using ComicsReadProgress.views;

namespace ComicsReadProgress
{
    public partial class MainWindow
    {
        private List<Issue> issues;
        private Issue selectedIssue;

        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            try
            {
                LoadIssues();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка загрузки базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadIssues()
        {
            issues = Repository.Select<Issue>().OrderBy(i => i.Released).ThenBy(i => i.SeriesTitle).ToList();
            if (issues.Count > 0)
                DisplayIssue(issues.First(i => !i.Read));
        }

        private void DisplayIssue(Issue issue)
        {
            Comic.DataContext = issue;
            selectedIssue = issue;
            Cover.Source = Utils.LoadImage(issue.Cover);
            ReadLabel.Visibility = issue.Read ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ReadIssueClick(object sender, RoutedEventArgs e)
        {
            var issue = Repository.Select<Issue>().First(i => i.Id == selectedIssue.Id);
            issue.Read = !issue.Read;
            Repository.Update(issue);
            MoveToNextComic();
        }

        private void MoveToNextComic()
        {
            var index = issues.FindIndex(i => i.Id == selectedIssue.Id);
            if (index + 1 == issues.Count)
                return;
            var issue = issues[index + 1];
            DisplayIssue(issue);
        }

        private void MoveToPreviousComic()
        {
            var index = issues.FindIndex(i => i.Id == selectedIssue.Id);
            if (index == 0)
                return;
            var issue = issues[index - 1];
            DisplayIssue(issue);
        }

        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            MoveToPreviousComic();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            MoveToNextComic();
        }

        private void CoverMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var issue = issues.Find(i => i.Id == selectedIssue.Id);
            System.Diagnostics.Process.Start(issue.WikiaAddress);
        }

        private void ManagementClick(object sender, RoutedEventArgs e)
        {
            new Management().ShowDialog();
            LoadIssues();
        }

        private void SeriesClick(object sender, RoutedEventArgs e)
        {
            new Series(selectedIssue).ShowDialog();
        }
    }
}
