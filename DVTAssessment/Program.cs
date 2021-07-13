using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVTAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            int aCount = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> a = new List<string>();

            for (int i = 0; i < aCount; i++)
            {
                string aItem = Console.ReadLine();
                a.Add(aItem);
            }

            int bCount = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> b = new List<string>();

            for (int i = 0; i < bCount; i++)
            {
                string bItem = Console.ReadLine();
                b.Add(bItem);
            }

            commonSubstring(a, b);
        }

        private static readonly object _Lock = new object();
        public static void commonSubstring(List<string> a, List<string> b)
        {
            //aCount and bCount are said to be both "n" from the instructions byt inputs allow them to be different from main
            //Thus I'll be finding the highest common index from both list to avoid and IndexOutOfBounds exception
            int maxCommonItemsCount = a.Count > b.Count ? b.Count : a.Count;
            Dictionary<int, string> result = new Dictionary<int, string>();

            Parallel.For(0, maxCommonItemsCount, i =>
            {
                //for (int i = 0; i < maxCommonItemsCount; i++)
                //{
                IEnumerable<string> aCombinations = from x in Enumerable.Range(0, a[i].Length)
                                                    from y in Enumerable.Range(0, a[i].Length - x + 1)
                                                    where y > 0
                                                    select a[i].Substring(x, y);

                IEnumerable<string> bCombinations = from x in Enumerable.Range(0, b[i].Length)
                                                    from y in Enumerable.Range(0, b[i].Length - x + 1)
                                                    where y > 0
                                                    select b[i].Substring(x, y);

                //bool containsValue = false;
                //foreach(string value in aCombinations)
                //{
                //    b[i].Contains(value);
                //    if (b[i].Contains(value))
                //    {
                //        containsValue = true;
                //        break;
                //    }
                //}
                IEnumerable<string> afd = aCombinations.Intersect(bCombinations);

                lock (_Lock)
                {
                    result.Add(i, afd.Any() ? "YES" : "NO");
                }

                //}
            });

            Console.WriteLine(string.Join("\n", result.OrderBy(k => k.Key).Select(v => v.Value)));

            //Console.WriteLine(afd.Any() ? "YES" : "NO");
        }
    }
}
