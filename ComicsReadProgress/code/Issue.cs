using System;

namespace ComicsReadProgress.code
{
    public class Issue
    {
        public int Id { get; set; }
        public string SeriesTitle { get; set; }
        public int Volume { get; set; }
        public string Number { get; set; }
        public string WikiaAddress { get; set; }
        public bool Read { get; set; }
        public byte[] Cover { get; set; }
        public DateTime Released { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(SeriesTitle))
                return "";
            return SeriesTitle + " Vol. " + Volume + " #" + Number;
        }
    }
}
