if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];
var input = File.ReadAllText(fileArg).Split(' ').Select(long.Parse).ToArray();

var amounts = ApplyRules(input, 75);
Console.WriteLine(amounts);

long ApplyRules(long[] seed, int iterations)
{

    var currentStoneCounter = new Dictionary<long, long>();
    foreach (var item in seed)
    {
        currentStoneCounter.TryAdd(item, 0);
        currentStoneCounter[item]++;
    }

    // For the amount of iterations
    for (var i = 0; i < iterations; i++)
    {
        // walk through each 'type' of rock and figure out how many of each next type of rock there will be
        // use a copy of the dictionary so we don't overwrite
        foreach (var (value, count) in currentStoneCounter.ToList())
        {
            // Remove stone count
            currentStoneCounter[value] -= count;

            // Add to counter based on rule
            if (value == 0)
            {
                currentStoneCounter.TryAdd(1, 0);
                currentStoneCounter[1] += count;
            }
            else if (value.ToString().Length % 2 == 0)
            {
                var valueAsString = value.ToString();

                //Split string
                int middleIndex = valueAsString.Length / 2;
                var firstHalf = long.Parse(valueAsString[..middleIndex]);
                var secondHalf = long.Parse(valueAsString[middleIndex..]);


                currentStoneCounter.TryAdd(firstHalf, 0);
                currentStoneCounter[firstHalf] += count;

                currentStoneCounter.TryAdd(secondHalf, 0);
                currentStoneCounter[secondHalf] += count;
            }
            else
            {
                var updated = value * 2024;
                currentStoneCounter.TryAdd(updated, 0);
                currentStoneCounter[updated] += count;
            }
        }
    }


    return currentStoneCounter.Select(_ => _.Value).Sum();
}

