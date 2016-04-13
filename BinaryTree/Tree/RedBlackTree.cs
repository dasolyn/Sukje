using System;

namespace BinaryTree {
    /// <summary>
    /// 레드-블랙 탐색 트리입니다.
    /// </summary>
    class RedBlackTree<T> : BinarySearchTree<T> where T : IComparable<T> {
        /// <summary>
        /// 레드-블랙 탐색 트리에 노드를 삽입합니다.
        /// </summary>
        /// <returns>삽입된 노드입니다.</returns>
        /// <exception cref="ArgumentException">해당 노드가 자손 혹은 부모를 가지고 있습니다. 아무 관계도 가지고 있지 않은 노드가 아니면 삽입할 수 없습니다.</exception>
        /// <exception cref="InvalidOperationException">이 레드-블랙 트리가 무결하지 않습니다. 이 트리가 가지고 있는 노드 중에 색깔이 없는 노드가 있을 경우 발생합니다.</exception>
        public override void Insert(Node<T> Node) {
            if (Node.Parent != null || Node.LeftChild != null || Node.RightChild != null) throw new ArgumentException();
            ColoredNode<T> insert = new ColoredNode<T>(Node.Data);
            base.Insert(insert);
            Node<T> temp = insert;
            while (true) {
                Node<T> parent = temp.Parent;
                if (IsNodeBlack(parent)) break;
                Node<T> grandparent = parent.Parent;
                Node<T> uncle;
                if (parent == grandparent.LeftChild) {
                    uncle = grandparent.RightChild;
                    if (IsNodeBlack(uncle)) {
                        if (temp == parent.RightChild) {
                            LeftRotation(parent);
                            temp = parent;
                            parent = temp.Parent;
                            grandparent = parent.Parent;
                        }
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                        RightRotation(grandparent);
                    } else {
                        SetColor(uncle, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                    }
                    temp = grandparent;
                } else {
                    uncle = grandparent.LeftChild;
                    if (IsNodeBlack(uncle)) {
                        if (temp == parent.LeftChild) {
                            RightRotation(parent);
                            temp = parent;
                            parent = temp.Parent;
                            grandparent = parent.Parent;
                        }
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                        LeftRotation(grandparent);
                    } else {
                        SetColor(uncle, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                    }
                    temp = grandparent;
                }
            }
            SetColor(Root, ColorOfNode.Black);
        }
        /// <summary>
        /// 레드-블랙 탐색 트리에서 트리의 일부인 특정 노드를 삭제합니다.
        /// </summary>
        /// <exception cref="InvalidOperationException">이 레드-블랙 트리가 무결하지 않습니다. 이 트리가 가지고 있는 노드 중에 색깔이 없는 노드가 있을 경우 발생합니다.</exception>
        public override void Delete(Node<T> Node) {
            base.Delete(Node);
            ColoredNode<T> colored = Node as ColoredNode<T>;
            if (colored == null) throw new InvalidOperationException();
            // DELETE FIXUP
        }
        private void LeftRotation(Node<T> Node) {
            Node<T> y = Node.RightChild;
            // y의 왼쪽 트리를 x의 오른쪽 트리로 이동
            Node.RightChild = y.LeftChild;
            // y의 새 부모 지정
            if (Node.Parent == null) Root = y;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = y;
            else Node.Parent.RightChild = y;
            // x를 y의 왼쪽 자식으로
            y.LeftChild = Node;
        }
        private void RightRotation(Node<T> Node) {
            Node<T> x = Node.LeftChild;
            // x의 오른쪽 트리를 y의 왼쪽 트리로 이동
            Node.LeftChild = x.RightChild;
            // x의 새 부모 지정
            if (Node.Parent == null) Root = x;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = x;
            else Node.Parent.RightChild = x;
            // y를 x의 오른쪽 자식으로
            x.RightChild = Node;
        }
        private bool IsNodeBlack(Node<T> Node) {
            if (Node == null) return true;
            ColoredNode<T> colored = Node as ColoredNode<T>;
            if (colored == null) throw new InvalidOperationException();
            if (colored.Color == ColorOfNode.Black) return true;
            else return false;
        }
        private void SetColor(Node<T> Node, ColorOfNode NewColor) {
            ColoredNode<T> colored = Node as ColoredNode<T>;
            if (colored == null) throw new InvalidOperationException();
            colored.Color = NewColor;
        }
    }
}
