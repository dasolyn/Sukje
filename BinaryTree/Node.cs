using System;

namespace BinaryTree {
    class Node<T> {
        private Node<T> _Parent = null;
        /// <summary>
        /// 이 노드의 부모 노드의 값을 가져올 수 있습니다. 이 프로퍼티에 null을 할당하여 이 노드와 부모의 연결을 해제할 수 있습니다.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <para>null이 아닌 값을 이 노드에 할당했을 경우 발생합니다. 이 노드가 새로 할당된 노드의 왼쪽 자식인지 오른쪽 자식인지 알 수 없기 때문에 연결을 할 수 없습니다.</para>
        /// <para>새로운 연결을 생성하고자 할 경우 부모 노드의 자식 속성에 값을 할당하십시오.</para>
        /// </exception>
        public Node<T> Parent {
            get {
                return _Parent;
            }
            set {
                if (value == null && _Parent != null) {
                    if (_Parent._LeftChild == this) _Parent._LeftChild = null;
                    else _Parent._RightChild = null;
                } else if (value != null) {
                    throw new ArgumentException();
                }
            }
        }
        private Node<T> _LeftChild = null;
        /// <summary>
        /// 이 노드의 왼쪽 자식 노드의 값을 가져오거나 설정할 수 있습니다. 값이 바뀔 때마다 그에 맞춰 자동으로 새 연결을 생성하거나 기존 연결을 해제합니다.
        /// </summary>
        public Node<T> LeftChild {
            get {
                return _LeftChild;
            }
            set {
                if (value == null && _LeftChild != null) {
                    _LeftChild._Parent = null;
                } else if (value != null) {
                    if (_LeftChild != null) _LeftChild.Parent = null;
                    value._Parent = this;
                }
                _LeftChild = value;
            }
        }
        private Node<T> _RightChild = null;
        /// <summary>
        /// 이 노드의 오른쪽 자식 노드의 값을 가져오거나 설정할 수 있습니다. 값이 바뀔 때마다 그에 맞춰 자동으로 새 연결을 생성하거나 기존 연결을 해제합니다.
        /// </summary>
        public Node<T> RightChild {
            get {
                return _RightChild;
            }
            set {
                if (value == null && _RightChild != null) {
                    _RightChild._Parent = null;
                } else if (value != null) {
                    if (_RightChild != null) _RightChild.Parent = null;
                    value._Parent = this;
                }
                _RightChild = value;
            }
        }
        /// <summary>
        /// 이 노드가 가지고 있는 데이터를 가져오거나 설정할 수 있습니다.
        /// </summary>
        public T Data { get; set; }
        public Node(T Data) {
            this.Data = Data;
        }
    }
}
