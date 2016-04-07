namespace BinaryTree {
    class Node<T> {
        public Node<T> Parent { get; private set; } = null;
        private Node<T> _LeftChild = null;
        public Node<T> LeftChild {
            get {
                return _LeftChild;
            }
            set {
                value.Parent = this;
                _LeftChild = value;
            }
        }
        private Node<T> _RightChild = null;
        public Node<T> RightChild {
            get {
                return _RightChild;
            }
            set {
                value.Parent = this;
                _RightChild = value;
            }
        }
        public T Data { get; }
        public Node(T Data) {
            this.Data = Data;
        }
    }
}
