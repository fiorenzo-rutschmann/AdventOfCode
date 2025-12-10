using System.Security.Cryptography;

namespace AdventOfCode2025;

public class Day9
{
    [Theory]
    [InlineData("./test/day9.txt", 50)]
    [InlineData("./input/day9.txt", 4771532800)]
    public void Part1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        var coords = lines.Select(line => (long.Parse(line.Split(',').ToArray()[0]), long.Parse(line.Split(',').ToArray()[1])));

        long Size((long x, long y) p1, (long x, long y) p2)
        {
            return (Math.Abs(p1.x - p2.x) + 1)* (Math.Abs(p1.y - p2.y) +1);
        }

        long maxSize = 0;
        foreach (var coord in coords) {
            foreach (var cord2 in coords)
            {
                if (cord2 == coord) continue;

                long size = Size(coord, cord2);

                if (size > maxSize) maxSize = size;
            }
        }

        result = maxSize;

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day9.txt", 24)]
    [InlineData("./input/day9.txt", 1544362560)]
    public void Part2(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        var coords = lines.Select(line => (long.Parse(line.Split(',').ToArray()[0]), long.Parse(line.Split(',').ToArray()[1]))).ToList();

        bool BetweenInclusive (long i, long x1, long x2)
        {
            if (x1 > x2)
            {
                return (x1 >= i && i >= x2);
            }

            if (x1 < x2)
            {
                return (x2 >= i && i >= x1);
            }

            if (x1 == x2 && x1 == i)
            {
                return true; //aka on point
            }

            return false; //no idea
        }

        int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        bool CoordIsInPolygon((long x, long y) coord)
        {
            int count = 0;

            foreach((int index, (long x, long y) coord) i in coords.Index())
            {
                int nextIndex = (i.index+1) % coords.Count();

                long thisX = i.coord.x;
                long thisY = i.coord.y;

                long nextX = coords[nextIndex].Item1;
                long nextY = coords[nextIndex].Item2;

                //on a line
                if (thisY == nextY && coord.y == thisY && BetweenInclusive(coord.x, thisX, nextX))
                {
                    return true;
                }

                if (thisX == nextX && coord.x == thisX && BetweenInclusive(coord.y, thisY, nextY))
                {
                    return true;
                }

                if (thisY == nextY && thisY < coord.y && BetweenInclusive(coord.x, thisX, nextX))
                {
                    count++;
                }

                //edgecase runalong.
                if (thisX == nextX && thisX == coord.x && thisY < coord.y && nextY < coord.y)
                {
                    int previousIndex = mod((i.index - 1) , coords.Count());
                    long prevX = coords[previousIndex].Item1;
                    long prevY = coords[previousIndex].Item2;

                    int nextNextIndex = (i.index + 2) % coords.Count();
                    long nextNextX = coords[nextNextIndex].Item1;
                    long nextNextY = coords[nextNextIndex].Item2;

                    if (prevX >= thisX && nextNextX <= thisX)
                        count++;

                    if (prevX <= thisX && nextNextX >= thisX)
                        count++;
                }
            }

            return (count % 2) == 1;
        }

        long Size((long x, long y) p1, (long x, long y) p2)
        {
            return (Math.Abs(p1.x - p2.x) + 1) * (Math.Abs(p1.y - p2.y) + 1);
        }

        bool AllCornersInPolygon((long x, long y) p1, (long x, long y) p2)
        {
            if (!CoordIsInPolygon((p2.x, p1.y)))
                return false;

            if (!CoordIsInPolygon((p1.x, p2.y)))
                return false;


            return true;
        }

        bool AllEdgesInPolygon((long x, long y) p1, (long x, long y) p2)
        {
            //for (long x = Math.Min(p1.x, p2.x); x <= Math.Max(p1.x, p2.x); x++)
            //{
            //    for (long y = Math.Min(p1.y, p2.y); y <= Math.Max(p1.y, p2.y); y++)
            //    {
            //        if (!CoordIsInPolygon((x,y)))
            //            return false;
            //    }
            //}

            //going right ways

            (long x, long y) c1 = (p2.x, p1.y);
            (long x, long y) c2 = (p1.x, p2.y);


            //p1.x
            for (long x = Math.Min(p1.x, c1.x); x <= Math.Max(p1.x, c1.x); x++)
            {
                if (!CoordIsInPolygon((x, p1.y)))
                    return false;
            }

            //p2.x
            for (long x = Math.Min(p2.x, c2.x); x <= Math.Max(p2.x, c2.x); x++)
            {
                if (!CoordIsInPolygon((x, p2.y)))
                    return false;
            }

            //p1.y
            for (long y = Math.Min(p1.y, c2.y); y <= Math.Max(p1.y, c2.y); y++)
            {
                if (!CoordIsInPolygon((p1.x, y)))
                    return false;
            }

            //p2.y
            for (long y = Math.Min(p2.y, c1.y); y <= Math.Max(p2.y, c1.y); y++)
            {
                if (!CoordIsInPolygon((p2.x, y)))
                    return false;
            }

            return true;
        }

        long maxSize = 0;
        foreach (var c1 in coords)
        {
            foreach (var c2 in coords)
            {
                if (c2 == c1) continue;

                long size = Size(c1, c2);

                if (size > maxSize && AllCornersInPolygon(c1, c2) && AllEdgesInPolygon(c1, c2))
                {
                    maxSize = size;
                }
            }
        }

        result = maxSize;
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day9.txt", 24)]
    [InlineData("./input/day9.txt", 1544362560)]
    public void Part2_optimise1(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        var coords = lines.Select(line => (long.Parse(line.Split(',').ToArray()[0]), long.Parse(line.Split(',').ToArray()[1]))).ToList();

        bool BetweenInclusive(long i, long x1, long x2)
        {
            if (x1 > x2)
            {
                return (x1 >= i && i >= x2);
            }

            if (x1 < x2)
            {
                return (x2 >= i && i >= x1);
            }

            if (x1 == x2 && x1 == i)
            {
                return true; //aka on point
            }

            return false; //no idea
        }

        int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        bool CoordIsInPolygon((long x, long y) coord)
        {
            int count = 0;

            foreach ((int index, (long x, long y) coord) i in coords.Index())
            {
                int nextIndex = (i.index + 1) % coords.Count();

                long thisX = i.coord.x;
                long thisY = i.coord.y;

                long nextX = coords[nextIndex].Item1;
                long nextY = coords[nextIndex].Item2;

                //on a line
                if (thisY == nextY && coord.y == thisY && BetweenInclusive(coord.x, thisX, nextX))
                {
                    return true;
                }

                if (thisX == nextX && coord.x == thisX && BetweenInclusive(coord.y, thisY, nextY))
                {
                    return true;
                }

                if (thisY == nextY && thisY < coord.y && BetweenInclusive(coord.x, thisX, nextX))
                {
                    count++;
                }

                //edgecase runalong.
                if (thisX == nextX && thisX == coord.x && thisY < coord.y && nextY < coord.y)
                {
                    int previousIndex = mod((i.index - 1), coords.Count());
                    long prevX = coords[previousIndex].Item1;
                    long prevY = coords[previousIndex].Item2;

                    int nextNextIndex = (i.index + 2) % coords.Count();
                    long nextNextX = coords[nextNextIndex].Item1;
                    long nextNextY = coords[nextNextIndex].Item2;

                    if (prevX >= thisX && nextNextX <= thisX)
                        count++;

                    if (prevX <= thisX && nextNextX >= thisX)
                        count++;
                }
            }

            return (count % 2) == 1;
        }

        long Size((long x, long y) p1, (long x, long y) p2)
        {
            return (Math.Abs(p1.x - p2.x) + 1) * (Math.Abs(p1.y - p2.y) + 1);
        }

        bool AllCornersInPolygon((long x, long y) p1, (long x, long y) p2)
        {
            if (!CoordIsInPolygon((p2.x, p1.y)))
                return false;

            if (!CoordIsInPolygon((p1.x, p2.y)))
                return false;


            return true;
        }

        bool AllEdgesInPolygon((long x, long y) p1, (long x, long y) p2)
        {
            //for (long x = Math.Min(p1.x, p2.x); x <= Math.Max(p1.x, p2.x); x++)
            //{
            //    for (long y = Math.Min(p1.y, p2.y); y <= Math.Max(p1.y, p2.y); y++)
            //    {
            //        if (!CoordIsInPolygon((x,y)))
            //            return false;
            //    }
            //}

            //going right ways

            (long x, long y) c1 = (p2.x, p1.y);
            (long x, long y) c2 = (p1.x, p2.y);


            //p1.x
            for (long x = Math.Min(p1.x, c1.x); x <= Math.Max(p1.x, c1.x); x++)
            {
                if (!CoordIsInPolygon((x, p1.y)))
                    return false;
            }

            //p2.x
            for (long x = Math.Min(p2.x, c2.x); x <= Math.Max(p2.x, c2.x); x++)
            {
                if (!CoordIsInPolygon((x, p2.y)))
                    return false;
            }

            //p1.y
            for (long y = Math.Min(p1.y, c2.y); y <= Math.Max(p1.y, c2.y); y++)
            {
                if (!CoordIsInPolygon((p1.x, y)))
                    return false;
            }

            //p2.y
            for (long y = Math.Min(p2.y, c1.y); y <= Math.Max(p2.y, c1.y); y++)
            {
                if (!CoordIsInPolygon((p2.x, y)))
                    return false;
            }

            return true;
        }

        List<(long size, (long x, long y) c1, (long x, long y) c2)> candidates = new();

        foreach (var c1 in coords)
        {
            foreach (var c2 in coords)
            {
                if (c2 == c1) continue;

                long size = Size(c1, c2);

                if (AllCornersInPolygon(c1, c2))
                {
                    candidates.Add((size, c1, c2));
                }
            }
        }

        foreach(var i in candidates.OrderByDescending(c => c.size))
        {
            if (AllEdgesInPolygon(i.c1, i.c2))
            {
                result = i.size; 
                break;
            }
        }

        Assert.Equal(expected, result);
    }
}