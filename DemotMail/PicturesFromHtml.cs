using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace DemotMail
{
    class PicturesFromHtml
    {
        private readonly string _url;

        public PicturesFromHtml(string url)
        {
            this._url = url;
        }

        /// <summary>
        /// Przeszukuje podaną stronę internetową i dodaje adresy plików do załaczenia do listy files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="phrase"></param>
        public void AdFileIf(List<string> files, string phrase)
        {
            LogFile.AddLog("Rozpoczęto przeszukiwanie strony w poszukiwaniu plików do załączenia");
            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            var html = System.Net.WebUtility.HtmlDecode(wc.DownloadString(_url));

            var doc = new HtmlDocument();
            var pageHtml = html;
            doc.LoadHtml(pageHtml);
            var nodes = doc.DocumentNode.Descendants("img");

            foreach (var node in nodes)
            {
                if(node.GetAttributeValue("alt","").Contains(phrase))
                {        
                    files.Add(node.GetAttributeValue("src", ""));
                    LogFile.AddLog("Dodano plik do listy o adresie " + node.GetAttributeValue("src", ""));
                }
            }

        }

    }
}
