using System;
using System.Collections.Generic;

namespace BinaryTree {
    public enum NodeTraversal { Left, Right }
    /// <summary>
    /// 단순한 이진 트리입니다. 순회 순서에 따라 노드를 탐색, 삽입, 제거할 수 있으며 트리를 열거하는 메서드를 제공합니다.
    /// </summary>
    public class BinaryTree<T> {
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
            if (Traversal.Length == 0) {
                if (Root != null) {
                    Node.LeftChild = Root.LeftChild;
                    Node.RightChild = Root.RightChild;
                }
                Root = Node;
                return;
            }
            Node<T> temp = Root;
            try {
                for (int i = 0; i < Traversal.Length - 1; i++) {
                    if (Traversal[i] == NodeTraversal.Left) temp = temp.LeftChild;
                    else temp = temp.RightChild;
                }
            } catch (NullReferenceException) {
                throw new ArgumentException();
            }
            if (Traversal[Traversal.Length - 1] == NodeTraversal.Left) {
                Node<T> prev = temp.LeftChild;
                if (prev != null) {
                    Node.LeftChild = prev.LeftChild;
                    Node.RightChild = prev.RightChild;
                }
                temp.LeftChild = Node;
            } else {
                Node<T> prev = temp.LeftChild;
                if (prev != null) {
                    Node.LeftChild = prev.LeftChild;
                    Node.RightChild = prev.RightChild;
                }
                temp.RightChild = Node;
            }
        }
        /// <summary>
        /// 현재 이진 트리를 중순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsInorderedEnumerable() {
            foreach (var i in InternalInorderedEnum(Root)) yield return i;
        }
        /// <summary>
        /// 현재 이진 트리를 선순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsPreorderedEnumerable() {
            foreach (var i in InternalPreorderedEnum(Root)) yield return i;
        }
        /// <summary>
        /// 현재 이진 트리를 후순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsPostorderedEnumerable() {
            foreach (var i in InternalPostorderedEnum(Root)) yield return i;
        }
        /// <summary>
        /// 현재 이진 트리를 레벨 순위로 순회하여 열거합니다.
        /// </summary>
        public IEnumerable<Node<T>> AsLevelorderedEnumerable() {
            foreach (var i in InternalLevelorderedEnum(Root)) yield return i;
        }
        /// <summary>
        /// 이진 트리에 포함된 모든 노드를 제거합니다.
        /// </summary>
        public void Clear() {
            Root = null;
        }

        /// <summary>
        /// 지정된 노드와 모든 하위 노드를 재귀적으로 순회하여 중순위로 열거하는 내부 메서드입니다.
        /// </summary>
        protected IEnumerable<Node<T>> InternalInorderedEnum(Node<T> Node) {
            if (Node == null) yield break;
            else {
                foreach (var i in InternalInorderedEnum(Node.LeftChild)) yield return i;
                yield return Node;
                foreach (var i in InternalInorderedEnum(Node.RightChild)) yield return i;
            }
        }
        /// <summary>
        /// 지정된 노드와 모든 하위 노드를 재귀적으로 순회하여 선순위로 열거하는 내부 메서드입니다.
        /// </summary>
        protected IEnumerable<Node<T>> InternalPreorderedEnum(Node<T> Node) {
            if (Node == null) yield break;
            else {
                yield return Node;
                foreach (var i in InternalPreorderedEnum(Node.LeftChild)) yield return i;
                foreach (var i in InternalPreorderedEnum(Node.RightChild)) yield return i;
            }
        }
        /// <summary>
        /// 지정된 노드와 모든 하위 노드를 재귀적으로 순회하여 후순위로 열거하는 내부 메서드입니다.
        /// </summary>
        protected IEnumerable<Node<T>> InternalPostorderedEnum(Node<T> Node) {
            if (Node == null) yield break;
            else {
                foreach (var i in InternalPostorderedEnum(Node.LeftChild)) yield return i;
                foreach (var i in InternalPostorderedEnum(Node.RightChild)) yield return i;
                yield return Node;
            }
        }
        /// <summary>
        /// 지정된 노드와 모든 하위 노드를 재귀적으로 순회하여 레벨순위로 열거하는 내부 메서드입니다.
        /// </summary>
        protected IEnumerable<Node<T>> InternalLevelorderedEnum(Node<T> Node) {
            if (Node == null) yield break;
            Queue<Node<T>> q = new Queue<Node<T>>();
            q.Enqueue(Node);
            do {
                Node<T> temp = q.Dequeue();
                yield return temp;
                if (temp.LeftChild != null) q.Enqueue(temp.LeftChild);
                if (temp.RightChild != null) q.Enqueue(temp.RightChild);
            } while (q.Count != 0);
        }

    }
}
