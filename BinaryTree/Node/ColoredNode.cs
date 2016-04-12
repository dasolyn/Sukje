namespace BinaryTree {
    public enum ColorOfNode { Black, Red };
    class ColoredNode<T> : Node<T> {
        public ColorOfNode Color;
        public ColoredNode(T Data) : base(Data) {
            Color = ColorOfNode.Red;
        }
    }
}
