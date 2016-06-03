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
                    output.WriteLine($"{i.Symbol:x2} {i.Length} {i.Frequency}");
                }
            }
            MessageBox.Show("해당 파일의 Runs를 모두 구했습니다. 실행파일과 같은 폴더의 runs.txt에 결과가 출력됩니다.");
        }
    }
}
