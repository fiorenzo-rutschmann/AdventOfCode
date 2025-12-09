namespace AdventOfCode2025;

public class Day8
{
    [Theory]
    [InlineData("./test/day8.txt", 40, 10)]
    [InlineData("./input/day8.txt", 50760, 1000)]
    public void Part1(string input, long expected, int junctions)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        List<(long x, long y, long z)> coords = new();

        foreach (string line in lines)
        {
            var items = line.Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

            coords.Add((items[0], items[1], items[2]));
        }

        double EuclidDist((long x, long y, long z) p1, (long x, long y, long z) p2)
        {
            return Math.Sqrt(
                 ((p1.x - p2.x) * (p1.x - p2.x)) +
                 ((p1.y - p2.y) * (p1.y - p2.y)) +
                 ((p1.z - p2.z) * (p1.z - p2.z))
             );
        }

        //Dictionary< double, ((long x, long y, long z),(long x, long y, long z))> distances = new();
        Dictionary< double, ((long x, long y, long z),(long x, long y, long z))> distances = new();

        List<(long x, long y, long z)> seen = new();

        foreach (var item in coords)
        {
            double minDist = double.MaxValue;
            (long x, long y, long z) p1 = default;
            (long x, long y, long z) p2 = default;

            foreach (var cord2 in coords)
            {
                if (item == cord2)
                    continue;

                double dist = EuclidDist(item, cord2);

                distances.TryAdd(dist, (item, cord2));

                //if (dist < minDist )
                //{
                //    minDist = dist;
                //    p1 = item;
                //    p2 = cord2;
                //}
            }

            //var dodo = 0;
            //if (!distances.TryAdd(minDist, (p1,p2)))
            //{
            //    if (distances[minDist] == (p1, p2) || distances[minDist] == (p2, p1))
            //    {
            //        dodo += 1;
            //    }
            //    else
            //    {
            //        dodo += 2;
            //    }
            //}
        }

        //double shortest = double.MaxValue;
        //(long x, long y, long z) p1 = default;
        //(long x, long y, long z) p2 = default;

        List<List<(long x, long y, long z)>> coordgroups = new();

        int count = 0;

        while (count < junctions)
        {
            var minKey = distances.Min(d => d.Key);
            var shortest = distances[minKey];

            var p1 = coordgroups.FirstOrDefault(cg => cg.Contains(shortest.Item1));
            var p2 = coordgroups.FirstOrDefault(cg => cg.Contains(shortest.Item2));

            if (p1 is not null && p2 is not null)
            {
                //these coords are in the same junkton
                if (p1 == p2)
                {
                    //destroy this key and continue
                    distances.Remove(minKey);
                    count++; //this better not be true FUCK!!!!
                    continue;
                }

                //different so combine
                p1.AddRange(p2);
                coordgroups.Remove(p2);
                distances.Remove(minKey);
                count++;
                continue;
            }

            if (p1 is null && p2 is null)
            {
                coordgroups.Add([shortest.Item1, shortest.Item2]);

                distances.Remove(minKey);
                count++;
                continue;
            }

            if (!(p1 ?? p2).Contains(shortest.Item1))
            {
                (p1 ?? p2).Add(shortest.Item1);
            }

            if (!(p1 ?? p2).Contains(shortest.Item2))
            {
                (p1 ?? p2).Add(shortest.Item2);
            }
            
            distances.Remove(minKey);
            count++;
        }

        //Safety ToList()
        foreach (var cordy in coords.Except(coordgroups.SelectMany(cg => cg)).ToList())
        {
            coordgroups.Add([cordy]);
        }

        //stupid multiply
        result = 1;
        coordgroups.Select(cg => cg.Count).OrderByDescending(cg => cg).Take(3).ToList().ForEach(cg => result *= cg);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("./test/day8.txt", 25272)]
    [InlineData("./input/day8.txt", 3206508875)]
    public void Part2(string input, long expected)
    {
        long result = 0;

        string[] lines = File.ReadAllLines(input);

        List<(long x, long y, long z)> coords = new();

        foreach (string line in lines)
        {
            var items = line.Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

            coords.Add((items[0], items[1], items[2]));
        }

        double EuclidDist((long x, long y, long z) p1, (long x, long y, long z) p2)
        {
            return Math.Sqrt(
                 ((p1.x - p2.x) * (p1.x - p2.x)) +
                 ((p1.y - p2.y) * (p1.y - p2.y)) +
                 ((p1.z - p2.z) * (p1.z - p2.z))
             );
        }

        //Dictionary< double, ((long x, long y, long z),(long x, long y, long z))> distances = new();
        Dictionary<double, ((long x, long y, long z), (long x, long y, long z))> distances = new();

        List<(long x, long y, long z)> seen = new();

        foreach (var item in coords)
        {
            double minDist = double.MaxValue;
            (long x, long y, long z) p1 = default;
            (long x, long y, long z) p2 = default;

            foreach (var cord2 in coords)
            {
                if (item == cord2)
                    continue;

                double dist = EuclidDist(item, cord2);

                distances.TryAdd(dist, (item, cord2));

                //if (dist < minDist )
                //{
                //    minDist = dist;
                //    p1 = item;
                //    p2 = cord2;
                //}
            }

            //var dodo = 0;
            //if (!distances.TryAdd(minDist, (p1,p2)))
            //{
            //    if (distances[minDist] == (p1, p2) || distances[minDist] == (p2, p1))
            //    {
            //        dodo += 1;
            //    }
            //    else
            //    {
            //        dodo += 2;
            //    }
            //}
        }

        //double shortest = double.MaxValue;
        //(long x, long y, long z) p1 = default;
        //(long x, long y, long z) p2 = default;

        List<List<(long x, long y, long z)>> coordgroups = new();
        
        foreach (var c in coords)
        {
            coordgroups.Add([c]);
        }

        int count = 0;

        while (true)
        {
            var minKey = distances.Min(d => d.Key);
            var shortest = distances[minKey];

            var p1 = coordgroups.FirstOrDefault(cg => cg.Contains(shortest.Item1));
            var p2 = coordgroups.FirstOrDefault(cg => cg.Contains(shortest.Item2));

            if (p1 is not null && p2 is not null)
            {
                //these coords are in the same junkton
                if (p1 == p2)
                {
                    //destroy this key and continue
                    distances.Remove(minKey);
                    count++; //this better not be true FUCK!!!!
                    continue;
                }

                //different so combine
                p1.AddRange(p2);
                coordgroups.Remove(p2);
                distances.Remove(minKey);
                count++;

                if (coordgroups.Count == 1)
                {
                    result = shortest.Item1.x * shortest.Item2.x;
                    break;
                }

                continue;
            }

            if (p1 is null && p2 is null)
            {
                coordgroups.Add([shortest.Item1, shortest.Item2]);

                distances.Remove(minKey);
                count++;
                continue;
            }

            if (!(p1 ?? p2).Contains(shortest.Item1))
            {
                (p1 ?? p2).Add(shortest.Item1);
            }

            if (!(p1 ?? p2).Contains(shortest.Item2))
            {
                (p1 ?? p2).Add(shortest.Item2);
            }

            distances.Remove(minKey);
            count++;
        }

        //Safety ToList()
        foreach (var cordy in coords.Except(coordgroups.SelectMany(cg => cg)).ToList())
        {
            coordgroups.Add([cordy]);
        }

        Assert.Equal(expected, result);
    }
}