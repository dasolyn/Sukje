using System;
using System.Collections.Generic;

namespace algorithms2 {
    class HeapPqueue<T> : Pqueue<T> where T : IComparable<T> {
        private List<T> datas = new List<T>();

        public void Insert(T one) {
            datas.Add(one);
            int i = datas.Count - 1;
            while (i > 0 && datas[parent(i)].CompareTo(datas[i]) < 0) {
                T temp = datas[parent(i)];
                datas[parent(i)] = datas[i];
                datas[i] = temp;
                i = parent(i);
            }
        }
        public T ExtractMax() {
            if (datas.Count == 0) throw new InvalidOperationException();

            T retval = datas[0];
            datas[0] = datas[datas.Count - 1];
            datas.RemoveAt(datas.Count - 1);
            heapify(datas, 0, datas.Count);
            return retval;
        }

        private void heapify(IList<T> Source, int Index, int HeapSize) {
            int Mother = Index;
            while (true) {
                int LeftChild = 2 * (Mother + 1) - 1;
                int RightChild = 2 * (Mother + 1);
                // 자식이 없는 경우
                if (LeftChild >= HeapSize) return;
                int BiggerChild;
                if (RightChild >= HeapSize) BiggerChild = LeftChild;
                else if (Source[LeftChild].CompareTo(Source[RightChild]) > 0) BiggerChild = LeftChild;
                else BiggerChild = RightChild;
                // 자기보다 큰 자식이 없을 경우
                if (Source[Mother].CompareTo(Source[BiggerChild]) >= 0) return;
                // 스왑
                T temp = Source[Mother];
                Source[Mother] = Source[BiggerChild];
                Source[BiggerChild] = temp;
                // 반복
                Mother = BiggerChild;
            }
        }
        private int parent(int index) {
            return (index + 1) / 2 - 1;
        }
    }
}
