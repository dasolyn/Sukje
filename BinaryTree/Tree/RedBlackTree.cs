using System;
using System.Linq;

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
        /// <exception cref="InvalidCastException">이 레드-블랙 트리가 무결하지 않습니다. 이 트리가 가지고 있는 노드 중에 색깔이 없는 노드가 있을 경우 발생합니다.</exception>
        public override void Insert(Node<T> Node) {
            if (Node.Parent != null || Node.LeftChild != null || Node.RightChild != null) {
                throw new ArgumentException();
            } else {
                ColoredNode<T> insert = new ColoredNode<T>(Node.Data);
                base.Insert(insert);
                try {
                    ColoredNode<T> temp = insert;
                    while (true) {
                        ColoredNode<T> parent = (ColoredNode<T>)temp.Parent;
                        if (IsNodeBlack(parent)) break;
                        ColoredNode<T> grandparent = (ColoredNode<T>)parent.Parent;
                        ColoredNode<T> uncle;
                        if (parent == grandparent.LeftChild) {
                            uncle = (ColoredNode<T>)grandparent.RightChild;
                            if (IsNodeBlack(uncle)) {
                                if (temp == parent.RightChild) {
                                    temp = parent;
                                    LeftRotation(temp);
                                }
                                parent = (ColoredNode<T>)temp.Parent;
                                grandparent = (ColoredNode<T>)parent.Parent;
                                parent.Color = ColorOfNode.Black;
                                grandparent.Color = ColorOfNode.Red;
                                RightRotation(grandparent);
                            } else {
                                uncle.Color = ColorOfNode.Black;
                                grandparent.Color = ColorOfNode.Red;
                            }
                            temp = grandparent;
                        } else {
                            uncle = (ColoredNode<T>)grandparent.LeftChild;
                            if (IsNodeBlack(uncle)) {
                                if (temp == parent.LeftChild) {
                                    temp = parent;
                                    RightRotation(temp);
                                }
                                parent = (ColoredNode<T>)temp.Parent;
                                grandparent = (ColoredNode<T>)parent.Parent;
                                parent.Color = ColorOfNode.Black;
                                grandparent.Color = ColorOfNode.Red;
                                LeftRotation(grandparent);
                            } else {
                                uncle.Color = ColorOfNode.Black;
                                grandparent.Color = ColorOfNode.Red;
                            }
                            temp = grandparent;
                        }
                    }
                    ((ColoredNode<T>)Root).Color = ColorOfNode.Black;
                } catch (InvalidCastException) { // 반복문 내의 Node<T> -> ColoredNode<T> 캐스팅이 실패시 발생
                    throw new InvalidOperationException();
                }
            }
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
        private bool IsNodeBlack(ColoredNode<T> Node) {
            if (Node == null) return true;
            else {
                if (Node.Color == ColorOfNode.Black) return true;
                else return false; 
            }
        }
    }
}
