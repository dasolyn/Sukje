using System.Collections.Generic;

namespace algorithms1 {
    class Day1 {
        #region Class1
        public int Class1(int n) {
            // base
            if (n % 2 == 0 || n < 1) return 0;
            // recursion
            return Class1(n - 2) + Class1(n - 1) + n;
        }
        #endregion
        #region Class2
        public int Class2(int n, int k) {
            // base
            if (k == 0) return 1;
            if (k == 1) return n;
            if (k > n) return 0;
            // recursion
            return Class2(n - 1, k) + Class2(n - 1, k - 1);
        }
        #endregion
        #region Class4
        public int Class4(int n) {
            // base
            if (n <= 1) return 1;
            // recursion
            return Class4(n - 1) + Class4(n - 2);
        }
        #endregion
        #region Class5
        public int Class5(int n) {
            // base
            if (n == 1) return 1;
            // recursion
            return n * (n + 1) / 2 + Class5(n - 1);
        }
        #endregion
        #region Class6
        public bool Class6(string s) {
            // base
            if (s.Length <= 1) return true;
            // recursion
            return (s[0] == s[s.Length - 1]) && Class6(s.Substring(1, s.Length - 2));
        }
        #endregion
        #region Class7
        public int Class7_Compare(string s1, string s2, int pos1, int pos2) {
            if (s1.Length > pos1 && s2.Length == pos2) return 1;
            else if (s1.Length == pos1 && s2.Length > pos2) return -1;
            else if (s1.Length == pos1 && s2.Length == pos2) return 0;

            if (s1[pos1] < s2[pos2]) return -1;
            else if (s1[pos1] > s2[pos2]) return 1;
            else return Class7_Compare(s1, s2, pos1 + 1, pos2 + 1);
        }
        #endregion
        #region Class8
        public int Class8(IList<int> data, int left, int right, int sum) {
            if (left >= right) return 0;

            int temp = data[left] + data[right];
            if (temp == sum) return 1 + Class8(data, left + 1, right - 1, sum);
            else if (temp > sum) return Class8(data, left, right - 1, sum);
            else return Class8(data, left + 1, right, sum);
        }
        #endregion
        #region Class9
        public int Class9(IList<int> data, int left, int right, int n) {
            if (left > right) {
                if (right < 0) return -1;
                else if (data[right] < n) return right;
                else return left;
            } else {
                int m = (left + right) / 2;
                if (data[m] == n) return m;
                else if (data[m] > n) return Class9(data, left, m - 1, n);
                else return Class9(data, m + 1, right, n);
            }
        }
        #endregion
        #region Class10
        public int Class10(IList<int> data1, IList<int> data2, int pos1, int pos2) {
            if (pos1 >= data1.Count || pos2 >= data2.Count) return 0;
            if (data1[pos1] == data2[pos2]) return 1 + Class10(data1, data2, pos1 + 1, pos2 + 1);
            else if (data1[pos1] < data2[pos2]) return Class10(data1, data2, pos1 + 1, pos2);
            else return Class10(data1, data2, pos1, pos2 + 1);
        }
        #endregion
    }
}

