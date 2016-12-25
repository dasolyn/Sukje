using System;
using System.Collections;
using System.Collections.Generic;

namespace Dasolyn {
    /// <summary>
    /// 키를 레드 블랙 트리에 넣어 정렬하고 탐색하는 키-값 딕셔너리입니다.
    /// </summary>
    /// <typeparam name="TKey">이 트리에서 사용할 키의 형식입니다. <see cref="IComparable{T}"/> 또는 <see cref="IComparable"/>을 구현하거나, 생성시에 적절한 비교자를 지정해야 합니다..</typeparam>
    /// <typeparam name="TValue">이 트리에서 사용할 값의 형식입니다.</typeparam>
    public class RedBlackTree<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary {
        private Node Root;
        /// <summary>
        /// 이 트리에서 사용하는 키 비교자를 가져옵니다.
        /// </summary>
        public IComparer<TKey> KeyComparer { get; }

        #region 생성자
        /// <summary>
        /// 기본 키 비교자를 사용하는 빈 레드 블랙 트리를 생성합니다.
        /// </summary>
        public RedBlackTree() {
            KeyComparer = Comparer<TKey>.Default;
        }
        /// <summary>
        /// 지정된 키 비교자를 사용하는 빈 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="KeyComparer">이 트리에서 사용할 키 비교자입니다.</param>
        public RedBlackTree(IComparer<TKey> KeyComparer) {
            this.KeyComparer = KeyComparer;
        }
        /// <summary>
        /// 지정된 키 비교를 사용하는 빈 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="KeyComparison">이 트리에서 사용할 키 비교입니다.</param>
        public RedBlackTree(Comparison<TKey> KeyComparison) {
            KeyComparer = Comparer<TKey>.Create(KeyComparison);
        }
        /// <summary>
        /// 기본 키 비교자를 사용하고 지정한 딕셔너리의 내용으로 채워진 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="Source">이 트리의 내용을 채우는 데 사용할 딕셔너리입니다.</param>
        public RedBlackTree(IEnumerable<KeyValuePair<TKey, TValue>> Source) : this() {
            foreach (KeyValuePair<TKey, TValue> i in Source) RedBlackAddNode(i.Key, i.Value);
        }
        /// <summary>
        /// 지정된 키 비교자를 사용하고 지정한 딕셔너리의 내용으로 채워진 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="Source">이 트리의 내용을 채우는 데 사용할 딕셔너리입니다.</param>
        /// <param name="KeyComparer">이 트리에서 사용할 키 비교자입니다.</param>
        public RedBlackTree(IEnumerable<KeyValuePair<TKey, TValue>> Source, IComparer<TKey> KeyComparer) : this(KeyComparer) {
            foreach (KeyValuePair<TKey, TValue> i in Source) RedBlackAddNode(i.Key, i.Value);
        }
        /// <summary>
        /// 지정된 키 비교를 사용하고 지정한 딕셔너리의 내용으로 채워진 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="Source">이 트리의 내용을 채우는 데 사용할 딕셔너리입니다.</param>
        /// <param name="KeyComparison">이 트리에서 사용할 키 비교입니다.</param>
        public RedBlackTree(IEnumerable<KeyValuePair<TKey, TValue>> Source, Comparison<TKey> KeyComparison) : this(KeyComparison) {
            foreach (KeyValuePair<TKey, TValue> i in Source) RedBlackAddNode(i.Key, i.Value);
        }
        /// <summary>
        /// 기본 키 비교자를 사용하고 지정한 키 컬렉션과 값 컬렉션의 내용으로 구성한 키-값 쌍으로 채워진 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="Keys">이 트리의 내용을 채우는데 사용할 키 컬렉션입니다.</param>
        /// <param name="Values">이 트리의 내용을 채우는데 사용할 값 컬렉션입니다.</param>
        /// <remarks>키 컬렉션과 값 컬렉션의 길이가 다를 경우, 보다 짧은 쪽에 맞춰지고 나머지 내용은 버립니다.</remarks>
        public RedBlackTree(IEnumerable<TKey> Keys, IEnumerable<TValue> Values) : this() {
            using (IEnumerator<TKey> KeyEtor = Keys.GetEnumerator())
            using (IEnumerator<TValue> ValueEtor = Values.GetEnumerator()) {
                while (KeyEtor.MoveNext() && ValueEtor.MoveNext()) {
                    RedBlackAddNode(KeyEtor.Current, ValueEtor.Current);
                }
            }
        }
        /// <summary>
        /// 지정된 키 비교자를 사용하고 지정한 키 컬렉션과 값 컬렉션의 내용으로 구성한 키-값 쌍으로 채워진 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="Keys">이 트리의 내용을 채우는데 사용할 키 컬렉션입니다.</param>
        /// <param name="Values">이 트리의 내용을 채우는데 사용할 값 컬렉션입니다.</param>
        /// <param name="KeyComparer">이 트리에서 사용할 키 비교자입니다.</param>
        /// <remarks>키 컬렉션과 값 컬렉션의 길이가 다를 경우, 보다 짧은 쪽에 맞춰지고 나머지 내용은 버립니다.</remarks>
        public RedBlackTree(IEnumerable<TKey> Keys, IEnumerable<TValue> Values, IComparer<TKey> KeyComparer) : this(KeyComparer) {
            using (IEnumerator<TKey> KeyEtor = Keys.GetEnumerator())
            using (IEnumerator<TValue> ValueEtor = Values.GetEnumerator()) {
                while (KeyEtor.MoveNext() && ValueEtor.MoveNext()) {
                    RedBlackAddNode(KeyEtor.Current, ValueEtor.Current);
                }
            }
        }
        /// <summary>
        /// 지정된 키 비교를 사용하고 지정한 키 컬렉션과 값 컬렉션의 내용으로 구성한 키-값 쌍으로 채워진 레드 블랙 트리를 생성합니다.
        /// </summary>
        /// <param name="Keys">이 트리의 내용을 채우는데 사용할 키 컬렉션입니다.</param>
        /// <param name="Values">이 트리의 내용을 채우는데 사용할 값 컬렉션입니다.</param>
        /// <param name="KeyComparison">이 트리에서 사용할 키 비교입니다.</param>
        /// <remarks>키 컬렉션과 값 컬렉션의 길이가 다를 경우, 보다 짧은 쪽에 맞춰지고 나머지 내용은 버립니다.</remarks>
        public RedBlackTree(IEnumerable<TKey> Keys, IEnumerable<TValue> Values, Comparison<TKey> KeyComparison) : this(KeyComparison) {
            using (IEnumerator<TKey> KeyEtor = Keys.GetEnumerator())
            using (IEnumerator<TValue> ValueEtor = Values.GetEnumerator()) {
                while (KeyEtor.MoveNext() && ValueEtor.MoveNext()) {
                    RedBlackAddNode(KeyEtor.Current, ValueEtor.Current);
                }
            }
        }
        #endregion

        #region 인덱스로 접근
        /// <summary>
        /// 지정된 키에 연관된 값을 가져오거나 설정합니다.
        /// </summary>
        /// <param name="Key">가져오거나 설정할 값의 키입니다.</param>
        /// <returns>지정된 키와 연관된 값입니다.</returns>
        /// <exception cref="ArgumentNullException">키가 null인 경우.</exception>
        /// <exception cref="KeyNotFoundException">값을 가져올 때 트리 내에 해당 키가 없는 경우.</exception>
        /// <remarks>지정된 키가 트리 내에 없으면 설정 작업에서는 새 키-값 쌍을 만들며, 가져오기 작업에서는 예외를 던집니다.</remarks>
        public TValue this[TKey Key] {
            get {
                if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
                Node found = RedBlackSearchNode(Key);
                if (found == null) throw new KeyNotFoundException("RedBlackTree`2: Key was not found in this dictionary");
                return found.Value;
            }
            set {
                if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
                Node found = RedBlackSearchNode(Key);
                if (found == null) RedBlackAddNode(Key, value);
                else found.Value = value;
            }
        }
        #endregion

        #region 추가
        /// <summary>
        /// 지정한 키-값 쌍을 트리에 추가합니다.
        /// </summary>
        /// <param name="Key">추가할 쌍의 키입니다.</param>
        /// <param name="Value">추가할 쌍의 값입니다. 값의 타입이 참조 형식 또는 <see cref="Nullable{T}"/> 인 경우 null이 될 수도 있습니다.</param>
        /// <exception cref="ArgumentNullException">키가 null인 경우.</exception>
        /// <exception cref="ArgumentException">같은 키를 가진 요소가 이미 트리 내에 존재하는 경우.</exception>
        public void Add(TKey Key, TValue Value) {
            if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
            Node found = RedBlackSearchNode(Key);
            if (found != null) throw new ArgumentException("RedBlackTree`2: Given key already exists in this dictionary", "Key");
            RedBlackAddNode(Key, Value);
        }
        #endregion

        #region 검색
        /// <summary>
        /// 트리에 지정한 키를 가진 쌍이 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="Key">검색할 키입니다.</param>
        /// <returns>트리에 지정한 키를 가진 쌍이 포함되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException">키가 null인 경우.</exception>
        public bool ContainsKey(TKey Key) {
            if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
            Node found = RedBlackSearchNode(Key);
            return found != null;
        }
        /// <summary>
        /// 지정한 키와 연관된 값을 가져옵니다.
        /// </summary>
        /// <param name="Key">검색할 키입니다.</param>
        /// <param name="Value">지정된 키를 가진 쌍이 있으면 해당 키와 연결된 값을 포함하며, 그렇지 않으면 값 타입의 기본값을 포함합니다.</param>
        /// <returns>트리에 지정한 키를 가진 쌍이 포함되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException">키가 null인 경우.</exception>
        public bool TryGetValue(TKey Key, out TValue Value) {
            if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
            Node found = RedBlackSearchNode(Key);
            if (found == null) {
                Value = default(TValue);
                return false;
            } else {
                Value = found.Value;
                return true;
            }
        }
        /// <summary>
        /// 트리에 지정한 값을 가진 쌍이 포함되어 있는지 확인합니다.
        /// </summary>
        /// <param name="Value">검색할 값입니다.</param>
        /// <returns>트리에 지정한 값을 가진 쌍이 포함되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool ContainsValue(TValue Value) {
            if (Value == null) {
                if (Root != null) {
                    Queue<Node> queue = new Queue<Node>();
                    queue.Enqueue(Root);
                    while (queue.Count > 0) {
                        Node e = queue.Dequeue();
                        if (e.LeftChild != null) queue.Enqueue(e.LeftChild);
                        if (e.RightChild != null) queue.Enqueue(e.RightChild);
                        if (e.Value == null) {
                            return true;
                        }
                    }
                }
            } else {
                if (Root != null) {
                    Queue<Node> queue = new Queue<Node>();
                    queue.Enqueue(Root);
                    while (queue.Count > 0) {
                        Node e = queue.Dequeue();
                        if (e.LeftChild != null) queue.Enqueue(e.LeftChild);
                        if (e.RightChild != null) queue.Enqueue(e.RightChild);
                        if (e.Value.Equals(Value)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 지정한 값과 연관된 키를 가져옵니다.
        /// </summary>
        /// <param name="Value">검색할 값입니다.</param>
        /// <param name="Key">지정된 값을 가진 쌍이 있으면 해당 값과 연결된 첫번째 키를 포함하며, 그렇지 않으면 키 타입의 기본값을 포함합니다.</param>
        /// <returns>트리에 지정한 값을 가진 쌍이 포함되어 있으면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryGetKey(TValue Value, out TKey Key) {
            if (Value == null) {
                if (Root != null) {
                    Queue<Node> queue = new Queue<Node>();
                    queue.Enqueue(Root);
                    while (queue.Count > 0) {
                        Node e = queue.Dequeue();
                        if (e.LeftChild != null) queue.Enqueue(e.LeftChild);
                        if (e.RightChild != null) queue.Enqueue(e.RightChild);
                        if (e.Value == null) {
                            Key = e.Key;
                            return true;
                        }
                    }
                }
            } else {
                if (Root != null) {
                    Queue<Node> queue = new Queue<Node>();
                    queue.Enqueue(Root);
                    while (queue.Count > 0) {
                        Node e = queue.Dequeue();
                        if (e.LeftChild != null) queue.Enqueue(e.LeftChild);
                        if (e.RightChild != null) queue.Enqueue(e.RightChild);
                        if (e.Value.Equals(Value)) {
                            Key = e.Key;
                            return true;
                        }
                    }
                }
            }
            Key = default(TKey);
            return false;
        }
        #endregion

        #region 제거
        /// <summary>
        /// 트리에서 모든 키와 값을 제거합니다.
        /// </summary>
        public void Clear() {
            Root = null;
            Count = 0;
        }
        /// <summary>
        /// 트리에서 지정한 키와 연관된 값 쌍을 제거합니다.
        /// </summary>
        /// <param name="Key">제거할 쌍의 키입니다.</param>
        /// <returns>성공적으로 쌍 하나를 제거하였으면 true, 그렇지 않으면 false입니다.</returns>
        /// <exception cref="ArgumentNullException">키가 null인 경우.</exception>
        public bool Remove(TKey Key) {
            if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
            Node found = RedBlackSearchNode(Key);
            if (found == null) return false;
            RedBlackDeleteNode(found);
            return true;
        }
        #endregion

        #region 기타
        /// <summary>
        /// 트리에 포함된 쌍의 갯수를 가져옵니다.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 트리에 포함된 모든 키를 순서대로 열거합니다.
        /// </summary>
        public IEnumerable<TKey> Keys {
            get {
                foreach (Node i in RedBlackEnumNode()) yield return i.Key;
            }
        }
        /// <summary>
        /// 트리에 포함된 모든 값을 순서대로 열거합니다.
        /// </summary>
        public IEnumerable<TValue> Values {
            get {
                foreach (Node i in RedBlackEnumNode()) yield return i.Value;
            }
        }
        /// <summary>
        /// 트리에 포함된 모든 키-값 쌍을 순서대로 열거하는 열거자를 가져옵니다.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (Node i in RedBlackEnumNode()) yield return new KeyValuePair<TKey, TValue>(i.Key, i.Value);
        }
        #endregion

        #region 명시적 인터페이스 구현
        private object InternalSyncRoot = new object();
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        object ICollection.SyncRoot => InternalSyncRoot;
        bool ICollection.IsSynchronized => false;
        void ICollection.CopyTo(Array Array, int ArrayIndex) {
            int cursor = 0;
            foreach (Node i in RedBlackEnumNode()) {
                if (cursor + ArrayIndex >= Array.Length || cursor + ArrayIndex < 0) return;
                else {
                    Array.SetValue(new KeyValuePair<TKey, TValue>(i.Key, i.Value), ArrayIndex + cursor);
                    cursor++;
                }
            }
        }
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> Item) {
            Add(Item.Key, Item.Value);
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> Item) {
            TValue result;
            if (TryGetValue(Item.Key, out result)) return result.Equals(Item.Value);
            else return false;
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> Item) {
            TValue result;
            if (TryGetValue(Item.Key, out result) && result.Equals(Item.Value)) return Remove(Item.Key);
            else return false;
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] Array, int ArrayIndex) {
            int cursor = 0;
            foreach (Node i in RedBlackEnumNode()) {
                if (cursor + ArrayIndex >= Array.Length || cursor + ArrayIndex < 0) return;
                else {
                    Array[ArrayIndex + cursor] = new KeyValuePair<TKey, TValue>(i.Key, i.Value);
                    cursor++;
                }
            }
        }
        ICollection IDictionary.Keys => new List<TKey>(Keys);
        ICollection IDictionary.Values => new List<TValue>(Values);
        bool IDictionary.IsFixedSize => false;
        object IDictionary.this[object Key] {
            get {
                if (Key is TKey && ContainsKey((TKey)Key)) return this[(TKey)Key];
                else return null;
            }
            set {
                if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null");
                if (Key is TKey == false) throw new ArgumentException("RedBlackTree`2: Type of key is not acceptable by this dictionary", "Key");
                if (value is TValue == false) throw new ArgumentException("RedBlackTree`2: Type of value is not acceptable by this dictionary", "Value");
                this[(TKey)Key] = (TValue)value;
            }
        }
        bool IDictionary.Contains(object Key) {
            if (Key is TKey) return ContainsKey((TKey)Key);
            else return false;
        }
        void IDictionary.Add(object Key, object Value) {
            if (Key == null) throw new ArgumentNullException("Key", "RedBlackTree`2: Key should not be null.");
            if (Key is TKey == false) throw new ArgumentException("RedBlackTree`2: Type of key is not acceptable by this dictionary", "Key");
            if (Value is TValue == false) throw new ArgumentException("RedBlackTree`2: Type of value is not acceptable by this dictionary", "Value");
            Add((TKey)Key, (TValue)Value);
        }
        IDictionaryEnumerator IDictionary.GetEnumerator() {
            return new DictionaryEnumerator(GetEnumerator());
        }
        void IDictionary.Remove(object Key) {
            if (Key == null) throw new ArgumentNullException("RedBlackTree`2");
            if (Key is TKey == false) return;
            Remove((TKey)Key);
        }
        bool IDictionary.IsReadOnly => false;
        private class DictionaryEnumerator : IDictionaryEnumerator {
            private IEnumerator<KeyValuePair<TKey, TValue>> InnerEnumerator;
            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> InnerEnumerator) {
                this.InnerEnumerator = InnerEnumerator;
            }
            public object Current => new DictionaryEntry(InnerEnumerator.Current.Key, InnerEnumerator.Current.Value);
            public DictionaryEntry Entry => new DictionaryEntry(InnerEnumerator.Current.Key, InnerEnumerator.Current.Value);
            public object Key => InnerEnumerator.Current.Key;
            public object Value => InnerEnumerator.Current.Value;
            public bool MoveNext() => InnerEnumerator.MoveNext();
            public void Reset() => InnerEnumerator.Reset();
        }
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => new List<TKey>(Keys);
        ICollection<TValue> IDictionary<TKey, TValue>.Values => new List<TValue>(Values);
        #endregion

        #region 내부 로직
        private Node RedBlackSearchNode(TKey key) {
            Node temp = Root;
            while (temp != null && KeyComparer.Compare(temp.Key, key) != 0) {
                if (KeyComparer.Compare(key, temp.Key) < 0) temp = temp.LeftChild;
                else temp = temp.RightChild;
            }
            return temp;
        }
        private void RedBlackAddNode(TKey key, TValue value) {
            Count++;
            Node insert = new Node(key, value);
            // 삽입
            if (Root == null) {
                Root = insert;
                Root.Color = Color.Black;
                return;
            }
            Node cursor = Root;
            while (true) {
                if (KeyComparer.Compare(insert.Key, cursor.Key) < 0) {
                    if (cursor.LeftChild == null) {
                        cursor.LeftChild = insert;
                        break;
                    } else {
                        cursor = cursor.LeftChild;
                    }
                } else {
                    if (cursor.RightChild == null) {
                        cursor.RightChild = insert;
                        break;
                    } else {
                        cursor = cursor.RightChild;
                    }
                }
            }
            // 정렬
            cursor = insert;
            while (true) {
                Node parent = cursor.Parent;
                if (GetColor(parent) == Color.Black) break;
                Node grandparent = parent.Parent;
                Node uncle;
                if (parent == grandparent.LeftChild) {
                    uncle = grandparent.RightChild;
                    if (GetColor(uncle) == Color.Black) {
                        if (cursor == parent.RightChild) {
                            // Case 2
                            LeftRotate(parent);
                            parent = cursor;
                            grandparent = parent.Parent;
                        }
                        // Case 3
                        parent.Color = Color.Black;
                        grandparent.Color = Color.Red;
                        RightRotate(grandparent);
                    } else {
                        // Case 1
                        uncle.Color = Color.Black;
                        parent.Color = Color.Black;
                        grandparent.Color = Color.Red;
                        cursor = grandparent;
                    }
                } else {
                    uncle = grandparent.LeftChild;
                    if (GetColor(uncle) == Color.Black) {
                        if (cursor == parent.LeftChild) {
                            RightRotate(parent);
                            parent = cursor;
                            grandparent = parent.Parent;
                        }
                        parent.Color = Color.Black;
                        grandparent.Color = Color.Red;
                        LeftRotate(grandparent);
                    } else {
                        uncle.Color = Color.Black;
                        parent.Color = Color.Black;
                        grandparent.Color = Color.Red;
                        cursor = grandparent;
                    }
                }
            }
            Root.Color = Color.Black;
        }
        private void RedBlackDeleteNode(Node delete) {
            Count--;
            // x와 y 구하기
            if (delete.LeftChild != null && delete.RightChild != null) {
                Node newdelete = GetSuccessor(delete);
                delete.Value = newdelete.Value;
                delete = newdelete;
            }
            Node cursor;
            if (delete.Parent.LeftChild == delete) cursor = delete.LeftChild;
            else cursor = delete.RightChild;
            Node py = delete.Parent;
            // 삭제
            if (delete.LeftChild == null && delete.RightChild == null) {
                // 자식 0개
                if (delete.Parent == null) Clear();
                else delete.Parent = null;
            } else {
                // 자식 1개
                if (delete.Parent == null) {
                    if (delete.LeftChild != null) Root = delete.LeftChild;
                    else Root = delete.RightChild;
                    Root.Parent = null;
                } else {
                    if (delete.LeftChild != null) delete.Parent.LeftChild = delete.LeftChild;
                    else delete.Parent.RightChild = delete.RightChild;
                }
            }
            if (GetColor(delete) == Color.Red) return;
            // 정렬
            while (cursor != Root && GetColor(cursor) == Color.Black) {
                Node parent;
                if (cursor == null) parent = py;
                else parent = cursor.Parent;
                if (cursor == parent.LeftChild) {
                    Node sibling = parent.RightChild;
                    if (GetColor(sibling) == Color.Red) {
                        // Case 1
                        sibling.Color = Color.Black;
                        parent.Color = Color.Red;
                        LeftRotate(parent);
                        sibling = parent.RightChild;
                    }
                    if (GetColor(sibling.LeftChild) == Color.Black && GetColor(sibling.RightChild) == Color.Black) {
                        // Case 2
                        sibling.Color = Color.Red;
                        cursor = parent;
                    } else {
                        if (GetColor(sibling.RightChild) == Color.Black) {
                            // Case 3
                            sibling.LeftChild.Color = Color.Black;
                            sibling.Color = Color.Red;
                            RightRotate(sibling);
                            sibling = parent.RightChild;
                        }
                        // Case 4
                        sibling.Color = GetColor(parent);
                        parent.Color = Color.Black;
                        sibling.RightChild.Color = Color.Black;
                        LeftRotate(parent);
                        cursor = Root;
                    }
                } else {
                    Node sibling = parent.LeftChild;
                    if (GetColor(sibling) == Color.Red) {
                        sibling.Color = Color.Black;
                        parent.Color = Color.Red;
                        RightRotate(parent);
                        sibling = parent.LeftChild;
                    }
                    if (GetColor(sibling.LeftChild) == Color.Black && GetColor(sibling.RightChild) == Color.Black) {
                        sibling.Color = Color.Red;
                        cursor = parent;
                    } else {
                        if (GetColor(sibling.LeftChild) == Color.Black) {
                            sibling.RightChild.Color = Color.Black;
                            sibling.Color = Color.Red;
                            LeftRotate(sibling);
                            sibling = parent.LeftChild;
                        }
                        sibling.Color = GetColor(parent);
                        parent.Color = Color.Black;
                        sibling.LeftChild.Color = Color.Black;
                        RightRotate(parent);
                        cursor = Root;
                    }
                }
            }
            cursor.Color = Color.Black;
        }
        private IEnumerable<Node> RedBlackEnumNode() {
            if (Root == null) yield break;
            else {
                Node cursor = Root;
                Stack<Node> stack = new Stack<Node>();
                while (cursor != null) {
                    stack.Push(cursor);
                    cursor = cursor.LeftChild;
                }
                while (stack.Count > 0) {
                    cursor = stack.Pop();
                    yield return cursor;
                    if (cursor.RightChild != null) {
                        cursor = cursor.RightChild;
                        while (cursor != null) {
                            stack.Push(cursor);
                            cursor = cursor.LeftChild;
                        }
                    }
                }
            }
        }
        private Color GetColor(Node node) {
            if (node == null) return Color.Black;
            return node.Color;
        }
        private void LeftRotate(Node node) {
            Node y = node.RightChild;
            node.RightChild = y.LeftChild;
            if (node.Parent == null) Root = y;
            else if (node.Parent.LeftChild == node) node.Parent.LeftChild = y;
            else node.Parent.RightChild = y;
            y.LeftChild = node;
        }
        private void RightRotate(Node node) {
            Node x = node.LeftChild;
            node.LeftChild = x.RightChild;
            if (node.Parent == null) Root = x;
            else if (node.Parent.LeftChild == node) node.Parent.LeftChild = x;
            else node.Parent.RightChild = x;
            x.RightChild = node;
        }
        private Node GetSuccessor(Node node) {
            if (node.RightChild != null) return GetMinimum(node.RightChild);
            Node origin = node;
            Node temp = node;
            while (true) {
                if (temp.Parent == null) return origin;
                else if (temp.Parent.LeftChild != temp) temp = temp.Parent;
                else return temp;
            }
        }
        private Node GetMinimum(Node parent) {
            Node temp = parent;
            while (temp.LeftChild != null) temp = temp.LeftChild;
            return temp;
        }
        private enum Color { Black, Red };
        private class Node {
            public Color Color;
            private Node _Parent = null;
            /// <summary>
            /// 이 노드의 부모 노드의 값을 가져올 수 있습니다. 이 프로퍼티에 null을 할당하여 이 노드와 부모의 연결을 해제할 수 있습니다.
            /// </summary>
            /// <exception cref="ArgumentException">
            /// <para>null이 아닌 값을 이 노드에 할당했을 경우 발생합니다. 이 노드가 새로 할당된 노드의 왼쪽 자식인지 오른쪽 자식인지 알 수 없기 때문에 연결을 할 수 없습니다.</para>
            /// <para>새로운 연결을 생성하고자 할 경우 부모 노드의 자식 속성에 값을 할당하십시오.</para>
            /// </exception>
            public Node Parent {
                get {
                    return _Parent;
                }
                set {
                    if (value != null) throw new ArgumentException();
                    if (_Parent != null) {
                        if (_Parent._LeftChild == this) _Parent._LeftChild = null;
                        else _Parent._RightChild = null;
                        _Parent = null;
                    }
                }
            }
            private Node _LeftChild = null;
            /// <summary>
            /// 이 노드의 왼쪽 자식 노드의 값을 가져오거나 설정할 수 있습니다. 값이 바뀔 때마다 그에 맞춰 자동으로 새 연결을 생성하거나 기존 연결을 해제합니다.
            /// </summary>
            public Node LeftChild {
                get {
                    return _LeftChild;
                }
                set {
                    if (_LeftChild != null) _LeftChild._Parent = null;
                    if (value != null) {
                        // 새로 붙일려는 노드가 다른 곳과 연결되어 있는 경우 먼저 그쪽을 해제해야 함.
                        if (value.Parent != null) value.Parent = null;
                        value._Parent = this;
                    }
                    _LeftChild = value;
                }
            }
            private Node _RightChild = null;
            /// <summary>
            /// 이 노드의 오른쪽 자식 노드의 값을 가져오거나 설정할 수 있습니다. 값이 바뀔 때마다 그에 맞춰 자동으로 새 연결을 생성하거나 기존 연결을 해제합니다.
            /// </summary>
            public Node RightChild {
                get {
                    return _RightChild;
                }
                set {
                    if (_RightChild != null) _RightChild._Parent = null;
                    if (value != null) {
                        // 새로 붙일려는 노드가 다른 곳과 연결되어 있는 경우 먼저 그쪽을 해제해야 함.
                        if (value.Parent != null) value.Parent = null;
                        value._Parent = this;
                    }
                    _RightChild = value;
                }
            }
            /// <summary>
            /// 이 노드의 키를 가져오거나 설정할 수 있습니다.
            /// </summary>
            public TKey Key { get; set; }
            /// <summary>
            /// 이 노드의 값을 가져오거나 설정할 수 있습니다.
            /// </summary>
            public TValue Value { get; set; }
            public Node(TKey Key, TValue Value) {
                this.Key = Key;
                this.Value = Value;
                Color = Color.Red;
            }
        }
        #endregion
    }
}
