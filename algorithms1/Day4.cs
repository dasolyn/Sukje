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
            int i = 0, j = 0;
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
        public static void QuickSort<T>(this IList<T> Source) where T : IComparable<T> {
            QuickSort(Source, 0, Source.Count - 1);
        }
        private static void QuickSort<T>(IList<T> Source, int StartIndex, int LastIndex) where T : IComparable<T> {
            if (StartIndex >= LastIndex) {
                return;
            } else {
                // 피벗보다 작은 원소를 왼쪽으로
                int small = StartIndex;
                for (int i = StartIndex + 1; i < LastIndex; i++) {
                    if (Source[i].CompareTo(Source[LastIndex]) < 0) {
                        T temp = Source[small];
                        Source[small] = Source[i];
                        Source[i] = temp;
                        small++;
                    }
                }
                // 피벗을 그 사이로
                {
                    T temp = Source[small];
                    Source[small] = Source[LastIndex];
                    Source[LastIndex] = temp;
                }
                // 재귀
                QuickSort(Source, StartIndex, small - 1);
                QuickSort(Source, small + 1, LastIndex);
            }
        }
        public static void JMSort<T>(this IList<T> Source) where T : IComparable<T> {
            JMSort(Source, 0, Source.Count - 1);
        }
        private static void JMSort<T>(IList<T> Source, int StartIndex, int LastIndex) where T : IComparable<T> {
            if (StartIndex >= LastIndex) {
                return;
            } else {
                int left = StartIndex, right = LastIndex - 1;
                while (left < right) {
                    while (left < right && Source[left].CompareTo(Source[LastIndex]) <= 0) left++;
                    while (left < right && Source[right].CompareTo(Source[LastIndex]) >= 0) right--;
                    if (left < right) {
                        T temp = Source[left];
                        Source[left] = Source[right];
                        Source[right] = temp;
                    }
                }
                if (Source[right].CompareTo(Source[LastIndex]) > 0) {
                    T temp = Source[right];
                    Source[right] = Source[LastIndex];
                    Source[LastIndex] = temp;
                }
                JMSort(Source, StartIndex, right);
                JMSort(Source, right + 1, LastIndex);
            }
        }
        public static void MedianQuickSort<T>(this IList<T> Source) where T : IComparable<T> {
            MedianQuickSort(Source, 0, Source.Count - 1);
        }
        private static void MedianQuickSort<T>(IList<T> Source, int StartIndex, int LastIndex) where T : IComparable<T> {
            if (StartIndex >= LastIndex) {
                return;
            } else {
                // 피벗 고르기
                int pivot;
                {
                    int mid = (StartIndex + LastIndex) / 2;
                    if (Source[StartIndex].CompareTo(Source[mid]) <= 0) {
                        if (Source[mid].CompareTo(Source[LastIndex]) <= 0) {
                            pivot = mid;
                        } else {
                            if (Source[StartIndex].CompareTo(Source[LastIndex]) <= 0) {
                                pivot = LastIndex;
                            } else {
                                pivot = StartIndex;
                            }
                        }
                    } else {
                        if (Source[mid].CompareTo(Source[LastIndex]) <= 0) {
                            if (Source[StartIndex].CompareTo(Source[LastIndex]) <= 0) {
                                pivot = StartIndex;
                            } else {
                                pivot = LastIndex;
                            }
                        } else {
                            pivot = mid;
                        }
                    }
                }
                // 피벗을 오른쪽 끝으로 옮긴다
                if (pivot != LastIndex) {
                    T temp = Source[LastIndex];
                    Source[LastIndex] = Source[pivot];
                    Source[pivot] = temp;
                }
                // 피벗보다 작은 원소를 왼쪽으로
                int small = StartIndex;
                for (int i = StartIndex + 1; i < LastIndex; i++) {
                    if (Source[i].CompareTo(Source[LastIndex]) < 0) {
                        T temp = Source[small];
                        Source[small] = Source[i];
                        Source[i] = temp;
                        small++;
                    }
                }
                // 피벗을 그 사이로
                {
                    T temp = Source[small];
                    Source[small] = Source[LastIndex];
                    Source[LastIndex] = temp;
                }
                // 재귀
                MedianQuickSort(Source, StartIndex, small - 1);
                MedianQuickSort(Source, small + 1, LastIndex);
            }
        }
        public static void HeapSort<T>(this IList<T> Source) where T : IComparable<T> {
            HeapSort_BuildHeap(Source);
            for (int i = Source.Count - 1; i > 0; i--) {
                T temp = Source[0];
                Source[0] = Source[i];
                Source[i] = temp;
                HeapSort_Heapify(Source, 0, i);
            }
        }
        private static void HeapSort_BuildHeap<T>(IList<T> Source) where T : IComparable<T> {
            for (int i = Source.Count / 2; i >= 0; i--) {
                HeapSort_Heapify(Source, i, Source.Count);
            }
        }
        private static void HeapSort_Heapify<T>(IList<T> Source, int Index, int HeapSize) where T : IComparable<T> {
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
        public static T QuickSelection<T>(this IList<T> Source, int k) where T : IComparable<T> {
            return QuickSelection(Source, 0, Source.Count - 1, k);
        }
        private static T QuickSelection<T>(IList<T> Source, int StartIndex, int LastIndex, int k) where T : IComparable<T> {
            // 피벗보다 작은 원소를 왼쪽으로
            int small = StartIndex;
            for (int i = StartIndex + 1; i < LastIndex; i++) {
                if (Source[i].CompareTo(Source[LastIndex]) < 0) {
                    T temp = Source[small];
                    Source[small] = Source[i];
                    Source[i] = temp;
                    small++;
                }
            }
            // 피벗을 그 사이로
            {
                T temp = Source[small];
                Source[small] = Source[LastIndex];
                Source[LastIndex] = temp;
            }
            // 재귀
            int smallcount = small - StartIndex;
            if (smallcount == k) return Source[small];
            else if (smallcount > k) return QuickSelection(Source, StartIndex, small - 1, k);
            else return QuickSelection(Source, small + 1, LastIndex, k - smallcount - 1);
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
