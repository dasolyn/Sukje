using System;
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
        public int Class7_Compare(string s1, string s2) {
            if (s1.Length > 0 && s2.Length == 0) return 1;
            else if (s1.Length == 0 && s2.Length > 0) return -1;
            else if (s1.Length == 0 && s2.Length == 0) return 0;

            if (s1[0] < s2[0]) return -1;
            else if (s1[0] > s2[0]) return 1;
            else return Class7_Compare(s1.Substring(1), s2.Substring(1));
        }
        #endregion
        #region Class8
        public int Class8(IList<int> data, int left, int right, int sum) {
            if (left == right) return 0;
            return Class8(data, left + 1, right, sum) + Class8_Right(data, left, right, sum);
        }
        public int Class8_Right(IList<int> data, int left, int right, int sum) {
            if (left == right) return 0;
            if (data[left] + data[right] < sum) return 0;
            if (data[left] + data[right] == sum) return 1; // 검색 성공
            else return Class8_Right(data, left, right - 1, sum);
        }
        #endregion
    }
}
