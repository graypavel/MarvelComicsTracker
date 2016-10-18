using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;

namespace ComicsReadProgress.code
{
    public class MarvelWikiaParser
    {
        public static IEnumerable<Issue> GetIssues(string address)
        {
            var issues = new List<Issue>();
            var website = new HtmlWeb();
            var document = website.Load(address);

            var element = document.GetElementbyId("gallery-0");
            if (element == null)
                return issues;
            var galleryItems = element.ChildNodes;
            foreach (var galleryItem in galleryItems)
            {
                var issue = new Issue();
                var title = galleryItem.ChildNodes[1].ChildNodes[0].ChildNodes[0].GetAttributeValue("title", "");

                var items = title.Split(new [] { " Vol "}, StringSplitOptions.RemoveEmptyEntries);

                var dateToParse = galleryItem.ChildNodes[1].ChildNodes[0].ChildNodes[2].InnerText;
                dateToParse = dateToParse.Replace('(', ' ').Replace(')', ' ').Trim();


                issue.Released = DateTime.Parse(dateToParse);
                issue.SeriesTitle = items[0];
                issue.Volume = int.Parse(items[1].Split(' ')[0]);
                issue.Number = items[1].Split(' ')[1];
                issue.Cover = GetIssueCoverUri("http://marvel.wikia.com" + galleryItem.ChildNodes[0].ChildNodes[0].ChildNodes[0].GetAttributeValue("href", ""));
                issue.WikiaAddress = "http://marvel.wikia.com" + galleryItem.ChildNodes[1].ChildNodes[0].ChildNodes[0].GetAttributeValue("href", "");
                issues.Add(issue);
            }
            return issues;
        }

        private static byte[] GetIssueCoverUri(string coverHtml)
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
