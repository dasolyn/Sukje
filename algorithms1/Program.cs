﻿#define Day4
#define Class_Test

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
            int res = r.Class7_Compare(s1, s2, 0, 0);
            if (res < 0) {
                Console.WriteLine("String 1 is first, String 2 is second.");
            } else if (res == 0) {
                Console.WriteLine("String 1 and 2 are same.");
            } else {
                Console.WriteLine("String 1 is second, String 2 is first.");
            }
#endif
#if Class8
            Console.Write("Input white-space seperated numbers (Class8): ");
            string s1 = Console.ReadLine();
            IList<int> s2 = s1.Split(' ').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
            Console.Write("Input the sum number (Class8): ");
            int s3 = int.Parse(Console.ReadLine());
            Console.WriteLine($"Result is {r.Class8(s2, 0, s2.Count - 1, s3)}");
#endif
#if Class9
            Console.Write("Input white-space seperated numbers (Class9): ");
            string s1 = Console.ReadLine();
            IList<int> s2 = s1.Split(' ').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
            Console.Write("Input the number for find (Class9): ");
            int s3 = int.Parse(Console.ReadLine());
            Console.WriteLine($"Result is {r.Class9(s2, 0, s2.Count - 1, s3)}");
#endif
#if Class10
            Console.Write("Input white-space seperated numbers1 (Class10): ");
            string s1 = Console.ReadLine();
            List<int> s2 = s1.Split(' ').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
            Console.Write("Input white-space seperated numbers2 (Class10): ");
            string s3 = Console.ReadLine();
            List<int> s4 = s3.Split(' ').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
            Console.WriteLine($"Result is {r.Class10(s2, s4, 0, 0)}");
#endif
#endif
#if Day2
            Day2 r = new Day2();
#if Class1
            List<string> strs = System.IO.File.ReadLines("D2C1.txt").ToList();
            List<List<int>> ints = new List<List<int>>();
            int size = 0,  startx = 0, starty = 0, endx = 0, endy = 0;
            for (int i = 0; i < strs.Count; i++) {
                if (i == 0) {
                    List<string> s1 = strs[0].Split(' ').ToList();
                    size = int.Parse(s1[0]);
                    startx = int.Parse(s1[1]);
                    starty = int.Parse(s1[2]);
                    endx = int.Parse(s1[3]);
                    endy = int.Parse(s1[4]);
                } else {
                    ints.Add(strs[i].Split(' ').Select(k => { if (int.Parse(k) == 1) return 1; else return 0; }).ToList());
                }
            }
            if (r.Class1(ints, startx, starty, endx, endy, size)) {
                Console.WriteLine("Class1: OK! You can reach that cell.");
            } else {
                Console.WriteLine("Class1: You can't reach that cell.");
            }
#endif
#if Class2
            List<string> strs = System.IO.File.ReadLines("D2C1.txt").ToList();
            List<List<int>> ints = new List<List<int>>();
            int size = 0,  startx = 0, starty = 0, endx = 0, endy = 0;
            for (int i = 0; i < strs.Count; i++) {
                if (i == 0) {
                    List<string> s1 = strs[0].Split(' ').ToList();
                    size = int.Parse(s1[0]);
                    startx = int.Parse(s1[1]);
                    starty = int.Parse(s1[2]);
                    endx = int.Parse(s1[3]);
                    endy = int.Parse(s1[4]);
                } else {
                    ints.Add(strs[i].Split(' ').Select(k => { if (int.Parse(k) == 1) return 1; else return 0; }).ToList());
                }
            }
            if (r.Class2(ints, startx, starty, endx, endy, size)) {
                Console.WriteLine("Class2: OK! You can reach that cell.");
            } else {
                Console.WriteLine("Class2: You can't reach that cell.");
            }
#endif
#if Class3
            List<string> strs = System.IO.File.ReadLines("D2C3.txt").ToList();
            List<List<int>> ints = new List<List<int>>();
            int size = 0;
            for (int i = 0; i < strs.Count; i++) {
                if (i == 0) {
                    List<string> s1 = strs[0].Split(' ').ToList();
                    size = int.Parse(s1[0]);
                } else {
                    ints.Add(strs[i].Split(' ').Select(k => { if (int.Parse(k) == 1) return 1; else return 0; }).ToList());
                }
            }
            List<int> blobs = r.Class3(ints, size);
            Console.WriteLine($"The number of blobs is {blobs.Count}");
            foreach (int i in blobs) {
                Console.WriteLine(i);
            }
#endif
#endif
#if Day3
            Day3 r = new Day3();
#if Class1
            for (int i = 1; i <= 15; i++) Console.WriteLine($"Class1, When N={i}, result is {r.Class1_Start(i)}");
#endif
#if Class2
            Console.Write("Input white-space seperated numbers (Class2): ");
            string s1 = Console.ReadLine();
            List<int> s2 = s1.Split(' ').Select(x => int.Parse(x)).OrderBy(x => x).ToList();
            Console.Write("Input the number for sum of subset's elements (Class2): ");
            int s3 = int.Parse(Console.ReadLine());
            Console.WriteLine($"Result is {r.Class2(s2, s3)}");
#endif
#endif
#if Day4
#if Class_Test
            List<int> data = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < 10; i++) {
                data.Add(rand.Next(-99999, 99999));
            }

            List<int> copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            copied.BubbleSort();
            watch.Stop();
            Console.WriteLine($"Bubble Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.SelectionSort();
            watch.Stop();
            Console.WriteLine($"Selection Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.InsertionSort();
            watch.Stop();
            Console.WriteLine($"Insertion Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.MergeSort();
            watch.Stop();
            Console.WriteLine($"Merge Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.QuickSort();
            watch.Stop();
            Console.WriteLine($"Quick Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.MedianQuickSort();
            watch.Stop();
            Console.WriteLine($"Median Quick Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.HeapSort();
            watch.Stop();
            Console.WriteLine($"Heap Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();

            copied = new List<int>(data);
            Console.WriteLine("Original Data: ");
            copied.Print();
            watch.Start();
            copied.JMSort();
            watch.Stop();
            Console.WriteLine($"JM Sort: {watch.ElapsedMilliseconds} ms");
            Console.WriteLine("Sorted Data: ");
            copied.Print();
            Console.WriteLine();
#endif
#if Class1
            // 데이터 생성
            List<List<long>> datas = new List<List<long>>();
            for (int i = 100; i <= 1000000; i *= 10) {
                List<long> data = new List<long>();
                Random rand = new Random();
                for (int j = 0; j < i; j++) {
                    byte[] buf = new byte[8];
                    rand.NextBytes(buf);
                    long randomlong = BitConverter.ToInt64(buf, 0);
                    data.Add(randomlong);
                }
                datas.Add(data);
            }

            // C# 내부 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.Sort();
                    watch.Stop();
                    Console.WriteLine($"C# internal sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 거품 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.BubbleSort();
                    watch.Stop();
                    Console.WriteLine($"Bubble sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 선택 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.SelectionSort();
                    watch.Stop();
                    Console.WriteLine($"Selection sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 삽입 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.InsertionSort();
                    watch.Stop();
                    Console.WriteLine($"Insertion sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 병합 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.MergeSort();
                    watch.Stop();
                    Console.WriteLine($"Merge sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 퀵 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.QuickSort();
                    watch.Stop();
                    Console.WriteLine($"Quick sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 중간값 사용 퀵 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.MedianQuickSort();
                    watch.Stop();
                    Console.WriteLine($"Median quick sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });
            
            // 힙 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.HeapSort();
                    watch.Stop();
                    Console.WriteLine($"Heap sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 정민 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.JMSort();
                    watch.Stop();
                    Console.WriteLine($"JM sort with {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });
#endif
#if Class2
            // 데이터 생성
            List<List<long>> datas = new List<List<long>>();
            for (int i = 100; i <= 1000000; i *= 10) {
                List<long> data = new List<long>();
                Random rand = new Random();
                for (int j = 0; j < i; j++) {
                    byte[] buf = new byte[8];
                    rand.NextBytes(buf);
                    long randomlong = BitConverter.ToInt64(buf, 0);
                    data.Add(randomlong);
                }
                datas.Add(data);
            }

            // 정렬된 배열에 대한 퀵 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    // 개수가 1만만 넘어도 스택 오버플로우가 뜸
                    if (d.Count >= 10000) break;
                    List<long> copy = new List<long>(d);
                    copy.Sort();
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.QuickSort();
                    watch.Stop();
                    Console.WriteLine($"Quick sort with sorted {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });

            // 정렬된 배열에 대한 중간값 사용 퀵 정렬
            System.Threading.Tasks.Task.Run(() => {
                foreach (var d in datas) {
                    List<long> copy = new List<long>(d);
                    copy.Sort();
                    System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                    copy.MedianQuickSort();
                    watch.Stop();
                    Console.WriteLine($"Median quick sort with sorted {d.Count:#,###} elements: {watch.ElapsedMilliseconds} ms");
                }
            });
#endif
#if Class3
            // 데이터 생성
            List<long> data = new List<long>();
            Random rand = new Random();
            for (int j = 0; j < 100000; j++) {
                byte[] buf = new byte[8];
                rand.NextBytes(buf);
                long randomlong = BitConverter.ToInt64(buf, 0);
                data.Add(randomlong);
            }

            Console.Write("Input the number, for find n-th smallest number(Class3): ");
            int findex = int.Parse(Console.ReadLine());

            // 퀵 정렬후 찾기
            System.Threading.Tasks.Task.Run(() => {
                List<long> copy = new List<long>(data);
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                copy.QuickSort();
                long found = copy[findex - 1];
                watch.Stop();
                Console.WriteLine($"Quick sort to find number: It takes {watch.ElapsedMilliseconds} ms, result is {found}");
            });

            // 퀵 선택
            System.Threading.Tasks.Task.Run(() => {
                List<long> copy = new List<long>(data);
                System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
                long found = copy.QuickSelection(findex - 1);
                watch.Stop();
                Console.WriteLine($"Quick selection to find number: It takes {watch.ElapsedMilliseconds} ms, result is {found}");
            });
#endif
#endif
            Console.ReadLine(); // 자동 종료 방지
        }
    }
}
