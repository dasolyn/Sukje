using System;

namespace BinaryTree {
    class BinarySearchTree<T> : BinaryTree<T> where T : IComparable<T> {
        /// <summary>
        /// 이진 검색 트리에 새 노드를 삽입합니다.
        /// </summary>
        public void InsertNodeToBST(Node<T> Node) {
            if (Root == null) {
                Root = Node;
                return;
            }
            Node<T> temp = Root;
            while (true) {
                if (Node.Data.CompareTo(temp.Data) < 0) {
                    if (temp.LeftChild == null) {
                        temp.LeftChild = Node;
                        break;
                    } else {
                        temp = temp.LeftChild;
                    }
                } else {
                    if (temp.RightChild == null) {
                        temp.RightChild = Node;
                        break;
                    } else {
                        temp = temp.RightChild;
                    }
                }
            }
            
        }
        /// <summary>
        /// 이진 검색 트리에서 지정한 값을 가진 노드를 찾습니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public Node<T> SearchNodeFromBST(T Data) {
            Node<T> temp = Root;
            do {
                if (Data.CompareTo(temp.Data) < 0) temp = temp.LeftChild;
                else temp = temp.RightChild;
            } while (temp != null && temp.Data.CompareTo(Data) != 0);
            if (temp == null) throw new ArgumentException();
            else return temp;
        }
        /// <summary>
        /// 이진 검색 트리에서 지정한 값을 가진 노드를 삭제합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public void DeleteNodeFromBST(T Data) {
            Node<T> delete = SearchNodeFromBST(Data);
            if (delete.LeftChild != null && delete.RightChild != null) {
                Node<T> suc = GetSuccessor(delete);
                delete.Data = suc.Data;
                DeleteSpecificNode(suc);
            } else {
                DeleteSpecificNode(delete);
            }
        }
        /// <summary>
        /// 자식이 1개 또는 0개 존재하는 특정 노드를 삭제하는 내부 메서드입니다.
        /// </summary>
        private void DeleteSpecificNode(Node<T> Node) {
            if (Node.LeftChild == null && Node.RightChild == null) {
                // 자식 0개
                if (Node.Parent == null) {
                    Clear();
                } else {
                    if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = null;
                    else Node.Parent.RightChild = null;
                    Node.Parent = null;
                }
            } else {
                // 자식 1개
                if (Node.Parent == null) {
                    if (Node.LeftChild != null) Root = Node.LeftChild;
                    else Root = Node.RightChild;
                    Root.Parent = null;
                } else {
                    if (Node.LeftChild != null) {
                        if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = Node.LeftChild;
                        else Node.Parent.RightChild = Node.LeftChild;
                    } else {
                        if (Node.Parent.LeftChild == Node) Node.Parent.LeftChild = Node.RightChild;
                        else Node.Parent.RightChild = Node.RightChild;
                    }
                    Node.Parent = null;
                }
                Node.LeftChild = null;
                Node.RightChild = null;
            }
        }
        private Node<T> GetSuccessor(Node<T> Node) {
            if (Node.RightChild != null) return GetMinOfTree(Node.RightChild);
            Node<T> origin = Node;
            Node<T> temp = Node;
            while (true) {
                if (temp.Parent == null) return origin;
                else if (temp.Parent.LeftChild != temp) temp = temp.Parent;
                else return temp;
            }
        }
        private Node<T> GetMinOfTree(Node<T> Parent) {
            Node<T> temp = Parent;
            while (temp.LeftChild != null) temp = temp.LeftChild;
            return temp;
        }
    }
}
