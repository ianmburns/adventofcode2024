if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllText(fileArg).Split(' ').Select(long.Parse).ToArray();

var amounts = input.Select(x => ApplyRules(x, 25)).SelectMany(_ => _);

Console.WriteLine(amounts.Count());

amounts = input.Select(x => ApplyRules(x, 75)).SelectMany(_ => _);
Console.WriteLine(amounts.Count());


static long[] ApplyRules(long value, int iterations)
{
    if (iterations == 0)
    {
        return [value];
    }

    // If the stone is engraved with the number 0, 
    //  it is replaced by a stone engraved with the number 1.
    // If the stone is engraved with a number that has an even number of digits,
    //  it is replaced by two stones. The left half of the digits are engraved on the new left stone,
    //  and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
    // If none of the other rules apply, the stone is replaced by a new stone;
    //  the old stone's number multiplied by 2024 is engraved on the new stone.


    if (value == 0)
    {
        return ApplyRules(1, iterations - 1);
    }

    // Maybe a better way to do this
    var valueAsString = value.ToString();
    if (valueAsString.Length % 2 == 0)
    {
        //Console.WriteLine(valueAsString);
        //Split string
        int middleIndex = valueAsString.Length / 2;
        string firstHalf = valueAsString[..middleIndex];
        string secondHalf = valueAsString[middleIndex..];

        return [.. ApplyRules(long.Parse(firstHalf), iterations - 1)
                , .. ApplyRules(long.Parse(secondHalf), iterations - 1)];
    }

    return ApplyRules(value * 2024, iterations - 1);
}

