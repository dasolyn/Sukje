namespace BinaryTree {
    class Node<T> {
        public Node<T> Parent { get; set; } = null;
        private Node<T> _LeftChild = null;
        public Node<T> LeftChild {
            get {
                return _LeftChild;
            }
            set {
                if (value != null) value.Parent = this;
                _LeftChild = value;
            }
        }
        private Node<T> _RightChild = null;
        public Node<T> RightChild {
            get {
                return _RightChild;
            }
            set {
                if (value != null) value.Parent = this;
                _RightChild = value;
            }
        }
        public T Data { get; set; }
        public Node(T Data) {
            this.Data = Data;
        }
    }
}
