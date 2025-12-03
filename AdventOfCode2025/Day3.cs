


using System.Linq;

namespace AdventOfCode2025;

public class Day3
{
    [Theory]
    [InlineData("./test/day3.txt", 357)]
    [InlineData("./input/day3.txt", 17095)]
    public void Part1(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        foreach(string line in lines)
        {
            result += LargestJolt(line);
        }


        Assert.Equal(expected, result);
    }

    private int LargestJolt(string line)
    {
        int currentJolt = 0;
        for (int i = 0; i < line.Length - 1; i++)
        {
            for (int j =i + 1; j < line.Length; j++)
            {
                currentJolt = Math.Max(currentJolt, int.Parse(String.Concat(line[i],line[j])));
            }
        }

        return currentJolt;
    }

    [Theory]
    [InlineData("./test/day3.txt", 3121910778619)]
    [InlineData("./input/day3.txt", 168794698570517)]
    public void Part2(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        foreach (string line in lines)
        {
            result += Largest12Jolt(line);
        }


        Assert.Equal(expected, result);
    }

    private long Largest12Jolt(string line)
    {
        //int len = line.Length;
        //return long.Parse(String.Concat(line.Select((c, index) => (int.Parse(c.ToString()), index)).OrderByDescending(o => o.Item1).ThenByDescending(o => o.Item2).Take(12).OrderBy(o => o.Item2).Select(c => c.Item1)));

        string current = string.Concat(line.TakeLast(12));

        for (int i = line.Length - 13; i >= 0; i--)
        {
            string testCand = current;

            for (int j = 0; j < 12; j++)
            {
                string c1 = line[i] + current.Remove(j, 1);
                long candidate = long.Parse(c1);

                if (candidate > long.Parse(testCand))
                {
                    testCand = c1;
                }
            }

            current = testCand;
        }

        return long.Parse(current);
    }
}
