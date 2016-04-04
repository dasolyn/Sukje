using System;
using System.Collections.Generic;

namespace BinaryTree {
    public enum NodeTraversal { Left, Right }
    class BinaryTree<T> {
        private Node<T> Root = null;
        /// <summary>
        /// 해당 순서에 따라 이진 트리를 탐색하여 결과 노드를 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 위치의 노드가 존재하지 않습니다.</exception>
        public Node<T> GetNodeByTraversal(params NodeTraversal[] Traversal) {
            try {
                Node<T> temp = Root;
                foreach (NodeTraversal i in Traversal) {
                    if (i == NodeTraversal.Left) temp = temp.LeftSibling;
                    else temp = temp.RightSibling;
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
        /// <exception cref="ArgumentException">해당 위치의 노드가 존재하지 않습니다.</exception>
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
                        temp = temp.LeftSibling;
                    } else {
                        parent = temp;
                        temp = temp.RightSibling;
                    }
                }
                if (Traversal[Traversal.Length - 1] == NodeTraversal.Left) parent.LeftSibling = Node;
                else parent.RightSibling = Node;
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
                foreach (var i in InternalInorderedEnum(Parent.LeftSibling)) yield return i;
                yield return Parent;
                foreach (var i in InternalInorderedEnum(Parent.RightSibling)) yield return i;
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
                foreach (var i in InternalPreorderedEnum(Parent.LeftSibling)) yield return i;
                foreach (var i in InternalPreorderedEnum(Parent.RightSibling)) yield return i;
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
                foreach (var i in InternalPostorderedEnum(Parent.LeftSibling)) yield return i;
                foreach (var i in InternalPostorderedEnum(Parent.RightSibling)) yield return i;
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
            while (q.Count != 0) {
                Node<T> temp = q.Dequeue();
                yield return temp;
                if (temp.LeftSibling != null) q.Enqueue(temp.LeftSibling);
                if (temp.RightSibling != null) q.Enqueue(temp.RightSibling);
            }
        }
    }
}
