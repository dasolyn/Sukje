using System;
using System.Collections.Generic;

namespace algorithms1 {
    class Day4<T> where T : IComparable<T> {
        public List<T> Data { get; set; } = new List<T>();
        /// <summary>
        /// 거품 정렬입니다.
        /// </summary>
        public void BubbleSort() {
            for (int i = 0; i < Data.Count - 1; i++) {
                for (int j = i + 1; j < Data.Count; j++) {
                    // Data[i]가 Data[j]보다 크면 스왑
                    if (Data[i].CompareTo(Data[j]) > 0) {
                        T temp = Data[i];
                        Data[i] = Data[j];
                        Data[j] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 선택 정렬입니다.
        /// </summary>
        public void SelectionSort() {
            for (int i = Data.Count - 1; i > 0; i--) {
                // 최대값 찾기
                int maxindex = 0;
                T maxvalue = Data[0];
                for (int j = 1; j <= i; j++) {
                    if (Data[j].CompareTo(maxvalue) > 0) {
                        maxvalue = Data[j];
                        maxindex = j;
                    }
                }
                // 그 값이 가장 오른쪽 값이 아니면 스왑
                if (i != maxindex) {
                    T temp = Data[i];
                    Data[i] = Data[maxindex];
                    Data[maxindex] = temp;
                }
            }
        }
        /// <summary>
        /// 삽입 정렬입니다.
        /// </summary>
        public void InsertionSort() {
            for (int i = 1; i < Data.Count; i++) {
                // 삽입할 자리 찾기
                int newindex = i;
                for (int j = 0; j < i; j++) {
                    if (Data[j].CompareTo(Data[i]) > 0) {
                        newindex = j;
                        break;
                    }
                }
                // 삽입
                T temp = Data[i];
                for (int j = i - 1; j >= newindex; j--) {
                    Data[j + 1] = Data[j];
                }
                Data[newindex] = temp;
            }
        }
        /// <summary>
        /// 합병 정렬입니다.
        /// </summary>
        public void MergeSort() {
            Data = MergeSort_Divide(Data);
        }
        private List<T> MergeSort_Divide(List<T> DividedList) {
            if (DividedList.Count == 1) {
                return DividedList;
            } else {
                int mid = DividedList.Count / 2;
                List<T> First = DividedList.GetRange(0, mid);
                List<T> Second = DividedList.GetRange(mid, DividedList.Count - mid);
                List<T> SortedFirst = MergeSort_Divide(First);
                List<T> SortedSecond = MergeSort_Divide(Second);
                return MergeSort_Merge(SortedFirst, SortedSecond);
            }
        }
        private List<T> MergeSort_Merge(List<T> FirstPart, List<T> SecondPart) {
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
        /// <summary>
        /// 리스트에 들어있는 값을 순서대로 출력합니다.
        /// </summary>
        public void PrintData() {
            for (int i = 0; i < Data.Count; i++) {
                Console.Write($"{Data[i]} ");
                if ((i + 1) % 10 == 0) Console.WriteLine();
            }
            if (Data.Count % 10 != 0) Console.WriteLine();
        }
    }
}
