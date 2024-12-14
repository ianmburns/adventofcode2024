Position[] directions = [
    new Position(-1,0), // up
    new Position(0,1), // right
    new Position(1,0), // down
    new Position(0,-1), // left
];

if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllLines(fileArg).Select(x => x.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();


var part1PathEndingCount = 0;
var part2UniqueEndingCount = 0;

for (var y = 0; y < input.Length; y++)
{
    {
        for (var x = 0; x < input[y].Length; x++)
        {
            if (input[y][x] == 0)
            {
                part1PathEndingCount += FindNumberOfPaths(y, x, 0).Count;
                part2UniqueEndingCount += FindNumberOfPathsPart2(y, x, 0);
            }
        }
    }
}


Console.WriteLine(part1PathEndingCount);
Console.WriteLine(part2UniqueEndingCount);


HashSet<Position> FindNumberOfPaths(int y, int x, int at)
{
    if (at == 9)
    {
        return [new(y, x)];
    }

    // look in all directions
    // we could also not look in the direction we came, but thats okay

    var items = directions.Select(d =>
    {
        var nextY = y + d.Y;
        var nextX = x + d.X;

        // If next is in bounds
        if (nextY >= 0 && nextY < input.Length
            && nextX >= 0 && nextX < input[nextY].Length)
        {
            if (input[nextY][nextX] == (at + 1))
            {
                return FindNumberOfPaths(nextY, nextX, at + 1);
            }
        }

        return [];
    }).SelectMany(_ => _);

    return items.ToHashSet();
}

int FindNumberOfPathsPart2(int y, int x, int at)
{
    if (at == 9)
    {
        return 1;
    }

    // look in all directions
    // we could also not look in the direction we came, but thats okay

    var items = directions.Select(d =>
    {
        var nextY = y + d.Y;
        var nextX = x + d.X;

        // If next is in bounds
        if (nextY >= 0 && nextY < input.Length
            && nextX >= 0 && nextX < input[nextY].Length)
        {
            if (input[nextY][nextX] == (at + 1))
            {
                return FindNumberOfPathsPart2(nextY, nextX, at + 1);
            }
        }

        return 0;
    }).Sum();

    return items;
}


internal readonly struct Position(int y, int x)
{
    public int Y { get; } = y;
    public int X { get; } = x;

    public override string ToString()
    {
        return $"X: {X} Y: {Y}";
    }
}
