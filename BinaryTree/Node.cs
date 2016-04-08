namespace BinaryTree {
    class Node<T> {
        private Node<T> _Parent = null;
        public Node<T> Parent {
            get {
                return _Parent;
            }
            set {
                // 이 노드가 Parent에 새로 할당된 노드의 왼쪽 자식인지 오른쪽 자식인지 알 수 없기 때문에 여기서는 연결을 할 수 없고, 연결 해제만 가능
                if (value == null && _Parent != null) {
                    if (_Parent._LeftChild == this) _Parent._LeftChild = null;
                    else _Parent._RightChild = null;
                }
                _Parent = value;
            }
        }
        private Node<T> _LeftChild = null;
        public Node<T> LeftChild {
            get {
                return _LeftChild;
            }
            set {
                // 연결 및 연결 해제
                if (value == null && _LeftChild != null) {
                    _LeftChild._Parent = null;
                } else if (value != null) {
                    if (_LeftChild != null) _LeftChild._Parent = null;
                    value._Parent = this;
                }
                _LeftChild = value;
            }
        }
        private Node<T> _RightChild = null;
        public Node<T> RightChild {
            get {
                return _RightChild;
            }
            set {
                // 연결 및 연결 해제
                if (value == null && _RightChild != null) {
                    _RightChild._Parent = null;
                } else if (value != null) {
                    if (_RightChild != null) _RightChild._Parent = null;
                    value._Parent = this;
                }
                _RightChild = value;
            }
        }
        public T Data { get; set; }
        public Node(T Data) {
            this.Data = Data;
        }
    }
}
