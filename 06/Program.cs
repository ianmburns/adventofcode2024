if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllLines(fileArg);

const char guardMoveUp = '^';
const char guardMoveRight = '>';
const char guardMoveDown = 'v';
const char guardMoveLeft = '<';

HashSet<char> guardChars = [guardMoveUp, guardMoveRight, guardMoveDown, guardMoveLeft];

// velocity is y, x
var velocityUp = new Tuple<int, int>(-1, 0);
var velocityRight = new Tuple<int, int>(0, 1);
var velocityDown = new Tuple<int, int>(1, 0);
var velocityLeft = new Tuple<int, int>(0, -1);

var guardVelocityMap = new Dictionary<char, Tuple<int, int>>
{
    { guardMoveUp, velocityUp },
    { guardMoveRight, velocityRight },
    { guardMoveDown, velocityDown },
    { guardMoveLeft, velocityLeft },
};

var velocityGuardMap = guardVelocityMap.ToDictionary(key => key.Value, value => value.Key);

var velocityTurnMap = new Dictionary<Tuple<int, int>, Tuple<int, int>>
{
    { velocityUp, velocityRight },
    { velocityRight, velocityDown },
    { velocityDown, velocityLeft },
    { velocityLeft, velocityUp },
};

const char blocker = '#';

//Find current position
var initialPosY = -1;
var initialPosX = -1;

for (var y = 0; y < input.Length; y++)
{
    for (var x = 0; x < input[y].Length; x++)
    {
        if (guardChars.Contains(input[y][x]))
        {
            initialPosY = y;
            initialPosX = x;
        }
    }
}


// Use a dictionary with a velocity to determine if we are in a loop, i.e. if we've already visited
// this spot going in the same direction
// Dictionary is Position -> Guard Direction
var capturedPositions = new Dictionary<Tuple<int, int>, HashSet<char>>();
var loopCount = 0;

var guard = input[initialPosY][initialPosX];
_ = Run(initialPosY, initialPosX, new Tuple<int, int>(-1,-1));
Console.WriteLine(capturedPositions.Count);

// part2
var obstacles = capturedPositions.Keys.ToList();

foreach (var obstaclePosition in obstacles)
{
    // Reset
    var posY = initialPosY;
    var posX = initialPosX;
    guard = input[posY][posX];
    capturedPositions.Clear();

    loopCount += Run(posY, posX, obstaclePosition) ? 1:0;
}

Console.WriteLine(loopCount);

// Print map
// foreach (var se in input)
// {
//     Console.WriteLine(se);
// }

return;

bool Run(int posY, int posX, Tuple<int, int> obstaclePosition)
{
    while (true)
    {
        //If true, we are in a loop
        if (CapturePosition(posY, posX))
        {
            return true;
        }

        var currentVelocity = guardVelocityMap[guard];
        // Check next space
        var nextPosY = posY + currentVelocity.Item1;
        var nextPosX = posX + currentVelocity.Item2;

        // Check if at boundary first
        if (nextPosY < 0 || nextPosX < 0 ||  nextPosY >= input.Length || nextPosX >= input[nextPosY].Length)
        {
            break;
        }
        
        // turn until unblocked
        bool BlockedByBlocker(int y, int x) => input[y][x] == blocker;
        bool BlockedByObstacle(int y, int x) => obstaclePosition.Item1 == y && obstaclePosition.Item2 == x;
        // just in case
        var startPosition = guard;
        while(BlockedByBlocker(nextPosY, nextPosX) || BlockedByObstacle(nextPosY, nextPosX))
        {
            // Get new velocity
            var turnedVelocity = velocityTurnMap[guardVelocityMap[guard]];

            // Move guard forward by turned position
            nextPosY = posY + turnedVelocity.Item1;
            nextPosX = posX + turnedVelocity.Item2;
            guard = velocityGuardMap[turnedVelocity];

            if (guard == startPosition)
            {
                throw new Exception("I'M SPINNING!!!");
            }
        }

        posY = nextPosY;
        posX = nextPosX;
    }

    return false;
}

// Returns true if we've visited this in the same direction
bool CapturePosition(int posY, int posX)
{
    //This is useful for debugging
    //input[posY] = ReplaceAt(input[posY], posX, 1, "X");

    var position = new Tuple<int, int>(posY, posX);
    
    if (!capturedPositions.TryAdd(position, [guard]))
    {
        if (capturedPositions[position].Contains(guard))
        {
            return true;
        }

        capturedPositions[position].Add(guard);
    }

    return false;
}

static string ReplaceAt(string str, int index, int length, string replace)
{
    return str.Remove(index, Math.Min(length, str.Length - index))
        .Insert(index, replace);
}