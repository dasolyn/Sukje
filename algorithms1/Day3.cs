using System.Collections.Generic;

namespace algorithms1 {
    class Day3 {
        public int Class1_Start(List<List<int>> board, int size) {
            // 첫번째 행
            int sum = 0;
            for (int i = 0; i < size; i++) sum += Class1_Find(board, size, 0, i);
            return sum;
        }
        public int Class1_Find(List<List<int>> board, int size, int x, int y) {
            // 다른 말과 충돌이 없는지 검사
            for (int i = 0; i < x; i++) {
                // 같은 열
                if (board[i][y] == 1) return 0;
                // 같은 대각선 1; x-- y-- 대각선
                if (y - (x - i) >= 0 && board[i][y - (x - i)] == 1) return 0;
                // 같은 대각선 2; x-- y++ 대각선
                if (y + (x - i) < size && board[i][y + (x - i)] == 1) return 0;
            }

            // 마지막 행
            if (size - 1 == x)
                return 1;

            board[x][y] = 1;
            // 다음 행 진행
            int sum = 0;
            for (int i = 0; i < size; i++) sum += Class1_Find(board, size, x + 1, i);
            board[x][y] = 0;
            return sum;
        }
    }
}
