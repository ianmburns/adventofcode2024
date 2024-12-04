if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var fileInput = File.ReadAllLines(fileArg);

var safeCount = fileInput
    .Select(line => line.Split(' ').Select(int.Parse).ToList())
    .Select(SafeCount)
    .Sum();

Console.WriteLine(safeCount);

var safeCountDampened = fileInput
    .Select(line => line.Split(' ').Select(int.Parse).ToList())
    .Select(nums =>
    {
        if (SafeCount(nums) == 1)
        {
            return 1;
        }

        //Retry, removing the next index per loop
        for (var i = 0; i < nums.Count; i++)
        {
            List<int> copy = [..nums];
            copy.RemoveAt(i);

            if (SafeCount(copy) == 1)
            {
                return 1;
            }
        }

        return 0;
    })
    .Sum();

Console.WriteLine(safeCountDampened);


return;

int SafeCount(List<int> ints)
{
    bool? isAscending = null;
    for (var i = 1; i < ints.Count; i++)
    {
        var diff = ints[i - 1] - ints[i];
        var direction = diff < 0;
        isAscending ??= direction;
        
        var valid = diff != 0
                    && Math.Abs(diff) <= 3
                    && isAscending == direction;

        if (!valid)
        {
            return 0;
        }
    }

    return 1;
}