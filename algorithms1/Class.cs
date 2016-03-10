namespace algorithms1 {
    class Class {
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
    }
}
