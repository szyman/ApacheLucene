using EDwI_lab1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace EDwI_lab1.Tools
{
    class Hyperlinks
    {

        public static List<HyperLink> GetHyperlinks(string input, IPAddress ipBaseServer)
        {
            List<HyperLink> result = new List<HyperLink>();
            MatchCollection matchCollection = Regex.Matches(input, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

            foreach (Match matchHyperLink in matchCollection)
            {
                string value = matchHyperLink.Groups[1].Value;
                Match matchLink = Regex.Match(value, @"href=""([^""]*)", RegexOptions.Singleline);

                Uri uri;
                if (matchLink.Success && Uri.TryCreate(matchLink.Groups[1].Value, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttp)
                {
                    //if (!result.Exists(e => e.Url.OriginalString == uri.OriginalString))
                    //{
                        result.Add(new HyperLink(uri, ipBaseServer));
                    //}
                }
            }
            return result;
        }

        public static void Recursive(List<HyperLink> links, IPAddress ipBaseServer, string baseLinksFileName, string externalLinksFileName, ref int level)
        {
            if (level > 0)
            {
                level--;
                foreach (HyperLink hyperLinkModel in links)
                {
                    try
                    {
                        List<HyperLink> list = GetHyperlinks(new Page(hyperLinkModel.Url.AbsoluteUri).document, ipBaseServer);

                        List<string> baseLinks = list.Where(l => l.IsBaseServer == true).Select(l => l.Url.ToString()).ToList();
                        List<string> externalLinks = list.Where(l => l.IsBaseServer == false).Select(l => l.Url.ToString()).ToList();

                        File.AppendAllLines(baseLinksFileName, baseLinks, Encoding.UTF8);
                        File.AppendAllLines(externalLinksFileName, externalLinks, Encoding.UTF8);

                        Recursive(list.Where(l => l.IsBaseServer == true).ToList(), ipBaseServer, baseLinksFileName, externalLinksFileName, ref level);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The remote server returned an error: (403) Forbidden.");
                        break;
                    }
                }
            }
        }

        public static void SaveLinksFile(string pathFile, List<string> list)
        {
            File.WriteAllLines(pathFile, list, Encoding.UTF8);
        }

        public static string GetLinksFile(string pathFile)
        {
            return File.ReadAllText(pathFile);
        }
    }
}
