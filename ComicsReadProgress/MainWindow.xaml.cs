using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            LoadIssues();
        }

        private void LoadIssues()
        {
            issues = Repository.Select<Issue>().OrderBy(i => i.Released).ThenBy(i => i.SeriesTitle).ToList();
            if (issues.Count > 0)
                DisplayIssue(issues.First());
        }

        private void DisplayIssue(Issue issue)
        {
            Comic.DataContext = issue;
            selectedIssue = issue;
            Cover.Source = LoadImage(issue.Cover);
            ReadLabel.Visibility = issue.Read ? Visibility.Visible : Visibility.Collapsed;
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
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
        }

        private void SeriesClick(object sender, RoutedEventArgs e)
        {
            new Series(selectedIssue).ShowDialog();
        }
    }
}
