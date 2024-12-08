using System.Collections.Generic;
using System.Numerics;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day8
    {
        private readonly ITestOutputHelper output;

        public Day8(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day8.txt", 14)]
        [InlineData("./test/day8_1.txt", 2)]
        [InlineData("./test/day8_2.txt", 4)]
        [InlineData("./test/day8_3.txt", 4)]
        [InlineData("./input/day8.txt", 301)]
        public void Part1(string input, long expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            char[][] map = inputString.Select(a => a.ToCharArray()).ToArray();

            int width = map[0].Length;
            int height = map.Length;

            //parse the puzzle
            Dictionary<char, List<(int y, int x)>> antenna = new();
            List<(int y, int x)> antinodes = new();

            int y, x;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '.')
                    {
                        antenna.TryAdd(map[y][x], new());
                        antenna[map[y][x]].Add((y, x));
                    }
                } 
            }

            foreach(var item in antenna )
            {
                for(int i = 0; i < item.Value.Count; i++)
                {
                    var pos1 = item.Value[i];
                    for (int j = i+1; j < item.Value.Count; j++)
                    {
                        var pos2 = item.Value[j];


                        //anti1
                        int anti1X = pos1.x + (pos1.x - pos2.x);
                        int anti1Y = pos1.y + (pos1.y - pos2.y);

                        //anti1 keep logic same as if they == then we want the opposite 
                        int anti2X = pos2.x + (pos2.x - pos1.x);
                        int anti2Y = pos2.y + (pos2.y - pos1.y);

                        //int absX = Math.Abs(pos1.x - pos2.x) * 2;
                        //int absY = Math.Abs(pos1.y - pos2.y) * 2;

                        ////anti1
                        //int anti1X = pos1.x > pos2.x ? pos1.x + absX : pos1.x - absX;
                        //int anti1Y = pos1.y > pos2.y ? pos1.y + absX : pos1.y - absX;

                        ////anti1 keep logic same as if they == then we want the opposite 
                        //int anti2X = pos1.x > pos2.x ? pos2.x - absX : pos2.x + absX;
                        //int anti2Y = pos1.y > pos2.y ? pos2.y - absX : pos2.y + absX;


                        antinodes.Add((anti1Y, anti1X));
                        antinodes.Add((anti2Y, anti2X));
                    }
                }
            }

            //distinct + out of bounds + not contained in antenna
            var AntennaPoints = antenna.SelectMany(d => d.Value).ToList();
            antinodes = antinodes.Distinct().Where(a => (a.x >= 0 && a.x < width) && (a.y >= 0 && a.y < height))/*.Where(a => !AntennaPoints.Contains(a))*/.ToList();

            foreach(var p in antinodes)
            {
                map[p.y][p.x] = '#';
            }

            foreach (var m in map)
            {
                output.WriteLine(new string(m));
            }

            result = antinodes.Count;
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day8.txt", 34)]
        [InlineData("./test/day8_4.txt", 9)]
        [InlineData("./input/day8.txt", 0)]
        public void Part2(string input, long expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            char[][] map = inputString.Select(a => a.ToCharArray()).ToArray();

            int width = map[0].Length;
            int height = map.Length;

            //parse the puzzle
            Dictionary<char, List<(int y, int x)>> antenna = new();
            List<(int y, int x)> antinodes = new();

            int y, x;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '.')
                    {
                        antenna.TryAdd(map[y][x], new());
                        antenna[map[y][x]].Add((y, x));
                    }
                }
            }

            foreach (var item in antenna)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    var pos1 = item.Value[i];
                    for (int j = i + 1; j < item.Value.Count; j++)
                    {
                        var pos2 = item.Value[j];

                        //anti1
                        var anti1V = pos1;

                        anti1V.x += (pos1.x - pos2.x);
                        anti1V.y += (pos1.y - pos2.y);

                        while ((anti1V.x >= 0 && anti1V.x < width) && (anti1V.y >= 0 && anti1V.y < height))
                        {
                            antinodes.Add(anti1V);

                            anti1V.x += (pos1.x - pos2.x);
                            anti1V.y += (pos1.y - pos2.y);
                        }

                        //anti2
                        var anti2V = pos1;

                        anti2V.x += (pos2.x - pos1.x);
                        anti2V.y += (pos2.y - pos1.y);

                        while ((anti2V.x >= 0 && anti2V.x < width) && (anti2V.y >= 0 && anti2V.y < height))
                        {
                            antinodes.Add(anti2V);

                            anti2V.x += (pos2.x - pos1.x);
                            anti2V.y += (pos2.y - pos1.y);
                        }
                    }
                }
            }

            //distinct + out of bounds + not contained in antenna
            var AntennaPoints = antenna.SelectMany(d => d.Value).ToList();

            //add antenna that are atleast 2
            foreach (var ant in antenna)
            {
                if (ant.Value.Count > 1)
                {
                    antinodes.AddRange(ant.Value);
                }
            }

            antinodes = antinodes.Distinct().Where(a => (a.x >= 0 && a.x < width) && (a.y >= 0 && a.y < height))/*.Where(a => !AntennaPoints.Contains(a))*/.ToList();

            foreach (var p in antinodes)
            {
                map[p.y][p.x] = '#';
            }

            foreach (var m in map)
            {
                output.WriteLine(new string(m));
            }

            result = antinodes.Count;
            Assert.Equal(expected, result);
        }
    }
}
