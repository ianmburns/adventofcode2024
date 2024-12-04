using System.Globalization;

if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var fileInput = File.ReadAllText(fileArg);

var total = 0;
var stop = "mul(x,x)".Length; // minimum stopping point

for (var i = 0; i < fileInput.Length - stop; i++)
{
    //Don't Mode
    var dont = fileInput[i..(i+7)];
    if (dont == "don't()")
    {
        //Loop until we find "do()"
        while (fileInput[i..(i+4)] != "do()" && i < (fileInput.Length - stop))
        {
            i++;
        }
        
        // check if we went too far
        if (i >= fileInput.Length - stop)
        {
            break;
        }
    }
    
    var mulParen = fileInput[i..(i+4)];
    if (mulParen != "mul(")
    {
        continue;
    }
    
    //find next )
    var j = i;
    while(j < fileInput.Length)
    {
        if (fileInput[j] == ')')
        {
            break;
        }

        j++;
    }

    if (fileInput[j] != ')')
    {
        continue;
    }

    /*
     * mul(1,2)
     * 12345678
     * i      j
     * we want 1,2
     * substring(5,3)
     * i+4,j-(i+4)
     */
    
    var between = fileInput.Substring(i+4, (j-(i+4)));

    var split = between.Split(',');
    if (split.Length != 2)
    {
        continue;
    }

    if (!int.TryParse(split[0], NumberStyles.None, null, out var first))
    {
        continue;
    }
    
    if (!int.TryParse(split[1], NumberStyles.None, null, out var second))
    {
        continue;
    }

    total += first * second;
}

Console.WriteLine(total);
