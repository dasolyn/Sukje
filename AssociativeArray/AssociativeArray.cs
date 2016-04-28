using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssociativeArray {
    /// <summary>
    /// 직접 구현한 연관 배열입니다. 레드 블랙 트리를 통해 구현되었습니다.
    /// </summary>
    class AssociativeArray<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey> {
        public ICollection<TKey> Keys {
            get {
                return new ReadOnlyCollection<TKey>(InternalKeys);
            }
        }
        public ICollection<TValue> Values {
            get {
                return new ReadOnlyCollection<TValue>(InternalValues);
            }
        }
        public int Count {
            get {
                return InternalKeys.Count;
            }
        }
        public bool IsReadOnly {
            get {
                return false;
            }
        }
        public TValue this[TKey key] {
            get {
                if (key == null) throw new ArgumentNullException();
                return InternalSearch(key).Value;
            }
            set {
                throw new NotImplementedException();
            }
        }
        public bool ContainsKey(TKey key) {
            throw new NotImplementedException();
        }
        public void Add(TKey key, TValue value) {
            throw new NotImplementedException();
        }
        public bool Remove(TKey key) {
            throw new NotImplementedException();
        }
        public bool TryGetValue(TKey key, out TValue value) {
            throw new NotImplementedException();
        }
        public void Add(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }
        public void Clear() {
            throw new NotImplementedException();
        }
        public bool Contains(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }
        public bool Remove(KeyValuePair<TKey, TValue> item) {
            throw new NotImplementedException();
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        #region 내부 메서드
        private Node<TKey, TValue> Root = null;
        private IEnumerable<Node<TKey, TValue>> InternalInorderedEnum(Node<TKey, TValue> Parent) {
            if (Parent == null) yield break;
            else {
                foreach (var i in InternalInorderedEnum(Parent.LeftChild)) yield return i;
                yield return Parent;
                foreach (var i in InternalInorderedEnum(Parent.RightChild)) yield return i;
            }
        }
        private List<TKey> InternalKeys = new List<TKey>();
        private List<TValue> InternalValues = new List<TValue>();
        private Node<TKey, TValue> InternalSearch(TKey Key) {
            Node<TKey, TValue> temp = Root;
            do {
                if (Key.CompareTo(temp.Key) < 0) temp = temp.LeftChild;
                else temp = temp.RightChild;
            } while (temp != null && temp.Key.CompareTo(Key) != 0);
            if (temp == null) throw new KeyNotFoundException();
            else return temp;
        }
        #endregion

        private class Node<TNodeKey, TNodeValue> {
            public enum ColorOfNode { Black, Red };
            public ColorOfNode Color;
            private Node<TNodeKey, TNodeValue> _Parent = null;
            /// <summary>
            /// 이 노드의 부모 노드를 가져올 수 있습니다. 이 프로퍼티에 null을 할당하여 이 노드와 부모의 연결을 해제할 수 있습니다.
            /// </summary>
            /// <exception cref="ArgumentException">
            /// <para>null이 아닌 값을 이 노드에 할당했을 경우 발생합니다. 이 노드가 새로 할당된 노드의 왼쪽 자식인지 오른쪽 자식인지 알 수 없기 때문에 연결을 할 수 없습니다.</para>
            /// <para>새로운 연결을 생성하고자 할 경우 부모 노드의 자식 속성에 값을 할당하십시오.</para>
            /// </exception>
            public Node<TNodeKey, TNodeValue> Parent {
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
            private Node<TNodeKey, TNodeValue> _LeftChild = null;
            /// <summary>
            /// 이 노드의 왼쪽 자식 노드를 가져오거나 설정할 수 있습니다. 값이 바뀔 때마다 그에 맞춰 자동으로 새 연결을 생성하거나 기존 연결을 해제합니다.
            /// </summary>
            public Node<TNodeKey, TNodeValue> LeftChild {
                get {
                    return _LeftChild;
                }
                set {
                    if (_LeftChild != null) _LeftChild.Parent = null;
                    if (value != null) value._Parent = this;
                    _LeftChild = value;
                }
            }
            private Node<TNodeKey, TNodeValue> _RightChild = null;
            /// <summary>
            /// 이 노드의 오른쪽 자식 노드를 가져오거나 설정할 수 있습니다. 값이 바뀔 때마다 그에 맞춰 자동으로 새 연결을 생성하거나 기존 연결을 해제합니다.
            /// </summary>
            public Node<TNodeKey, TNodeValue> RightChild {
                get {
                    return _RightChild;
                }
                set {
                    if (_RightChild != null) _RightChild.Parent = null;
                    if (value != null) value._Parent = this;
                    _RightChild = value;
                }
            }
            /// <summary>
            /// 이 노드가 가지고 있는 데이터를 가져오거나 설정할 수 있습니다.
            /// </summary>
            public TNodeKey Key { get; set; }
            /// <summary>
            /// 이 노드가 가지고 있는 데이터를 가져오거나 설정할 수 있습니다.
            /// </summary>
            public TNodeValue Value { get; set; }
            public Node(TNodeKey Key, TNodeValue Value) {
                this.Key = Key;
                this.Value = Value;
                Color = ColorOfNode.Red;
            }
        }
    }
}
