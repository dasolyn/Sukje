namespace BinaryTree {
    class Node<T> {
        public Node<T> Parent { get; set; } = null;
        private Node<T> _LeftSibling = null;
        public Node<T> LeftSibling {
            get {
                return _LeftSibling;
            }
            set {
                if (value != null) value.Parent = this;
                _LeftSibling = value;
            }
        }
        private Node<T> _RightSibling = null;
        public Node<T> RightSibling {
            get {
                return _RightSibling;
            }
            set {
                if (value != null) value.Parent = this;
                _RightSibling = value;
            }
        }
        public T Data { get; set; }
        public Node(T Data) {
            this.Data = Data;
        }
    }
}
