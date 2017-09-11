using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFeedReader
{
    public class NewsItem
    {        public string link { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string creator { get; set; }
        public string pubDate { get; set; }
        public string mediaContent { get; set; }
    }
}
