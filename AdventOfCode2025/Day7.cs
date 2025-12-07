using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace AdventOfCode2025;

public class Day7
{
    [Theory]
    [InlineData("./test/day7.txt", 21)]
    [InlineData("./input/day7.txt", 1507)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        List<int> indexes = [lines.First().IndexOf('S')];

        List<int> nextIndexes = [];

        foreach ( var line in lines.Skip(1) )
        {
            foreach( var linDex in line.Index())
            {
                if (linDex.Item == '^' && indexes.RemoveAll(ind => ind == linDex.Index) > 0)
                {
                    nextIndexes.Add(linDex.Index + 1);
                    nextIndexes.Add(linDex.Index - 1);

                    result += 1;
                }
            }

            indexes.AddRange(nextIndexes);
            nextIndexes = [];
        }

        //result = indexes.Count();

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day7.txt", 40)]
    [InlineData("./input/day7.txt", 1537373473728)]
    public void Part2(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        List<(int, long)> indexes = [(lines.First().IndexOf('S'), 1)];

        List<(int, long)> nextIndexes = [];

        foreach (var line in lines.Skip(1))
        {
            foreach (var linDex in line.Index())
            {
                if (linDex.Item == '^' && indexes.FindAll(ind => ind.Item1 == linDex.Index).Count() > 0 )
                {
                    var beams = indexes.FindAll(ind => ind.Item1 == linDex.Index).Sum(ind => ind.Item2);

                    nextIndexes.Add((linDex.Index + 1, beams));
                    nextIndexes.Add((linDex.Index - 1, beams));


                    indexes.RemoveAll(ind => ind.Item1 == linDex.Index);
                }
            }

            indexes.AddRange(nextIndexes);
            nextIndexes = [];
        }

        result = indexes.Sum(ind => ind.Item2);

        //result = indexes.Count();

        Assert.Equal(expected, result);
    }
}
