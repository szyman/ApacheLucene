using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using NReadability;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using EDwI_lab1.Model;
using EDwI_lab1.Tools;
using EDwI_lab1.View;
using System.Windows.Forms;

//http://stackoverflow.com/questions/980073/hashset-intersectwith-count-words-but-only-unique
namespace EDwI_lab2
{
    class Program
    {

        private static readonly string pageAddress1 = "http://en.wikipedia.org/wiki/Popoli";

        private static readonly string[] pageAddressArray = new string[] {
        "http://en.wikipedia.org/wiki/Bagrat_II_Bagratuni", 
        "http://en.wikipedia.org/wiki/Popoli",
        "http://en.wikipedia.org/wiki/James_Whiteside_McCay",
        "http://en.wikipedia.org/wiki/River_City_Star",
        "http://en.wikipedia.org/wiki/Marius_Lyle",
        "http://en.wikipedia.org/wiki/Sea_Breeze,_New_Jersey",
        "http://en.wikipedia.org/wiki/Baron_Ribblesdale",
        "http://en.wikipedia.org/wiki/Selective_fire",
        "http://en.wikipedia.org/wiki/Barbara_Merrill",
        "http://en.wikipedia.org/wiki/Sacred_Heart_Church,_St_Ives",
        "http://en.wikipedia.org/wiki/Andrei_Markovits",
        "http://en.wikipedia.org/wiki/Place_Sathonay",
        "http://en.wikipedia.org/wiki/Laura_Branigan_%28album%29",
        "http://en.wikipedia.org/wiki/Matt_Lashoff",
        "http://en.wikipedia.org/wiki/Dahiyat_Qudsaya"
        };

        private static readonly int k = 10;
        private static readonly int THRESH = 4;

        private static readonly string htmlPageFileName1 = "!downloadedPage.html";
        private static readonly string htmlPageFileName2 = "!downloadedPage2.html";

        private static readonly string gatherWordsFileName1 = "!filteredPage.txt";
        private static readonly string gatherWordsFileName2 = "!filteredPage2.txt";

        private static readonly string CountWordsFileName1 = "!wordCounter.txt";
        private static readonly string CountWordsFileName2 = "!wordCounter2.txt";


        private static readonly int docsCount = 15;
        private static Page[] docs = new Page[docsCount]; 


        static void Main(string[] args)
        {
            /*
            Page doc = new Page(pageAddress1);
           
            //Zad1
            string gatheredWords = GatherWords.filterWords(doc.document);
            GatherWords.saveFilteredWords(gatheredWords, gatherWordsFileName1);
            
            //Zad2
            CountWords countWords1 = new CountWords(THRESH, k);
            var countedWordsDoc1 = countWords1.countWordsFromFile(gatherWordsFileName1);
            countWords1.saveCountedWordsToFile(countedWordsDoc1, CountWordsFileName1);
            
            //Zad3
            //var verctorDoc1 = CompareDocs.GetVector(countedWordsDoc1);
            //var verctorDoc2 = CompareDocs.GetVector(countedWordsDoc2);

            //double docsSimilarity = CompareDocs.GetCosinus(verctorDoc1, verctorDoc2);

            

            for(int i = 0; i < docsCount; i++)
            {
                    docs[i] = new Page(pageAddressArray[i]);
                    gatheredWords = GatherWords.filterWords(docs[i].document);
                    docs[i].countedWords = CountWords.countWordsFromString(gatheredWords);
            }

            int compareCount = (docsCount * (docsCount - 1)) / 2;
            Dictionary<string, double> compareResults = new Dictionary<string, double>();
            for (int i = 0; i < docsCount; i++)
            {
                var verctorDoc1 = CompareDocs.GetVector(docs[i].countedWords);
                for (int j = 0; j < docsCount; j++)
                {
                    if (i == j)
                        break;
                    var verctorDoc2 = CompareDocs.GetVector(docs[j].countedWords);
                    compareResults.Add(docs[i].name + " - " + docs[j].name, CompareDocs.GetCosinus(verctorDoc1, verctorDoc2));
                    //Console.WriteLine(docs[i].name + "\t" + docs[j].name + "\t" + CompareDocs.GetCosinus(verctorDoc1, verctorDoc2));
                }
            }


            //Zad4
            Page page = new Page("http://kis.p.lodz.pl/");
            IPAddress[] ip_Addresses = Dns.GetHostAddresses("kis.p.lodz.pl");
            List<HyperLink> list = Hyperlinks.GetHyperlinks(page.document, ip_Addresses[0]);
            List<string> baseLinks = list.Where(l => l.IsBaseServer == true).Select(l=>l.Url.ToString()).ToList();
            List<string> externalLinks = list.Where(l => l.IsBaseServer == false).Select(l => l.Url.ToString()).ToList();

            Hyperlinks.SaveLinksFile("baseLinks.txt", baseLinks);
            Hyperlinks.SaveLinksFile("externalLinks.txt", externalLinks);

            int level = 1;
            Hyperlinks.Recursive(list, ip_Addresses[0], "baseLinks.txt", "externalLinks.txt", ref level);
           // Hyperlinks.Recursive(list, ip_Addresses[0], "baseLinks.txt", "externalLinks.txt", 2);

            //Zad5
            int indexedBooks = 0; //powinno byc 20tys-30tys
            LuceneIndexing lucene = new LuceneIndexing(@"D:\temp");
            for (int i = 1; i <= 9; i++)
            {
                List<Book> books = lucene.ReadTitleBooks(@"H:\" + i);
                lucene.BuildTitleIndex(books);
                indexedBooks += books.Count;
            }
            lucene.IndexWriterLucene.Optimize();
            lucene.IndexWriterLucene.Dispose();
            List<Book> searchResults = lucene.SearchTitle("Christmas", 100, 1, "Title");
            lucene.BuildTitleIndex(lucene.ReadTitleBooks(@"J:\1\0"));
            */

            //Zad6
            
            //int indexedBooks = 0; //powinno byc 20tys-30tys
            //LuceneIndexing lucene = new LuceneIndexing(@"C:\webmining\content_books3", true, true);
            
            //for (int i = 1; i <= 9; i++)
            //{
            /*
            List<Book> books = lucene.ReadContentBooks(@"F:\7");
                lucene.BuildTitleContentIndex(books);
                indexedBooks += books.Count;
            
            //}
            lucene.IndexWriterLucene.Optimize();
            lucene.IndexWriterLucene.Dispose();
            */
            
            
            //indexedBooks = lucene.IndexReaderLucene.NumDocs();
            //List<Book> searchResults = lucene.SearchTitleContent("nursing practice theory", 100, 1, "Content");
            
            
            //Zad7
            Application.EnableVisualStyles();
            Application.Run(new InternetBotForm(true));
            

            Console.WriteLine("Done");

        }
    }
}
