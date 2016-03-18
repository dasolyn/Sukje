using System;
using System.Collections.Generic;

namespace algorithms1 {
    class Day3 {
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
            if (board.Count - 1 == size) return 1;

            // 다음 행 진행
            board.Add(col);
            int sum = 0;
            for (int i = 0; i < size; i++) sum += Class1_Find(board, size, i);
            board.RemoveAt(board.Count - 1);
            return sum;
        }
    }
}
