if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllLines(fileArg);

// split input into rules and updates
var rules = input.TakeWhile(x => x.Length != 0).ToList();
var rulesParse = rules.Select(x => x.Split('|'))
    .ToLookup(x => x[0], x => x[1]);
var updates = input.Skip(rules.Count + 1); // include extra blank line
var updatesParse = updates.Select(x => x.Split(',')).ToArray();

var correctlyOrderedSum = 0;
var incorrectlyOrderedSum = 0;
foreach (var update in updatesParse)
{
    // Console.WriteLine("--------");
    // Console.WriteLine("Update: " + string.Join(", ", update));
    if (update
        .Select((p, i) => (p, i))// All doesnt allow index :| 
        .Skip(1) // first item is always valid
        .All((pi) =>
        {
            var (page, index) = (pi.p, pi.i);
            var pageRules = rulesParse[page];
            var pagesBefore = update[..index];

            // Console.WriteLine("Page: " + page);
            // Console.WriteLine("Index: "+ index);
            // Console.WriteLine("Page Rules: " + string.Join(",", pageRules));
            // Console.WriteLine("Pages Before: " + string.Join(",", pagesBefore));
            return pageRules.All(p => !pagesBefore.Contains(p));
        }))
    {
        correctlyOrderedSum += int.Parse(update[update.Length / 2]);
    }
    else
    {
        // Order by number of applicable rules
        incorrectlyOrderedSum += update.OrderBy(page =>
        {
            return rulesParse[page].Count(rule => update.Contains(rule));
        })
            .Skip(update.Length /2)
            .Select(int.Parse)
            .First();
    }
}

Console.WriteLine(correctlyOrderedSum);
Console.WriteLine(incorrectlyOrderedSum);