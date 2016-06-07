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
            List<HuffmanRun> runs = HuffmanClass.CollectRuns(File.ReadAllBytes(TxtFile.Text));
            using (StreamWriter output = File.CreateText("runs.txt")) {
                foreach (HuffmanRun i in runs) {
                    output.WriteLine(i);
                }
            }
            HuffmanRun root = HuffmanClass.CreateHuffmanTree(runs);
            using (StreamWriter output = File.CreateText("tree.txt")) {
                HuffmanPreorderTraverse(output, root, 0);
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
    }
}
