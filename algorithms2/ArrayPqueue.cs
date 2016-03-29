using System;
using System.Collections.Generic;

namespace algorithms2 {
    class ArrayPqueue<T> : Pqueue<T> where T : IComparable<T> {
        private List<T> datas = new List<T>();

        public void Insert(T one) {
            datas.Add(one);
        }
        public T ExtractMax() {
            if (datas.Count == 0) throw new InvalidOperationException();

            int maxindex = 0;
            T maxvalue = default(T);
            for (int i = 0; i < datas.Count; i++) {
                if (i == 0) {
                    maxindex = 0;
                    maxvalue = datas[0];
                } else if (datas[i].CompareTo(maxvalue) > 0) {
                    maxindex = i;
                    maxvalue = datas[i];
                }
            }
            T retval = datas[maxindex];
            datas[maxindex] = datas[datas.Count - 1];
            datas.RemoveAt(datas.Count - 1);
            return retval;
        }
    }
}
