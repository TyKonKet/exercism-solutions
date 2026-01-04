public static class SimpleCalculator
{
    private static readonly Dictionary<string, Func<int, int, int>> _operations = new()
    {
        ["+"] = SimpleOperation.Addition,
        ["*"] = SimpleOperation.Multiplication,
        ["/"] = SimpleOperation.Division,
    };

    public static string Calculate(int operand1, int operand2, string? operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        
        if (string.IsNullOrWhiteSpace(operation))
            throw new ArgumentException(null, nameof(operation));

        if (!_operations.TryGetValue(operation, out var op))
            throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}");

        try
        {
            var result = op(operand1, operand2);
            return $"{operand1} {operation} {operand2} = {result}";
        }
        catch (DivideByZeroException)
        {
            return "Division by zero is not allowed.";
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }
}