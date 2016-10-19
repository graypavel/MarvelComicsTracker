using System;
using System.Net;
using HtmlAgilityPack;

namespace ComicsReadProgress.code
{
    public class MarvelWikiaParser
    {
        private readonly string webAddress = "http://marvel.wikia.com/wiki/Category:Week_{W},_{Y}";
        private readonly HtmlNodeCollection galleryItems;

        public MarvelWikiaParser(int week, int year)
        {
            webAddress = webAddress
                .Replace("{W}", week.ToString("00"))
                .Replace("{Y}", year.ToString("0000"));

            var htmlDocument = new HtmlWeb().Load(webAddress);
            var htmlNode = htmlDocument.GetElementbyId("gallery-0");
            if (htmlNode == null)
                return;
            galleryItems = htmlNode.ChildNodes;
        }

        public int GetIssuesCount()
        {
            if (galleryItems == null)
                return 0;
            return galleryItems.Count;
        }

        public Issue GetIssue(int number)
        {
            var issue = new Issue();
            var title = galleryItems[number].ChildNodes[1].ChildNodes[0].ChildNodes[0].GetAttributeValue("title", "");
            var items = title.Split(new [] { " Vol "}, StringSplitOptions.RemoveEmptyEntries);
            var dateToParse = galleryItems[number].ChildNodes[1].ChildNodes[0].ChildNodes[2].InnerText;
            dateToParse = dateToParse.Replace('(', ' ').Replace(')', ' ').Trim();
            issue.Released = DateTime.Parse(dateToParse);
            issue.SeriesTitle = items[0];
            issue.Volume = int.Parse(items[1].Split(' ')[0]);
            issue.Number = items[1].Split(' ')[1];
            issue.Cover = GetIssueCover("http://marvel.wikia.com" + galleryItems[number].ChildNodes[0].ChildNodes[0].ChildNodes[0].GetAttributeValue("href", ""));
            issue.WikiaAddress = "http://marvel.wikia.com" + galleryItems[number].ChildNodes[1].ChildNodes[0].ChildNodes[0].GetAttributeValue("href", "");
            return issue;
        }

        private static byte[] GetIssueCover(string coverHtml)
        {
            var website = new HtmlWeb();
            var document = website.Load(coverHtml);
            var element = document.GetElementbyId("mw-content-text");
            if(element == null)
                return new byte[] {};
            var address = element.ChildNodes[0].ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "");
            var webClient = new WebClient();
            return webClient.DownloadData(address);
        }
    }
}
