#define CLASS6

using System;

namespace algorithms1 {
    class Program {
        static void Main(string[] args) {
            Class r = new Class();
#if CLASS1
            Console.Write("Input the number (Class1): ");
            string s = Console.ReadLine();
            Console.WriteLine($"Result is {r.Class1(int.Parse(s))}");
#endif
#if CLASS2
            Console.Write("Input the number 1 (Class2): ");
            string s1 = Console.ReadLine();
            Console.Write("Input the number 2 (Class2): ");
            string s2 = Console.ReadLine();
            Console.WriteLine($"Result is {r.Class2(int.Parse(s1), int.Parse(s2))}");
#endif
#if CLASS4
            Console.Write("Input the number (Class4): ");
            string s = Console.ReadLine();
            // 피보나치 수열임
            // 순환식: R_n = R_(n-1) + R_(n-2) , R_z | z=0,1 = 1
            Console.WriteLine($"Result is {r.Class4(int.Parse(s))}");
#endif
#if CLASS5
            Console.Write("Input the number (Class5): ");
            string s = Console.ReadLine();
            // n번째 층의 공의 갯수는 1부터 시작하고 마지막 항이 n인 등차수열의 합
            // 순환식: R_n = n*(n+1)/2 + R_(n-1) , R_1 = 1
            Console.WriteLine($"Result is {r.Class5(int.Parse(s))}");
#endif
#if CLASS6
            Console.Write("Input the number (Class6): ");

#endif
            Console.ReadLine(); // 자동 종료 방지
        }
    }
}
