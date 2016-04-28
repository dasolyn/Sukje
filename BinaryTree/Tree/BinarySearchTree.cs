using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTree {
    /// <summary>
    /// 기초적인 이진 탐색 트리입니다. 트리의 균형을 자동으로 보정하는 과정이 수행되지 않습니다.
    /// </summary>
    public class BinarySearchTree<T> : BinaryTree<T>, IEnumerable<Node<T>> where T : IComparable<T> {
        /// <summary>
        /// 이진 탐색 트리에 데이터를 삽입합니다.
        /// </summary>
        public void Insert(T Data) {
            Insert(new Node<T>(Data));
        }
        /// <summary>
        /// 이진 탐색 트리에 노드를 삽입합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 노드가 자손 혹은 부모를 가지고 있습니다. 아무 관계도 가지고 있지 않은 노드가 아니면 삽입할 수 없습니다.</exception>
        public virtual void Insert(Node<T> Node) {
            if (Node.Parent != null || Node.LeftChild != null || Node.RightChild != null) throw new ArgumentException();
            if (Root == null) {
                Root = Node;
                return;
            }
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
        /// <summary>
        /// 이진 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 노드를 찾아 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public Node<T> Search(T Data) {
            Node<T> temp = Root;
            while (temp != null && temp.Data.CompareTo(Data) != 0) {
                if (Data.CompareTo(temp.Data) < 0) temp = temp.LeftChild;
                else temp = temp.RightChild;
            }
            if (temp == null) throw new ArgumentException();
            else return temp;
        }
        /// <summary>
        /// 이진 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 노드의 데이터를 찾아 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public T SearchData(T Data) {
            return Search(Data).Data;
        }
        /// <summary>
        /// 이진 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 데이터를 갖는 노드를 삭제합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public void Delete(T Data) {
            Delete(Search(Data));
        }
        /// <summary>
        /// 이진 탐색 트리에서 트리의 일부인 특정 노드를 삭제합니다.
        /// </summary>
        public virtual void Delete(Node<T> Node) {
            if (Node.LeftChild != null && Node.RightChild != null) {
                Node<T> suc = GetSuccessor(Node);
                Node.Data = suc.Data;
                InternalDelete(suc);
            } else {
                InternalDelete(Node);
            }
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
        /// <summary>
        /// 이진 탐색 트리에서 특정 노드의 바로 다음 정렬 순서를 가진 노드를 구하는 내부 메서드입니다.
        /// </summary>
        protected Node<T> GetSuccessor(Node<T> Node) {
            if (Node.RightChild != null) return GetMinimum(Node.RightChild);
            Node<T> origin = Node;
            Node<T> temp = Node;
            while (true) {
                if (temp.Parent == null) return origin;
                else if (temp.Parent.LeftChild != temp) temp = temp.Parent;
                else return temp;
            }
        }
        /// <summary>
        /// 이진 탐색 트리에서 특정 노드를 루트로 하는 부분 트리의 최소값을 구하는 내부 메서드입니다.
        /// </summary>
        protected Node<T> GetMinimum(Node<T> Parent) {
            Node<T> temp = Parent;
            while (temp.LeftChild != null) temp = temp.LeftChild;
            return temp;
        }

        #region IEnumerable
        public IEnumerator<Node<T>> GetEnumerator() {
            foreach (var i in InternalInorderedEnum(Root)) yield return i;
        }
        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var i in AsInorderedEnumerable()) yield return i;
        }
        #endregion
    }
}
