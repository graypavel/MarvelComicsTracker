using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ComicsReadProgress
{
    /// <summary>
    /// Interaction logic for Management.xaml
    /// </summary>
    public partial class Management : Window
    {
        public Management()
        {
            InitializeComponent();
            UpdateView();
        }

        private void UpdateView()
        {
            ComicsList.DataContext = Repository.Select<Issue>().ToList();
        }

        private void LoadComicsClick(object sender, RoutedEventArgs e)
        {
            var parser = new MarvelWikiaParser();
            var address = $"http://marvel.wikia.com/wiki/Category:Week_{Week.Text},_{Year.Text}";
            try
            {
                var issues = parser.GetIssues(address);
                Repository.Inserts(issues);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateView();
        }
    }
}
