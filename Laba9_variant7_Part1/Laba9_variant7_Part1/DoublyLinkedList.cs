using System;
using System.Collections;
using System.Collections.Generic;

namespace Laba9_variant7_Part1
{
    public class Node<T>
    {
        public T Data;
        public Node<T> Prev;
        public Node<T> Next;

        public Node(T data)
        {
            Data = data;
        }
    }

    public class DoublyLinkedList<T> : IEnumerable<T>
    {
        private Node<T> head;
        private Node<T> tail;
        public int Count { get; private set; }

        public DoublyLinkedList() { }

        public void AddLast(T data)
        {
            var node = new Node<T>(data);
            if (head == null)
            {
                head = tail = node;
            }
            else
            {
                tail.Next = node;
                node.Prev = tail;
                tail = node;
            }
            Count++;
        }

        public bool RemoveAt(int index)
        {
            var node = GetNodeAt(index);
            if (node == null) return false;

            if (node.Prev != null) node.Prev.Next = node.Next;
            else head = node.Next;

            if (node.Next != null) node.Next.Prev = node.Prev;
            else tail = node.Prev;

            Count--;
            return true;
        }

        public T GetAt(int index)
        {
            var node = GetNodeAt(index);
            if (node == null) throw new ArgumentOutOfRangeException();
            return node.Data;
        }

        public bool SetAt(int index, T data)
        {
            var node = GetNodeAt(index);
            if (node == null) return false;
            node.Data = data;
            return true;
        }

        private Node<T> GetNodeAt(int index)
        {
            if (index < 0 || index >= Count) return null;
            Node<T> current;
            if (index < Count / 2)
            {
                current = head;
                for (int i = 0; i < index; i++) current = current.Next;
            }
            else
            {
                current = tail;
                for (int i = Count - 1; i > index; i--) current = current.Prev;
            }
            return current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
