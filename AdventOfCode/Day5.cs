using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5
    {
        [Theory]
        [InlineData("./test/day5.txt", 143)]
        [InlineData("./input/day5.txt", 5651)]
        public void Part1(string input, int expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            //seperator == blank line
            int middle = inputString.Index().First(a => a.Item == "").Index;

            string[] ordering = inputString[0..middle];
            string[] orders = inputString[(middle + 1)..];

            //parse ordering

            var orderingtuples = ordering.Select(a => (int.Parse(a.Split('|')[0]), int.Parse(a.Split('|')[1])));

            List<int> middles = new();

            foreach (var o in orders)
            {
                var oi = o.Split(',').Select(a => int.Parse(a)).ToArray();

                bool bad = false;
                for (int i = 0; i < oi.Length; i++)
                {
                    //before
                    for(int j = i; j > 0; j--)
                    {
                        if (orderingtuples.Any(a => a.Item1 == oi[i] && a.Item2 == oi[j]))
                        {
                            bad = true; break;
                        }
                    }

                    if (bad)
                    {
                        break;
                    }

                    //after
                    for (int j = i; j < oi.Length; j++)
                    {
                        if (orderingtuples.Any(a => a.Item1 == oi[j] && a.Item2 == oi[i]))
                        {
                            bad = true; break;
                        }
                    }

                    if (bad)
                    {
                        break;
                    }
                }
                
                if (!bad)
                {
                    middles.Add(oi[oi.Length / 2]);
                }
            }

            result = middles.Sum();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day5.txt", 123)]
        [InlineData("./input/day5.txt", 4743)]
        public void Part2(string input, int expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            //seperator == blank line
            int middle = inputString.Index().First(a => a.Item == "").Index;

            string[] ordering = inputString[0..middle];
            string[] orders = inputString[(middle + 1)..];

            //parse ordering
            var orderingtuples = ordering.Select(a => (int.Parse(a.Split('|')[0]), int.Parse(a.Split('|')[1])));

            var ordersSorted = orders.Select(a => a.Split(',').Select(a => int.Parse(a)).ToList()).ToList();

            List<int> middles = new();

            foreach (var order in ordersSorted)
            {
                //copy;
                List<int> orderC = order.ToList();

                //sort
                order.Sort((a, b) => elfsort(a, b, orderingtuples));

                if (!Enumerable.SequenceEqual(orderC, order))
                {
                    //middle
                    middles.Add(order[order.Count / 2]);
                }
            }

            result = middles.Sum();

            Assert.Equal(expected, result);
        }

        private int elfsort(int a, int b, IEnumerable<(int, int)> orderingtuples)
        {
            //less than
            if (orderingtuples.Any(o => o.Item1 == a && o.Item2 == b))
            {
                return -1;
            }

            if (orderingtuples.Any(o => o.Item1 == b && o.Item2 == a))
            {
                return 1;
            }

            return 0;
        }
    }
}
