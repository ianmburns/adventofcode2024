if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllText(fileArg);

var inputInts = input.Trim().Select(x => int.Parse(x.ToString())).ToList();
//Set up array
var totalLength = inputInts.Sum();
var a = new int?[totalLength];

var lazyCounter = 0;

// Lookup of number and a tuple of how many iterations of that position, and the starting index in the array
var numberLookup = new List<KeyValuePair<int, Tuple<int, int>>>();

for (var i = 0; i < inputInts.Count; i++)
{
    var amount = inputInts[i];
    var option = i % 2 == 0 ? i / 2 : (int?)null;

    if (option.HasValue)
    {
        numberLookup.Insert(0, new(option.Value, new(amount, lazyCounter)));
    }

    for (var j = 0; j < amount; j++)
    {
        a[lazyCounter++] = option;
    }
}

//Part1([.. a]);

Part2([.. a]);

return;

static void Part1(int?[] input)
{
    var pointerStart = 0;
    var pointerEnd = input.Length - 1;

    //Stop when pointers reach each other
    while (pointerStart != pointerEnd)
    {
        //Move end pointer until we find a filled space
        if (input[pointerEnd] == null)
        {
            pointerEnd--;
            continue;
        }

        if (input[pointerStart] != null)
        {
            pointerStart++;
            continue;
        }

        (input[pointerStart], input[pointerEnd]) = (input[pointerEnd], input[pointerStart]);
        pointerEnd--;
    }

    //calculate checksum
    var checksum = input.Select((num, i) => (long)(num ?? 0) * i).Sum();
    Console.WriteLine(checksum);
}

void Part2(int?[] input)
{

    // Use lookup to loop
    foreach (var pair in numberLookup)
    {
        //Find a range that matches the same number of null spaces
        var amount = pair.Value.Item1;
        var firstIndex = pair.Value.Item2;

        for (var i = 0; i < firstIndex; i++)
        {
            //If all are null, move into the space
            if (input[i..(i + amount)].All(x => x == null))
            {
                // Swap the ranges
                for (int swapRangeStart = 0; swapRangeStart < amount; swapRangeStart++)
                {
                    (input[i + swapRangeStart], input[firstIndex + swapRangeStart]) = (input[firstIndex + swapRangeStart], input[i + swapRangeStart]);
                }

                break;
            }

        }
    }

    //calculate checksum
    var checksum = input.Select((num, i) => (long)(num ?? 0) * i).Sum();
    Console.WriteLine(checksum);
}