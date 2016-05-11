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
        public IEnumerable<T> AsTopologyOrderEnumerable() {
            int[] visited = new int[Size];
            visited.Initialize();
            LinkedList<T> r = new LinkedList<T>();
            for (int i = 0; i < Size; i++)
                if (visited[i] == 0) InternalDFSTraversal(i, visited, r);
            foreach (T i in r) yield return i;
        }
        private void InternalDFSTraversal(int index, int[] visited, LinkedList<T> linkedlist) {
            visited[index] = 1;
            Node<T> temp = source[index];
            while (temp.Next != null) {
                if (visited[temp.Next.Index] == 0) InternalDFSTraversal(temp.Next.Index, visited, linkedlist);
                temp = temp.Next;
            }
            linkedlist.AddFirst(source[index].Data);
        }
        public void MakeEdge(int a, int b) {
            MakeDirectedEdge(a, b);
            MakeDirectedEdge(b, a);
        }
        public bool CheckEdge(int from, int to) {
            if (from == to) return false;
            Node<T> temp = source[from];
            while (temp.Next != null) {
                if (temp.Index == to) return true;
                temp = temp.Next;
            }
            return false;
        }
        public void MakeDirectedEdge(int from, int to) {
            if (source[from] == null || source[to] == null) throw new ArgumentException();
            if (from == to) throw new ArgumentException();
            if (CheckEdge(from, to)) throw new ArgumentException();
            Node<T> temp = source[from];
            while (temp.Next != null) temp = temp.Next;
            temp.Next = new Node<T> { Index = to };
        }
        private class Node<TNode> {
            public int Index = -1;
            public Node<TNode> Next;
            public TNode Data;
        }
    }
}
