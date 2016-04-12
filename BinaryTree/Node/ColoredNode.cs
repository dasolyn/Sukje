namespace BinaryTree {
    class ColoredNode<T> : Node<T> {
        public enum ColorOfNode { Black, Red };
        public ColorOfNode Color;
        public ColoredNode(T Data) : base(Data) { }
        public ColoredNode(T Data, ColorOfNode Color) : base(Data) {
            this.Color = Color;
        }
    }
}
