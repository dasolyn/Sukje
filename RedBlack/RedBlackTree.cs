using System;
using System.Collections;
using System.Collections.Generic;

namespace RedBlack {
    public class RedBlackTree<TKey, TValue> : IDictionary<TKey, TValue> {
        private Node Root;
        public IComparer<TKey> Comparer { get; }

        #region 생성자
        public RedBlackTree() {
            Comparer = Comparer<TKey>.Default;
        }
        public RedBlackTree(IComparer<TKey> Comparer) {
            this.Comparer = Comparer;
        }
        public RedBlackTree(Comparison<TKey> Comparison) {
            Comparer = Comparer<TKey>.Create(Comparison);
        }
        public RedBlackTree(IEnumerable<KeyValuePair<TKey, TValue>> Source) {
            Comparer = Comparer<TKey>.Default;
            foreach (KeyValuePair<TKey, TValue> i in Source) RedBlackAddNode(i.Key, i.Value);
        }
        public RedBlackTree(IEnumerable<KeyValuePair<TKey, TValue>> Source, IComparer<TKey> Comparer) {
            this.Comparer = Comparer;
            foreach (KeyValuePair<TKey, TValue> i in Source) RedBlackAddNode(i.Key, i.Value);
        }
        public RedBlackTree(IEnumerable<KeyValuePair<TKey, TValue>> Source, Comparison<TKey> Comparison) {
            Comparer = Comparer<TKey>.Create(Comparison);
            foreach (KeyValuePair<TKey, TValue> i in Source) RedBlackAddNode(i.Key, i.Value);
        }
        public RedBlackTree(IEnumerable<TKey> Keys, IEnumerable<TValue> Values) {
            Comparer = Comparer<TKey>.Default;
            using (IEnumerator<TKey> KeyEtor = Keys.GetEnumerator())
            using (IEnumerator<TValue> ValueEtor = Values.GetEnumerator()) {
                while (KeyEtor.MoveNext() && ValueEtor.MoveNext()) {
                    RedBlackAddNode(KeyEtor.Current, ValueEtor.Current);
                }
            }
        }
        public RedBlackTree(IEnumerable<TKey> Keys, IEnumerable<TValue> Values, IComparer<TKey> Comparer) {
            this.Comparer = Comparer;
            using (IEnumerator<TKey> KeyEtor = Keys.GetEnumerator())
            using (IEnumerator<TValue> ValueEtor = Values.GetEnumerator()) {
                while (KeyEtor.MoveNext() && ValueEtor.MoveNext()) {
                    RedBlackAddNode(KeyEtor.Current, ValueEtor.Current);
                }
            }
        }
        public RedBlackTree(IEnumerable<TKey> Keys, IEnumerable<TValue> Values, Comparison<TKey> Comparison) {
            Comparer = Comparer<TKey>.Create(Comparison);
            using (IEnumerator<TKey> KeyEtor = Keys.GetEnumerator())
            using (IEnumerator<TValue> ValueEtor = Values.GetEnumerator()) {
                while (KeyEtor.MoveNext() && ValueEtor.MoveNext()) {
                    RedBlackAddNode(KeyEtor.Current, ValueEtor.Current);
                }
            }
        }
        #endregion

        #region 인덱스
        public TValue this[TKey Key] {
            get {
                if (Key == null) throw new ArgumentNullException();
                Node found = RedBlackSearchNode(Key);
                if (found == null) throw new KeyNotFoundException();
                return found.Value;
            }
            set {
                if (Key == null) throw new ArgumentNullException();
                Node found = RedBlackSearchNode(Key);
                if (found == null) {
                    RedBlackAddNode(Key, value);
                } else {
                    found.Value = value;
                }
            }
        }
        #endregion

        #region 삽입
        public void Add(KeyValuePair<TKey, TValue> Item) {
            if (Item.Key == null) return;
            Node found = RedBlackSearchNode(Item.Key);
            if (found != null) return;
            RedBlackAddNode(Item.Key, Item.Value);
        }
        public void Add(TKey Key, TValue Value) {
            if (Key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(Key);
            if (found != null) throw new ArgumentException();
            RedBlackAddNode(Key, Value);
        }
        #endregion

        #region 검색
        public bool Contains(KeyValuePair<TKey, TValue> Item) {
            if (Item.Key == null) return false;
            Node found = RedBlackSearchNode(Item.Key);
            if (found == null) return false;
            else return found.Value.Equals(Item.Value);
        }
        public bool ContainsKey(TKey Key) {
            if (Key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(Key);
            return found != null;
        }
        public bool TryGetValue(TKey Key, out TValue Value) {
            if (Key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(Key);
            if (found == null) {
                Value = default(TValue);
                return false;
            } else {
                Value = found.Value;
                return true;
            }
        }
        public bool TryGetKey(TValue Value, out TKey Key) {
            if (Value == null) throw new ArgumentNullException();
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
            Key = default(TKey);
            return false;
        }
        public bool ContainsValue(TValue Value) {
            if (Value == null) throw new ArgumentNullException();
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
            return false;
        }
        #endregion

        #region 삭제
        public void Clear() {
            Root = null;
            Count = 0;
        }
        public bool Remove(KeyValuePair<TKey, TValue> Item) {
            if (Item.Key == null) return false;
            Node found = RedBlackSearchNode(Item.Key);
            if (found == null || found.Value.Equals(Item.Value)) return false;
            RedBlackDeleteNode(found);
            return true;
        }
        public bool Remove(TKey Key) {
            if (Key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(Key);
            if (found == null) return false;
            RedBlackDeleteNode(found);
            return true;
        }
        #endregion

        #region 기타
        public int Count { get; private set; }
        public bool IsReadOnly => false;
        public ICollection<TKey> Keys {
            get {
                List<TKey> keylist = new List<TKey>();
                foreach (var i in this) {
                    keylist.Add(i.Key);
                }
                return keylist;
            }
        }
        public ICollection<TValue> Values {
            get {
                List<TValue> valuelist = new List<TValue>();
                foreach (var i in this) {
                    valuelist.Add(i.Value);
                }
                return valuelist;
            }
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] Array, int ArrayIndex) {
            int cursor = 0;
            foreach (Node i in RedBlackEnumNode()) {
                if (cursor + ArrayIndex >= Array.Length) return;
                else {
                    Array[ArrayIndex + cursor] = new KeyValuePair<TKey, TValue>(i.Key, i.Value);
                    cursor++;
                }
            }
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (Node i in RedBlackEnumNode()) yield return new KeyValuePair<TKey, TValue>(i.Key, i.Value);
        }
        IEnumerator IEnumerable.GetEnumerator() {
            foreach (Node i in RedBlackEnumNode()) yield return new KeyValuePair<TKey, TValue>(i.Key, i.Value);
        }
        #endregion

        #region 내부 로직
        private Node RedBlackSearchNode(TKey key) {
            Node temp = Root;
            while (temp != null && Comparer.Compare(temp.Key, key) != 0) {
                if (Comparer.Compare(key, temp.Key) < 0) temp = temp.LeftChild;
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
                if (Comparer.Compare(insert.Key, cursor.Key) < 0) {
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
                if (parent.Color == Color.Black) break;
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
                    if (delete.LeftChild != null) {
                        Root = delete.LeftChild;
                        delete.LeftChild = null;
                    } else {
                        Root = delete.RightChild;
                        delete.RightChild = null;
                    }
                } else {
                    if (delete.LeftChild != null) {
                        delete.LeftChild.Parent = delete.Parent;
                        delete.Parent = null;
                    } else {
                        delete.RightChild.Parent = delete.Parent;
                        delete.Parent = null;
                    }
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
