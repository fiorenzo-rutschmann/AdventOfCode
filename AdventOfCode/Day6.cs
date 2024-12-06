using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day6
    {
        [Theory]
        [InlineData("./test/day6.txt", 41)]
        [InlineData("./input/day6.txt", 4374)]
        public void Part1(string input, int expected)
        {
            string[] inputString = File.ReadAllLines(input);

            char[][] map = inputString.Select(a => a.ToCharArray()).ToArray();

            int x = 0, y = 0;

            bool found = false;
            for ( y = 0; y < map.Length; y++) 
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '^')
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            int vx = 0, vy = -1, vz;

            int count = 0;

            while ( (x + vx) >= 0 && (x + vx) < map[1].Length && (y + vy) >= 0 && (y + vy) < map.Length)
            {
                if (map[(y + vy)][(x + vx)] == '#')
                {
                    //rotate right
                    vy *= -1;
                    vz = vx;
                    vx = vy;
                    vy = vz;

                    continue;
                }

                map[y][x] = 'X';
                count++;

                //update
                x = x + vx;
                y = y + vy;

            }

            //last square
            map[y][x] = 'X';

            long result = 0;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'X')
                    {
                        result++;
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day6.txt", 6)]
        [InlineData("./input/day6.txt", 1705)]
        public void Part2(string input, int expected)
        {
            string[] inputString = File.ReadAllLines(input);

            char[][] map = inputString.Select(a => a.ToCharArray()).ToArray();

            int x = 0, y = 0;

            bool found = false;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '^')
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            int startX = x, startY = y; 

            int vx = 0, vy = -1, vz;

            int count = 0;

            while ((x + vx) >= 0 && (x + vx) < map[1].Length && (y + vy) >= 0 && (y + vy) < map.Length)
            {
                if (map[(y + vy)][(x + vx)] == '#')
                {
                    //rotate right
                    vy *= -1;
                    vz = vx;
                    vx = vy;
                    vy = vz;

                    continue;
                }

                map[y][x] = 'X';
                count++;

                //update
                x = x + vx;
                y = y + vy;

            }

            //last square
            map[y][x] = 'X';

            long part1Result = 0;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'X')
                    {
                        part1Result++;
                    }
                }
            }


            //hint for optimisation mark the square you rotate, if you hit that on a rotate again then its infinite loop but brute force better hahah
            int part2Result = 0;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    //place obsticle on path.
                    if (map[y][x] == 'X')
                    {
                        map[y][x] = 'O';
                        if (ThresholdCount(map, startX, startY))
                        {
                            part2Result++;
                        }
                        map[y][x] = 'X';
                    }
                }
            }

            Assert.Equal(expected, part2Result);
        }



        private bool ThresholdCount(char[][] map, int x, int y, int threshold = 5000)
        {
            int vx = 0, vy = -1, vz;

            int count = 0;

            while ((x + vx) >= 0 && (x + vx) < map[1].Length && (y + vy) >= 0 && (y + vy) < map.Length)
            {
                if (map[(y + vy)][(x + vx)] == '#' || map[(y + vy)][(x + vx)] == 'O')
                {
                    //rotate right
                    vy *= -1;
                    vz = vx;
                    vx = vy;
                    vy = vz;

                    continue;
                }

                //map[y][x] = 'X';
                count++;

                //update
                x = x + vx;
                y = y + vy;

                if (count > threshold)
                {
                    break;
                }

            }

            //last square
            map[y][x] = 'X';

            long part1Result = 0;
            for (y = 0; y < map.Length; y++)
            {
                for (x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == 'X')
                    {
                        part1Result++;
                    }
                }
            }

            if (count > threshold)
            {
                return true;
            }

            return false;

        }
    }
}
