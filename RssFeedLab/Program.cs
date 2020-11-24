using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace RssFeedLab
{
    class Program
    {
        /*
          <item>
      <title><![CDATA[Amazon sorry for Sidewalk 'confusion']]></title>
      <description><![CDATA[The new network, which uses other people's broadband, is not launching in the UK.]]></description>
      <link>https://www.bbc.co.uk/news/technology-55059696</link>
      <guid isPermaLink="true">https://www.bbc.co.uk/news/technology-55059696</guid>
      <pubDate>Tue, 24 Nov 2020 18:21:43 GMT</pubDate>
    </item>
          */
        static void Main(string[] args)
        {
            string[] urls = new string[] { "http://feeds.bbci.co.uk/news/world/rss.xml", "http://feeds.bbci.co.uk/news/technology/rss.xml" };
            List<string> result = new List<string>();
            List<Thread> threads = new List<Thread>();

            foreach(var url in urls)
            {
                Thread rssThread = new Thread(() =>
               {
                   GetTop3Feeds(url);
                   result.AddRange(GetTop3Feeds(url));
               });

                threads.Add(rssThread);
            }

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join(5000));

        }

        private static string[] GetTop3Feeds(string url)
        {
            var doc = XDocument.Load(url);

            var query = (from channelItem in doc.Descendants("item")
                        select channelItem.Element("title").Value).Take(3);

            return query.ToArray();
        }
    }
}
