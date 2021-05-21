using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LFSR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //int a = Convert.ToInt32("101010011", 2);
        }

        private void DecryptButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Color.FromRgb(165, 143, 170));
        }

        private void EncryptButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Color.FromRgb(165, 143, 170));
        }
        private void OpenFileButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Color.FromRgb(165, 143, 170));
        }

        private void EncryptButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Color.FromRgb(237, 237, 208));

        }

        private void DecryptButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Color.FromRgb(237, 237, 208));

        }


        private void OpenFileButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Control)sender).Background = new SolidColorBrush(Color.FromRgb(237, 237, 208));

        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathField.Text = openFileDialog.FileName;
                byte[] info = File.ReadAllBytes(FilePathField.Text);
                string input = string.Empty;
                foreach (var item in info)
                {
                    string buff = Convert.ToString(item, 16);

                    while (buff.Length < 2)
                    {
                        buff = buff.Insert(0, "0");
                    }
                    input += buff;
                }
                BaseContentField.Text = input;
            }
            
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaseStateField.IsMaskCompleted && CheckRegisterState(BaseStateField.Text) &&
                BaseContentField.Text.Trim() != string.Empty)
            {
                LFSRCipher cipher = new LFSRCipher(Convert.ToInt32(BaseStateField.Text.Substring(2), 2));

                long length = BaseContentField.Text.Length / 2;

                cipher.CountKey(length);

                string result = string.Empty;
                string key = string.Empty;
                for (long i = 0; i < length; i++)
                {
                    byte b = Convert.ToByte(BaseContentField.Text.Substring((int)(i * 2), 2), 16);
                    key += Convert.ToString(cipher.Key[i], 2);

                    string buff = Convert.ToString(b ^ cipher.Key[i], 16);
                    while (buff.Length < 2)
                    {
                        buff = buff.Insert(0, "0");
                    }
                    result += buff;
                }
                KeyField.Text = key;

                ResultField.Text = result;

                if (MessageBox.Show("Сохранить результат как файл?", "Сохранить?", MessageBoxButton.YesNo) == 
                    MessageBoxResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName,
                            FileMode.Create, FileAccess.Write))
                        {
                            for (int i = 0; i < length; i++)
                            {
                                fileStream.WriteByte(Convert.ToByte(result.Substring(i * 2, 2), 16));
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaseStateField.IsMaskCompleted && CheckRegisterState(BaseStateField.Text) &&
                BaseContentField.Text.Trim() != string.Empty)
            {
                LFSRCipher cipher = new LFSRCipher(Convert.ToInt32(BaseStateField.Text.Substring(2),2));

                long length = BaseContentField.Text.Length / 2;

                cipher.CountKey(length);

                string result = string.Empty;
                string key = string.Empty;
                for (long i = 0; i < length; i++)
                {
                    byte b = Convert.ToByte(BaseContentField.Text.Substring((int)(i * 2), 2), 16);
                    key += Convert.ToString(cipher.Key[i], 2);

                    string buff = Convert.ToString(b ^ cipher.Key[i], 16);
                    while (buff.Length < 2)
                    {
                        buff = buff.Insert(0, "0");
                    }
                    result += buff;
                }
                KeyField.Text = key;

                ResultField.Text = result;
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        bool CheckRegisterState(string input)
        {
            return Regex.IsMatch(input, @"0b[0-1]{26}");
        }
    }
}
