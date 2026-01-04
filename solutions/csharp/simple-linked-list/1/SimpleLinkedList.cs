using System.Collections;

public class SimpleLinkedList<T> : IEnumerable<T>
{
    private int _count = 0;

    private Node? Head { get; set; }

    public int Count => _count;

    public SimpleLinkedList() { }

    public SimpleLinkedList(params IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Push(value);
        }
    }

    public void Push(T value)
    {
        if (Head is null)
        {
            Head = new Node(value);
        }
        else
        {
            var newNode = new Node(value)
            {
                Next = Head
            };
            Head = newNode;
        }
        _count++;
    }

    public T Pop()
    {
        if (Head is null)
            throw new InvalidOperationException("The list is empty.");

        var value = Head.Value;
        Head = Head.Next;
        _count--;
        return value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new SimpleLinkedListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class Node
    {
        public T Value;

        public Node? Next;

        public Node(T value)
        {
            Value = value;
        }
    }

    private class SimpleLinkedListEnumerator : IEnumerator<T>
    {
        private Node? current;

        private readonly SimpleLinkedList<T> _values;

        public T Current => current!.Value;

        object IEnumerator.Current => Current!;

        public SimpleLinkedListEnumerator(SimpleLinkedList<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            _values = values;
        }

        public void Dispose()
        {
            current = null;
        }

        public bool MoveNext()
        {
            if (current is null)
            {
                // First call to MoveNext
                if (_values.Head is null)
                    return false;

                current = _values.Head;
            }
            else
            {
                // Subsequent calls to MoveNext
                current = current.Next;
                if (current is null)
                    return false;
            }
            return true;
        }

        public void Reset()
        {
            current = null;
        }
    }
}