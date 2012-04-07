using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PassGen
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string charSet = "";

            if (cbLowerCase.IsChecked == true) charSet += "abcdefghijklmnopqrstuvwxyz";
            if (cbUpperCase.IsChecked == true) charSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (cbNumbers.IsChecked == true) charSet += "0123456789";
            if (cbSymbols.IsChecked == true) charSet += "@#$%&?!+-*/=";

            if (charSet.Equals("")) return;
            
            Random rnd = new Random();
            
            int len = string.IsNullOrWhiteSpace(tbPassLen.Text) ? 0 : int.Parse(tbPassLen.Text);

            string pass = "";
            for (int i = 0; i < len; i++)
            {
                 pass += charSet[rnd.Next(charSet.Length)];
            }

            string passLine = "Password: " + pass;

            int begin = textBox1.Text.LastIndexOf("Password:");
            if (begin == -1)
            {
                textBox1.AppendText("\n" + passLine);
            }
            else
            {
                int end = textBox1.Text.IndexOf("\n", begin);
                string s;
                if (end == -1)
                    s = textBox1.Text.Remove(begin);
                else
                    s = textBox1.Text.Remove(begin, end - begin);
                s = s.Insert(begin, passLine);
                textBox1.Text = s;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult res;
            res = MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.No) e.Cancel = true;
           
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog fsave = new Microsoft.Win32.SaveFileDialog();
            fsave.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            fsave.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            string site = "";
            int begin = textBox1.Text.IndexOf("Site:");
            if (begin != -1)
            {
                site = textBox1.Text.Remove(0,begin + 5);
                int end = site.IndexOf("\n");
                if (end != -1)
                    site = site.Remove(end);
            }

            string filename = "";
            foreach (char ch in site)
            {
                if (ch == '.')
                    filename += "_";
                else if (char.IsLetterOrDigit(ch))
                    filename += ch;

            }
            fsave.FileName = filename;

            if (fsave.ShowDialog() == true)
            {
                string[] strs = new string[textBox1.LineCount];
                for (int i = 0; i < textBox1.LineCount; i++)
                {
                    strs[i] = textBox1.GetLineText(i);
                }
                System.IO.File.WriteAllLines(fsave.FileName, strs);
                
            }
        }

        private void tbPassLen_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text) {
                if (Char.IsDigit(c) != true)
                {
                    e.Handled = true;
                    break;
                }
            }
             
        }

        private void tbPassLen_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int val;
            if (tbPassLen.Text != "")
                val = Convert.ToInt32(tbPassLen.Text);
            else
                val = 0;

            if (e.Delta.GetHashCode() > 0)
                val++;
            else
                val--;

            tbPassLen.Text = val.ToString();
        }
    }
}
