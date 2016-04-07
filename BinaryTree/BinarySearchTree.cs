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
                    if (temp.LeftSibling == null) {
                        temp.LeftSibling = Node;
                        break;
                    } else {
                        temp = temp.LeftSibling;
                    }
                } else {
                    if (temp.RightSibling == null) {
                        temp.RightSibling = Node;
                        break;
                    } else {
                        temp = temp.RightSibling;
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
                if (Data.CompareTo(temp.Data) < 0) temp = temp.LeftSibling;
                else temp = temp.RightSibling;
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
            if (delete.LeftSibling != null && delete.RightSibling != null) {
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
            if (Node.LeftSibling == null && Node.RightSibling == null) {
                // 자식 0개
                if (Node.Parent == null) {
                    Clear();
                } else {
                    if (Node.Parent.LeftSibling == Node) Node.Parent.LeftSibling = null;
                    else Node.Parent.RightSibling = null;
                    Node.Parent = null;
                }
            } else {
                // 자식 1개
                if (Node.Parent == null) {
                    if (Node.LeftSibling != null) Root = Node.LeftSibling;
                    else Root = Node.RightSibling;
                    Root.Parent = null;
                } else {
                    if (Node.LeftSibling != null) {
                        if (Node.Parent.LeftSibling == Node) Node.Parent.LeftSibling = Node.LeftSibling;
                        else Node.Parent.RightSibling = Node.LeftSibling;
                    } else {
                        if (Node.Parent.LeftSibling == Node) Node.Parent.LeftSibling = Node.RightSibling;
                        else Node.Parent.RightSibling = Node.RightSibling;
                    }
                    Node.Parent = null;
                }
                Node.LeftSibling = null;
                Node.RightSibling = null;
            }
        }
        private Node<T> GetSuccessor(Node<T> Node) {
            if (Node.RightSibling != null) return GetMinOfTree(Node.RightSibling);
            Node<T> origin = Node;
            Node<T> temp = Node;
            while (true) {
                if (temp.Parent == null) return origin;
                else if (temp.Parent.LeftSibling != temp) temp = temp.Parent;
                else return temp;
            }
        }
        private Node<T> GetMinOfTree(Node<T> Parent) {
            Node<T> temp = Parent;
            while (temp.LeftSibling != null) temp = temp.LeftSibling;
            return temp;
        }
    }
}
