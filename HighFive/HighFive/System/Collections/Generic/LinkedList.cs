namespace System.Collections.Generic
{
    using System;
    using System.Diagnostics;

    public class LinkedList<T> : ICollection<T>, System.Collections.ICollection, IReadOnlyCollection<T>
    {
        // This LinkedList is a doubly-Linked circular list.
        internal LinkedListNode<T> head;
        internal int count;
        internal int version;

        // names for serialization
        const String VersionName = "Version";
        const String CountName = "Count";
        const String ValuesName = "Data";

        public LinkedList()
        {
        }

        public LinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        public int Count
        {
            get { return count; }
        }

        public LinkedListNode<T> First
        {
            get { return head; }
        }

        public LinkedListNode<T> Last
        {
            get { return head == null ? null : head.prev; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            LinkedListNode<T> result = new LinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node.next, result);
            return result;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node.next, newNode);
            newNode.list = this;
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            LinkedListNode<T> result = new LinkedListNode<T>(node.list, value);
            InternalInsertNodeBefore(node, result);
            if (node == head)
            {
                head = result;
            }
            return result;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            InternalInsertNodeBefore(node, newNode);
            newNode.list = this;
            if (node == head)
            {
                head = newNode;
            }
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> result = new LinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
                head = result;
            }
            return result;
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            ValidateNewNode(node);

            if (head == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
                head = node;
            }
            node.list = this;
        }

        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> result = new LinkedListNode<T>(this, value);
            if (head == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(head, result);
            }
            return result;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            ValidateNewNode(node);

            if (head == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(head, node);
            }
            node.list = this;
        }

        public void Clear()
        {
            LinkedListNode<T> current = head;
            while (current != null)
            {
                LinkedListNode<T> temp = current;
                current = current.Next;   // use Next the instead of "next", otherwise it will loop forever
                temp.Invalidate();
            }

            head = null;
            count = 0;
            version++;
        }

        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException();
            }

            LinkedListNode<T> node = head;
            if (node != null)
            {
                do
                {
                    array[index++] = node.item;
                    node = node.next;
                } while (node != head);
            }
        }

        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> node = head;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (c.Equals(node.item, value))
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
                else
                {
                    do
                    {
                        if (node.item == null)
                        {
                            return node;
                        }
                        node = node.next;
                    } while (node != head);
                }
            }
            return null;
        }

        public LinkedListNode<T> FindLast(T value)
        {
            if (head == null) return null;

            LinkedListNode<T> last = head.prev;
            LinkedListNode<T> node = last;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                if (value != null)
                {
                    do
                    {
                        if (c.Equals(node.item, value))
                        {
                            return node;
                        }

                        node = node.prev;
                    } while (node != last);
                }
                else
                {
                    do
                    {
                        if (node.item == null)
                        {
                            return node;
                        }
                        node = node.prev;
                    } while (node != last);
                }
            }
            return null;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T value)
        {
            LinkedListNode<T> node = Find(value);
            if (node != null)
            {
                InternalRemoveNode(node);
                return true;
            }
            return false;
        }

        public void Remove(LinkedListNode<T> node)
        {
            ValidateNode(node);
            InternalRemoveNode(node);
        }

        public void RemoveFirst()
        {
            if (head == null) { throw new InvalidOperationException(); }
            InternalRemoveNode(head);
        }

        public void RemoveLast()
        {
            if (head == null) { throw new InvalidOperationException(); }
            InternalRemoveNode(head.prev);
        }

        private void InternalInsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            newNode.next = node;
            newNode.prev = node.prev;
            node.prev.next = newNode;
            node.prev = newNode;
            version++;
            count++;
        }

        private void InternalInsertNodeToEmptyList(LinkedListNode<T> newNode)
        {
            Debug.Assert(head == null && count == 0, "LinkedList must be empty when this method is called!");
            newNode.next = newNode;
            newNode.prev = newNode;
            head = newNode;
            version++;
            count++;
        }

        internal void InternalRemoveNode(LinkedListNode<T> node)
        {
            Debug.Assert(node.list == this, "Deleting the node from another list!");
            Debug.Assert(head != null, "This method shouldn't be called on empty list!");
            if (node.next == node)
            {
                Debug.Assert(count == 1 && head == node, "this should only be true for a list with only one node");
                head = null;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
                if (head == node)
                {
                    head = node.next;
                }
            }
            node.Invalidate();
            count--;
            version++;
        }

        internal void ValidateNewNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != null)
            {
                throw new InvalidOperationException();
            }
        }


        internal void ValidateNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.list != this)
            {
                throw new InvalidOperationException();
            }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException();
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException();
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException();
            }

            T[] tArray = array as T[];
            if (tArray != null)
            {
                CopyTo(tArray, index);
            }
            else
            {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                {
                    throw new ArgumentException();
                }

                object[] objects = array as object[];
                if (objects == null)
                {
                    throw new ArgumentException();
                }
                LinkedListNode<T> node = head;
                try
                {
                    if (node != null)
                    {
                        do
                        {
                            objects[index++] = node.item;
                            node = node.next;
                        } while (node != head);
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException();
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private LinkedList<T> list;
            private LinkedListNode<T> node;
            private int version;
            private T current;
            private int index;

            const string LinkedListName = "LinkedList";
            const string CurrentValueName = "Current";
            const string VersionName = "Version";
            const string IndexName = "Index";

            internal Enumerator(LinkedList<T> list)
            {
                this.list = list;
                version = list.version;
                node = list.head;
                current = default(T);
                index = 0;
            }

            public T Current
            {
                get { return current; }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || (index == list.Count + 1))
                    {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
                    }

                    return current;
                }
            }

            public bool MoveNext()
            {
                if (version != list.version)
                {
                    throw new InvalidOperationException();
                }

                if (node == null)
                {
                    index = list.Count + 1;
                    return false;
                }

                ++index;
                current = node.item;
                node = node.next;
                if (node == list.head)
                {
                    node = null;
                }
                return true;
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != list.version)
                {
                    throw new InvalidOperationException();
                }

                current = default(T);
                node = list.head;
                index = 0;
            }

            public void Dispose()
            {
            }
        }

    }

    public sealed class LinkedListNode<T>
    {
        internal LinkedList<T> list;
        internal LinkedListNode<T> next;
        internal LinkedListNode<T> prev;
        internal T item;

        public LinkedListNode(T value)
        {
            this.item = value;
        }

        internal LinkedListNode(LinkedList<T> list, T value)
        {
            this.list = list;
            this.item = value;
        }

        public LinkedList<T> List
        {
            get { return list; }
        }

        public LinkedListNode<T> Next
        {
            get { return next == null || next == list.head ? null : next; }
        }

        public LinkedListNode<T> Previous
        {
            get { return prev == null || this == list.head ? null : prev; }
        }

        public T Value
        {
            get { return item; }
            set { item = value; }
        }

        internal void Invalidate()
        {
            list = null;
            next = null;
            prev = null;
        }
    }
}