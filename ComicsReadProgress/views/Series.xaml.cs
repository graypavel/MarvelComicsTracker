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

namespace ComicsReadProgress.views
{
    /// <summary>
    /// Interaction logic for Series.xaml
    /// </summary>
    public partial class Series : Window
    {
        public Series(Issue issue)
        {
            InitializeComponent();
            var issues = Repository.Select<Issue>()
                .Where(i => i.SeriesTitle == issue.SeriesTitle && i.Volume == issue.Volume)
                .OrderBy(i => i.Number);
            SeriesList.DataContext = issues.ToList();
            SeriesTitle.Text = issue.SeriesTitle + " Vol. " + issue.Volume;
        }
    }
}
