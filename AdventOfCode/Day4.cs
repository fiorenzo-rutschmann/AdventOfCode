using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day4
    {
        [Theory]
        [InlineData("./test/day4.txt", 18)]
        [InlineData("./input/day4.txt", 2297)]
        public void Part1(string input, int expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            //for each x check all angles
            int width = inputString[0].Length;
            int height = inputString.Length;

            int y = 0;
            foreach (string line in inputString)
            {
                int x = 0;
                foreach (char c in line)
                {
                    if (c == 'X' || c == 'x')
                        result += findXMAS(inputString, x, y, width, height);

                    x++;
                }
                y++;
            }

            Assert.Equal(expected, result);
        }

        private long findXMAS(string[] inputString, int x, int y, int width, int height)
        {

            int ret = 0;

            ret += findXmasVec(inputString, x, y, width, height, -1, 0); //left
            ret += findXmasVec(inputString, x, y, width, height, +1, 0); //right
            ret += findXmasVec(inputString, x, y, width, height, 0, -1); //up
            ret += findXmasVec(inputString, x, y, width, height, 0, +1); //down

            ret += findXmasVec(inputString, x, y, width, height, -1, -1); //upleft
            ret += findXmasVec(inputString, x, y, width, height, -1, +1); //upright
            ret += findXmasVec(inputString, x, y, width, height, +1, -1); //downleft
            ret += findXmasVec(inputString, x, y, width, height, +1, +1); //downright

            return ret;

        }

        private int findXmasVec(string[] inputString, int x, int y, int width, int height, int vecX, int vexY)
        {
            string output = inputString[y][x].ToString();

            for (int i = 1; i < 4; i++)
            {
                int xt = x + (vecX * i);
                int yt = y + (vexY * i);

                if (xt >= 0 && yt >= 0 && xt < width && yt < width)
                {
                    output += inputString[yt][xt];
                }
                else
                {
                    return 0;
                }
            }
            
            if (output.Equals("XMAS", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }

            return 0;
        }

        [Theory]
        [InlineData("./test/day4.txt", 9)]
        [InlineData("./input/day4.txt", 1745)]
        public void Part2(string input, int expected)
        {
            string[] inputString = File.ReadAllLines(input);
            long result = 0;

            //for each x check all angles
            int width = inputString[0].Length;
            int height = inputString.Length;

            int y = 0;
            foreach (string line in inputString)
            {
                int x = 0;
                foreach (char c in line)
                {
                    if (c == 'A' || c == 'a')
                        result += findMASX(inputString, x, y, width, height);

                    x++;
                }
                y++;
            }

            Assert.Equal(expected, result);
        }

        private long findMASX(string[] inputString, int x, int y, int width, int height)
        {
            char topLeft = getChar(inputString, x - 1, y - 1, width, height);
            char topRight = getChar(inputString, x + 1, y - 1, width, height);
            char botLeft = getChar(inputString, x - 1, y + 1, width, height);
            char botRight = getChar(inputString, x + 1, y + 1, width, height);

            if ( 
                ((topLeft == 'M' && botRight == 'S') || (topLeft == 'S' && botRight == 'M'))

                &&

                ((topRight == 'M' && botLeft == 'S') || (topRight == 'S' && botLeft == 'M'))
            )
            {
                return 1;
            }

            return 0;

        }

        private char getChar(string[] inputString, int x, int y, int width, int height)
        {
            //top left
            int xt = x;
            int yt = y;
            if (xt >= 0 && yt >= 0 && xt < width && yt < width)
            {
                return inputString[yt][xt];
            }
            else
                return ' ';
        }
    }
}
