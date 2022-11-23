using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public class LinkedListQueue<T> : IEnumerable<T>
    {
        LinkedList<T> queueLinkedList;

        public LinkedListQueue()
        {
            queueLinkedList = new LinkedList<T>();
        }

        public void EnQueue(T itemInUnsert) => queueLinkedList.AddLast(itemInUnsert);

        public bool DeQueue()
        {
            if (queueLinkedList.GetAt(0) == null) return false;

            queueLinkedList.RemoveFirst();
            return true;
        }

        public T Peek()
        {
            return this.PeekAt(0);
        }

        public T PeekAt(int index)
        {
            return queueLinkedList.GetAt(index);
        }

        public LinkedListNode<T> GetFirstNode()
        {
            return queueLinkedList.GetFirstNode(); 
        }

        public IEnumerator<T> GetEnumerator() => queueLinkedList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
