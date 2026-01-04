public class Deque<T>
{
    private Node? Head { get; set; }

    private Node? Tail { get; set; }

    public void Push(T value)
    {
        var node = new Node(value);
        if (Tail is null)
        {
            Head = node;
            Tail = node;
        }
        else
        {
            Tail.Next = node;
            node.Previous = Tail;
            Tail = node;
        }
    }

    public T Pop()
    {
        if (Tail is null)
        {
            throw new InvalidOperationException("Deque is empty.");
        }

        var oldTail = Tail;
        Tail = oldTail.Previous;
        if (Tail is null)
        {
            Head = null;
        }
        else
        {
            Tail.Next = null;
        }
        return oldTail.Value;
    }

    public void Unshift(T value)
    {
        var node = new Node(value);
        if (Head is null)
        {
            Head = node;
            Tail = node;
        }
        else
        {
            Head.Previous = node;
            node.Next = Head;
            Head = node;
        }
    }

    public T Shift()
    {
        if (Head is null)
        {
            throw new InvalidOperationException("Deque is empty.");
        }

        var oldHead = Head;
        Head = oldHead.Next;
        if (Head is null)
        {
            Tail = null;
        }
        else
        {
            Head.Previous = null;
        }
        return oldHead.Value;
    }

    private class Node
    {
        public T Value { get; set; }
        public Node? Next { get; set; }

        public Node? Previous { get; set; }

        public Node(T value) => Value = value;
    }
}