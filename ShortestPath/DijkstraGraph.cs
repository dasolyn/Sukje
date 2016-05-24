using System;
using System.Collections.Generic;

namespace ShortestPath {
    /// <summary>
    /// 다익스트라 알고리즘을 사용하여 두 점 사이의 최단 경로를 구할 수 있는 무방향 그래프입니다.
    /// </summary>
    class DijkstraGraph<TKey, TValue> where TKey : IComparable<TKey>, IComparable, IEquatable<TKey> {
        private SortedDictionary<TKey, Node> nodes = new SortedDictionary<TKey, Node>();
        /// <summary>
        /// 그래프에 포함된 꼭지점의 개수입니다.
        /// </summary>
        public int Size {
            get {
                return nodes.Count;
            }
        }
        /// <summary>
        /// get: 지정한 키를 가진 꼭지점의 데이터를 반환합니다.
        /// set: 지정한 키를 가진 꼭지점을 추가하거나 기존 꼭지점의 데이터를 변경합니다.
        /// </summary>
        /// <exception cref="KeyNotFoundException">get시에 지정한 키를 가진 꼭지점이 없습니다.</exception>
        public TValue this[TKey key] {
            get {
                return nodes[key].Data;
            }
            set {
                nodes[key] = new Node(value);
            }
        }
        /// <summary>
        /// 해당 두 점을 잇는 변의 가중치를 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">지정한 두 점이 존재하지 않거나, 해당 두 점을 잇는 변이 존재하지 않습니다.</exception>
        public double GetWeightOfEdge(TKey from, TKey to) {
            if (nodes.ContainsKey(from) && nodes.ContainsKey(to) && nodes[from].Sibling.ContainsKey(to)) return nodes[from].Sibling[to];
            else throw new ArgumentException();
        }
        /// <summary>
        /// 해당 두 점을 잇고 지정한 가중치를 갖는 변을 생성합니다.
        /// </summary>
        /// <exception cref="ArgumentException">지정한 두 점이 존재하지 않거나, 이미 해당 두 점을 잇는 변이 존재합니다.</exception>
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
        /// <returns>경로가 존재하지 않을 경우 <c>Route</c>는 <c>null</c>이 됩니다.</returns>
        public PathResult ShortestPath(TKey from, TKey to) {
            SortedDictionary<TKey, double> dist = new SortedDictionary<TKey, double>();
            SortedDictionary<TKey, TKey> prev = new SortedDictionary<TKey, TKey>();
            MinPQueue minqueue = new MinPQueue();
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

        /// <summary>
        /// 가장 가까운 경로를 담는 클래스입니다.
        /// </summary>
        public class PathResult {
            /// <summary>
            /// 경로 상의 모든 변의 가중치 합계입니다.
            /// </summary>
            public double TotalDistance;
            /// <summary>
            /// 경로 상의 모든 꼭지점을 출발점에서부터 차례대로 나열할 수 있는 스택입니다.
            /// </summary>
            public Stack<TKey> Route;
        }
        private class Node {
            public SortedDictionary<TKey, double> Sibling;
            public TValue Data;
            public Node(TValue Data) {
                this.Data = Data;
                Sibling = new SortedDictionary<TKey, double>();
            }
        }
        private struct HeapElement : IComparable<HeapElement> {
            public TKey Index;
            public double Distance;
            public int CompareTo(HeapElement other) {
                return Distance.CompareTo(other.Distance);
            }
        }
        private class MinPQueue {
            private List<HeapElement> datas = new List<HeapElement>();
            public void Insert(HeapElement one) {
                datas.Add(one);
                int i = datas.Count - 1;
                while (i > 0 && datas[parent(i)].CompareTo(datas[i]) > 0) {
                    HeapElement temp = datas[parent(i)];
                    datas[parent(i)] = datas[i];
                    datas[i] = temp;
                    i = parent(i);
                }
            }
            public HeapElement ExtractMin() {
                if (datas.Count == 0) throw new InvalidOperationException();
                HeapElement retval = datas[0];
                datas[0] = datas[datas.Count - 1];
                datas.RemoveAt(datas.Count - 1);
                heapify(datas, 0, datas.Count);
                return retval;
            }
            private void heapify(List<HeapElement> Source, int Index, int HeapSize) {
                int Mother = Index;
                while (true) {
                    int LeftChild = 2 * (Mother + 1) - 1;
                    int RightChild = 2 * (Mother + 1);
                    // 자식이 없는 경우
                    if (LeftChild >= HeapSize) return;
                    int SmallerChild;
                    if (RightChild >= HeapSize) SmallerChild = LeftChild;
                    else if (Source[LeftChild].CompareTo(Source[RightChild]) < 0) SmallerChild = LeftChild;
                    else SmallerChild = RightChild;
                    // 자기보다 작은 자식이 없을 경우
                    if (Source[Mother].CompareTo(Source[SmallerChild]) <= 0) return;
                    // 스왑
                    HeapElement temp = Source[Mother];
                    Source[Mother] = Source[SmallerChild];
                    Source[SmallerChild] = temp;
                    // 반복
                    Mother = SmallerChild;
                }
            }
            private int parent(int index) {
                return (index + 1) / 2 - 1;
            }
        }
    }
}
