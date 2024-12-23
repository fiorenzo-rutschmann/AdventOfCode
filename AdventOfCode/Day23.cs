using Xunit.Abstractions;
using System.Linq;
using Xunit.Sdk;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day23
    {
        private readonly ITestOutputHelper output;

        public Day23(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day23.txt", 7)]
        [InlineData("./input/day23.txt", 0)]
        public void Part1(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            //parse input
            List<(string, string)> connections = new();
            
            foreach (var line in input)
            {
                string[] pcs = line.Split('-');

                connections.Add((pcs[0], pcs[1]));
            }

            Dictionary<string, List<string>> maps = new();

            var distpcs = connections.Select(a => a.Item1).Concat(connections.Select(a => a.Item2)).Distinct().ToList();

            foreach (var pc in distpcs)
            {
                List<string> connected = new();
                connected.AddRange(connections.Where(a => a.Item1 == pc).Select(a => a.Item2));
                connected.AddRange(connections.Where(a => a.Item2 == pc).Select(a => a.Item1));

                maps.Add(pc, connected);
            }

            List<List<string>> threepcs = new();

            foreach (var map in maps.Where(a => a.Key.StartsWith("t")))
            {
                //startswith T
                string pc1 = map.Key;

                foreach (var pc2 in map.Value)
                {
                    //if (pc2 == pc1) continue;

                    foreach(var pc3 in maps[pc2])
                    { 
                        if (pc3 == pc2)
                            { continue; }

                        foreach (var pc4 in maps[pc3])
                        {
                            if (pc4 == pc1)
                            {
                                threepcs.Add(new() { pc1, pc2, pc3 });
                            }
                        }
                    }
                }
            }

            foreach (var tpc in threepcs.Index())
            {
                output.WriteLine($"{tpc.Index}:{tpc.Item[0]}-{tpc.Item[1]}-{tpc.Item[2]}");
            }

            //distinct the links
            List<List<string>> distthreepcs = new();
            for (var i = 0; i < threepcs.Count; i++)
            {
                bool same = false;
                for (var j = i+1; j < threepcs.Count; j++)
                {
                    if (!threepcs[i].Except(threepcs[j], StringComparer.OrdinalIgnoreCase).Any())
                    {
                        same = true;
                    }
                }

                if (!same)
                {
                    distthreepcs.Add(threepcs[i]);
                }
            }

            foreach (var tpc in distthreepcs.Index())
            {
                output.WriteLine($"{tpc.Index}:{tpc.Item[0]}-{tpc.Item[1]}-{tpc.Item[2]}");
            }

            result = distthreepcs.Count;

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("./test/day23.txt", 0)]
        [InlineData("./input/day23.txt", 0)]
        public void Part2(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            

            Assert.Equal(expected, result);
        }
    }
}
