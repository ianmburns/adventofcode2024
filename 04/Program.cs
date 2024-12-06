if (args == null || args.Length == 0)
{
    throw new ArgumentException("You must provide at least one argument.");
}

var fileArg = args[0];

var input = File.ReadAllLines(fileArg);

//directions
// {row, col} to add to initial 'X' character
//up
int[,] up = { { -1, 0 },{ -2, 0 },{ -3, 0 } };
// up right
int[,] upRight = { { -1, 1 },{ -2, 2 },{ -3, 3 } };

// right
int[,] right = { { 0, 1 },{ 0, 2 },{ 0, 3 } };

// down right
int[,] downRight = { { 1, 1 },{ 2, 2 },{ 3, 3 } };

// down
int[,] down = { { 1, 0 },{ 2, 0 },{ 3, 0 } };

// down left
int[,] downLeft = { { 1, -1 },{ 2, -2 },{ 3, -3 } };

// left
int[,] left = { { 0, -1 },{ 0, -2 },{ 0, -3 } };

// up left
int[,] upLeft = { { -1, -1 },{ -2, -2 },{ -3, -3 } };

// XMAS
var xmasSearch = new[] { up, upRight, right, downRight, down, downLeft, left, upLeft };

//M.M     1.3
//.A.     .A.
//S.S     4.2
// Start with A and look around for "msms" but in different positions

int[,] msms = { {-1, -1}, {1, 1}, {-1, 1}, {1, -1} }; //1,2,3,4
int[,] mssm = { {-1, -1}, {1, 1}, {1, -1}, {-1, 1} }; //1,2,4,3
int[,] smsm = { {1, 1}, {-1, -1}, {1, -1}, {-1, 1} }; //2,1,4,3 
int[,] smms = { {1, 1}, {-1, -1}, {-1, 1}, {1, -1} }; //2,1,3,4

var x_masSearch = new[] { msms, mssm, smsm, smms };

// loop through each line
var foundCount = 0;
for (var row = 0; row < input.Length; row++)
{
    for (var col = 0; col < input[row].Length; col++)
    {
        if (input[row][col] != 'X')
        {
            continue;
        }

        foundCount += xmasSearch.Count(s => Search(input, row, col, s, "MAS"));
    }
}

Console.WriteLine(foundCount);

foundCount = 0;
for (var row = 0; row < input.Length; row++)
{
    for (var col = 0; col < input[row].Length; col++)
    {
        if (input[row][col] != 'A')
        {
            continue;
        }

        foundCount += x_masSearch.Count(s => Search(input, row, col, s, "MSMS"));
    }
}

Console.WriteLine(foundCount);
return;

bool Search(string[] lines, int rowPos, int colPos, int[,] coordinates, string searchTerm)
{
    // foreach letter in search term
    for (var searchTermIndex = 0; searchTermIndex < searchTerm.Length; searchTermIndex++)
    {
        var row = coordinates[searchTermIndex, 0] + rowPos;
        var col = coordinates[searchTermIndex, 1] + colPos;

        var expectedLetter = searchTerm[searchTermIndex];
        //try
        //{
            if ((row < 0 || row > lines.Length-1) || (col < 0 || col > lines[row].Length-1))
            {
                return false;
            }
            
            //Console.WriteLine($"{searchTermIndex}: {rowPos}, {colPos}");
            //Console.WriteLine($"input[{row}][{col}]");
            var actualLetter = lines[row][col];
            
            //Console.WriteLine($"{expectedLetter} == {actualLetter}");
            if (expectedLetter != actualLetter)
            {
                return false;
            }
        //}
        //catch
        //{
        //    Console.WriteLine($"{row} {col}");
        //    Console.WriteLine($"{lines.Length}");
        //    throw;
        //}
    }

    return true;
}