using System.Text;

public class SgfTree
{
    public SgfTree(IDictionary<string, string[]> data, params SgfTree[] children)
    {
        Data = data;
        Children = children;
    }

    public IDictionary<string, string[]> Data { get; }
    public SgfTree[] Children { get; }
}

public class SgfParser
{
    public static SgfTree ParseTree(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Invalid input.");
        }

        var parser = new Parser(input);
        var tree = parser.ParseGameTree();
        if (!parser.AtEnd)
        {
            throw new ArgumentException("Invalid input.");
        }

        return tree;
    }

    private sealed class Parser
    {
        private readonly string _input;
        private int _index;

        public Parser(string input)
        {
            _input = input;
        }

        public bool AtEnd => _index >= _input.Length;

        public SgfTree ParseGameTree()
        {
            Expect('(');

            var nodes = new List<IDictionary<string, string[]>>();
            while (TryPeek(';'))
            {
                nodes.Add(ParseNode());
            }

            if (nodes.Count == 0)
            {
                throw new ArgumentException("Invalid input.");
            }

            var children = new List<SgfTree>();
            while (TryPeek('('))
            {
                children.Add(ParseGameTree());
            }

            Expect(')');

            var tree = new SgfTree(nodes[^1], [.. children]);
            for (var i = nodes.Count - 2; i >= 0; i--)
            {
                tree = new SgfTree(nodes[i], tree);
            }

            return tree;
        }

        private Dictionary<string, string[]> ParseNode()
        {
            Expect(';');

            var properties = new Dictionary<string, List<string>>();
            while (!AtEnd && !IsChar(';') && !IsChar('(') && !IsChar(')'))
            {
                var key = ParsePropertyIdentifier();
                if (!TryPeek('['))
                {
                    throw new ArgumentException("Invalid input.");
                }

                if (properties.ContainsKey(key))
                {
                    throw new ArgumentException("Invalid input.");
                }

                var values = new List<string>();
                while (TryPeek('['))
                {
                    values.Add(ParsePropertyValue());
                }

                if (values.Count == 0)
                {
                    throw new ArgumentException("Invalid input.");
                }

                properties[key] = values;
            }

            var data = new Dictionary<string, string[]>(properties.Count);
            foreach (var entry in properties)
            {
                data[entry.Key] = [.. entry.Value];
            }

            return data;
        }

        private string ParsePropertyIdentifier()
        {
            if (AtEnd || !IsUpper(Current))
            {
                throw new ArgumentException("Invalid input.");
            }

            var start = _index;
            while (!AtEnd && IsUpper(Current))
            {
                _index++;
            }

            return _input[start.._index];
        }

        private string ParsePropertyValue()
        {
            Expect('[');

            var builder = new StringBuilder();
            var escaped = false;
            while (true)
            {
                if (AtEnd)
                {
                    throw new ArgumentException("Invalid input.");
                }

                var ch = Current;
                _index++;

                if (escaped)
                {
                    if (ch == '\n')
                    {
                        // Escaped newline is removed.
                    }
                    else if (char.IsWhiteSpace(ch))
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Append(ch);
                    }

                    escaped = false;
                    continue;
                }

                if (ch == '\\')
                {
                    escaped = true;
                    continue;
                }

                if (ch == ']')
                {
                    break;
                }

                if (ch == '\n')
                {
                    builder.Append('\n');
                }
                else if (char.IsWhiteSpace(ch))
                {
                    builder.Append(' ');
                }
                else
                {
                    builder.Append(ch);
                }
            }

            return builder.ToString();
        }

        private char Current => _input[_index];

        private bool TryPeek(char expected) => !AtEnd && _input[_index] == expected;

        private bool IsChar(char expected) => !AtEnd && _input[_index] == expected;

        private void Expect(char expected)
        {
            if (AtEnd || _input[_index] != expected)
            {
                throw new ArgumentException("Invalid input.");
            }

            _index++;
        }

        private static bool IsUpper(char value) => value >= 'A' && value <= 'Z';
    }
}
