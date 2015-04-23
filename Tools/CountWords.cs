using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDwI_lab1.Tools
{
    class CountWords
    {
        private int THRESH;
        private int k;

        //public Dictionary<string, int> countedWords { get; set; }

        public CountWords(int THRESH, int k)
        {
            this.THRESH = THRESH;
            this.k = k;
        }

        public Dictionary<string, int> countWordsFromFile(string inputFileName)
        {
            Dictionary<string, int> wordsCountsDict = new Dictionary<string, int>();
            using (var sr = new StreamReader(@inputFileName, Encoding.Default))
            {
                foreach (var word in sr.ReadLine().Split(' '))
                {
                    int c = 0;
                    if (wordsCountsDict.TryGetValue(word, out c))
                    {
                        wordsCountsDict[word] = c + 1;
                    }
                    else
                    {
                        wordsCountsDict.Add(word, 1);
                    }
                }
            }
            return wordsCountsDict;
        }

        public static Dictionary<string, int> countWordsFromString(string gatheredWords)
        {
            Dictionary<string, int> wordsCountsDict = new Dictionary<string, int>();

                foreach (var word in gatheredWords.Split(' '))
                {
                    int c = 0;
                    if (wordsCountsDict.TryGetValue(word, out c))
                    {
                        wordsCountsDict[word] = c + 1;
                    }
                    else
                    {
                        wordsCountsDict.Add(word, 1);
                    }
                }
            return wordsCountsDict;
        }

        public void saveCountedWordsToFile(Dictionary<string, int> countedWorlds, string outputFileName)
        {

            var sortedDict = from entry in countedWorlds orderby entry.Value descending select entry;

            using (var sw = new StreamWriter(outputFileName))
            {

                int i = 0;
                foreach (var row in sortedDict)
                {
                    if (row.Value <= THRESH || i >= k)
                        break;
                    //Console.WriteLine(row.Key + "\t-\t" + row.Value);
                    sw.WriteLine(row.Key + "\t-\t" + row.Value);
                    i++;
                }

                sw.Close();
            }
            Console.WriteLine("finish");
        }
    }
}
