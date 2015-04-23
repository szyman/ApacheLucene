using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace EDwI_lab1.Tools
{
    static class GatherWords
    {

        public static string filterWords(string htmlCode)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlCode);
            
            if (doc.DocumentNode.SelectNodes("//body/script") != null)
            {
                foreach (var item in doc.DocumentNode.SelectNodes("//body/script"))
                {
                    item.Remove();
                }
            }

            //foreach (HtmlNode subNode in doc.DocumentNode.ChildNodes)
            //{
            //    if (subNode.NodeType == HtmlNodeType.Comment)
            //    {
            //        doc.DocumentNode.ParentNode.RemoveChild(subNode);
            //    }
            //}
            
                string text = doc.DocumentNode.SelectSingleNode("//body").InnerText;

            text = Regex.Replace(text, @"<[^>]*>", " ");
            text = Regex.Replace(text, @"<!-- (.+?)-->", " ");
            text = Regex.Replace(text, @"\s+", " ").Trim();
            text = Regex.Replace(text, @"[^\w\s]", " ");

            
            return text;
        }

        public static void saveFilteredWords(string gatheredWords, string outputFile)
        {
            System.IO.File.WriteAllText(@outputFile, gatheredWords.ToLower());
        }

        public static string DownloadHtml(Uri uri)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    using (Stream stream = webClient.OpenRead(uri.AbsoluteUri))
                    {
                        using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
                catch(WebException)
                {
                    Console.WriteLine("Cannot open webpage: " + uri.AbsoluteUri);
                    return "";
                }
                catch(IOException)
                {
                    Console.WriteLine("IOException: " + uri.AbsoluteUri);
                    return "";
                }
            }
        }

        public static string ParseHtmlToText(string input)
        {
            //script and style
            Regex scriptStyleRegex = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)"
                , RegexOptions.Singleline | RegexOptions.IgnoreCase
            );
            input = scriptStyleRegex.Replace(input, "");

            //tags
            Regex tagRegex = new Regex("<.*?>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            input = tagRegex.Replace(input, " ");

            //punctuations mark
            input = new string(input.ToCharArray().Select(c => !char.IsPunctuation(c) ? char.ToLower(c) : ' ').ToArray());

            //<>
            input = input.Replace("<", "");
            input = input.Replace(">", "");

            //white spaces
            input = Regex.Replace(input, @"\s+", " ");

            return input;
        }

        public static string GetText(Uri uri)
        {
            return ParseHtmlToText(DownloadHtml(uri));
        }


    }
}
