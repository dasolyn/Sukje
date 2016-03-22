//#define SuppressDay4

using System;
using System.Collections.Generic;

namespace algorithms1 {
#if !SuppressDay4
    public static class Day4 {
        public static void BubbleSort<T>(this IList<T> Source) where T : IComparable<T> {
            for (int i = 0; i < Source.Count - 1; i++) {
                for (int j = i + 1; j < Source.Count; j++) {
                    if (Source[i].CompareTo(Source[j]) > 0) {
                        T temp = Source[i];
                        Source[i] = Source[j];
                        Source[j] = temp;
                    }
                }
            }
        }
        public static void SelectionSort<T>(this IList<T> Source) where T : IComparable<T> {
            for (int i = Source.Count - 1; i > 0; i--) {
                // 최대값 찾기
                int maxindex = 0;
                T maxvalue = Source[0];
                for (int j = 1; j <= i; j++) {
                    if (Source[j].CompareTo(maxvalue) > 0) {
                        maxvalue = Source[j];
                        maxindex = j;
                    }
                }
                // 그 값이 가장 오른쪽 값이 아니면 스왑
                if (i != maxindex) {
                    T temp = Source[i];
                    Source[i] = Source[maxindex];
                    Source[maxindex] = temp;
                }
            }
        }
        public static void InsertionSort<T>(this IList<T> Source) where T : IComparable<T> {
            for (int i = 1; i < Source.Count; i++) {
                // 삽입할 자리 찾기
                int newindex = i;
                for (int j = 0; j < i; j++) {
                    if (Source[j].CompareTo(Source[i]) > 0) {
                        newindex = j;
                        break;
                    }
                }
                // 삽입
                T temp = Source[i];
                for (int j = i - 1; j >= newindex; j--) {
                    Source[j + 1] = Source[j];
                }
                Source[newindex] = temp;
            }
        }
        public static void MergeSort<T>(this IList<T> Source) where T : IComparable<T> {
            IList<T> sorted = MergeSort_Divide(Source);
            Source.Clear();
            foreach (T i in sorted) {
                Source.Add(i);
            }
        }
        private static IList<T> MergeSort_Divide<T>(IList<T> source) where T : IComparable<T> {
            if (source.Count == 1) {
                return source;
            } else {
                int mid = source.Count / 2;
                List<T> First = new List<T>();
                List<T> Second = new List<T>();
                for (int i = 0; i < source.Count; i++) {
                    if (i < mid) First.Add(source[i]);
                    else Second.Add(source[i]);
                }
                IList<T> SortedFirst = MergeSort_Divide(First);
                IList<T> SortedSecond = MergeSort_Divide(Second);
                return MergeSort_Merge(SortedFirst, SortedSecond);
            }
        }
        private static IList<T> MergeSort_Merge<T>(IList<T> FirstPart, IList<T> SecondPart) where T : IComparable<T> {
            int i = 0;
            int j = 0;
            List<T> merged = new List<T>();
            while (true) {
                if (FirstPart[i].CompareTo(SecondPart[j]) <= 0) {
                    merged.Add(FirstPart[i]);
                    i++;
                } else {
                    merged.Add(SecondPart[j]);
                    j++;
                }
                if (i == FirstPart.Count || j == SecondPart.Count) break;
            }
            for (; i < FirstPart.Count; i++) merged.Add(FirstPart[i]);
            for (; j < SecondPart.Count; j++) merged.Add(SecondPart[j]);
            return merged;
        }
        public static void Print<T>(this IEnumerable<T> Source) {
            int index = 0;
            foreach (T i in Source) {
                Console.Write($"{i} ");
                index++;
                if (index % 10 == 0) Console.WriteLine();
            }
            if (index % 10 != 0) Console.WriteLine();
        }
    }
#endif
}
