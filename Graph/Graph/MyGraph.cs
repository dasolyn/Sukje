using System;
using System.Collections.Generic;

namespace Graph {
    public class MyGraph<T> {
        private List<Edge> edges = new List<Edge>();
        private Dictionary<int, Node<T>> nodes = new Dictionary<int, Node<T>>();
        public int Size {
            get {
                return nodes.Count;
            }
        }
        public int EdgeCount {
            get {
                return edges.Count;
            }
        }
        public T this[int index] {
            get {
                return nodes[index].Data;
            }
            set {
                nodes[index] = new Node<T>(index, value);
            }
        }

        public double GetWeightOfEdge(int from, int to) {
            foreach (var i in nodes[from].Sibling) if (i.Index == to) return i.Weight;
            return double.MaxValue;
        }

        #region 너비우선 탐색
        public IEnumerable<T> AsBFSTraversalEnumerable(int startindex, Func<int, bool> DistanceFilter) {
            if (nodes[startindex] == null) yield break;
            Queue<int> queue = new Queue<int>();
            List<int> visited = new List<int>();
            Dictionary<int, int> distance = new Dictionary<int, int>();
            queue.Enqueue(startindex);
            distance[startindex] = 0;
            while (queue.Count != 0) {
                Node<T> parent = nodes[queue.Dequeue()];
                foreach (var i in parent.Sibling) {
                    if (!visited.Contains(i.Index)) {
                        visited.Add(i.Index);
                        distance[i.Index] = distance[parent.Index] + 1;
                        if (DistanceFilter?.Invoke(distance[i.Index]) ?? false) yield return nodes[i.Index].Data;
                        queue.Enqueue(i.Index);
                    }
                }
            }
        }
        #endregion

        #region 위상 정렬
        public Stack<T> TopologySort() {
            List<int> visited = new List<int>();
            Stack<T> r = new Stack<T>();
            for (int i = 0; i < Size; i++)
                if (!visited.Contains(i)) InternalDFSTraversal(i, visited, r);
            return r;
        }
        private void InternalDFSTraversal(int index, List<int> visited, Stack<T> resultstack) {
            visited.Add(index);
            foreach (var i in nodes[index].Sibling) if (!visited.Contains(i.Index)) InternalDFSTraversal(i.Index, visited, resultstack);
            resultstack.Push(nodes[index].Data);
        }
        #endregion

        #region 에지 생성 및 확인
        /// <summary>
        /// 양방향 변을 만듭니다.
        /// </summary>
        public void MakeEdge(int a, int b, double weight = 1.0) {
            MakeDirectedEdge(a, b, weight);
            MakeDirectedEdge(b, a, weight);
            edges.Add(new Edge { a = a, b = b, weight = weight });
        }
        /// <summary>
        /// from에서 to로 가는 변이 있는지 확인합니다.
        /// </summary>
        public bool CheckEdge(int from, int to) {
            if (from == to) return false;
            return nodes[from].Sibling.FindIndex(s => s.Index == to) != -1;
        }
        /// <summary>
        /// 방향성이 있는 변을 만듭니다. 최소 비용 생성나무를 찾고자 하는 경우 이 메서드를 사용하면 안됩니다.
        /// </summary>
        public void MakeDirectedEdge(int from, int to, double weight = 1.0) {
            if (nodes[from] == null || nodes[to] == null) throw new ArgumentException();
            if (from == to) throw new ArgumentException();
            if (CheckEdge(from, to)) throw new ArgumentException();
            nodes[from].Sibling.Add(new AdjNode<T> { Index = to, Weight = weight });
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
            while (key.Count < edges.Count) key.Add(double.MaxValue);
            key[0] = 0D;
            for (int i = 0; i < edges.Count; i++) {
                // 최소값 찾기
                double min = double.MaxValue;
                int minindex = 0;
                for (int j = 0; j < key.Count; j++) {
                    if (visited.Contains(j)) continue;
                    if (key[j] < min) {
                        min = key[j];
                        minindex = j;
                    }
                }
                yield return new Tuple<int, int>(edges[minindex].a, edges[minindex].b);
                visited.Add(minindex);
                // 찾은 에지와 인접한 에지의 key 변경
                for (int j = 0; j < key.Count; j++) {
                    if (visited.Contains(j)) continue;
                    if (edges[j].a == edges[minindex].a || edges[j].a == edges[minindex].b || edges[j].b == edges[minindex].a || edges[j].b == edges[minindex].b) {
                        if (key[j] > edges[j].weight) {
                            key[j] = edges[j].weight;
                        }
                    }
                }
            }
        }
        #endregion

        #region Dijkstra
        public DijkstraResult DijkstraShortestPath(int from, int to) {
            Dictionary<int, double> dist = new Dictionary<int, double>();
            Dictionary<int, int> prev = new Dictionary<int, int>();
            MinPqueue<DijkstraHeapElement> minqueue = new MinPqueue<DijkstraHeapElement>(); 
            List<int> visited = new List<int>();
            for (int i = 0; i < Size; i++) {
                if (i == from) {
                    dist[i] = 0.0;
                    minqueue.Insert(new DijkstraHeapElement { Index = i, Distance = dist[i] });
                } else {
                    dist[i] = double.MaxValue;
                }
            }
            bool success = false;
            while (true) {
                // 최소값 찾기
                DijkstraHeapElement min;
                try {
                    do {
                        min = minqueue.ExtractMin();
                    } while (visited.Contains(min.Index));
                } catch (InvalidOperationException) {
                    break;
                }
                // 방문한 것으로 마킹
                visited.Add(min.Index);
                if (min.Index == to) {
                    success = true;
                    break;
                }
                // 인접한 에지 relax
                foreach (var i in nodes[min.Index].Sibling) {
                    if (dist[i.Index] > dist[min.Index] + i.Weight) {
                        dist[i.Index] = dist[min.Index] + i.Weight;
                        minqueue.Insert(new DijkstraHeapElement { Index = i.Index, Distance = dist[i.Index] });
                        prev[i.Index] = min.Index;
                    }
                }
            }
            if (success) {
                Stack<int> result = new Stack<int>();
                int cur = to;
                while (true) {
                    result.Push(cur);
                    if (cur == from) break;
                    cur = prev[cur];
                }
                return new DijkstraResult { TotalDistance = dist[to], Route = result };
            } else {
                return new DijkstraResult { TotalDistance = double.MaxValue, Route = null };
            }
        }
        #endregion

        public class DijkstraResult {
            public double TotalDistance;
            public Stack<int> Route;
        }
        private class Node<TNode> {
            public int Index;
            public List<AdjNode<TNode>> Sibling;
            public TNode Data;
            public Node(int Index, TNode Data) {
                this.Index = Index;
                this.Data = Data;
                Sibling = new List<AdjNode<TNode>>();
            }
        }
        private struct AdjNode<TNode> {
            public int Index;
            public double Weight;
        }
        private struct Edge {
            public int a;
            public int b;
            public double weight;
        }
        private struct DijkstraHeapElement : IComparable<DijkstraHeapElement> {
            public int Index;
            public double Distance;
            public int CompareTo(DijkstraHeapElement other) {
                return Distance.CompareTo(other.Distance);
            }
        }
    }
}
