#define Day1
#define Class8

using System;
using System.Collections.Generic;
using System.Linq;

namespace algorithms1 {
    class Program {
        static void Main(string[] args) {
#if Day1
            Day1 r = new Day1();
#if Class1
            Console.Write("Input the number (Class1): ");
            int s = int.Parse(Console.ReadLine());
            Console.WriteLine($"Result is {r.Class1(s)}");
#endif
#if Class2
            Console.Write("Input the number 1 (Class2): ");
            int s1 = int.Parse(Console.ReadLine());
            Console.Write("Input the number 2 (Class2): ");
            int s2 = int.Parse(Console.ReadLine());
            Console.WriteLine($"Result is {r.Class2(s1, s2)}");
#endif
#if Class4
            Console.Write("Input the number (Class4): ");
            int s = int.Parse(Console.ReadLine());
            // 피보나치 수열임
            // 순환식: R_n = R_(n-1) + R_(n-2) , R_z | z=0,1 = 1
            Console.WriteLine($"Result is {r.Class4(s)}");
#endif
#if Class5
            Console.Write("Input the number (Class5): ");
            int s = int.Parse(Console.ReadLine());
            // n번째 층의 공의 갯수는 1부터 시작하고 마지막 항이 n인 등차수열의 합
            // 순환식: R_n = n*(n+1)/2 + R_(n-1) , R_1 = 1
            Console.WriteLine($"Result is {r.Class5(s)}");
#endif
#if Class6
            Console.Write("Input the string (Class6): ");
            string s = Console.ReadLine();
            if (r.Class6(s)) Console.WriteLine("It is palindrome");
            else Console.WriteLine("It is not palindrome");
#endif
#if Class7
            Console.Write("Input the string1 (Class7): ");
            string s1 = Console.ReadLine();
            Console.Write("Input the string2 (Class7): ");
            string s2 = Console.ReadLine();
            int res = r.Class7_Compare(s1, s2);
            if (res < 0) {
                Console.WriteLine("String 1 is first, String 2 is second.");
            } else if (res == 0) {
                Console.WriteLine("String 1 and 2 are same.");
            } else {
                Console.WriteLine("String 1 is second, String 2 is first.");
            }
#endif
#if Class8
            Console.Write("Input the white-space seperated numbers (Class8): ");
            string s1 = Console.ReadLine();
            IList<int> s2 = s1.Split(' ').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
            Console.Write("Input the sum number (Class8): ");
            int s3 = int.Parse(Console.ReadLine());
            Console.WriteLine($"Result is {r.Class8(s2, 0, s2.Count - 1, s3)}");
#endif
#endif
            Console.ReadLine(); // 자동 종료 방지
        }
    }
}
