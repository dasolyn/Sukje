using System;
using System.Collections.Generic;

namespace Graph {
    class MyGraph<T> {
        private List<Edge> edges = new List<Edge>();
        private Node<T>[] nodes;
        public int Size {
            get {
                return nodes.Length;
            }
        }
        public MyGraph(int size) {
            nodes = new Node<T>[size];
        }
        public T this[int index] {
            get {
                return nodes[index].Data;
            }
            set {
                nodes[index] = new Node<T> { Index = index, Data = value };
            }
        }
        public IEnumerable<T> AsBFSTraversalEnumerable(int startindex, Func<int, bool> DistanceFilter) {
            if (nodes[startindex] == null) yield break;
            Queue<int> queue = new Queue<int>();
            int[] visited = new int[Size];
            int[] distance = new int[Size];
            visited.Initialize();
            distance.Initialize();
            queue.Enqueue(startindex);
            while (queue.Count != 0) {
                int tempindex = queue.Dequeue();
                Node<T> parent = nodes[tempindex];
                Node<T> adj = parent.Next;
                while (adj != null) {
                    if (visited[adj.Index] == 0) {
                        visited[adj.Index] = 1;
                        distance[adj.Index] = distance[parent.Index] + 1;
                        if (DistanceFilter?.Invoke(distance[adj.Index]) ?? false) yield return nodes[adj.Index].Data;
                        queue.Enqueue(adj.Index);
                    }
                    adj = adj.Next;
                }
            }
        }
        public IEnumerable<T> AsTopologyOrderEnumerable() {
            int[] visited = new int[Size];
            visited.Initialize();
            Stack<T> r = new Stack<T>();
            for (int i = 0; i < Size; i++)
                if (visited[i] == 0) InternalDFSTraversal(i, visited, r);
            foreach (T i in r) yield return i;
        }
        private void InternalDFSTraversal(int index, int[] visited, Stack<T> stack) {
            visited[index] = 1;
            Node<T> temp = nodes[index];
            while (temp.Next != null) {
                if (visited[temp.Next.Index] == 0) InternalDFSTraversal(temp.Next.Index, visited, stack);
                temp = temp.Next;
            }
            stack.Push(nodes[index].Data);
        }
        public void MakeEdge(int a, int b, double weight = 1.0) {
            MakeDirectedEdge(a, b);
            MakeDirectedEdge(b, a);
            edges.Add(new Edge { a = a, b = b, weight = weight });
        }
        public bool CheckEdge(int from, int to) {
            if (from == to) return false;
            Node<T> temp = nodes[from];
            while (temp.Next != null) {
                if (temp.Index == to) return true;
                temp = temp.Next;
            }
            return false;
        }
        public void MakeDirectedEdge(int from, int to) {
            if (nodes[from] == null || nodes[to] == null) throw new ArgumentException();
            if (from == to) throw new ArgumentException();
            if (CheckEdge(from, to)) throw new ArgumentException();
            Node<T> temp = nodes[from];
            while (temp.Next != null) temp = temp.Next;
            temp.Next = new Node<T> { Index = to };
        }

        #region Kruskal
        public IEnumerable<Tuple<int, int>> AsKruskalMSTEdgeEnumerable() {
            List<int> set = new List<int>();
            List<int> setsize = new List<int>();
            for (int i = 0; i < Size; i++) {
                set.Add(i);
                setsize.Add(1);
            }
            edges.Sort((a, b) => a.weight.CompareTo(b.weight));
            foreach (Edge i in edges) {
                if (FindRoot(i.a, set) != FindRoot(i.b, set)) {
                    yield return new Tuple<int, int>(i.a, i.b);
                    UnionSet(i.a, i.b, set, setsize); 
                }
            }
        }
        private int FindRoot(int num, List<int> set) {
            int res = num;
            while (true) {
                if (res == set[res]) return res;
                else res = set[res];
            }
        }
        private void UnionSet(int u, int v, List<int> set, List<int> size) {
            // this is weighted union
            int x = FindRoot(u, set);
            int y = FindRoot(v, set);
            if (size[x] > size[y]) {
                set[y] = x;
                size[x] += size[y];
            } else {
                set[x] = y;
                size[y] += size[x];
            }
        }
        #endregion

        private class Node<TNode> {
            public int Index = -1;
            public Node<TNode> Next;
            public TNode Data;
        }
        private class Edge {
            public int a;
            public int b;
            public double weight;
        }
    }
}
