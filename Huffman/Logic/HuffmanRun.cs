using System;

namespace Huffman {
    public class HuffmanRun : IEquatable<HuffmanRun> {
        public byte Symbol { get; set; }
        public int Length { get; set; }
        public int Frequency { get; set; }
        public HuffmanRun LeftChild { get; set; }
        public HuffmanRun RightChild { get; set; }
        public HuffmanRun(byte Symbol) {
            this.Symbol = Symbol;
            Length = 1;
            Frequency = 1;
            LeftChild = null;
            RightChild = null;
        }
        public HuffmanRun(HuffmanRun LeftChild, HuffmanRun RightChild) {
            Symbol = 0;
            Length = 0;
            Frequency = LeftChild.Frequency + RightChild.Frequency;
            this.LeftChild = LeftChild;
            this.RightChild = RightChild;
        }

        public override string ToString() {
            return $"{Symbol:x2} {Length} {Frequency}";
        }
        public override bool Equals(object other) {
            HuffmanRun o = other as HuffmanRun;
            if (ReferenceEquals(o, null)) return false;
            else return Equals(o);
        }
        public bool Equals(HuffmanRun other) {
            return !ReferenceEquals(other, null) && Symbol.Equals(other.Symbol) && Length.Equals(other.Length);
        }
        public override int GetHashCode() {
            return unchecked(Symbol.GetHashCode() + Length.GetHashCode());
        }
        public static bool operator ==(HuffmanRun a, HuffmanRun b) {
            if (ReferenceEquals(a, null)) return ReferenceEquals(b, null);
            return a.Equals(b);
        }
        public static bool operator !=(HuffmanRun a, HuffmanRun b) {
            if (ReferenceEquals(a, null)) return !ReferenceEquals(b, null);
            return !a.Equals(b);
        }
    }
}
