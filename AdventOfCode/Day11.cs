using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    public class Day11
    {
        private readonly ITestOutputHelper output;

        public Day11(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day11.txt", 55312)]
        [InlineData("./input/day11.txt", 233050)]
        public void Part1(string file, long expected)
        {
            string input = File.ReadAllText(file);
            long result = 0;

            List<long> stones = input.Split(' ').Select(a => long.Parse(a)).ToList();

            int blinks = 25;

            for (int i = 0; i < blinks; i++)
            {
                List<long> newStones = new List<long>();

                foreach (long stone in stones)
                {
                    if (stone == 0)
                    {
                        newStones.Add(1);
                    }
                    else if (stone.ToString().Length % 2 == 0)
                    {
                        string str = stone.ToString();

                        string left = str.Substring(0, str.Length / 2);
                        string right = str.Substring(str.Length / 2);

                        newStones.Add(long.Parse(left));
                        newStones.Add(long.Parse(right));
                    }
                    else
                    {
                        newStones.Add(stone * 2024);
                    }
                }

                stones = newStones;
            }

            result = stones.Count;

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day11.txt", 65601038650482)]
        [InlineData("./input/day11.txt", 276661131175807)]
        public void Part2(string file, long expected)
        {
            string input = File.ReadAllText(file);
            long result = 0;

            List<long> stones1 = input.Split(' ').Select(a => long.Parse(a)).ToList();
            int blinks = 75;

            Dictionary<long, long> stoneCounts = new Dictionary<long, long>();
            foreach (var stone in stones1)
            {
                if (stoneCounts.ContainsKey(stone))
                    stoneCounts[stone]++;
                else
                    stoneCounts.Add(stone, 1);
            }

            foreach(long keystone in stoneCounts.Keys)
            {
                //List<long> stones = new List<long>() { keystone };
                Dictionary<long, long> stones = new Dictionary<long, long>();
                stones.Add(keystone, stoneCounts[keystone]);

                for (int i = 0; i < blinks; i++)
                {
                    //List<long> newStones = new List<long>();
                    Dictionary<long, long> newStones = new Dictionary<long, long>();

                    foreach (long stone in stones.Keys)
                    {
                        if (stone == 0)
                        {
                            if (newStones.ContainsKey(1))
                                newStones[1] += stones[stone];
                            else
                                newStones.Add(1, stones[stone]);

                        }
                        else if (Convert.ToInt64(Math.Floor(Math.Log10(stone) + 1)) % 2 == 0)//(stone.ToString().Length % 2 == 0)
                        {
                            string str = stone.ToString();

                            string left = str.Substring(0, str.Length / 2);
                            string right = str.Substring(str.Length / 2);

                            long lleft = long.Parse(left);
                            long lright = long.Parse(right);

                            if (newStones.ContainsKey(lleft))
                                newStones[lleft] += stones[stone];
                            else
                                newStones.Add(lleft, stones[stone]);

                            if (newStones.ContainsKey(lright))
                                newStones[lright] += stones[stone];
                            else
                                newStones.Add(lright, stones[stone]);
                        }
                        else
                        {
                            long times2024 = stone * 2024;
                            
                            if (newStones.ContainsKey(times2024))
                                newStones[times2024] += stones[stone];
                            else
                                newStones.Add(times2024, stones[stone]);
                        }
                    }

                    stones = newStones;
                }

                //result += stones.Count;

                result += stones.Sum(a => a.Value);
            }

            Assert.Equal(expected, result);
        }
    }
}
