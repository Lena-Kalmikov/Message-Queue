using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class DoubleLinkedList<T> : IEnumerable<T>
    {
        Node start;
        Node end;
        public int Count { get; private set; }

        public void AddFirst(T value)
        {
            Node node = new Node(value);
            node.next = start;
            node.previous = null;

            if (start != null)
                start.previous = node;

            if (end == null)
                end = node;

            start = node;
            Count++;
        }

        public void AddLast(T value)
        {
            Node node = new Node(value);
            node.next = null;

            if (start == null)
            {
                node.previous = null;
                start = node;
                end = node;
                Count++;
                return;
            }

            Node currentLast = end;
            currentLast.next = node;

            end = node;
            end.next = null;
            end.previous = currentLast;

            Count++;
        }

        public bool RemoveFirst()
        {
            if (start == null) return false;
            else
            {
                start = start.next;
                if (start == null) end = start;
                Count--;
                return true;
            }
        }

        public bool RemoveLast()
        {
            Node oneBeforeLast = end.previous;
            oneBeforeLast.next = null;
            end = oneBeforeLast;
            Count--;
            return true;
        }

        public T GetLast()
        {
            return end.value;
        }

        public T GetFirstValue()
        {
            return start.value;
        }

        public Node GetFirstNode()
        {
            return start;
        }

        public Node GetLastNode()
        {
            return end;
        }

        public T GetAt(int index)
        {
            if (index < 0 || index >= Count) return default;

            Node tmp = start;

            for (int i = 0; i < index; i++)
            {
                tmp = tmp.next;
            }
            return  tmp.value;
        }

        public bool AddAt(int index, T value)
        {
            Node node = new Node(value);
            node.next = null;
            node.previous = null;

            if (index < 1)
            {
                Console.Write("\nPosition should be 1 or higher.");
            }
            else if (index == 1)
            {
                node.next = start;
                start.previous = node;
                start = node;
            }
            else
            {
                Node tmp = start;

                for (int i = 1; i < index - 1; i++)
                {
                    if (tmp != null)
                    {
                        tmp = tmp.next;
                    }
                }

                if (tmp != null)
                {
                    node.next = tmp.next;
                    node.previous = tmp;
                    tmp.next = node;
                    if (node.next != null)
                        node.next.previous = node;
                }
                else
                {
                    Console.Write("\nCan't add here, the previous node is null!");
                }
            }

            return true;
        }

        public bool RemoveAt(int index)
        {
            if (index > Count)
                return false;

            Node tmp = start;

            for (int i = 0; i < index - 1; i++)
            {
                if (tmp != null)
                {
                    tmp = tmp.next;
                }
            }

            Node index_prev = tmp.previous;
            Node index_next = tmp.next;

            if (index_prev != null)
                index_prev.next = index_next;

            if (index_next != null)
                index_next.previous = index_prev;

            Count--;

            return true;
        }

        public override string ToString()
        {
            StringBuilder allValues = new StringBuilder();

            Node tmp = start;
            while (tmp != null)
            {
                allValues.Append(tmp.value + " ");
                tmp = tmp.next;
            }
            return allValues.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = start;
            while (current != null)
            {
                yield return current.value;
                current = current.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Node
        {
            public T value;
            public Node next;
            public Node previous;

            public Node(T value)
            {
                this.value = value;
                next = null;
            }

            public Node(T value, Node nextVal, Node prevVal)
            {
                this.value = value;
                next = nextVal;
                previous = prevVal;
            }
        }
    }
}
