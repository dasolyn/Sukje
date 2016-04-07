using System;
using System.Collections.Generic;

namespace BinaryTree {
    public enum NodeTraversal { Left, Right }
    class BinaryTree<T> {
        protected Node<T> Root = null;
        /// <summary>
        /// 해당 순서에 따라 이진 트리를 탐색하여 결과 노드를 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 위치의 노드가 존재하지 않습니다.</exception>
        public Node<T> GetNodeByTraversal(params NodeTraversal[] Traversal) {
            try {
                Node<T> temp = Root;
                foreach (NodeTraversal i in Traversal) {
                    if (i == NodeTraversal.Left) temp = temp.LeftChild;
                    else temp = temp.RightChild;
                }
                if (temp == null) throw new ArgumentException();
                return temp;
            } catch (NullReferenceException) {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// 이진 트리의 해당 위치에 노드를 삽입합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 위치에 접근할 수 없습니다. 부모 노드가 없는 위치에 노드를 삽입할려고 할 때 발생합니다.</exception>
        public void SetNodeByTraversal(Node<T> Node, params NodeTraversal[] Traversal) {
            try {
                if (Traversal.Length == 0) {
                    Root = Node;
                    return;
                }
                Node<T> temp = Root;
                Node<T> parent = null;
                foreach (NodeTraversal i in Traversal) {
                    if (i == NodeTraversal.Left) {
                        parent = temp;
                        temp = temp.LeftChild;
                    } else {
                        parent = temp;
                        temp = temp.RightChild;
                    }
                }
                if (Traversal[Traversal.Length - 1] == NodeTraversal.Left) parent.LeftChild = Node;
                else parent.RightChild = Node;
            } catch (NullReferenceException) {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// 현재 이진 트리를 중순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsInorderedEnumerable() {
            foreach (var i in InternalInorderedEnum(Root)) yield return i;
        }
        private IEnumerable<Node<T>> InternalInorderedEnum(Node<T> Parent) {
            if (Parent == null) yield break;
            else {
                foreach (var i in InternalInorderedEnum(Parent.LeftChild)) yield return i;
                yield return Parent;
                foreach (var i in InternalInorderedEnum(Parent.RightChild)) yield return i;
            }
        }
        /// <summary>
        /// 현재 이진 트리를 선순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsPreorderedEnumerable() {
            foreach (var i in InternalPreorderedEnum(Root)) yield return i;
        }
        private IEnumerable<Node<T>> InternalPreorderedEnum(Node<T> Parent) {
            if (Parent == null) yield break;
            else {
                yield return Parent;
                foreach (var i in InternalPreorderedEnum(Parent.LeftChild)) yield return i;
                foreach (var i in InternalPreorderedEnum(Parent.RightChild)) yield return i;
            }
        }
        /// <summary>
        /// 현재 이진 트리를 후순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsPostorderedEnumerable() {
            foreach (var i in InternalPostorderedEnum(Root)) yield return i;
        }
        private IEnumerable<Node<T>> InternalPostorderedEnum(Node<T> Parent) {
            if (Parent == null) yield break;
            else {
                foreach (var i in InternalPostorderedEnum(Parent.LeftChild)) yield return i;
                foreach (var i in InternalPostorderedEnum(Parent.RightChild)) yield return i;
                yield return Parent;
            }
        }
        /// <summary>
        /// 현재 이진 트리를 레벨 순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsLevelOrderedEnumerable() {
            if (Root == null) yield break;
            Queue<Node<T>> q = new Queue<Node<T>>();
            q.Enqueue(Root);
            do {
                Node<T> temp = q.Dequeue();
                yield return temp;
                if (temp.LeftChild != null) q.Enqueue(temp.LeftChild);
                if (temp.RightChild != null) q.Enqueue(temp.RightChild);
            } while (q.Count != 0);
        }
        /// <summary>
        /// 이진 트리의 모든 노드를 초기화합니다.
        /// </summary>
        public void Clear() {
            Root = null;
        }
    }
}
