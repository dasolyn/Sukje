using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Huffman {
    public static class HuffmanClass {
        /// <summary>
        /// 스트림에서 Huffman Run을 수집하고 그 Run들을 담은 가변 배열을 반환합니다.
        /// </summary>
        public static List<HuffmanRun> CollectRuns(Stream Stream) {
            List<HuffmanRun> listrun = new List<HuffmanRun>();
            HuffmanRun run = null;
            using (BinaryReader br = new BinaryReader(Stream, Encoding.Default, true)) {
                while (br.BaseStream.Position < br.BaseStream.Length) {
                    byte ch = br.ReadByte();
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
            }
            // 마지막 문자 저장
            if (run != null) {
                HuffmanRun found = listrun.Find(t => t == run);
                if (found == null) listrun.Add(run);
                else found.Frequency++;
            }
            return listrun;
        }
        /// <summary>
        /// 컬렉션에 포함되어 있는 Huffman Run을 이용하여 트리를 구성하고 트리의 루트 Run을 반환합니다.
        /// </summary>
        public static HuffmanRun CreateHuffmanTree(IEnumerable<HuffmanRun> CollectedRuns) {
            MinPQueue<HuffmanRun> heap = new MinPQueue<HuffmanRun>(CollectedRuns, (a, b) => a.Frequency.CompareTo(b.Frequency));
            while (heap.Count > 1) {
                HuffmanRun run1 = heap.Dequeue();
                HuffmanRun run2 = heap.Dequeue();
                heap.Enqueue(new HuffmanRun(run1, run2));
            }
            return heap.Dequeue();
        }
        /// <summary>
        /// Huffman 트리의 루트 Run부터 시작하여 모든 Leaf Run에 Code Word를 할당합니다.
        /// </summary>
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
        /// <summary>
        /// 컬렉션에 포함된 모든 Huffman Run을, Symbol을 키로 하는 고정 배열에 연결 리스트 형태로 달려 있도록 구성하고 그 배열을 반환합니다.
        /// </summary>
        public static LinkedList<HuffmanRun>[] StoreRunsIntoArray(IEnumerable<HuffmanRun> CollectedRuns) {
            LinkedList<HuffmanRun>[] array = new LinkedList<HuffmanRun>[256];
            foreach (HuffmanRun i in CollectedRuns) {
                if (array[i.Symbol] == null) array[i.Symbol] = new LinkedList<HuffmanRun>();
                array[i.Symbol].AddLast(i);
            }
            return array;
        }
        /// <summary>
        /// 연결 리스트 배열에서 주어진 Symbol, Length와 같은 값을 가진 Huffman Run을 탐색합니다.
        /// </summary>
        private static HuffmanRun FindRun(LinkedList<HuffmanRun>[] Source, byte Symbol, int Length) {
            LinkedList<HuffmanRun> ll = Source[Symbol];
            LinkedListNode<HuffmanRun> cursor = ll.First;
            while (cursor != null) {
                if (cursor.Value.Length == Length) return cursor.Value;
                else cursor = cursor.Next;
            }
            return null;
        }
        /// <summary>
        /// Huffman 헤더를 작성합니다.
        /// 주어진 스트림에 Huffman Run의 갯수와 원본 파일의 크기, 각각의 Run 정보를 씁니다.
        /// </summary>
        public static void OutputFrequency(Stream Stream, ICollection<HuffmanRun> CollectedRuns, long OriginalFileSize) {
            using (BinaryWriter bw = new BinaryWriter(Stream, Encoding.Default, true)) {
                bw.Write(CollectedRuns.Count);
                bw.Write(OriginalFileSize);
                foreach (HuffmanRun i in CollectedRuns) {
                    bw.Write(i.Symbol);
                    bw.Write(i.Length);
                    bw.Write(i.Frequency);
                }
            }
        }
        /// <summary>
        /// Huffman 인코드 메서드입니다. 입력 스트림에서 한 바이트씩 읽으면서 인코딩하여 출력 스트림에 씁니다.
        /// Huffman Run을 탐색하는데 연결 리스트 배열을 사용합니다.
        /// </summary>
        public static void Encode(Stream InputStream, Stream OutputStream, LinkedList<HuffmanRun>[] LinkedListOfRuns) {
            int BufferLength = 0;
            uint Buffer = 0;
            HuffmanRun run = null;
            using (BinaryReader br = new BinaryReader(InputStream, Encoding.Default, true))
            using (BinaryWriter bw = new BinaryWriter(OutputStream, Encoding.Default, true)) {
                while (br.BaseStream.Position < br.BaseStream.Length) {
                    byte ch = br.ReadByte();
                    if (run == null) {
                        run = new HuffmanRun(ch);
                    } else {
                        if (run.Symbol == ch) {
                            run.Length++;
                        } else {
                            HuffmanRun found = FindRun(LinkedListOfRuns, run.Symbol, run.Length);
                            Buffer = (Buffer << found.CodeWordLen) + found.CodeWord;
                            BufferLength += found.CodeWordLen;
                            while (BufferLength > 8) {
                                byte[] ba = BitConverter.GetBytes(Buffer >> (BufferLength - 8));
                                bw.Write(ba[0]);
                                BufferLength -= 8;
                            }
                            run = new HuffmanRun(ch);
                        }
                    }
                }
                // 마지막 Run 인코딩
                if (run != null) {
                    HuffmanRun found = FindRun(LinkedListOfRuns, run.Symbol, run.Length);
                    Buffer = (Buffer << found.CodeWordLen) + found.CodeWord;
                    BufferLength += found.CodeWordLen;
                    while (BufferLength > 8) {
                        byte[] ba = BitConverter.GetBytes(Buffer >> (BufferLength - 8));
                        bw.Write(ba[0]);
                        BufferLength -= 8;
                    }
                }
                // 마지막으로 버퍼에 찌꺼기 남아있는지 확인하고 비움
                if (BufferLength > 0) {
                    byte[] ba = BitConverter.GetBytes(Buffer << (8 - BufferLength));
                    bw.Write(ba[0]);
                }
            }
        }
        /// <summary>
        /// Huffman 헤더를 읽어들입니다.
        /// 스트림에서 헤더를 읽어들여 Huffman Run을 담은 가변 배열과 원래 파일의 크기를 반환합니다.
        /// </summary>
        /// <exception cref="EndOfStreamException">헤더를 읽는 도중 예기치 않은 스트림의 끝이 나타났습니다.</exception>
        public static List<HuffmanRun> InputFrequency(Stream Stream, out long OriginalLength) {
            List<HuffmanRun> list = new List<HuffmanRun>();
            using (BinaryReader br = new BinaryReader(Stream, Encoding.Default, true)) {
                int count = br.ReadInt32();
                OriginalLength = br.ReadInt64();
                for (int i = 0; i < count; i++) {
                    byte symbol = br.ReadByte();
                    int length = br.ReadInt32();
                    int frequency = br.ReadInt32();
                    HuffmanRun run = new HuffmanRun(symbol);
                    run.Length = length;
                    run.Frequency = frequency;
                    list.Add(run);
                }
            }
            return list;
        }
        /// <summary>
        /// Huffman 디코드 메서드입니다. 입력 스트림에서 한 바이트씩 읽으면서 디코딩하여 출력 스트림에 씁니다.
        /// Huffman Run을 탐색하는데 Huffman 트리의 루트 Run이 필요합니다.
        /// </summary>
        /// <param name="InputStream">압축을 해제할 입력 스트림입니다. Huffman 헤더가 포함되면 안됩니다.</param>
        /// <param name="OriginalLength">압축 전 파일의 길이입니다.</param>
        /// <exception cref="EndOfStreamException">파일의 원래 길이에 해당하는 디코딩을 완료하기 전에 입력 스트림의 끝이 나타났습니다.</exception>
        public static void Decode(Stream InputStream, Stream OutputStream, HuffmanRun RootOfTree, long OriginalLength) {
            int WrittenBytes = 0, BitCnt = 1;
            byte mask = 128;
            using (BinaryReader br = new BinaryReader(InputStream, Encoding.Default, true))
            using (BinaryWriter bw = new BinaryWriter(OutputStream, Encoding.Default, true)) {
                byte ch = br.ReadByte();
                while (WrittenBytes < OriginalLength) {
                    HuffmanRun p = RootOfTree;
                    while (true) {
                        if (p.LeftChild == null || p.RightChild == null) {
                            for (int j = 0; j < p.Length; j++) bw.Write(p.Symbol);
                            WrittenBytes += p.Length;
                            break;
                        } else if ((ch & mask) == 0) {
                            p = p.LeftChild;
                        } else {
                            p = p.RightChild;
                        }
                        if (BitCnt++ == 8) {
                            ch = br.ReadByte();
                            BitCnt = 1;
                        } else {
                            ch <<= 1;
                        }
                    }
                }
            }
        }
    }
}
