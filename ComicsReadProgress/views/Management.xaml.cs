using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ComicsReadProgress.code;

namespace ComicsReadProgress.views
{
    public partial class Management
    {
        private ObservableCollection<Issue> issues; 

        public Management()
        {
            InitializeComponent();
            LoadFromDatabase();
            ComicsList.DataContext = issues;
            DataContext = Properties.Settings.Default;
        }

        private void LoadFromDatabase()
        {
            issues = new ObservableCollection<Issue>(Repository.Select<Issue>()
                .OrderBy(i => i.Released)
                .ThenBy(i => i.SeriesTitle));
            ComicsList.SelectedIndex = 0;
        }

        private void LoadComicsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var parser = new MarvelWikiaParser(int.Parse(Week.Text), int.Parse(Year.Text));
                var loader = new WeekLoader(parser);
                if (loader.ShowDialog() == true)
                {
                    var addedIssues = 0;
                    foreach (var issue in loader.Issues.Where(issue => Repository.Select<Issue>().Count(i => i.WikiaAddress == issue.WikiaAddress) == 0))
                    {
                        Repository.Insert(issue);
                        addedIssues++;
                    }
                    MessageBox.Show($"Добавлено {addedIssues} выпусков, пропущено {loader.Issues.Count - addedIssues} выпусков", "",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    MoveToNextWeek();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadFromDatabase();
        }

        private static void MoveToNextWeek()
        {
            if (Properties.Settings.Default.Week == 53)
            {
                Properties.Settings.Default.Year++;
                Properties.Settings.Default.Week = 1;
            }
            else
                Properties.Settings.Default.Week++;
        }

        private void ComicsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComicsList.SelectedIndex == -1)
                return;
            var issue = ComicsList.SelectedItem as Issue;
            IssuePanel.DataContext = issue;
            Cover.Source = Utils.LoadImage(issue?.Cover);
        }

        private void DeleteIssueClick(object sender, RoutedEventArgs e)
        {
            var selectedIssue = ComicsList.SelectedItem as Issue;
            var issue = Repository.Select<Issue>().First(i => i.Id == selectedIssue.Id);
            Repository.Delete(issue);
            issues.Remove(selectedIssue);
        }

        private void SortByReleaseDate(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            issues = new ObservableCollection<Issue>(
                issues
                    .OrderBy(i => i.Released)
                    .ThenBy(i => i.SeriesTitle));
            ComicsList.DataContext = issues;
        }

        private void SortByTitle(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            issues = new ObservableCollection<Issue>(
                issues
                    .OrderBy(i => i.SeriesTitle)
                    .ThenBy(i => i.Released));
            ComicsList.DataContext = issues;
        }
    }
}
