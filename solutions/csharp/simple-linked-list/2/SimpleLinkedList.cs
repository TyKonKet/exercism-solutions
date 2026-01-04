using System.Collections;

public class SimpleLinkedList<T> : IEnumerable<T>
{
    private Node? Head { get; set; }

    public int Count { get; private set; }

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
        Head = new Node(value, Head);
        Count++;
    }

    public T Pop()
    {
        if (Head is null)
            throw new InvalidOperationException("The list is empty.");

        var value = Head.Value;
        Head = Head.Next;
        Count--;
        return value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = Head;
        while (current is not null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class Node(T value, Node? next)
    {
        public T Value = value;

        public Node? Next = next;
    }
}