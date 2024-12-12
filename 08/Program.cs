using System.Configuration.Assemblies;
using System.Text;

if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllLines(fileArg);

// Find all positions
var positionLookup = input.Select((row, y) =>
{
    return row
    .Select((col, x) =>
    {
        return new KeyValuePair<char, Position>(col, new(y, x));
    })
    .Where(x => x.Key != '.');
})
.SelectMany(_ => _)
.ToLookup(x => x.Key, x => x.Value);

// Boundaries - Antinodes cannot exceed this length
var yBoundaryIndex = input.Length - 1;
var xBoundaryIndex = input[0].Length - 1;

HashSet<Position> antinodes = [];
HashSet<Position> antinodes2 = [];
foreach (var lookup in positionLookup.AsParallel())
{
    foreach (var position in lookup.AsParallel())
    {
        var otherPositions = lookup.Except([position]);
        foreach (var otherPosition in otherPositions.AsParallel())
        {
            var antinode = position.FindAntiNode(otherPosition);

            // Part two draws a line from 0,0 to X,Y in the vector of two nodes

            // Hardly explained edge case for part 2
            if (antinode.Y >= 0
                && antinode.Y <= yBoundaryIndex
                && antinode.X >= 0
                && antinode.X <= xBoundaryIndex)
            {
                antinodes.Add(antinode);
            }

            // Find the least common multiple between the diffs
            //Pythagarus
            var z = Math.Sqrt(Math.Pow(Math.Abs(position.X - antinode.X), 2) + Math.Pow(Math.Abs(position.Y - antinode.Y), 2));
            // "Draw" a line from X edge to Y edge
            // Bruteforce, but loop through all possible positions and see if is divisible by z


            // // part two, keep looping until we are out of bounds
            // while (antinode.Y >= 0
            //        && antinode.Y <= yBoundaryIndex
            //        && antinode.X >= 0
            //        && antinode.X <= xBoundaryIndex)
            // {
            //     antinodes.Add(antinode);

            //     // antinode becomes next, start becomes current antinode
            //     (antinode, start) = (antinode.FindAntiNode(start), antinode);
            //     //Console.WriteLine(antinode);

            // }
        }
    }
}
Console.WriteLine("-----");

//Console.WriteLine(antiNodes.Count);
antinodes.OrderBy(_ => _.X).ThenBy(_ => _.Y).ToList().ForEach(x => Console.WriteLine(x));
//Console.WriteLine("-----");
//existingPositions.ToList().ForEach(x => Console.WriteLine(x));
// How many distinct antinodes?
Console.WriteLine(antinodes.Distinct().Count());

return;

static int gcf(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static int lcm(int a, int b)
{
    return (a / gcf(a, b)) * b;
}

internal readonly struct Position(int y, int x)
{
    public int Y { get; } = y;
    public int X { get; } = x;

    public Position FindAntiNode(Position pos2)
    {
        return new Position(Y + (Y - pos2.Y), X + (X - pos2.X));
    }

    public override string ToString()
    {
        return $"X: {X} Y: {Y}";
    }
}
