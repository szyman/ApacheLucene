using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EDwI_lab1.Model
{
    class Page
    {
        public string name { get; set; }
        public string document { get; set; }
        public string splitedWords { get; set; }
        public Dictionary<string, int> countedWords { get; set; } 


        public Page(string url)
        {
            gaterHtmlCode(url);
            this.name = url;
        }

        private void gaterHtmlCode(string url)
        {
            using (WebClient client = new WebClient())
            {
                using (Stream stream = client.OpenRead(url))
                {
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        document = streamReader.ReadToEnd();
                    }
                }
            }
        }

        public void saveToFile(string url, string pageFileName)
        {
            using (WebClient client = new WebClient())                        
            {
                client.DownloadFile(url, @pageFileName);
            }
        }

        
    }
}
