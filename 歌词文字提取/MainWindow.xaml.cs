using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
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
using System.Drawing;
using System.IO;

namespace 歌词文字提取
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HashSet<string> pys = new HashSet<string>();
        string soundSourcePath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            //string input = geci.Text;
            //char[] chars = input.ToArray();
            //HashSet<char> set = new HashSet<char>();
            //for (int i = 0; i < chars.Length; i++)
            //{
            //    if (chars[i] >= 0x4e00 && chars[i] <= 0x9fbb)
            //        set.Add(chars[i]);
            //}
            //words = set.ToArray();
            //Array.Sort(words);
            //wordslb.ItemsSource = null;
            //wordslb.ItemsSource = words;
            pys.Clear();
            HashSet<char> words = new HashSet<char>();
            string input = geci.Text;
            for (int i = 0; i < input.Length; i++)
            {
                if (ChineseChar.IsValidChar(input[i]))
                {
                    ChineseChar chineseChar = new ChineseChar(input[i]);
                    List<string> list = chineseChar.Pinyins.ToList();
                    foreach (var item in list)
                    {
                        if(!String.IsNullOrEmpty(item)&&!String.IsNullOrWhiteSpace(item))
                        pys.Add(item);
                    }
                    words.Add(input[i]);
                }
            }

            pinyinslb.ItemsSource = null;
            pinyinslb.ItemsSource = pys.OrderBy(p => p);
            pinyinCounttb.Text = $"可能的读音数：{pys.Count}";
            wordCounttb.Text = $"单字字数：{words.Count}";
        }

        private void searchbtn_Click(object sender, RoutedEventArgs e)
        {
            //string searchString = searchtb.Text;
            //char searchword = searchString[0];
            //if (searchword >= 0x4e00 && searchword <= 0x9fbb && words.Contains(searchword))
            //{
            //    resulttb.Text = "存在";
            //}
            //else
            //{
            //    resulttb.Text = "不存在";
            //}
            string searchString = searchtb.Text;
            if (pys.Contains(searchString))
            {
                resulttb.Text = "存在";
            }
            else
            {
                resulttb.Text = "不存在";
            }
        }

        private void leftWordbtn_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = soundSourcePath;
            InputBox.Visibility = Visibility.Visible;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if(InputTextBox.Text.Length == 0)
            {
                return;
            }
            if (!Directory.Exists(soundSourcePath))
            {
                return;
            }
            soundSourcePath = InputTextBox.Text;
            InputBox.Visibility = Visibility.Collapsed;
            string[] paths = Directory.GetFiles(soundSourcePath);
            HashSet<string> names = new HashSet<string>();
            for (int i = 0; i < paths.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(paths[i]);
                string name = fileInfo.Name;
                names.Add(name.Substring(0,name.Length-4));
            }
            List<string> leftPinyins = new List<string>();
            foreach (var item in pys)
            {
                if (!names.Contains(item))
                {
                    leftPinyins.Add(item);
                }
            }
            leftWordslb.ItemsSource = null;
            leftWordslb.ItemsSource = leftPinyins.OrderBy(p=>p);
            leftWordstb.Text = $"缺失读音数：{leftPinyins.Count}";
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = Visibility.Collapsed;
        }
    }
}
