﻿using System;
using System.Collections.Generic;

namespace Huffman {
    /// <summary>
    /// 힙을 이용해 구현한 최소 우선순위 큐입니다.
    /// </summary>
    public class MinPQueue<T> {
        private List<T> datas;
        public IComparer<T> Comparer { get; private set; }
        public int Count {
            get {
                return datas.Count;
            }
        }

        #region 생성자
        public MinPQueue() {
            datas = new List<T>();
            Comparer = Comparer<T>.Default;
        }
        public MinPQueue(IComparer<T> Comparer) {
            datas = new List<T>();
            this.Comparer = Comparer;
        }
        public MinPQueue(Comparison<T> Comparison) {
            datas = new List<T>();
            Comparer = Comparer<T>.Create(Comparison);
        }
        public MinPQueue(IEnumerable<T> Source) {
            Comparer = Comparer<T>.Default;
            datas = new List<T>();
            foreach (T i in Source) Insert(i);
        }
        public MinPQueue(IEnumerable<T> Source, IComparer<T> Comparer) {
            this.Comparer = Comparer;
            datas = new List<T>();
            foreach (T i in Source) Insert(i);
        }
        public MinPQueue(IEnumerable<T> Source, Comparison<T> Comparison) {
            Comparer = Comparer<T>.Create(Comparison);
            datas = new List<T>();
            foreach (T i in Source) Insert(i);
        }
        #endregion

        public void Insert(T one) {
            datas.Add(one);
            int i = datas.Count - 1;
            while (i > 0 && Comparer.Compare(datas[parent(i)], datas[i]) > 0) {
                T temp = datas[parent(i)];
                datas[parent(i)] = datas[i];
                datas[i] = temp;
                i = parent(i);
            }
        }
        public T ExtractMin() {
            if (datas.Count == 0) throw new InvalidOperationException();
            T retval = datas[0];
            datas[0] = datas[datas.Count - 1];
            datas.RemoveAt(datas.Count - 1);
            heapify(datas);
            return retval;
        }
        private void heapify(List<T> Source) {
            int Mother = 0;
            while (true) {
                int LeftChild = 2 * (Mother + 1) - 1;
                int RightChild = 2 * (Mother + 1);
                // 자식이 없는 경우
                if (LeftChild >= Source.Count) return;
                int SmallerChild;
                if (RightChild >= Source.Count) SmallerChild = LeftChild;
                else if (Comparer.Compare(Source[LeftChild], Source[RightChild]) <= 0) SmallerChild = LeftChild;
                else SmallerChild = RightChild;
                // 자기보다 작은 자식이 없을 경우
                if (Comparer.Compare(Source[Mother], Source[SmallerChild]) <= 0) return;
                // 스왑
                T temp = Source[Mother];
                Source[Mother] = Source[SmallerChild];
                Source[SmallerChild] = temp;
                // 반복
                Mother = SmallerChild;
            }
        }
        private int parent(int index) => (index + 1) / 2 - 1;
    }
}
