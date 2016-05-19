using System;
using System.Collections.Generic;

namespace ShortestPath {
    /// <summary>
    /// 다익스트라 알고리즘을 사용하여 두 점 사이의 최단 경로를 구할 수 있는 무방향 그래프입니다.
    /// </summary>
    class DijkstraGraph<TKey, TValue> where TKey : IComparable<TKey>, IComparable, IEquatable<TKey> {
        private SortedDictionary<TKey, Node<TValue>> nodes = new SortedDictionary<TKey, Node<TValue>>();
        /// <summary>
        /// 그래프에 포함된 점의 개수입니다.
        /// </summary>
        public int Size {
            get {
                return nodes.Count;
            }
        }
        public TValue this[TKey key] {
            get {
                return nodes[key].Data;
            }
            set {
                nodes[key] = new Node<TValue>(value);
            }
        }
        /// <summary>
        /// 해당 두 점을 잇는 변의 가중치를 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 두 점을 잇는 변이 존재하지 않습니다.</exception>
        public double GetWeightOfEdge(TKey from, TKey to) {
            if (nodes.ContainsKey(from) && nodes.ContainsKey(to) && nodes[from].Sibling.ContainsKey(to)) return nodes[from].Sibling[to];
            else throw new ArgumentException();
        }
        /// <summary>
        /// 해당 두 점을 잇고 지정한 가중치를 갖는 변을 생성합니다.
        /// </summary>
        public void MakeEdge(TKey from, TKey to, double weight) {
            if (nodes.ContainsKey(from) && nodes.ContainsKey(to) && !from.Equals(to) && !CheckEdge(from, to)) {
                nodes[from].Sibling.Add(to, weight);
                nodes[to].Sibling.Add(from, weight);
            } else {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// 해당 두 점을 잇는 변이 존재하는지 검사합니다.
        /// </summary>
        public bool CheckEdge(TKey from, TKey to) {
            if (nodes.ContainsKey(from) && nodes.ContainsKey(to)) return nodes[from].Sibling.ContainsKey(to);
            else return false;
        }
        /// <summary>
        /// 해당 두 점을 잇는 가장 가까운 경로를 찾아 반환합니다.
        /// </summary>
        public PathResult ShortestPath(TKey from, TKey to) {
            SortedDictionary<TKey, double> dist = new SortedDictionary<TKey, double>();
            SortedDictionary<TKey, TKey> prev = new SortedDictionary<TKey, TKey>();
            MinPQueue<HeapElement> minqueue = new MinPQueue<HeapElement>();
            SortedSet<TKey> visited = new SortedSet<TKey>();
            dist.Add(from, 0.0);
            minqueue.Insert(new HeapElement { Index = from, Distance = dist[from] });
            bool success = false;
            while (true) {
                // 최소값 찾기
                HeapElement min;
                try {
                    do {
                        min = minqueue.ExtractMin();
                    } while (visited.Contains(min.Index));
                } catch (InvalidOperationException) {
                    break;
                }
                // 방문한 것으로 마킹
                visited.Add(min.Index);
                if (min.Index.Equals(to)) {
                    success = true;
                    break;
                }
                // 인접한 에지 relax
                foreach (var i in nodes[min.Index].Sibling) {
                    if (!dist.ContainsKey(i.Key) || dist[i.Key] > dist[min.Index] + i.Value) {
                        dist[i.Key] = dist[min.Index] + i.Value;
                        minqueue.Insert(new HeapElement { Index = i.Key, Distance = dist[i.Key] });
                        prev[i.Key] = min.Index;
                    }
                }
            }
            if (success) {
                Stack<TKey> result = new Stack<TKey>();
                TKey cur = to;
                while (true) {
                    result.Push(cur);
                    if (cur.Equals(from)) break;
                    cur = prev[cur];
                }
                return new PathResult { TotalDistance = dist[to], Route = result };
            } else {
                return new PathResult { TotalDistance = double.MaxValue, Route = null };
            }
        }

        public class PathResult {
            public double TotalDistance;
            public Stack<TKey> Route;
        }
        private class Node<TNodeValue> {
            public SortedDictionary<TKey, double> Sibling;
            public TNodeValue Data;
            public Node(TNodeValue Data) {
                this.Data = Data;
                Sibling = new SortedDictionary<TKey, double>();
            }
        }
        private struct HeapElement : IComparable<HeapElement>, IComparable, IEquatable<HeapElement> {
            public TKey Index;
            public double Distance;
            public int CompareTo(object obj) {
                HeapElement? ele = obj as HeapElement?;
                if (!ele.HasValue) throw new ArgumentException();
                else return CompareTo(ele.Value);
            }
            public int CompareTo(HeapElement other) {
                return Distance.CompareTo(other.Distance);
            }
            public bool Equals(HeapElement other) {
                return Distance.Equals(other.Distance);
            }
        }
    }
}
