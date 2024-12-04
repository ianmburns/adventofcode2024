using System.Linq; 

if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var fileInput = File.ReadAllText(fileArg);

var parsedInput = ParseInput(fileInput);

var pivotInput = PivotInput(parsedInput);

var orderedInput = OrderInput(pivotInput);

var sumOfDifferences = GetSumOfDifferences(orderedInput);

Console.WriteLine(sumOfDifferences);


static int[][] ParseInput(string input)
{
    var lines = input.Trim().Split(Environment.NewLine);
    var result = new int[lines.Length][];

    for (var i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        var split = line.Split(' ');
        var first = int.Parse(split[0]);
        var last = int.Parse(split[^1]);

        result[i] = [first, last];
    }

    return result;
}

static int[][] PivotInput(int[][] input)
{
    var result = new int[2][];
    result[0] = new int[input.Length];
    result[1] = new int[input.Length];
    
    for (var i = 0; i < input.Length; i++)
    {
        result[0][i] = input[i][0];
        result[1][i] = input[i][1];
    }
    
    return result;
}

static int[][] OrderInput(int[][] input)
{
    var result = new int[2][];

    
    result[0] = input[0].OrderBy(x => x).ToArray();
    result[1] = input[1].OrderBy(x => x).ToArray();
    
    return result;
}

static int GetSumOfDifferences(int[][] input)
{
    var sumOfDifferences = 0;

    for (var i = 0; i < input[0].Length; i++)
    {
        var first = input[0][i];
        var second = input[1][i];

        sumOfDifferences += Math.Abs(first - second);
    }

    return sumOfDifferences;
}