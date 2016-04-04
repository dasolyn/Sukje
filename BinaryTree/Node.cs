namespace BinaryTree {
    class Node<T> {
        public Node<T> Parent { get; private set; } = null;
        private Node<T> _LeftSibling = null;
        public Node<T> LeftSibling {
            get {
                return _LeftSibling;
            }
            set {
                value.Parent = this;
                _LeftSibling = value;
            }
        }
        private Node<T> _RightSibling = null;
        public Node<T> RightSibling {
            get {
                return _RightSibling;
            }
            set {
                value.Parent = this;
                _RightSibling = value;
            }
        }
        public T Data { get; }
        public Node(T Data) {
            this.Data = Data;
        }
    }
}
