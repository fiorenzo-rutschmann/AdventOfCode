using System.Text;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode
{
    public class Day15
    {
        private readonly ITestOutputHelper output;

        public Day15(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("./test/day15.txt", 10092)]
        [InlineData("./test/day15_1.txt", 2028)]
        [InlineData("./input/day15.txt", 1398947)]
        public void Part1(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            //seperator == blank line
            int middle = input.Index().First(a => a.Item == "").Index;

            string[] ordering = input[0..middle];
            string moves = string.Join(null, input[(middle + 1)..]);

            char[][] map = ordering.Select(a => a.ToCharArray()).ToArray();

            (int, int) position = (0, 0);

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == '@')
                    {
                        //map[row][col] = '.';
                        position = (row, col);
                    }
                }
            }

            foreach (char move in moves)
            {
                switch (move)
                {
                    case '^': position = MovePosition(map, position, (-1, 0)); break;
                    case 'v': position = MovePosition(map, position, (+1, 0)); break;
                    case '<': position = MovePosition(map, position, (0, -1)); break;
                    case '>': position = MovePosition(map, position, (0, +1)); break;
                    default: break;
                }

                //output
                //StringBuilder sb = new();
                //for (int row = 0; row < map.Length; row++)
                //{
                //    sb.AppendLine();
                //    sb.Append(map[row]);
                //}

                //output.WriteLine(sb.ToString());
            }


            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == 'O')
                    {
                        result += 100 * row + col;
                    }
                }
            }

            Assert.Equal(expected, result);
        }

        private (int, int) MovePosition(char[][] map, (int, int) position, (int, int) vector)
        {
            char next = map[position.Item1 + vector.Item1][position.Item2 + vector.Item2];
            //wall
            if (next == '#')
            {
                return position;
            }

            //move next 0 in line
            if (next == 'O')
            {
                MovePosition(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector);
            }

            char next2 = map[position.Item1 + vector.Item1][position.Item2 + vector.Item2];
            char current = map[position.Item1][position.Item2];
            if (next2 == '.')
            {
                map[position.Item1 + vector.Item1][position.Item2 + vector.Item2] = current;
                map[position.Item1][position.Item2] = '.';
                return (position.Item1 + vector.Item1, position.Item2 + vector.Item2);
            }

            return position;
        }

        private bool CanMovePositionP2(char[][] map, (int, int) position, (int, int) vector)
        {
            char next = map[position.Item1 + vector.Item1][position.Item2 + vector.Item2];

            if (next == '.')
            {
                return true;
            }

            if (next == '#')
            {
                return false;
            }

            char current = map[position.Item1][position.Item2];
            //if im [ or ] and the vector points to my twin then only check the twin.
            if (current == '[' && next == ']' && vector.Item2 == +1)
            {
                return CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector);
            }

            if (current == ']' && next == '[' && vector.Item2 == -1)
            {
                return CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector);
            }

            if (next == '[')
            {
                return CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector) &&
                CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2 + 1), vector);
            }

            if (next == ']')
            {
                return CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector) &&
                CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2 - 1), vector);
            }

            Assert.Fail("should not make it here");
            return false;
        }


        private (int, int) MovePositionP2(char[][] map, (int, int) position, (int, int) vector)
        {
            char next = map[position.Item1 + vector.Item1][position.Item2 + vector.Item2];
            //wall
            if (next == '#')
            {
                return position;
            }

            //move next [] in line
            if (next == '[')
            {
                if (CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector) &&
                    CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2 + 1), vector))
                {
                    //move in order
                    MovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2 + 1), vector);
                    MovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector);
                }
            }

            if (next == ']')
            {
                if (CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector) &&
                    CanMovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2 - 1), vector))
                {
                    //move in order
                    MovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2 - 1), vector);
                    MovePositionP2(map, (position.Item1 + vector.Item1, position.Item2 + vector.Item2), vector);
                }
            }

            char next2 = map[position.Item1 + vector.Item1][position.Item2 + vector.Item2];
            char current = map[position.Item1][position.Item2];
            if (next2 == '.')
            {
                map[position.Item1 + vector.Item1][position.Item2 + vector.Item2] = current;
                map[position.Item1][position.Item2] = '.';
                return (position.Item1 + vector.Item1, position.Item2 + vector.Item2);
            }

            return position;
        }

        [Theory]
        [InlineData("./test/day15.txt", 9021)]
        [InlineData("./test/day15_2.txt", 618)]
        [InlineData("./input/day15.txt", 1397393)]
        public void Part2(string file, long expected)
        {
            string[] input = File.ReadAllLines(file);
            long result = 0;

            //seperator == blank line
            int middle = input.Index().First(a => a.Item == "").Index;

            string[] ordering = input[0..middle];
            string moves = string.Join(null, input[(middle + 1)..]);

            char[][] map = ordering.Select(
                a =>
                    a.ToCharArray()
                    .SelectMany(
                            b =>
                            {
                                switch (b)
                                {
                                    case '#': return new char[] { '#', '#' };
                                    case '.': return new char[] { '.', '.' };
                                    case 'O': return new char[] { '[', ']' };
                                    case '@': return new char[] { '@', '.' };
                                    default: return new char[] { };
                                }
                            }
                    ).ToArray()
            ).ToArray();

            //output
            StringBuilder initaloutput = new();

            initaloutput.AppendLine($"initial map:");
            for (int row = 0; row < map.Length; row++)
            {
                initaloutput.Append(map[row]);
                initaloutput.AppendLine();
            }

            output.WriteLine(initaloutput.ToString());

            (int, int) position = (0, 0);

            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == '@')
                    {
                        //map[row][col] = '.';
                        position = (row, col);
                    }
                }
            }

            foreach (var move in moves.Index())
            {
                switch (move.Item)
                {
                    case '^': position = MovePositionP2(map, position, (-1, 0)); break;
                    case 'v': position = MovePositionP2(map, position, (+1, 0)); break;
                    case '<': position = MovePositionP2(map, position, (0, -1)); break;
                    case '>': position = MovePositionP2(map, position, (0, +1)); break;
                    default: break;
                }

                //output
                StringBuilder sb = new();

                sb.AppendLine($"move {move.Index} = {move}:");
                for (int row = 0; row < map.Length; row++)
                {
                    sb.Append(map[row]);
                    sb.AppendLine();
                }

                output.WriteLine(sb.ToString());
            }


            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == '[')
                    {
                        result += 100 * row + col;
                    }
                }
            }

            Assert.Equal(expected, result);
        }
    }
}
