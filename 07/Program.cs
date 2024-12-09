using System.Text;

if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllLines(fileArg);

Calculate[] allOperators =
[
    Add,
    Mul,
    Combine
];

// Save permutations
var permDic = new Dictionary<int, List<List<Calculate>>>();

long equationSum = 0;
foreach (var equation in input.AsParallel())
{
    var expressions = equation.Split(':');
    var expectedResult = long.Parse(expressions[0]);

    var terms = expressions[1].Trim().Split(' ').Select(long.Parse).ToArray();

    // Loop through all iterations of operators until we find one that matches the result

    // a + b + c
    // a * b + c
    // a + b * c
    // a * b * c

    // Set up all combinations
    var numberOfOperations = terms.Length - 1;

    List<List<Calculate>> permutations;
    if (permDic.TryGetValue(numberOfOperations, out var found))
    {
        permutations = found;
    }
    else
    {
        permutations = GetCombinations(allOperators, numberOfOperations);
        permDic[numberOfOperations] = permutations;
    }

    // It would be better to use a tree structure but instead we are
    // just making a cache that holds a 'hash' or sorts for when we reach
    // a part of a tree that is too large, so anything that contains that part
    // of the key is invalid
    // e.g. if "A + B" is invalid then "A + B + C" will be invalid and we can skip the computation
    var invalidCache = new HashSet<string>();
    foreach (var perm in permutations)
    {
        // Check if any item has been invalid
        var invalid = perm
            .Select((_, pi) =>
            {
                return ToCacheKey(perm[0..(pi + 1)]); // end range is exclusive
            })
            .Any(invalidCache.Contains);
        if (invalid)
        {
            continue;
        }


        var i = 0;

        // LINQ Aggregate method, but with a short-circuit
        // var result = terms.Aggregate((a, b) => perm[i++](a, b));
        using var enumerator = terms.AsEnumerable().GetEnumerator();
        if (!enumerator.MoveNext())
        {
            break;
        }

        var result = enumerator.Current;
        while (enumerator.MoveNext())
        {
            result = perm[i++](result, enumerator.Current);
            // Short circuit, stop early if greater than current result
            if (result > expectedResult)
            {

                // Add cache keys to invalid cache
                invalidCache.Add(ToCacheKey(perm[0..i])); // end range is exclusive but we already incremented the value
                break;
            }
        }

        if (result == expectedResult)
        {
            // Sum all test values
            equationSum += expectedResult;
            break;
        }
    }
}

Console.WriteLine(equationSum);
return;

// I should use a tree, but lets just do this
static string ToCacheKey(List<Calculate> calculates)
{
    var sb = new StringBuilder();

    foreach (var calculate in calculates)
    {
        sb.Append(calculate.Method.Name);
    }

    return sb.ToString();

}

/* ChatGPT Special */
static List<List<T>> GetCombinations<T>(T[] set, int size)
{
    var result = new List<List<T>>();
    GenerateCombinations(new List<T>(), set, size, result);
    return result;
}

static void GenerateCombinations<T>(List<T> current, T[] set, int size, List<List<T>> result)
{
    if (current.Count == size)
    {
        result.Add([.. current]);
        return;
    }

    foreach (var item in set)
    {
        current.Add(item);
        GenerateCombinations(current, set, size, result);
        current.RemoveAt(current.Count - 1); // Backtrack
    }
}

static long Add(long a, long b)
{
    return a + b;
}

static long Mul(long a, long b)
{
    return a * b;
}

static long Combine(long a, long b)
{
    // could do this with math but...
    return long.Parse($"{a}{b}");
}

delegate long Calculate(long x, long y);
