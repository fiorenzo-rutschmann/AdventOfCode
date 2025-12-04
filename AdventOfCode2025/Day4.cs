


using System.Linq;

namespace AdventOfCode2025;

public class Day4
{
    [Theory]
    [InlineData("./test/day4.txt", 13)]
    [InlineData("./input/day4.txt", 1344)]
    public void Part1(string input, long expected)
    {
        string[] lines = File.ReadAllLines(input);

        long result = 0;

        foreach (int y in Enumerable.Range(0, lines.Length))
        {
            foreach (int x in Enumerable.Range(0, lines[y].Length))
            {
                if (lines[y][x] == '@')
                {
                    int neighbours = 0;

                    foreach(var direction in new (int dx, int dy)[] { (0,1), (1,0), (0,-1), (-1,0), (-1,-1), (1,1), (-1,1), (1,-1) })
                    {
                        int nx = x + direction.dx;
                        int ny = y + direction.dy;
                        if (nx >= 0 && nx < lines[y].Length && ny >= 0 && ny < lines.Length && lines[ny][nx] == '@')
                        {
                            neighbours++;
                        }
                    }

                    if (neighbours < 4)
                        result++;
                }
            }
        }

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day4.txt", 43)]
    [InlineData("./input/day4.txt", 8112)]
    public void Part2(string input, long expected)
    {
        string[] lines1 = File.ReadAllLines(input);

        List<char[]> lines = lines1.Select(line => line.ToCharArray()).ToList(); 

        long result = 0;

        bool keep = true;

        do
        {
            keep = false;
            foreach (int y in Enumerable.Range(0, lines.Count))
            {
                foreach (int x in Enumerable.Range(0, lines[y].Length))
                {
                    if (lines[y][x] == '@')
                    {
                        int neighbours = 0;

                        foreach (var direction in new (int dx, int dy)[] { (0, 1), (1, 0), (0, -1), (-1, 0), (-1, -1), (1, 1), (-1, 1), (1, -1) })
                        {
                            int nx = x + direction.dx;
                            int ny = y + direction.dy;
                            if (nx >= 0 && nx < lines[y].Length && ny >= 0 && ny < lines.Count && lines[ny][nx] == '@')
                            {
                                neighbours++;
                            }
                        }

                        if (neighbours < 4)
                        {
                            result++;
                            lines[y][x] = 'x';
                            keep = true;
                        }

                    }
                }
            }
        } while (keep);

        Assert.Equal(expected, result);
    }
}
