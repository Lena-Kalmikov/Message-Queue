using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class LinkedList<T> : IEnumerable<T>
    {
        LinkedListNode<T> start;
        LinkedListNode<T> end;
        public int Count { get; private set; }

        public LinkedListNode<T> GetFirstNode()
        {
            return start;
        }

        public void AddFirst(T value) // O(1) - O(c)
        {
            //add managment to end
            LinkedListNode<T> n = new LinkedListNode<T>(value);
            n.next = start;
            start = n;
            if (end == null) end = n;
            Count++;
        }

        public void AddLast(T value)  // O(1)
        {
            if (start == null)
            {
                AddFirst(value);
                return;
            }

            LinkedListNode<T> n = new LinkedListNode<T>(value);
            end.next = n;
            end = n;
            Count++;
        }

        public bool RemoveFirst() // O(1)
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


        public T GetAt(int index) // O(n)
        {
            if (index < 0 || index >= Count) return default;

            LinkedListNode<T> tmp = start;
            for (int i = 0; i < index; i++)
            {
                tmp = tmp.next;
            }
            return tmp.Value;
        }


        public override string ToString()
        {
            StringBuilder allValues = new StringBuilder();

            LinkedListNode<T> tmp = start;
            while (tmp != null)
            {
                allValues.Append(tmp.Value + " ");
                tmp = tmp.next;
            }
            return allValues.ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> current = start;

            while (current != null)
            {
                yield return current.Value;
                current = current.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() // for old, non-generic environments
        {
            return GetEnumerator();
        }
    }
}
