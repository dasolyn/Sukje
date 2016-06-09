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
        #endregion

        #region 인덱스
        public TValue this[TKey key] {
            get {
                if (key == null) throw new ArgumentNullException();
                Node found = RedBlackSearchNode(key);
                if (found == null) throw new KeyNotFoundException();
                return found.Value;
            }
            set {
                if (key == null) throw new ArgumentNullException();
                Node found = RedBlackSearchNode(key);
                if (found == null) {
                    RedBlackAddNode(key, value);
                } else {
                    found.Value = value;
                }
            }
        }
        #endregion

        #region 삽입
        public void Add(KeyValuePair<TKey, TValue> item) {
            if (item.Key == null) return;
            Node found = RedBlackSearchNode(item.Key);
            if (found != null) return;
            RedBlackAddNode(item.Key, item.Value);
        }
        public void Add(TKey key, TValue value) {
            if (key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(key);
            if (found != null) throw new ArgumentException();
            RedBlackAddNode(key, value);
        }
        #endregion

        #region 검색
        public bool Contains(KeyValuePair<TKey, TValue> item) {
            if (item.Key == null) return false;
            Node found = RedBlackSearchNode(item.Key);
            if (found == null) return false;
            else return found.Value.Equals(item.Value);
        }
        public bool ContainsKey(TKey key) {
            if (key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(key);
            return found != null;
        }
        public bool TryGetValue(TKey key, out TValue value) {
            if (key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(key);
            if (found == null) {
                value = default(TValue);
                return false;
            } else {
                value = found.Value;
                return true;
            }
        }
        #endregion

        #region 삭제
        public void Clear() {
            Root = null;
            Count = 0;
        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {
            if (item.Key == null) return false;
            Node found = RedBlackSearchNode(item.Key);
            if (found == null || found.Value.Equals(item.Value)) return false;
            RedBlackDeleteNode(found);
            return true;
        }
        public bool Remove(TKey key) {
            if (key == null) throw new ArgumentNullException();
            Node found = RedBlackSearchNode(key);
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
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            int cursor = 0;
            foreach (Node i in RedBlackEnumNode()) {
                if (cursor + arrayIndex >= array.Length) return;
                else {
                    array[arrayIndex + cursor] = new KeyValuePair<TKey, TValue>(i.Key, i.Value);
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
        private Node RedBlackSearchNode(TKey Key) {
            Node temp = Root;
            while (temp != null && Comparer.Compare(temp.Key, Key) != 0) {
                if (Comparer.Compare(Key, temp.Key) < 0) temp = temp.LeftChild;
                else temp = temp.RightChild;
            }
            return temp;
        }
        private void RedBlackAddNode(TKey Key, TValue Value) {
            Count++;
            Node insert = new Node(Key, Value);
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
        private Color GetColor(Node Node) {
            if (Node == null) return Color.Black;
            return Node.Color;
        }
        private void LeftRotate(Node Node) {
            Node y = Node.RightChild;
            Node.RightChild = y.LeftChild;
            if (Node.Parent == null) Root = y;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = y;
            else Node.Parent.RightChild = y;
            y.LeftChild = Node;
        }
        private void RightRotate(Node Node) {
            Node x = Node.LeftChild;
            Node.LeftChild = x.RightChild;
            if (Node.Parent == null) Root = x;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = x;
            else Node.Parent.RightChild = x;
            x.RightChild = Node;
        }
        private Node GetSuccessor(Node Node) {
            if (Node.RightChild != null) return GetMinimum(Node.RightChild);
            Node origin = Node;
            Node temp = Node;
            while (true) {
                if (temp.Parent == null) return origin;
                else if (temp.Parent.LeftChild != temp) temp = temp.Parent;
                else return temp;
            }
        }
        private Node GetMinimum(Node Parent) {
            Node temp = Parent;
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
