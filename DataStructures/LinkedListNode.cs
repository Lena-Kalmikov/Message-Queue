namespace DataStructures
{
    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> next; //reference to the next Node

        public LinkedListNode(T value)
        {
            this.Value = value;
            next = null;
        }

        public LinkedListNode(T value, LinkedListNode<T> nextVal)
        {
            this.Value = value;
            next = nextVal;
        }
    }
}