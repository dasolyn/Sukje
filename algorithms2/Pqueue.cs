using System;
using System.Collections;
using System.Collections.Generic;

namespace algorithms2 {
    interface Pqueue<T> : IReadOnlyCollection<T>, ICollection, IEnumerable<T>, IEnumerable where T : IComparable<T> {
        void Insert(T one);
        T ExtractMax();
    }
}
