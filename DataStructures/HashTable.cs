using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class HashTable<TKey, TValue> //: IEnumerable<TKey>
    {
        System.Collections.Generic.LinkedList<Data>[] hashArray;
        int itemsCount = 0;

        public HashTable(int capacity = 100)
        {
            hashArray = new System.Collections.Generic.LinkedList<Data>[capacity];
        }

        public List<string>  GetKeys()
        {
            List<string> keys = new List<string>();
            
            foreach (var list in hashArray)
            {
                if (list != null)
                {
                    foreach (Data keyValueItem in list)
                    {
                        keys.Add(keyValueItem.key.ToString());
                    }
                }
            }

            return keys;
        }

        public void Add(TKey key, TValue value) // O(1) + O(n) when reallocation needed
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] == null) 
                hashArray[ind] = new System.Collections.Generic.LinkedList<Data>();
            else
            {
                if (ContainsKey(key))
                    throw new ArgumentException($"An item with the same key: {key} has already been added.");
            }

            hashArray[ind].AddLast(new Data(key, value));
            itemsCount++;

            if (itemsCount > hashArray.Length)
            {
                ReHash(); // resize and re-allocate all items (reuse Add + KeyToIndex)
            }
        }

        private void ReHash()  // O(n)
        {
            var tmp = hashArray;
            hashArray = new System.Collections.Generic.LinkedList<Data>[hashArray.Length * 2];
            itemsCount = 0;

            foreach (var list in tmp)
            {
                if (list != null)
                {
                    foreach (Data keyValueItem in list)
                    {
                        Add(keyValueItem.key, keyValueItem.value);
                    }
                }
            }
        }

        public double CalcAverLoad()
        {
            // return hashArray.Average(lst => (lst == null) ? 0 : lst.Count);
            return hashArray.Where(lst => lst != null).Average(lst => lst.Count);

        }

        public TValue GetValue(TKey key)
        {
            int ind = KeyToIndex(key);
            Data keyValue;
            if (hashArray[ind] != null)
            {
                keyValue = hashArray[ind].FirstOrDefault(item => item.key.Equals(key));
                if (keyValue != null) return keyValue.value;
            }
            throw new KeyNotFoundException($"No such key: {key}");
        }

        public bool ContainsKey(TKey key) // O(1)
        {
            int ind = KeyToIndex(key);
            if (hashArray[ind] == null) return false;
            return hashArray[ind].Any(item => item.key.Equals(key));
        }

        private int KeyToIndex(TKey key) // return valid index of array
        {
            int calcRes = key.GetHashCode(); // !!!
            // normalize calcRes to be a valid index for current array size
            return Math.Abs(calcRes % hashArray.Length);
        }

        class Data
        {
            public TKey key;
            public TValue value;

            public Data(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
