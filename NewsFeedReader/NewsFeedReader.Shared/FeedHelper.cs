using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsFeedReader
{
    public static class FeedHelper
    {
        private static List<NewsItem> newsList;
        private static string newsFeedAPI = "http://www.forbes.com/technology/index.xml";

        private static async Task<bool> getNewsFeedFromForbes()
        {
            newsList = new List<NewsItem>();
            try
            {
                HttpClient client = new HttpClient();
                Stream response = await client.GetStreamAsync(newsFeedAPI);

                StreamReader sr = new StreamReader(response, Encoding.UTF8);

                XDocument testXML = XDocument.Load(sr);
                XNamespace dc = "http://purl.org/dc/elements/1.1/";
                XNamespace media = "http://search.yahoo.com/mrss/";

                foreach (XElement newsElement in testXML.Descendants("item"))
                {
                    NewsItem ni = new NewsItem();

                    ni.title = newsElement.Element("title").Value;
                    ni.description = newsElement.Element("description").Value;
                    ni.link = newsElement.Element("link").Value;
                    ni.pubDate = newsElement.Element("pubDate").Value;


                    if (newsElement.Element("title") != null)
                    { ni.title = newsElement.Element("title").Value; }
                    else { ni.title = "noTitle"; }


                    if (newsElement.Element("description") != null)
                    { ni.description = newsElement.Element("description").Value; }
                    else { ni.description = "noDescription"; }


                    if (newsElement.Element("link") != null)
                    { ni.link = newsElement.Element("link").Value; }
                    else { ni.link = "noLink"; }


                    if (newsElement.Element("pubDate") != null)
                    { ni.pubDate = newsElement.Element("pubDate").Value; }
                    else { ni.pubDate = "noPubDate"; }

                    if (newsElement.Element(dc + "creator") != null)
                    { ni.creator = newsElement.Element(dc + "creator").Value; }
                    else { ni.creator = "noCreator"; }

                    if (newsElement.Element(media + "content") != null)
                    { ni.mediaContent = newsElement.Element(media + "content").Attribute("url").Value; }
                    else { ni.mediaContent = "noImage"; }

                    newsList.Add(ni);
                }
                return true;
            }
            catch (Exception ex) { Debug.WriteLine("Exception: " + ex.ToString()); return false; }
        }

        public static async Task<List<NewsItem>> getNewsLists(bool reDownloadFeed)
        {
            bool result = true;
            if (newsList == null || newsList.Count == 0 || reDownloadFeed)
            {
                result = await getNewsFeedFromForbes();
            }
            if (result)
                return newsList;
            else
                return null;
        }
    }
}
