using System.Drawing.Printing;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day16
    {
        private readonly ITestOutputHelper output;

        public Day16(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day16.txt", 7036)]
        [InlineData("./test/day16_1.txt", 11048)]
        [InlineData("./input/day16.txt", 0)]
        public void Part1(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            char[][] map = input.Select(a => a.ToCharArray()).ToArray();

            (int, int) position = (0, 0);

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == 'S')
                    {
                        //map[row][col] = '.';
                        position = (row, col);
                    }
                }
            }

            HashSet<(int, int)> path = new() { position };

            var madeit = MoveHorse( map, position, path, position, (0,+1), 0);

            result = madeit.Item2;

            Assert.Equal(expected, result);
        }

        long lowestpoints = 534748;
        private (int, int)[] NSEW = [(-1, 0), (+1, 0), (0, -1), (0, +1)];

        private (bool, long) MoveHorse(char[][] map, (int, int) position, HashSet<(int, int)> path, (int, int) last, (int,int) vector, long points)
        {
            if (points > lowestpoints)
            {
                return (false, 0);
            }
            
            if (map[position.Item1][position.Item2] == '#')
            {
                return (false, 0);
            }

            if (map[position.Item1][position.Item2] == 'E')
            {
                PrintMap(map, path, points);

                if (points < lowestpoints )
                {
                    lowestpoints = points;
                    return (true, points);
                }

                return (false, points);
            }

            long lowest = long.MaxValue;
            bool found = false;
            //try lowestcost first


            HashSet<(int, int)> nextPath1 = new(path);
            nextPath1.Add((position.Item1 + vector.Item1, position.Item2 + vector.Item2));

            var output = MoveHorse(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), nextPath1, position, vector, points+1);

            if (output.Item1)
            {
                found = true;
                if (output.Item2 != 0 && output.Item2 < lowest)
                {
                    lowest = output.Item2;
                }
            }


            foreach (var vec in NSEW.Where(a => a != vector))
            {
                long thispoints = points;

                var nextPosition = (position.Item1 + vec.Item1, position.Item2 + vec.Item2);

                if (nextPosition == last)
                {
                    continue;
                }

                if (vec == vector)
                {
                    thispoints += 1;
                }
                else
                {
                    thispoints += 1001;
                }

                if (thispoints > lowestpoints)
                {
                    continue;
                }

                if (path.Contains(nextPosition))
                {
                    continue;
                }

                HashSet<(int, int)> nextPath = new(path);
                nextPath.Add(nextPosition);

                var output1 = MoveHorse(map, nextPosition, nextPath, position, vec, thispoints);

                if (output1.Item1)
                {
                    found = true;
                    if (output1.Item2 != 0 && output1.Item2 < lowest)
                    {
                        lowest = output1.Item2;
                    }
                }
            }

            return (found, lowest);
        }

        private void PrintMap(char[][] map, HashSet<(int, int)> path, long points)
        {
            //output
            StringBuilder sb = new();

            sb.AppendLine($"Points = {points}");
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (path.Contains((row,col)))
                    {
                        sb.Append('H');
                    }
                    else
                    {
                        sb.Append(map[row][col]);
                    }

                }
                sb.AppendLine();
            }

            output.WriteLine(sb.ToString());
        }

        private long CalculatePoints(List<(int, int)> paths)
        {
            long score = 0;

            (int, int) lastVector = (0, +1);
            for(int i = 1; i < paths.Count(); i++)
            {
                var path = paths[i];
                (int, int) lastPosition = paths[i-1];

                (int, int) vector = (path.Item1 - lastPosition.Item1, path.Item2 - lastPosition.Item2);

                if (vector != lastVector)
                {
                    score += 1000;
                }

                score++; 
                

                lastVector = vector;
            }

            return score;
        }



        [Theory]
        [InlineData("./test/day16.txt", 7036)]
        [InlineData("./test/day16_1.txt", 11048)]
        [InlineData("./input/day16.txt", 0)]
        public void Part1_Dykstras(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            char[][] map = input.Select(a => a.ToCharArray()).ToArray();

            (int, int) position = (0, 0);

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == 'S')
                    {
                        //map[row][col] = '.';
                        position = (row, col);
                    }
                }
            }

            PriorityQueue<(int, int, int, int, long), long> queue = new();
            //HashSet<(int, int, int, int)> seenNodes = new();
            Dictionary<(int, int), long> seenNodes = new(); //seen nodes at price
            queue.Enqueue((position.Item1, position.Item2, 0, +1, 0), 0);

            long lowestpoints = long.MaxValue;

            while(queue.Count > 0)
            {
                var current = queue.Dequeue();

                long points = current.Item5;

                int vecY = current.Item3;
                int vecX = current.Item4;

                if (points > lowestpoints)
                {
                    continue;
                }

                if (map[current.Item1][current.Item2] == '#')
                {
                    continue;
                }

                if (map[current.Item1][current.Item2] == 'E')
                {
                    //PrintMap(map, path, points);

                    if (points < lowestpoints)
                    {
                        lowestpoints = points;
                    }

                    continue;
                }

                //mostcost 
                //rotate right
                int vexRX = vecY * -1;
                int vexRY = vecX;
                var lastR = (current.Item1 + vexRY, current.Item2 + vexRX, vecY, vecX, points + 1001);
                if (!seenNodes.ContainsKey((lastR.Item1, lastR.Item2)) || seenNodes[(lastR.Item1, lastR.Item2)] >= lastR.Item5)
                {
                    if (!seenNodes.ContainsKey((lastR.Item1, lastR.Item2)))
                        seenNodes.Add((lastR.Item1, lastR.Item2), lastR.Item5);
                    else
                        seenNodes[(lastR.Item1, lastR.Item2)] = lastR.Item5;
                    
                    queue.Enqueue(lastR, lastR.Item5);
                }

                //rotate left
                int vexLX = vecY;
                int vexLY = vecX * -1;
                var lastL = (current.Item1 + vexLY, current.Item2 + vexLX, vecY, vecX, points + 1001);
                if (!seenNodes.ContainsKey((lastL.Item1, lastL.Item2)) || seenNodes[(lastL.Item1, lastL.Item2)] >= lastL.Item5)
                {
                    if (!seenNodes.ContainsKey((lastL.Item1, lastL.Item2)))
                        seenNodes.Add((lastL.Item1, lastL.Item2), lastL.Item5);
                    else
                        seenNodes[(lastL.Item1, lastL.Item2)] = lastL.Item5;

                    queue.Enqueue(lastL, lastL.Item5);
                }

                //leastcost
                var next = (current.Item1 + vecY, current.Item2 + vecX, vecY, vecX, points + 1);
                if (!seenNodes.ContainsKey((next.Item1, next.Item2)) || seenNodes[(next.Item1, next.Item2)] >= next.Item5)
                {
                    if (!seenNodes.ContainsKey((next.Item1, next.Item2)))
                        seenNodes.Add((next.Item1, next.Item2), next.Item5);
                    else
                        seenNodes[(next.Item1, next.Item2)] = next.Item5;
                    
                    queue.Enqueue(next, next.Item5);
                }
            }

            result = lowestpoints;
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day16.txt", 0)]
        [InlineData("./input/day16.txt", 0)]
        public void Part2(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;


            Assert.Equal(expected, result);
        }
    }
}
