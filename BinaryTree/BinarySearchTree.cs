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
            // Case 1 : 자식 노드가 없는 경우
            if (delete.LeftSibling == null && delete.RightSibling == null) {
                if (delete != Root) {
                    if (delete.Parent.LeftSibling == delete) delete.Parent.LeftSibling = null;
                    else delete.Parent.RightSibling = null;
                } else {
                    Clear();
                }
            // Case 3 : 자식 노드가 2개인 경우
            } else if (delete.LeftSibling != null && delete.RightSibling != null) {

            // Case 2: 자식 노드가 1개인 경우
            } else {

            }
        }
        private Node<T> SearchSuccessor(Node<T> Node) {

        }
    }
}
