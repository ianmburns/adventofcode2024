List<Position> directions = [
    new Position(-1,0), // up
    new Position(0,1), // right
    new Position(1,0), // down
    new Position(0,-1), // left
];

Position[][] outerCorners = [
    [ directions[0], directions[1]], // up and right
    [ directions[0], directions[3]], // up and left
    [ directions[2], directions[1]], // down and right
    [ directions[2], directions[3]], // down and left
];


if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];
var input = File.ReadAllLines(fileArg);


HashSet<Position> allVisitedPositions = [];
var totalValue = 0;

for (var y = 0; y < input.Length; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {

        var position = new Position(y, x);

        // Skip if we've alreay seen this position
        if (allVisitedPositions.Contains(position))
        {
            continue;
        }

        var currentValue = input[y][x];
        var area = 0;
        var perimeter = 0;
        var sides = 0;

        HashSet<Position> currentVisitedPositions = [];
        Stack<Position> stack = new();
        stack.Push(position);

        while (stack.Count > 0)
        {
            var currentPosition = stack.Pop();
            allVisitedPositions.Add(currentPosition);
            if (currentVisitedPositions.Add(currentPosition))
            {
                // Console.WriteLine($"Adding area for \"{currentValue}\" at {currentPosition.Y} {currentPosition.X}");
                area += 1;
            }

            foreach (var direction in directions)
            {
                var nextPosition = currentPosition.Next(direction);

                var isInBounds = nextPosition.IsInBounds(input);

                // Add permiter if we hit a wall
                if (!isInBounds)
                {
                    // Console.WriteLine($"Adding boundary perimeter for \"{currentValue}\" at {currentPosition.Y} {currentPosition.X}");
                    perimeter += 1;

                    continue;
                }


                var nextValue = input[nextPosition.Y][nextPosition.X];
                // Add perimiter if we hit a value that doesn't match our current search
                if (currentValue != nextValue)
                {
                    // Console.WriteLine($"Adding not match perimeter for \"{currentValue}\"!= {nextValue} at {currentPosition.Y} {currentPosition.X}");
                    perimeter += 1;
                    continue;
                }

                // If its the same value, add it to the stack
                // but only if we haven't already visited it
                if (currentVisitedPositions.Contains(nextPosition) || stack.Contains(nextPosition)) // I dont love this
                {
                    continue;
                }

                stack.Push(nextPosition);
            }
        }

        //Console.WriteLine($"Value: {currentValue} Area: {area} Permiter: {perimeter} Total: {area * perimeter}");
        totalValue += (area * sides);
    }
}

Console.WriteLine(totalValue);
return;


internal readonly struct Position(int y, int x)
{
    public int Y { get; } = y;
    public int X { get; } = x;

    public bool IsInBounds(string[] input)
    {
        return Y >= 0 && Y < input.Length
            && X >= 0 && X < input[Y].Length;
    }

    public Position Next(Position velocity)
    {
        var nextY = Y + velocity.Y;
        var nextX = X + velocity.X;

        return new Position(nextY, nextX);
    }

    public override string ToString()
    {
        return $"X: {X} Y: {Y}";
    }
}


