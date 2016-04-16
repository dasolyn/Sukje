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
            // Insert Fix-up
            Node<T> temp = insert;
            while (true) {
                Node<T> parent = temp.Parent;
                if (GetColor(parent) == ColorOfNode.Black) break;
                Node<T> grandparent = parent.Parent;
                Node<T> uncle;
                if (parent == grandparent.LeftChild) {
                    uncle = grandparent.RightChild;
                    if (GetColor(uncle) == ColorOfNode.Black) {
                        if (temp == parent.RightChild) {
                            // Case 2
                            LeftRotate(parent);
                            parent = parent.Parent;
                            grandparent = parent.Parent;
                        }
                        // Case 3
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                        RightRotate(grandparent);
                    } else {
                        // Case 1
                        SetColor(uncle, ColorOfNode.Black);
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                        temp = grandparent;
                    }
                } else {
                    uncle = grandparent.LeftChild;
                    if (GetColor(uncle) == ColorOfNode.Black) {
                        if (temp == parent.LeftChild) {
                            RightRotate(parent);
                            parent = parent.Parent;
                            grandparent = parent.Parent;
                        }
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                        LeftRotate(grandparent);
                    } else {
                        SetColor(uncle, ColorOfNode.Black);
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(grandparent, ColorOfNode.Red);
                        temp = grandparent;
                    }
                }
            }
            SetColor(Root, ColorOfNode.Black);
        }
        /// <summary>
        /// 레드-블랙 탐색 트리에서 트리의 일부인 특정 노드를 삭제합니다.
        /// </summary>
        /// <exception cref="InvalidOperationException">이 레드-블랙 트리가 무결하지 않습니다. 이 트리가 가지고 있는 노드 중에 색깔이 없는 노드가 있을 경우 발생합니다.</exception>
        public override void Delete(Node<T> Node) {
            // x와 y 구하기
            Node<T> delete;
            if (Node.LeftChild != null && Node.RightChild != null) {
                delete = GetSuccessor(Node);
                Node.Data = delete.Data;
            } else {
                delete = Node;
            }
            Node<T> temp;
            if (Node.Parent.LeftChild == Node) temp = delete.LeftChild;
            else temp = delete.RightChild;
            Node<T> py = delete.Parent;
            // 삭제
            InternalDelete(delete);
            if (GetColor(delete) == ColorOfNode.Red) return;
            // Delete Fix-up
            while (temp != Root && GetColor(temp) == ColorOfNode.Black) {
                Node<T> parent;
                if (temp == null) parent = py;
                else parent = temp.Parent;
                Node<T> sibling;
                if (temp == parent.LeftChild) {
                    sibling = parent.RightChild;
                    if (GetColor(sibling) == ColorOfNode.Red) {
                        // Case 1
                        SetColor(sibling, ColorOfNode.Black);
                        SetColor(parent, ColorOfNode.Red);
                        LeftRotate(parent);
                        sibling = parent.RightChild;
                    }
                    if (GetColor(sibling.LeftChild) == ColorOfNode.Black && GetColor(sibling.RightChild) == ColorOfNode.Black) {
                        // Case 2
                        SetColor(sibling, ColorOfNode.Red);
                        temp = parent;
                    } else {
                        if (GetColor(sibling.RightChild) == ColorOfNode.Black) {
                            // Case 3
                            SetColor(sibling.LeftChild, ColorOfNode.Black);
                            SetColor(sibling, ColorOfNode.Red);
                            RightRotate(sibling);
                            sibling = parent.RightChild;
                        }
                        // Case 4
                        SetColor(sibling, GetColor(parent));
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(sibling.RightChild, ColorOfNode.Black);
                        LeftRotate(parent);
                        temp = Root;
                    }
                } else {
                    sibling = parent.LeftChild;
                    if (GetColor(sibling) == ColorOfNode.Red) {
                        SetColor(sibling, ColorOfNode.Black);
                        SetColor(parent, ColorOfNode.Red);
                        RightRotate(parent);
                        sibling = parent.LeftChild;
                    }
                    if (GetColor(sibling.LeftChild) == ColorOfNode.Black && GetColor(sibling.RightChild) == ColorOfNode.Black) {
                        SetColor(sibling, ColorOfNode.Red);
                        temp = parent;
                    } else {
                        if (GetColor(sibling.LeftChild) == ColorOfNode.Black) {
                            SetColor(sibling.RightChild, ColorOfNode.Black);
                            SetColor(sibling, ColorOfNode.Red);
                            LeftRotate(sibling);
                            sibling = parent.LeftChild;
                        }
                        SetColor(sibling, GetColor(parent));
                        SetColor(parent, ColorOfNode.Black);
                        SetColor(sibling.LeftChild, ColorOfNode.Black);
                        RightRotate(parent);
                        temp = Root;
                    }
                }
            }
            SetColor(temp, ColorOfNode.Black);
        }

        /// <summary>
        /// 레드-블랙 트리에서 특정 노드를 루트로 하는 부분 트리를 좌회전하는 내부 메서드입니다.
        /// </summary>
        protected void LeftRotate(Node<T> Node) {
            Node<T> y = Node.RightChild;
            Node.RightChild = y.LeftChild;
            if (Node.Parent == null) Root = y;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = y;
            else Node.Parent.RightChild = y;
            y.LeftChild = Node;
        }
        /// <summary>
        /// 레드-블랙 트리에서 특정 노드를 루트로 하는 부분 트리를 우회전하는 내부 메서드입니다.
        /// </summary>
        protected void RightRotate(Node<T> Node) {
            Node<T> x = Node.LeftChild;
            Node.LeftChild = x.RightChild;
            if (Node.Parent == null) Root = x;
            else if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = x;
            else Node.Parent.RightChild = x;
            x.RightChild = Node;
        }
        /// <summary>
        /// 레드-블랙 트리에서 특정 노드의 색깔을 설정하는 내부 메서드입니다.
        /// </summary>
        /// <exception cref="InvalidOperationException">지정된 노드가 색깔을 가지지 못하는 노드입니다. <c>ColoredNode</c>가 아닌 노드이면 발생합니다.</exception>
        protected void SetColor(Node<T> Node, ColorOfNode NewColor) {
            if (Node == null) return;
            ColoredNode<T> colored = Node as ColoredNode<T>;
            if (colored == null) throw new InvalidOperationException();
            colored.Color = NewColor;
        }
        /// <summary>
        /// 레드-블랙 트리에서 특정 노드의 색깔을 가져오는 내부 메서드입니다.
        /// </summary>
        /// <exception cref="InvalidOperationException">지정된 노드가 색깔을 가지지 못하는 노드입니다. <c>ColoredNode</c>가 아닌 노드이면 발생합니다.</exception>
        protected ColorOfNode GetColor(Node<T> Node) {
            if (Node == null) return ColorOfNode.Black;
            ColoredNode<T> colored = Node as ColoredNode<T>;
            if (colored == null) throw new InvalidOperationException();
            return colored.Color;
        }
    }
}
