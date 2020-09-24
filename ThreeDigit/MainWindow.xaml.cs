using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

namespace ThreeDigit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Rim.IsReadOnly = true;
        }
        static class ConvNumToRom
        {
            static Dictionary<int, string> ra = new Dictionary<int, string>
            { { 1000, "M" },  { 900, "CM" },  { 500, "D" },  { 400, "CD" },  { 100, "C" },
                              { 90 , "XC" },  { 50 , "L" },  { 40 , "XL" },  { 10 , "X" },
                              { 9  , "IX" },  { 5  , "V" },  { 4  , "IV" },  { 1  , "I" } };

            public static string ToRoman(int number) => ra
                .Where(d => number >= d.Key)
                .Select(d => d.Value + ToRoman(number - d.Key))
                .FirstOrDefault();

            static Dictionary<int, string> hundreds = new Dictionary<int, string>
            { {100, "сто" }, {200, "двести" }, {300, "триста" }, {400, "четыреста" }, {500, "пятьсот" },
              {600, "шестьсот" }, {700, "семьсот" }, {800, "восемьсот" }, {900, "девятьсот"} };

            public static int Hundreds(string s) => hundreds
                .Where(d => string.Equals(s, d.Value))
                .Select(d => d.Key).FirstOrDefault();

            static Dictionary<int, string> tens = new Dictionary<int, string>
            { {20, "двадцать" }, {30,"тридцать" }, {40, "сорок" }, {50, "пятьдесят" }, {60, "шестьдесят" },
              {70, "семьдесят" }, {80, "восемьдесят" }, {90, "девяносто" } };

            public static int Tens(string s) => tens
                .Where(d => string.Equals(s, d.Value))
                .Select(d => d.Key).FirstOrDefault();

            static Dictionary<int, string> teens = new Dictionary<int, string>
            { {10, "десять" }, {11, "одиннадцать" }, {12, "двенадцать" }, {13, "тринадцать" }, {14, "четырнадцать" }, 
              {15, "пятнадцать" }, {16, "шестнадцать" }, {17, "семнадцать" }, {18, "восемнадцать" }, {19, "девятнадцать" } };

            public static int Teens(string s) => teens
                .Where(d => string.Equals(s, d.Value))
                .Select(d => d.Key).FirstOrDefault();

            static Dictionary<int, string> units = new Dictionary<int, string> 
            { {1, "один" }, {2, "два" }, {3, "три" }, {4, "четыре" }, {5, "пять" }, 
              {6, "шесть" }, {7, "семь" }, {8, "восемь" }, {9, "девять" } };

            public static int Units(string s) => units
                .Where(d => string.Equals(s, d.Value))
                .Select(d => d.Key).FirstOrDefault();

            public static IEnumerable<int> Searcher(List<string> str)
            {
                int i = 0;
                foreach (var s in str)
                {
                    if (i > 3) yield break;
                    if (Hundreds(s) != 0) yield return 100;
                    else if (Tens(s) != 0) yield return 20;
                    else if (Teens(s) != 0) yield return 10;
                    else if (Units(s) != 0) yield return 1;
                    else
                    {
                        yield return 0;
                        yield break;
                    }
                    i++;
                }
            }
            public static int Sorter(List<int> arr)
            {
                for (int i = 0; i < arr.Count-1; i++)
                {
                    if (arr[i] == 100 && arr[i + 1] == 100)
                    {
                        MessageBox.Show("Ошибка! После сотен не могут идти сотни");
                        return 1;
                    }                                                     
                    if (arr[i] == 20 && arr[i + 1] != 1)
                    {
                        MessageBox.Show("Ошибка! После десяток могут идти только единицы");
                        return 2;
                    }
                                                                           
                    if (arr[i] == 10)
                    {
                        MessageBox.Show("Ошибка! После числе 10-19 не может идти число");
                        return 3;
                    }
                                                                          
                    if (arr[i] == 1)
                    {
                        MessageBox.Show("Ошибка! После единиц не может идти число");
                        return 4;
                    }                                                    
                }
                return 0;                                                           // OK
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int value = 0;
            List<string> str = new List<string>();
            str.AddRange(Rus.Text.ToLower().Split(' '));
            while (str.Contains(""))
                str.Remove("");
            if (str.Count == 0)
            {
                MessageBox.Show("Вы ввели пустую строку");
                return;
            }
            var arr = ConvNumToRom.Searcher(str).ToList();
            if (arr.Contains(0))
            {
                MessageBox.Show("Ошибка в слове " + str[arr.IndexOf(0)]);
                return;
            }
            if (ConvNumToRom.Sorter(arr) == 0)
            {
                for (int i=0; i< arr.Count;i++)
                {
                    value += ConvNumToRom.Hundreds(str[i]);
                    value += ConvNumToRom.Tens(str[i]);
                    value += ConvNumToRom.Teens(str[i]);
                    value += ConvNumToRom.Units(str[i]);
                }   
                Rim.Text = ConvNumToRom.ToRoman(value);
            }
                

        }
    }
}