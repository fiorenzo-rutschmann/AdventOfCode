namespace AdventOfCode
{
    public class Day1
    {
        [Theory]
        [InlineData("./test/day1.txt", 11)]
        [InlineData("./input/day1.txt", 3569916)]
        public void Part1(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);
            int result = 0;

            //Get the two lists
            var left = lines.Select(a => int.Parse(a.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).First())).Order();
            var right = lines.Select(a => int.Parse(a.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Last())).Order();

            var zip = left.Zip(right);

            foreach (var line in zip)
            {
                result += Math.Abs(line.Second - line.First);
            }

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("./test/day1.txt", 31)]
        [InlineData("./input/day1.txt", 26407426)]
        public void Part2(string input, int expected)
        {
            string[] lines = File.ReadAllLines(input);
            int result = 0;

            //Get the two lists
            var left = lines.Select(a => int.Parse(a.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).First())).Order();
            var right = lines.Select(a => int.Parse(a.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Last())).Order();

            foreach (var i in left)
            {
                result += (i * right.Count(a => a == i));
            }

            Assert.Equal(expected, result);
        }
    }
}
