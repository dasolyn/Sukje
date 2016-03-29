using System;

namespace algorithms2 {
    interface Pqueue<T> where T : IComparable<T> {
        void Insert(T one);
        T ExtractMax();
    }
}
