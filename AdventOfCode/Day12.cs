using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using Xunit.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode
{
    public class Day12
    {
        private readonly ITestOutputHelper output;

        public Day12(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day12.txt", 140)]
        [InlineData("./input/day12.txt", 1363484)]
        public void Part1(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            char[][] map = input.Select(a => a.ToCharArray()).ToArray();

            Dictionary<char, int> fences = new Dictionary<char, int>();

            for (int row = 0; row < map.Length; row++) 
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    char c = map[row][col];
                    if ( c != ' ')
                    {
                        int count =  CountFencesForGarden(map, row, col, c);
                        result += count;
                        //output.WriteLine($"{c} = {count}");
                    }
                }
            }


            Assert.Equal(expected, result);
        }

        private (int, int)[] NSEW = [(-1, 0), (+1, 0), (0, -1), (0, +1)];

        

        private int CountFencesForGarden(char[][] map, int row, int col, char c)
        {
            //List<(int, int)> garden = new List<(int, int)> ();

            int fences = 0;
            int area = 0;
            char gardenchar = map[row][col];

            Queue<(int, int)> coords = new Queue<(int, int)> ();

            HashSet<(int, int)> seen = new HashSet<(int, int)>();

            coords.Enqueue((row, col));
            seen.Add((row, col));

            while (coords.Count > 0)
            {
                (int, int) coord = coords.Dequeue();

                area++;

                foreach((int,int) i in NSEW)
                {
                    int rowy = i.Item1 + coord.Item1;
                    int colx = i.Item2 + coord.Item2;

                    if (rowy >= 0 && rowy < map.Length && colx >= 0 && colx < map[0].Length)
                    {
                        char next = map[rowy][colx];

                        if (seen.Contains((rowy,colx)))
                        {
                            continue;
                        }

                        if (next == gardenchar )
                        {
                            coords.Enqueue((rowy, colx));
                            seen.Add((rowy, colx));
                        }
                        else
                        {
                            fences++;
                        }
                    }
                    else
                    {
                        fences++;
                    }
                }
            }

            //wipe the garden
            foreach(var coord in seen)
            {
                map[coord.Item1][coord.Item2] = ' ';
            }

            output.WriteLine($"{gardenchar} = {area} * {fences} = {area * fences}");
            return fences * area;
        }


        [Theory]
        [InlineData("./test/day12.txt", 80)]
        [InlineData("./test/day12_1.txt", 436)]
        [InlineData("./test/day12_3.txt", 236)]
        [InlineData("./input/day12.txt", 276661131175807)]
        public void Part2(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            char[][] map = input.Select(a => a.ToCharArray()).ToArray();

            Dictionary<char, int> fences = new Dictionary<char, int>();

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    char c = map[row][col];
                    if (c != ' ')
                    {
                        int count = CountFencesForGardenP2(map, row, col, c);
                        result += count;
                        //output.WriteLine($"{c} = {count}");
                    }
                }
            }


            Assert.Equal(expected, result);
        }

        public struct FenceSection
        {
            public int row;
            public int col;
            public (int, int) direction;
        }

        private (int, int)[][] tangentNSEW = [[(0, +1), (0, -1)], [(0, +1), (0, -1)], [(+1, 0), (-1, 0)], [(+1, 0), (-1, 0)]];

        private int CountFencesForGardenP2(char[][] map, int row, int col, char c)
        {
            //List<(int, int)> garden = new List<(int, int)> ();

            int fences = 0;
            int area = 0;
            char gardenchar = map[row][col];

            Queue<(int, int)> coords = new Queue<(int, int)>();

            HashSet<(int, int)> seen = new HashSet<(int, int)>();
            HashSet<FenceSection> fs = new HashSet<FenceSection>();

            coords.Enqueue((row, col));
            seen.Add((row, col));

            while (coords.Count > 0)
            {
                (int, int) coord = coords.Dequeue();

                area++;

                for( int j = 0; j < NSEW.Length; j++)  
                {
                    (int, int) i = NSEW[j];

                    FenceSection fenceSection = new FenceSection()
                    {
                        row = coord.Item1,
                        col = coord.Item2,
                        direction = i
                    };

                    //fence allready counted
                    if (fs.Contains(fenceSection))
                    {
                        continue;
                    }

                    int rowy = i.Item1 + coord.Item1;
                    int colx = i.Item2 + coord.Item2;

                    if (rowy >= 0 && rowy < map.Length && colx >= 0 && colx < map[0].Length)
                    {
                        if (seen.Contains((rowy, colx)))
                        {
                            continue;
                        }

                        char next = map[rowy][colx];

                        if (next == gardenchar)
                        {
                            coords.Enqueue((rowy, colx));
                            seen.Add((rowy, colx));
                            continue;
                        }
                    }
                    
                    //if fence worthy 
                    if (
                        !(rowy >= 0 && rowy < map.Length && colx >= 0 && colx < map[0].Length) ||
                        map[rowy][colx] != gardenchar
                    ) {


                        fences++;

                        //if (gardenchar == 'E')
                        //    output.WriteLine($"Fence {fences}");

                        fs.Add(fenceSection);
                        
                        //if (gardenchar == 'E')
                        //    output.WriteLine($"{gardenchar} = {fenceSection.row} {fenceSection.col}  d {fenceSection.direction}");

                        //continue fence
                        foreach ((int, int) t in tangentNSEW[j])
                        {
                            (int, int) current = coord;

                            while (true)
                            {
                                current = (current.Item1 + t.Item1, current.Item2 + t.Item2);

                                //not OOB
                                if (!(current.Item1 >= 0 && current.Item1 < map.Length && current.Item2 >= 0 && current.Item2 < map[0].Length))
                                {
                                    break;
                                }

                                //not in garden
                                if (map[current.Item1][current.Item2] != gardenchar)
                                {
                                    break;
                                }

                                int fencerow = current.Item1 + i.Item1;
                                int fencecol = current.Item2 + i.Item2;

                                //if fence worthy 
                                if (
                                    !(fencerow >= 0 && fencerow < map.Length && fencecol >= 0 && fencecol < map[0].Length) ||
                                    map[fencerow][fencecol] != gardenchar
                                )
                                {
                                    FenceSection tangentfence = new FenceSection()
                                    {
                                        row = current.Item1,
                                        col = current.Item2,
                                        direction = i
                                    };

                                    fs.Add(tangentfence);
                                    
                                    //if (gardenchar == 'E')
                                    //    output.WriteLine($"{gardenchar} = {tangentfence.row} {tangentfence.col}  d {tangentfence.direction}");

                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //wipe the garden
            foreach (var coord in seen)
            {
                map[coord.Item1][coord.Item2] = ' ';
            }

            output.WriteLine($"{gardenchar} = {area} * {fences} = {area * fences}");

            //output fencesections
            //if (gardenchar == 'E')
            //foreach (FenceSection fence in fs)
            //{
            //    output.WriteLine($"{gardenchar} = {fence.row} {fence.col}  d {fence.direction}");
            //}


            return fences * area;
        }

    }
}
