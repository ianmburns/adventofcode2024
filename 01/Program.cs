if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var fileInput = File.ReadAllLines(fileArg);

var (first, second) = ParseLines(fileInput);

var sumOfDifferences = first.Zip(second, (a, b) => Math.Abs(a - b)).Sum();

Console.WriteLine(sumOfDifferences);

var sumOfSimilarity = first.Select(f => f * second.Count(s => s == f)).Sum();

Console.WriteLine(sumOfSimilarity);

return;

// Returns two arrays of equal length, ordered ascending
static Tuple<int[], int[]> ParseLines(string[] input)
{
    var parts = input.Select(line => line.Split(' ')).ToArray();

	var first = parts.Select(p => int.Parse(p[0])).Order().ToArray();
	var second =  parts.Select(p => int.Parse(p[^1])).Order().ToArray();

    return new Tuple<int[], int[]>(first, second);
}