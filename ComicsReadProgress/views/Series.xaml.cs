using System.Linq;
using ComicsReadProgress.code;

namespace ComicsReadProgress.views
{
    public partial class Series
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
