using System;
using System.Collections.Generic;

namespace algorithms1 {
    class Day3 {
        #region Class1
        public int Class1_Start(int size) {
            // 첫번째 행
            int sum = 0;
            List<int> board = new List<int>();
            for (int i = 0; i < size; i++) sum += Class1_Find(board, size, i);
            return sum;
        }
        public int Class1_Find(List<int> board, int size, int col) {
            // 다른 말과 충돌이 없는지 검사
            for (int i = 0; i < board.Count; i++) {
                // 같은 열
                if (col == board[i]) return 0;
                // 같은 대각선
                if (Math.Abs(board.Count - i) == Math.Abs(col - board[i])) return 0;
            }
            // 마지막 행
            if (board.Count + 1 == size) return 1;

            // 다음 행 진행
            board.Add(col);
            int sum = 0;
            for (int i = 0; i < size; i++) sum += Class1_Find(board, size, i);
            board.RemoveAt(board.Count - 1);
            return sum;
        }
        #endregion
        #region Class2
        public int Class2(List<int> numbers, int sum) {
            List<int> subset = new List<int>();
            int count = 0;
            for (int i = 0; i < numbers.Count; i++) count += Class2_Find(numbers, i, subset, sum);
            return count;
        }
        public int Class2_Find(List<int> numbers, int pos, List<int> subset, int sum) {
            if (Class2_Sum(subset) + numbers[pos] > sum) {
                return 0;
            } else if (Class2_Sum(subset) + numbers[pos] == sum) {
                foreach (int i in subset) Console.Write($"{i} ");
                Console.WriteLine($"{numbers[pos] }");
                return 1;
            } else {
                subset.Add(numbers[pos]);
                int count = 0;
                for (int i = pos + 1; i < numbers.Count; i++) count += Class2_Find(numbers, i, subset, sum);
                subset.RemoveAt(subset.Count - 1);
                return count;
            }
        }
        public int Class2_Sum(List<int> set) {
            int sum = 0;
            foreach (int i in set) sum += i;
            return sum;
        }
        #endregion
    }
}
