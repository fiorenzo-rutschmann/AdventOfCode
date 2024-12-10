using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day10
    {
        private readonly ITestOutputHelper output;

        public Day10(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day10.txt", 1)]
        [InlineData("./test/day10_1.txt", 2)]
        [InlineData("./test/day10_2.txt", 4)]
        [InlineData("./test/day10_3.txt", 3)]
        [InlineData("./test/day10_4.txt", 36)]
        [InlineData("./input/day10.txt", 538)]
        public void Part1(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            char[][] map = input.Select(a => a.ToCharArray()).ToArray();
            long result = 0;

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '0')
                    {
                        var output = calculatePaths(map, (y, x));
                        result += output.Distinct().Count();
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        public List<(int,int)> calculatePaths(char[][] map, (int, int) index)
        {
            var ret = new List<(int,int)> ();

            //recursive function
            char current = map[index.Item1][index.Item2];

            if (current == '.')
            {
                return ret;
            }

            if (current == '9')
                return new List<(int, int)>() { index };

            int currentInt = int.Parse(current.ToString());

            //up
            int up = index.Item1 - 1;
            if (up >= 0 && up < map.Length && Char.IsDigit(map[up][index.Item2]) && int.Parse(map[up][index.Item2].ToString()) == (currentInt+1))
            {
                ret.AddRange(calculatePaths(map, (up, index.Item2)));
            }

            //down
            int down = index.Item1 + 1;
            if (down >= 0 && down < map.Length && Char.IsDigit(map[down][index.Item2]) && int.Parse(map[down][index.Item2].ToString()) == (currentInt + 1))
            {
                ret.AddRange(calculatePaths(map, (down, index.Item2)));
            }

            //left
            int left = index.Item2 - 1;
            if (left >= 0 && left < map[0].Length && Char.IsDigit(map[index.Item1][left]) && int.Parse(map[index.Item1][left].ToString()) == (currentInt + 1))
            {
                ret.AddRange(calculatePaths(map, (index.Item1, left)));
            }

            //right
            int right = index.Item2 + 1;
            if (right >= 0 && right < map[0].Length && Char.IsDigit(map[index.Item1][right]) && int.Parse(map[index.Item1][right].ToString()) == (currentInt + 1))
            {
                ret.AddRange(calculatePaths(map, (index.Item1, right)));
            }

            return ret;
        }

        [Theory]
        [InlineData("./test/day10_4.txt", 81)]
        [InlineData("./input/day10.txt", 1110)]
        public void Part2(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            char[][] map = input.Select(a => a.ToCharArray()).ToArray();
            long result = 0;


            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '0')
                    {
                        var output = calculatePaths(map, (y, x));
                        result += output.Count();
                    }
                }
            }

            Assert.Equal(expected, result);
        }
    }
}
