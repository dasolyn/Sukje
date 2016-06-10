using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Huffman {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        private void BtnFileDialog(object sender, RoutedEventArgs e) {
            OpenFileDialog dlg = new OpenFileDialog();
            bool? res = dlg.ShowDialog();
            if (res == true) {
                TxtFile.Text = dlg.FileName;
            }
        }
        private void BtnRun(object sender, RoutedEventArgs e) {
            byte[] file = File.ReadAllBytes(TxtFile.Text);
            long filesize = file.LongLength;
            List<HuffmanRun> runs = HuffmanClass.CollectRuns(file);
            using (StreamWriter output = File.CreateText("runs.txt")) {
                foreach (HuffmanRun i in runs) {
                    output.WriteLine(i);
                }
            }
            HuffmanRun root = HuffmanClass.CreateHuffmanTree(runs);
            HuffmanClass.AssignCodewords(root);
            using (StreamWriter output = File.CreateText("tree.txt")) {
                HuffmanPreorderTraverse(output, root, 0);
            }
            using (FileStream f = new FileStream("result.z", FileMode.OpenOrCreate))
            using (BinaryWriter output = new BinaryWriter(f)) {
                HuffmanClass.OutputFrequency(output, runs, filesize);
                HuffmanClass.Encode(file, output, HuffmanClass.BuildArrayOfLinkedList(runs));
            }
        }
        private void HuffmanPreorderTraverse(StreamWriter output, HuffmanRun Node, int depth) {
            for (int i = 0; i < depth; i++) output.Write(" ");
            if (Node != null) {
                output.WriteLine(Node);
                HuffmanPreorderTraverse(output, Node.LeftChild, depth + 1);
                HuffmanPreorderTraverse(output, Node.RightChild, depth + 1);
            } else {
                output.WriteLine("null");
            }
        }
        private void BtnDecompress(object sender, RoutedEventArgs e) {
            using (FileStream fin = new FileStream("result.z", FileMode.Open))
            using (BinaryReader input = new BinaryReader(fin))
            using (FileStream fout = new FileStream("result.gif", FileMode.OpenOrCreate))
            using (BinaryWriter output = new BinaryWriter(fout)) {
                long originallength;
                List<HuffmanRun> runs = HuffmanClass.InputFrequency(input, out originallength);
                HuffmanRun root = HuffmanClass.CreateHuffmanTree(runs);
                HuffmanClass.AssignCodewords(root);
                HuffmanClass.Decode(input, output, root, originallength);
            }
        }
    }
}
