using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDwI_lab1.Tools
{
    class CompareDocs
    {
        public static List<double> GetVector(Dictionary<string, int> countedWords)
        {
            List<double> result = new List<double>();

            int countWords = countedWords.Sum(s => s.Value);

            foreach (string word in countedWords.Keys)
            {
                result.Add(Convert.ToDouble(countedWords[word]) / countWords);
            }

            return result;
        }

        public static double GetCosinus(List<double> vector1, List<double> vector2)
        {
            double result = 0;

            double numerator = 0;
            int cosCount = vector1.Count < vector2.Count ? vector1.Count : vector2.Count;
            for (int i = 0; i < cosCount; i++)
            {
                numerator += vector1[i] * vector2[i];
            }

            double lengthVector1 = Math.Sqrt(vector1.Sum(s => s * s));
            double lengthVector2 = Math.Sqrt(vector2.Sum(s => s * s));

            double denominator = lengthVector1 * lengthVector2;

            result = denominator != 0 ? numerator / denominator : 0;

            return result;
        }
    }
}
