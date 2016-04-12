using System;

namespace BinaryTree {
    /// <summary>
    /// 레드-블랙 탐색 트리입니다.
    /// </summary>
    class RedBlackTree<T> : BinarySearchTree<T> where T : IComparable<T> {
        public override Node<T> Insert(T Data) {
            // 빨간 노드를 일반 BST와 동일한 방법으로 삽입
            Node<T> insert = new ColoredNode<T>(Data, ColoredNode<T>.ColorOfNode.Red);
            Insert(insert);
            // 픽스
            return null;
        }
        public override Node<T> Insert(Node<T> Node) {
            return base.Insert(Node);
        }
        public override void Delete(T Data) {
            throw new NotImplementedException();
        }
        protected void LeftRotation(Node<T> Node) {
            Node<T> y = Node.RightChild;
            if (Node.Parent == null) Root = y;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = y;
            else Node.Parent.RightChild = y;
            // y의 왼쪽 트리를 x의 오른쪽 트리로 이동
            Node.RightChild = y.LeftChild;
            // x를 y의 왼쪽 자식으로
            y.LeftChild = Node;
        }
        protected void RightRotation(Node<T> Node) {
            Node<T> x = Node.LeftChild;
            if (Node.Parent == null) Root = x;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = x;
            else Node.Parent.RightChild = x;
            // x의 오른쪽 트리를 y의 왼쪽 트리로 이동
            x.LeftChild = Node.RightChild;
            // y를 x의 오른쪽 자식으로
            x.RightChild = Node;
        }
    }
}
