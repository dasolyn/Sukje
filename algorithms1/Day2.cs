using System.Collections.Generic;

namespace algorithms1 {
    class Day2 {
        #region Class1
        public bool Class1(List<List<int>> ints, int startx, int starty, int endx, int endy, int size) {
            // 범위 벗어남
            if (startx < 0 || startx >= size) return false;
            if (starty < 0 || starty >= size) return false;
            // 빈 자리 아님
            if (ints[startx][starty] != 0) return false;
            // 도착
            if (startx == endx && starty == endy) return true;

            ints[startx][starty] = 2;

            // x--
            if (startx - 1 >= 0 && ints[startx - 1][starty] != 1) {
                if (Class1(ints, startx - 2, starty - 1, endx, endy, size) || Class1(ints, startx - 2, starty + 1, endx, endy, size)) return true;
            }
            // x++
            if (startx + 1 < size && ints[startx + 1][starty] != 1) {
                if (Class1(ints, startx + 2, starty - 1, endx, endy, size) || Class1(ints, startx + 2, starty + 1, endx, endy, size)) return true;
            }
            // y--
            if (starty - 1 >= 0 && ints[startx][starty - 1] != 1) {
                if (Class1(ints, startx - 1, starty - 2, endx, endy, size) || Class1(ints, startx + 1, starty - 2, endx, endy, size)) return true;
            }
            // y++
            if (starty + 1 < size && ints[startx][starty + 1] != 1) {
                if (Class1(ints, startx - 1, starty + 2, endx, endy, size) || Class1(ints, startx + 1, starty + 2, endx, endy, size)) return true;
            }
            return false;
        }
        #endregion
        #region Class2
        public bool Class2(List<List<int>> ints, int startx, int starty, int endx, int endy, int size) {
            // 범위 벗어남
            if (startx < 0 || startx >= size) return false;
            if (starty < 0 || starty >= size) return false;
            // 빈 자리 아님
            if (ints[startx][starty] != 0) return false;
            // 도착
            if (startx == endx && starty == endy) return true;

            ints[startx][starty] = 2;

            // x++
            bool ok = false;
            if (startx + 1 < size) {
                for (int i = startx + 1; i < size; i++) {
                    if (!ok && ints[i][starty] == 1) { ok = true; continue; }
                    if (ok && ints[i][starty] == 0 && Class2(ints, i, starty, endx, endy, size)) return true;
                }
            }
            // x--
            ok = false;
            if (startx - 1 >= 0) {
                for (int i = startx - 1; i >= 0; i--) {
                    if (!ok && ints[i][starty] == 1) { ok = true; continue; }
                    if (ok && ints[i][starty] == 0 && Class2(ints, i, starty, endx, endy, size)) return true;
                }
            }
            // y++
            ok = false;
            if (starty + 1 < size) {
                for (int i = starty + 1; i < size; i++) {
                    if (!ok && ints[startx][i] == 1) { ok = true; continue; }
                    if (ok && ints[startx][i] == 0 && Class2(ints, startx, i, endx, endy, size)) return true;
                }
            }
            // y--
            ok = false;
            if (starty - 1 >= 0) {
                for (int i = starty - 1; i >= 0; i--) {
                    if (!ok && ints[startx][i] == 1) { ok = true; continue; }
                    if (ok && ints[startx][i] == 0 && Class2(ints, startx, i, endx, endy, size)) return true;
                }
            }

            return false;
        }
        #endregion
        #region Class3
        public List<int> Class3(List<List<int>> ints, int size) {
            List<int> res = new List<int>();
            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if (ints[i][j] == 1) {
                        int r = Class3_CountBlob(ints, i, j, size);
                        if (r > 0) res.Add(r);
                    }
                }
            }
            return res;
        }
        private int Class3_CountBlob(List<List<int>> ints, int x, int y, int size) {
            if (x < 0 || y < 0 || x >= size || y >= size) return 0;
            if (ints[x][y] != 1) return 0;

            ints[x][y] = 2;

            return 1
                + Class3_CountBlob(ints, x + 1, y, size)
                + Class3_CountBlob(ints, x + 1, y + 1, size)
                + Class3_CountBlob(ints, x, y + 1, size)
                + Class3_CountBlob(ints, x - 1, y, size)
                + Class3_CountBlob(ints, x - 1, y - 1, size)
                + Class3_CountBlob(ints, x, y - 1, size)
                + Class3_CountBlob(ints, x + 1, y - 1, size)
                + Class3_CountBlob(ints, x - 1, y + 1, size);
        }
        #endregion
    }
}
