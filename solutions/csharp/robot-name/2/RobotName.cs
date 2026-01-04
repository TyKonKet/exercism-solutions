public sealed class Robot : IDisposable
{
    private RobotName _robotName = RobotName.Get();

    public string Name { get => _robotName.ToString(); }

    public void Reset()
    {
        var newName = RobotName.Get();
        _robotName.Dispose();
        _robotName = newName;
    }

    public void Dispose()
    {
        _robotName.Dispose();
    }

    private sealed class RobotName(string name) : IDisposable
    {
        private static readonly HashSet<string> _availableNames = [.. Enumerable.Range(0, 676000)
            .OrderBy(i => i % 321)
            .Select(i => $"{(char)('A' + (i / 26000) % 26)}{(char)('A' + (i / 1000) % 26)}{(i % 1000):D3}")];

        public static RobotName Get()
        {
            if (_availableNames.Count == 0)
            {
                throw new InvalidOperationException("No more robot names available.");
            }
            var selectedName = _availableNames.First();
            _availableNames.Remove(selectedName);
            return new RobotName(selectedName);
        }

        public void Dispose()
        {
            _availableNames.Add(name);
        }

        public override string ToString()
        {
            return name;
        }
    }
}