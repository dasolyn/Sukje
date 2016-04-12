using System;

namespace BinaryTree {
    /// <summary>
    /// 기초적인 이진 탐색 트리입니다. 트리의 균형을 자동으로 보정하는 과정이 수행되지 않습니다.
    /// </summary>
    class BinarySearchTree<T> : SearchTree<T> where T : IComparable<T> {
        /// <summary>
        /// 이진 탐색 트리에 데이터를 삽입합니다.
        /// </summary>
        public override void Insert(T Data) {
            InternalInsert(new Node<T>(Data));
        }
        /// <summary>
        /// 이진 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 노드의 데이터를 찾아 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public override T Search(T Data) {
            return InternalSearch(Data).Data;
        }
        /// <summary>
        /// 이진 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 노드를 삭제합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public override void Delete(T Data) {
            Node<T> delete = InternalSearch(Data);
            if (delete.LeftChild != null && delete.RightChild != null) {
                Node<T> suc = GetSuccessor(delete);
                delete.Data = suc.Data;
                InternalDelete(suc);
            } else {
                InternalDelete(delete);
            }
        }
        protected void InternalInsert(Node<T> Node) {
            if (Root == null) {
                Root = Node;
            } else {
                Node<T> temp = Root;
                while (true) {
                    if (Node.Data.CompareTo(temp.Data) < 0) {
                        if (temp.LeftChild == null) {
                            temp.LeftChild = Node;
                            return;
                        } else {
                            temp = temp.LeftChild;
                        }
                    } else {
                        if (temp.RightChild == null) {
                            temp.RightChild = Node;
                            return;
                        } else {
                            temp = temp.RightChild;
                        }
                    }
                }
            }
        }
        protected Node<T> InternalSearch(T Data) {
            Node<T> temp = Root;
            do {
                if (Data.CompareTo(temp.Data) < 0) temp = temp.LeftChild;
                else temp = temp.RightChild;
            } while (temp != null && temp.Data.CompareTo(Data) != 0);
            if (temp == null) throw new ArgumentException();
            else return temp;
        }
        /// <summary>
        /// 자식이 1개 또는 0개 존재하는 특정 노드를 삭제하는 내부 메서드입니다.
        /// </summary>
        protected void InternalDelete(Node<T> Node) {
            if (Node.LeftChild == null && Node.RightChild == null) {
                // 자식 0개
                if (Node.Parent == null) Clear();
                else Node.Parent = null;
            } else {
                // 자식 1개
                if (Node.Parent == null) {
                    if (Node.LeftChild != null) {
                        Root = Node.LeftChild;
                        Node.LeftChild = null;
                    } else {
                        Root = Node.RightChild;
                        Node.RightChild = null;
                    }
                } else {
                    if (Node.LeftChild != null) {
                        Node.LeftChild.Parent = Node.Parent;
                        Node.Parent = null;
                    } else {
                        Node.RightChild.Parent = Node.Parent;
                        Node.Parent = null;
                    }
                }
            }
        }
        protected Node<T> GetSuccessor(Node<T> Node) {
            if (Node.RightChild != null) return GetMinOfTree(Node.RightChild);
            Node<T> origin = Node;
            Node<T> temp = Node;
            while (true) {
                if (temp.Parent == null) return origin;
                else if (temp.Parent.LeftChild != temp) temp = temp.Parent;
                else return temp;
            }
        }
        protected Node<T> GetMinOfTree(Node<T> Parent) {
            Node<T> temp = Parent;
            while (temp.LeftChild != null) temp = temp.LeftChild;
            return temp;
        }
    }
}
