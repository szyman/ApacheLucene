
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Documents;
using EDwI_lab1.Model;
using System.IO.Compression;
using Ionic.Zip;
using System.IO;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using System.Text.RegularExpressions;

namespace EDwI_lab1.Tools
{
    class LuceneIndexing
    {
        private Analyzer AnalyzerLucene { get; set; }
        private Lucene.Net.Store.Directory DirectoryLucene { get; set; }
        public IndexWriter IndexWriterLucene { get; set; }
        public IndexReader IndexReaderLucene { get; set; }
        private string IndexPath { get; set; }

        public LuceneIndexing(string indexPath, bool getIndexedBooks, bool continueIndexing)
        {
            IndexPath = indexPath;
            DirectoryLucene = FSDirectory.Open(IndexPath);
            AnalyzerLucene = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            if (getIndexedBooks == false)
            {
                if (System.IO.Directory.Exists(IndexPath) && continueIndexing == false)
                    System.IO.Directory.Delete(IndexPath, true);
                IndexWriterLucene = new IndexWriter(DirectoryLucene, AnalyzerLucene, IndexWriter.MaxFieldLength.UNLIMITED);

            }
            
            IndexReaderLucene = IndexReader.Open(DirectoryLucene, true);

            //AnalyzerLucene = new WhitespaceAnalyzer();
        }

        public void BuildTitleIndex(List<Book> dataToIndex)
        {
            foreach (Book book in dataToIndex)
            {
                Document document = new Document();
                document.Add(new Field("Title", book.Title, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Path", book.Path, Field.Store.YES, Field.Index.ANALYZED));
                IndexWriterLucene.AddDocument(document);
            }

        }

        public void BuildTitleContentIndex(List<Book> dataToIndex)
        {
            int i = 0;
            foreach (Book book in dataToIndex)
            {
                Document document = new Document();
                document.Add(new Field("Title", book.Title, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Content", book.Content, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Path", book.Path, Field.Store.YES, Field.Index.ANALYZED));
                IndexWriterLucene.AddDocument(document);
                i++;
            }
            Console.WriteLine("finished");
            //IndexWriterLucene.Optimize();
            //IndexWriterLucene.Dispose();
            //DirectoryLucene.Dispose();
        }

        public List<Book> ReadTitleBooks(string directory)
        {
            List<Book> result = new List<Book>();
            string[] zipFiles = System.IO.Directory.GetFiles(directory, "*.zip", System.IO.SearchOption.AllDirectories).ToArray();

            foreach (string name in zipFiles)
            {
                ZipFile zipArchive = ZipFile.Read(name);
                foreach (var file in zipArchive.Entries.Where(w => w.FileName.EndsWith(".txt")))
                {
                    using (StreamReader streamReader = new StreamReader(file.OpenReader()))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (line.StartsWith("Title:"))
                            {
                                result.Add(new Book()
                                {
                                    Title = line.Remove(0, "Title: ".Length),
                                    Path = name
                                });
                                break;
                            }
                        }
                    }

                }
            }
            return result.OrderBy(o => o.Title).ToList();
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int start, end;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                start = strSource.IndexOf(strStart, 0) + strStart.Length;
                end = strSource.IndexOf(strEnd, start);
                return strSource.Substring(start, end - start);
            }
            else
            {
                return "";
            }
        }

        public List<Book> ReadContentBooks(string directory)
        {
            List<Book> result = new List<Book>();
            string[] zipFiles = System.IO.Directory.GetFiles(directory, "*.zip", System.IO.SearchOption.AllDirectories).ToArray();

            string content;
            char[] c = null;

            foreach (string name in zipFiles)
            {
                try
                {
                    ZipFile zipArchive = ZipFile.Read(name);

                    foreach (var file in zipArchive.Entries.Where(w => w.FileName.EndsWith(".txt")))
                    {
                        using (StreamReader streamReader = new StreamReader(file.OpenReader()))
                        {

                                //string book = streamReader.ReadToEnd();
                            string book = "";
                            while (streamReader.Peek() >= 0)
                                {
                                    c = new char[1024 * 1024];
                                    streamReader.Read(c, 0, c.Length);
                                    //The output will look odd, because
                                    //only five characters are read at a time.
                                    book = new string(c);
                                }

                                streamReader.Dispose();

                                string title = getBetween(book, "Title: ", "\n").TrimEnd();

                                var regex = new Regex("[*][*][*].+[*][*][*]");
                                var matches = regex.Matches(book);
                                if (matches.Count > 1)
                                    content = book.Substring(matches[0].Index + matches[0].Length, matches[1].Index - matches[0].Index - matches[0].Length).Trim();
                                else
                                {
                                    Console.WriteLine("Regex not matched");
                                    content = book;
                                }
                                result.Add(new Book()
                                {
                                    Title = title,
                                    Path = name,
                                    Content = content
                                });
                            }

                        }
                    }
                
                catch(ZipException)
                {
                    Console.WriteLine("ZipException: " + directory + " , " + name);
                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("OutOfMemoryException: " + directory + " , " + name);
                }
            }
            return result;
        }

        public List<Book> SearchTitle(string searchTerm, int countPerPage, int currentPage, string term)
        {
            List<Book> results = new List<Book>();

            IndexSearcher searcher = new IndexSearcher(IndexReader.Open(DirectoryLucene, true));
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, term, AnalyzerLucene);

            Query query = parser.Parse(searchTerm);

            TopDocs topDocs = searcher.Search(query, currentPage * countPerPage);
            int count = topDocs.TotalHits;

            for (int i = (currentPage - 1) * countPerPage;
                i < currentPage * countPerPage && i < topDocs.ScoreDocs.Length;
                i++)
            {
                Document currentDocument = searcher.Doc(topDocs.ScoreDocs[i].Doc);
                results.Add(new Book()
                {
                    Title = currentDocument.Get("Title"),
                    Path = currentDocument.Get("Path"),
                    Content = currentDocument.Get("Content")
                });
            }

            return results;
        }

        public List<Book> SearchTitleContent(string searchTerm, int countPerPage, int currentPage, string term)
        {
            List<Book> results = new List<Book>();
            IndexSearcher searcher = new IndexSearcher(IndexReaderLucene);

            var mainQuery = new BooleanQuery();
            mainQuery.Add(new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", AnalyzerLucene).Parse(searchTerm)
                , Occur.SHOULD);
            mainQuery.Add(new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Content", AnalyzerLucene).Parse(searchTerm)
                , Occur.MUST);
            PhraseQuery query = new PhraseQuery();
            String[] words = searchTerm.Split(new Char[] { ' ' });
            foreach (String word in words)
            {
                query.Add(new Term("Content", word));
            }
            TopDocs topDocs = searcher.Search(query, currentPage * countPerPage);
            int count = topDocs.TotalHits;

            for (int i = (currentPage - 1) * countPerPage;
                i < currentPage * countPerPage && i < topDocs.ScoreDocs.Length;
                i++)
            {
                Document currentDocument = searcher.Doc(topDocs.ScoreDocs[i].Doc);
                results.Add(new Book()
                {
                    Title = currentDocument.Get("Title"),
                    Path = currentDocument.Get("Path"),
                    Content = currentDocument.Get("Content")
                });
            }

            //DirectoryLucene.Dispose();
            return results;
        }


        //Lab 7


        public void BuildWebPagesIndex(List<WebPage> dataToIndex)
        {
            int i = 0;
            foreach (WebPage item in dataToIndex)
            {
                Document document = new Document();
                document.Add(new Field("Content", item.Content, Field.Store.YES, Field.Index.ANALYZED));
                document.Add(new Field("Address", item.Address, Field.Store.YES, Field.Index.ANALYZED));
                IndexWriterLucene.AddDocument(document);
                i++;
            }
            IndexWriterLucene.Optimize();
            IndexWriterLucene.Dispose();
            //DirectoryLucene.Dispose();
        }

        public List<WebPage> searchWebPage(String search, int count)
        {
            List<WebPage> result = new List<WebPage>();

            IndexSearcher searcher = new IndexSearcher(IndexReader.Open(DirectoryLucene, true));
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Content", AnalyzerLucene);


            //Query query = parser.Parse(search);
            
            PhraseQuery query = new PhraseQuery();
            String[] words = search.Split(new Char[] {' '});
        foreach (String word in words) {
            query.Add(new Term("Content", word));
        }

        TopScoreDocCollector collector = TopScoreDocCollector.Create(100, true);
        searcher.Search(query, collector);
        ScoreDoc[] hits = collector.TopDocs().ScoreDocs;
            TopDocs topDocs = searcher.Search(query, 1);
            int total = topDocs.TotalHits;

/*
for(int i=0;i<hits.Length;++i) {
    int docId = hits[i].Doc;
    Document d = searcher.Doc(docId);
    Console.WriteLine("Url:" + d.Get("Address"));
    Console.WriteLine("Content:" + d.Get("Content"));
}
 * */
            if (total > 0)
            {
                //topDocs = searcher.Search(query, total);
                for (int i = 0; i < hits.Length; i++)
                {
                    TermDocs termDocs = IndexReaderLucene.TermDocs();
                    termDocs.Seek(new Term("Content", search));
                    termDocs.SkipTo(hits[i].Doc);
                    int docId = hits[i].Doc;
                    Document currentDocument = searcher.Doc(docId);

                    result.Add(new WebPage(currentDocument.Get("Address"))
                    {
                        Hits = termDocs.Freq
                    });
                }
            }

            return result.OrderByDescending(o => o.Hits).Take(count).ToList();
        }

    }
}
