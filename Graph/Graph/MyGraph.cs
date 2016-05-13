﻿using System;
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

        #region 너비우선 탐색
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
        #endregion

        #region 위상 정렬
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
        #endregion

        #region 에지 생성 및 확인
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
        #endregion

        #region Kruskal
        public IEnumerable<Tuple<int, int>> AsKruskalMSTEdgeEnumerable() {
            if (edges.Count == 0) yield break;
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

        #region Prim
        public IEnumerable<Tuple<int, int>> AsPrimMSTEdgeEnumerable() {
            if (edges.Count == 0) yield break;
            List<int> visited = new List<int>();
            List<double> key = new List<double>();
            while (visited.Count < edges.Count) {
                visited.Add(0);
                key.Add(double.MaxValue);
            }
            key[0] = 0D;
            for (int i = 0; i < edges.Count; i++) {
                // 최소값 찾기
                double min = double.MaxValue;
                int minindex = 0;
                for (int j = 0; j < key.Count; j++) {
                    if (visited[j] == 1) continue;
                    if (key[j] < min) {
                        min = key[j];
                        minindex = j;
                    }
                }
                yield return new Tuple<int, int>(edges[minindex].a, edges[minindex].b);
                visited[minindex] = 1;
                // 찾은 에지와 인접한 에지의 key 변경
                for (int j = 0; j < key.Count; j++) {
                    if (visited[j] == 1) continue;
                    if (edges[j].a == edges[minindex].a || edges[j].a == edges[minindex].b || edges[j].b == edges[minindex].a || edges[j].b == edges[minindex].b) {
                        if (key[j] > edges[j].weight) {
                            key[j] = edges[j].weight;
                        }
                    }
                }
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
