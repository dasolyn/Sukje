using System;
using System.Collections.Generic;

namespace Graph {
    class MyGraph<T> {
        public int Size {
            get {
                return source.Length;
            }
        }
        private Node<T>[] source;
        public MyGraph(int size) {
            source = new Node<T>[size];
        }
        public T this[int index] {
            get {
                return source[index].Data;
            }
            set {
                source[index] = new Node<T> { Index = index, Data = value };
            }
        }
        public IEnumerable<T> AsBFSTraversalEnumerable(int startindex, Func<int, bool> DistanceFilter) {
            if (source[startindex] == null) yield break;
            Queue<int> queue = new Queue<int>();
            int[] visited = new int[Size];
            int[] distance = new int[Size];
            visited.Initialize();
            distance.Initialize();
            queue.Enqueue(startindex);
            while (queue.Count != 0) {
                int tempindex = queue.Dequeue();
                Node<T> parent = source[tempindex];
                Node<T> adj = parent.Next;
                while (adj != null) {
                    if (visited[adj.Index] == 0) {
                        visited[adj.Index] = 1;
                        distance[adj.Index] = distance[parent.Index] + 1;
                        if (DistanceFilter?.Invoke(distance[adj.Index]) ?? false) yield return source[adj.Index].Data;
                        queue.Enqueue(adj.Index);
                    }
                    adj = adj.Next;
                }
            }
        }
        public void MakeNewEdge(int a, int b) {
            if (source[a] == null || source[b] == null) throw new ArgumentException();
            if (a == b) throw new ArgumentException();
            if (CheckEdge(a, b)) throw new ArgumentException();
            Node<T> temp = source[a];
            while (temp.Next != null) temp = temp.Next;
            temp.Next = new Node<T> { Index = b };
            temp = source[b];
            while (temp.Next != null) temp = temp.Next;
            temp.Next = new Node<T> { Index = a };
        }
        public bool CheckEdge(int a, int b) {
            if (a == b) return false;
            Node<T> temp = source[a];
            while (temp.Next != null) {
                if (temp.Index == b) return true;
                temp = temp.Next;
            }
            return false;
        }
        private class Node<TNode> {
            public int Index = -1;
            public Node<TNode> Next;
            public TNode Data;
        }
    }
}
