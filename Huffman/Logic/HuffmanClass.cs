using System;
using System.Collections.Generic;
using System.IO;

namespace Huffman {
    public static class HuffmanClass {
        public static List<HuffmanRun> CollectRuns(byte[] b) {
            List<HuffmanRun> listrun = new List<HuffmanRun>();
            HuffmanRun run = null;
            foreach (byte ch in b) {
                if (run == null) {
                    run = new HuffmanRun(ch);
                } else {
                    if (run.Symbol == ch) {
                        run.Length++;
                    } else {
                        HuffmanRun found = listrun.Find(t => t == run);
                        if (found == null) listrun.Add(run);
                        else found.Frequency++;
                        run = new HuffmanRun(ch);
                    }
                }
            }
            // 마지막 문자 저장
            if (run != null) {
                HuffmanRun found = listrun.Find(t => t == run);
                if (found == null) listrun.Add(run);
                else found.Frequency++;
            }
            return listrun;
        }
        public static HuffmanRun CreateHuffmanTree(List<HuffmanRun> CollectedRuns) {
            MinPQueue<HuffmanRun> heap = new MinPQueue<HuffmanRun>(CollectedRuns, (a, b) => a.Frequency.CompareTo(b.Frequency));
            while (heap.Count > 1) {
                HuffmanRun run1 = heap.Dequeue();
                HuffmanRun run2 = heap.Dequeue();
                heap.Enqueue(new HuffmanRun(run1, run2));
            }
            return heap.Dequeue();
        }
        public static void AssignCodewords(HuffmanRun RootRun) {
            AssignCodewordsRecursive(RootRun, 0, 0);
        }
        private static void AssignCodewordsRecursive(HuffmanRun Run, uint Codeword, int CodewordLen) {
            if (Run.LeftChild == null || Run.RightChild == null) {
                Run.CodeWord = Codeword;
                Run.CodeWordLen = CodewordLen;
            } else {
                AssignCodewordsRecursive(Run.LeftChild, Codeword << 1, CodewordLen + 1);
                AssignCodewordsRecursive(Run.RightChild, (Codeword << 1) + 1, CodewordLen + 1);
            }
        }
        public static LinkedList<HuffmanRun>[] BuildArrayOfLinkedList(List<HuffmanRun> CollectedRuns) {
            LinkedList<HuffmanRun>[] array = new LinkedList<HuffmanRun>[256];
            foreach (HuffmanRun i in CollectedRuns) {
                if (array[i.Symbol] == null) array[i.Symbol] = new LinkedList<HuffmanRun>();
                array[i.Symbol].AddLast(i);
            }
            return array;
        }
        public static HuffmanRun FindRunBySymbol(LinkedList<HuffmanRun>[] Source, byte Symbol, int Length) {
            LinkedList<HuffmanRun> ll = Source[Symbol];
            LinkedListNode<HuffmanRun> cursor = ll.First;
            while (cursor != null) {
                if (cursor.Value.Length == Length) return cursor.Value;
                else cursor = cursor.Next;
            }
            return null;
        }
        public static void OutputFrequency(BinaryWriter output, List<HuffmanRun> CollectedRuns, long OriginalFileSize) {
            output.Write(CollectedRuns.Count);
            output.Write(OriginalFileSize);
            foreach (HuffmanRun i in CollectedRuns) {
                output.Write(i.Symbol);
                output.Write(i.Length);
                output.Write(i.Frequency);
            }
        }
        public static void Encode(byte[] fin, BinaryWriter fout, LinkedList<HuffmanRun>[] LinkedListOfRuns) {
            int BufferLength = 0;
            uint Buffer = 0;
            HuffmanRun run = null;
            foreach (byte ch in fin) {
                if (run == null) {
                    run = new HuffmanRun(ch);
                } else {
                    if (run.Symbol == ch) {
                        run.Length++;
                    } else {
                        HuffmanRun found = FindRunBySymbol(LinkedListOfRuns, run.Symbol, run.Length);
                        Buffer = (Buffer << found.CodeWordLen) + found.CodeWord;
                        BufferLength += found.CodeWordLen;
                        while (BufferLength > 8) {
                            byte[] ba = BitConverter.GetBytes(Buffer >> (BufferLength - 8));
                            fout.Write(ba[0]);
                            BufferLength -= 8;
                        }
                        run = new HuffmanRun(ch);
                    }
                }
            }
            // 마지막 Run 인코딩
            if (run != null) {
                HuffmanRun found = FindRunBySymbol(LinkedListOfRuns, run.Symbol, run.Length);
                Buffer = (Buffer << found.CodeWordLen) + found.CodeWord;
                BufferLength += found.CodeWordLen;
                while (BufferLength > 8) {
                    byte[] ba = BitConverter.GetBytes(Buffer >> (BufferLength - 8));
                    fout.Write(ba[0]);
                    BufferLength -= 8;
                }
            }
            // 마지막으로 버퍼에 찌꺼기 남아있는지 확인하고 비움
            if (BufferLength > 0) {
                byte[] ba = BitConverter.GetBytes(Buffer << (8 - BufferLength));
                fout.Write(ba[0]);
            }
        }
        public static List<HuffmanRun> InputFrequency(BinaryReader fin, out long OriginalLength) {
            int count = fin.ReadInt32();
            long originallength = fin.ReadInt64();
            List<HuffmanRun> list = new List<HuffmanRun>();
            for (int i = 0; i < count; i++) {
                byte symbol = fin.ReadByte();
                int length = fin.ReadInt32();
                int frequency = fin.ReadInt32();
                HuffmanRun run = new HuffmanRun(symbol);
                run.Length = length;
                run.Frequency = frequency;
                list.Add(run);
            }
            OriginalLength = originallength;
            return list;
        }
        public static void Decode(BinaryReader fin, BinaryWriter fout, HuffmanRun RootOfTree, long OriginalLength) {
            int BytesRead = 0, BitCnt = 1, mask = 1, bits = 8;
            mask <<= bits - 1;
            for (int ch = fin.ReadByte(); ch != -1 && BytesRead < OriginalLength;) {
                HuffmanRun p = RootOfTree;
                while (true) {
                    if (p.LeftChild == null || p.RightChild == null) {
                        for (int j = 0; j < p.Length; j++) {
                            fout.Write(p.Symbol);
                        }
                        BytesRead += p.Length;
                        break;
                    } else if ((ch & mask) == 0) {
                        p = p.LeftChild;
                    } else {
                        p = p.RightChild;
                    }
                    if (BitCnt++ == bits) {
                        ch = fin.ReadByte();
                        BitCnt = 1;
                    } else {
                        ch <<= 1;
                    }
                }
            }
        }
    }
}
