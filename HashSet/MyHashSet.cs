using System;
using System.Collections;
using System.Collections.Generic;

namespace HashSet {
    class MyHashSet<T> : ICollection<T> {
        public class HashNode<TInternalData> {
            public HashNode<TInternalData> Next;
            public int HashValue {
                get {
                    return Math.Abs(Data.GetHashCode());
                }
            }
            public TInternalData Data;
            public HashNode(TInternalData Data) {
                this.Data = Data;
            }
        }
        public HashNode<T>[] Source;
        private float LoadFactor;
        private void ResizeIfBig() {
            if (Count > LoadFactor * Source.Length) {
                HashNode<T>[] newarray = new HashNode<T>[Source.Length * 2];
                int cursorindex = 0;
                HashNode<T> cursor = Source[0];
                while (true) {
                    if (cursor == null) {
                        cursorindex++;
                        if (cursorindex >= Source.Length) break;
                        cursor = Source[cursorindex];
                    } else {
                        newarray[cursor.HashValue % newarray.Length] = cursor;
                        if (cursor.Next == null) {
                            cursorindex++;
                            if (cursorindex >= Source.Length) break;
                            cursor = Source[cursorindex];
                        } else cursor = cursor.Next;
                    }
                }
                Source = newarray;
            }
        }
        public int Count { get; private set; } = 0;
        public bool IsReadOnly { get; } = false;
        public MyHashSet() : this(4, 0.8f) { }
        public MyHashSet(int Capacity, float LoadFactor) {
            Source = new HashNode<T>[Capacity];
            this.LoadFactor = LoadFactor;
        }
        public bool Add(T item) {
            int hashindex = Math.Abs(item.GetHashCode() % Source.Length);
            if (Source[hashindex] == null) {
                Source[hashindex] = new HashNode<T>(item);
            } else {
                HashNode<T> cursor = Source[hashindex];
                while (true) {
                    if (cursor.Data.Equals(item)) return false;
                    if (cursor.Next == null) break;
                    else cursor = cursor.Next;
                }
                cursor.Next = new HashNode<T>(item);
            }
            Count++;
            ResizeIfBig();
            return true;
        }
        public void Clear() {
            for (int i = 0; i < Source.Length; i++) Source[i] = null;
        }
        public bool Contains(T item) {
            int hashindex = Math.Abs(item.GetHashCode() % Source.Length);
            if (Source[hashindex] == null) return false;
            else {
                HashNode<T> cursor = Source[hashindex];
                while (true) {
                    if (cursor.Data.Equals(item)) return true;
                    if (cursor.Next == null) return false;
                    else cursor = cursor.Next;
                }
            }
        }
        public void CopyTo(T[] array, int arrayIndex) {
            IEnumerator<T> etor = GetEnumerator();
            for (int i = arrayIndex; i < array.Length; i++) {
                if (!etor.MoveNext()) break;
                array[i] = etor.Current;
            }
            etor.Dispose();
        }
        public IEnumerator<T> GetEnumerator() {
            int cursorindex = 0;
            HashNode<T> cursor = Source[0];
            while (true) {
                if (cursor == null) {
                    cursorindex++;
                    if (cursorindex >= Source.Length) yield break;
                    else cursor = Source[cursorindex];
                } else {
                    yield return cursor.Data;
                    if (cursor.Next == null) {
                        cursorindex++;
                        if (cursorindex >= Source.Length) yield break;
                        else cursor = Source[cursorindex];
                    } else cursor = cursor.Next;
                }
            }
        }
        public bool Remove(T item) {
            int hashindex = Math.Abs(item.GetHashCode() % Source.Length);
            if (Source[hashindex] == null) return false;
            else {
                HashNode<T> parent = null;
                HashNode<T> cursor = Source[hashindex];
                while (true) {
                    if (cursor.Data.Equals(item)) {
                        if (parent == null) Source[hashindex] = cursor.Next;
                        else parent.Next = cursor.Next;
                        Count--;
                        return true;
                    }
                    if (cursor.Next == null) {
                        return false;
                    } else {
                        parent = cursor;
                        cursor = cursor.Next;
                    }
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var i in this) yield return i;
        }
        public bool IsEmpty() {
            return Count == 0;
        }
        void ICollection<T>.Add(T item) {
            Add(item);
        }
    }
}
