using System;
using System.Collections;
using System.Collections.Generic;

namespace Laba9_variant7_Part3
{
    internal class Node<T>
    {
        public T Data;
        public Node<T> Prev;
        public Node<T> Next;
        public Node(T data) => Data = data;
    }

    internal class DoublyLinkedList<T> : IEnumerable<T>
    {
        private Node<T> head;
        private Node<T> tail;
        public int Count { get; private set; }

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

        public T GetAt(int index)
        {
            var node = GetNodeAt(index);
            if (node == null) throw new ArgumentOutOfRangeException(nameof(index));
            return node.Data;
        }

        public bool SetAt(int index, T data)
        {
            var node = GetNodeAt(index);
            if (node == null) return false;
            node.Data = data;
            return true;
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

        private Node<T> GetNodeAt(int index)
        {
            if (index < 0 || index >= Count) return null;
            Node<T> cur;
            if (index < Count / 2)
            {
                cur = head;
                for (int i = 0; i < index; i++) cur = cur.Next;
            }
            else
            {
                cur = tail;
                for (int i = Count - 1; i > index; i--) cur = cur.Prev;
            }
            return cur;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var cur = head;
            while (cur != null)
            {
                yield return cur.Data;
                cur = cur.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
