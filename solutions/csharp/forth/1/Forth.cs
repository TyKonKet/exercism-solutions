public static class Forth
{
    private static readonly Stack<long> _stack = [];

    private static readonly Dictionary<string, string> _words = new(StringComparer.OrdinalIgnoreCase);

    public static string Evaluate(string[] instructionsLines)
    {
        Clear();

        foreach (var instructions in instructionsLines)
        {
            ParseInstructions(instructions);
        }

        return string.Join(" ", _stack.Reverse());
    }

    private static void ParseInstructions(string instructions)
    {
        var instructionsList = instructions
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // pre-process instructions to handle word definitions
        // definitions have the form: ": wordName instructions ;"
        if (instructionsList.Contains(":"))
        {
            var indexesToRemove = new List<int>();
            var definitionParsingStarted = false;
            for (int i = 0; i < instructionsList.Length; i++)
            {
                if (definitionParsingStarted)
                {
                    var wordName = instructionsList[i];
                    if (long.TryParse(wordName, out _))
                    {
                        throw new InvalidOperationException("Cannot redefine numbers");
                    }
                    indexesToRemove.Add(i);

                    var wordInstructios = new List<string>();
                    i++;
                    while (definitionParsingStarted)
                    {
                        indexesToRemove.Add(i);
                        definitionParsingStarted = instructionsList[i] != ";";
                        if (definitionParsingStarted)
                        {
                            wordInstructios.Add(instructionsList[i]);
                        }
                        i++;
                    }

                    // word instructions "pointing" to previously defined words must be expanded now
                    for (int j = 0; j < wordInstructios.Count; j++)
                    {
                        if (_words.TryGetValue(wordInstructios[j], out var existingWordInstructions))
                        {
                            wordInstructios[j] = existingWordInstructions;
                        }
                    }
                    // store the new word definition
                    _words[wordName] = string.Join(" ", wordInstructios);
                }
                else
                {
                    definitionParsingStarted = instructionsList[i] == ":";
                    indexesToRemove.Add(i);
                }
            }

            // remove definition instructions from the main instruction list to allow multiple definitions and definitions-instructions mixing on the same line
            var tempList = new List<string>(instructionsList);
            for (int i = indexesToRemove.Count - 1; i >= 0; i--)
            {
                tempList.RemoveAt(indexesToRemove[i]);
            }
            instructionsList = [.. tempList];
        }

        foreach (var instruction in instructionsList)
        {
            RunInstruction(instruction);
        }
    }

    private static void RunInstruction(string instruction)
    {
        if (_words.TryGetValue(instruction, out var wordInstructions))
        {
            ParseInstructions(wordInstructions);
            return;
        }

        switch (instruction.ToLowerInvariant())
        {
            case "dup":
                RunDup();
                break;
            case "drop":
                RunDrop();
                break;
            case "swap":
                RunSwap();
                break;
            case "over":
                RunOver();
                break;
            case "+":
                RunAdd();
                break;
            case "-":
                RunSubtract();
                break;
            case "*":
                RunMultiply();
                break;
            case "/":
                RunDivide();
                break;
            default:
                RunLiteral(instruction);
                break;
        }
    }

    private static void RunLiteral(string instruction)
    {
        if (long.TryParse(instruction, out var literal))
        {
            _stack.Push(literal);
        }
        else
        {
            throw new InvalidOperationException("Invalid token: " + instruction);
        }
    }

    private static void RunDup()
    {
        var a = _stack.Peek();
        _stack.Push(a);
    }

    private static void RunDrop() => _stack.Pop();

    private static void RunSwap()
    {
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(a);
        _stack.Push(b);
    }

    private static void RunOver()
    {
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(b);
        _stack.Push(a);
        _stack.Push(b);
    }

    private static void RunAdd()
    {
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(a + b);
    }

    private static void RunSubtract()
    {
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(b - a);
    }

    private static void RunMultiply()
    {
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(a * b);
    }

    private static void RunDivide()
    {
        var a = _stack.Pop();
        var b = _stack.Pop();
        _stack.Push(b / a);
    }

    private static void Clear()
    {
        _stack.Clear();
        _words.Clear();
    }
}