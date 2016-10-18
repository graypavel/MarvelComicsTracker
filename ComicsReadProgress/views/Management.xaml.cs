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
        public Management()
        {
            InitializeComponent();
            UpdateView();
        }

        private void UpdateView()
        {
            ComicsList.DataContext = new ObservableCollection<Issue>(
                Repository.Select<Issue>()
                    .OrderBy(i => i.Released)
                    .ThenBy(i => i.SeriesTitle));
            ComicsList.SelectedIndex = 0;
        }

        private void LoadComicsClick(object sender, RoutedEventArgs e)
        {
            var address = $"http://marvel.wikia.com/wiki/Category:Week_{Week.Text},_{Year.Text}";
            try
            {
                var issues = MarvelWikiaParser.GetIssues(address).ToArray();
                var addedIssues = 0;
                foreach (var issue in issues)
                {
                    if (Repository.Select<Issue>().Count(i => i.WikiaAddress == issue.WikiaAddress) == 0)
                    {
                        Repository.Insert(issue);
                        addedIssues++;
                    }
                }
                MessageBox.Show($"Добавлено {addedIssues} выпусков, пропущено {issues.Length - addedIssues} выпусков", "",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateView();
        }

        private void ComicsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComicsList.SelectedIndex == -1)
                return;
            var issue = ComicsList.SelectedItem as Issue;
            IssuePanel.DataContext = issue;
            Cover.Source = Utils.LoadImage(issue.Cover);
        }

        private void DeleteIssueClick(object sender, RoutedEventArgs e)
        {
            var selectedIssue = ComicsList.SelectedItem as Issue;
            var issue = Repository.Select<Issue>().First(i => i.Id == selectedIssue.Id);
            Repository.Delete(issue);
            UpdateView();
        }
    }
}
