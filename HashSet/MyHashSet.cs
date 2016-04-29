using System;
using System.Collections;
using System.Collections.Generic;

namespace HashSet {
    class MyHashSet<T> : ICollection<T> {
        #region Private 멤버
        private delegate bool ResizeFilter(int Count, int FactoredLength);
        private class HashNode<TInternalData> {
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
        private HashNode<T>[] Source = new HashNode<T>[4];
        private float LoadFactor = 0.8f;
        private void ResizeCapacity(ResizeFilter Filter, int NewSize) {
            if (Filter(Count, (int)(LoadFactor * Source.Length))) {
                HashNode<T>[] oldsource = Source;
                Source = new HashNode<T>[NewSize];
                Count = 0;
                int cursorindex = 0;
                HashNode<T> cursor = oldsource[0];
                while (true) {
                    if (cursor == null) {
                        cursorindex++;
                        if (cursorindex >= oldsource.Length) break;
                        else cursor = oldsource[cursorindex];
                    } else {
                        AddNode(cursor);
                        HashNode<T> temp = cursor.Next;
                        cursor.Next = null;
                        cursor = temp;
                    }
                }
            }
        }
        private bool AddNode(HashNode<T> Node) {
            int hashindex = Node.HashValue % Source.Length;
            if (Source[hashindex] == null) {
                Source[hashindex] = Node;
            } else {
                HashNode<T> cursor = Source[hashindex];
                while (true) {
                    if (cursor.Data.Equals(Node.Data)) return false;
                    if (cursor.Next == null) break;
                    else cursor = cursor.Next;
                }
                cursor.Next = Node;
            }
            Count++;
            return true;
        }
        #endregion

        #region ICollection 구현
        public int Count { get; private set; } = 0;
        public bool IsReadOnly { get; } = false;
        public bool Add(T item) {
            bool res = AddNode(new HashNode<T>(item));
            ResizeCapacity((c, f) => c > f, Source.Length * 2);
            return res;
        }
        public void Clear() {
            for (int i = 0; i < Source.Length; i++) Source[i] = null;
            Count = 0;
            ResizeCapacity((c, f) => c < f / 4, 4);
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
            using (IEnumerator<T> etor = GetEnumerator()) {
                for (int i = arrayIndex; i < array.Length; i++) {
                    if (!etor.MoveNext()) break;
                    array[i] = etor.Current;
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
                        ResizeCapacity((c, f) => c < f / 4, Source.Length / 2);
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
        void ICollection<T>.Add(T item) {
            Add(item);
        }
        #endregion

        #region 추가 구현
        public int HashTableSize {
            get {
                return Source.Length;
            }
        }
        public IEnumerable<T> AsEnumerableByHashValue(int value) {
            if (value >= Source.Length) yield break;
            HashNode<T> cursor = Source[value];
            while (true) {
                if (cursor == null) {
                    yield break;
                } else {
                    yield return cursor.Data;
                    cursor = cursor.Next;
                }
            }
        }
        public bool IsEmpty() {
            return Count == 0;
        }
        #endregion

        #region IEnumerable 구현
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
                    cursor = cursor.Next;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var i in this) yield return i;
        }
        #endregion
    }
}
