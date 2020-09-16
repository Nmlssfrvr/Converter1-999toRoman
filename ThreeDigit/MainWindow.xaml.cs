using System;
using System.Collections.Generic;
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
        static class Conv3hNumToRom
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
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int value = 0;
            Rim.Text = "";
            List<string> str = new List<string>();
            str.AddRange(Rus.Text.ToLower().Split(' '));
            if (str.Count > 3)
            {
                Rim.Text = "Вы ввели не трёхзначное число";
                return;
            }
            if (Conv3hNumToRom.Hundreds(str[0]) == 0)
            {
                Rim.Text = "Ошибка в первой части числа";
                return;
            }
            else value += Conv3hNumToRom.Hundreds(str[0]);
            if (str.Count == 2)
            {
                if (0 == Conv3hNumToRom.Tens(str[1]))
                {
                    if (0 == Conv3hNumToRom.Teens(str[1]))
                    {
                        if (0 == Conv3hNumToRom.Units(str[1]))
                        {
                            Rim.Text = "Ошибка во второй части числа";
                            return;
                        }
                        else value += Conv3hNumToRom.Units(str[1]);
                    }
                    else value += Conv3hNumToRom.Teens(str[1]);
                }
                else value += Conv3hNumToRom.Tens(str[1]);
            } else
            if (str.Count == 3)
             {
                
                if (0 == Conv3hNumToRom.Tens(str[1]))
                {
                    Rim.Text = "Ошибка во второй части числа";
                    return;
                } else value += Conv3hNumToRom.Tens(str[1]);
                if (0 == Conv3hNumToRom.Units(str[2]))
                {
                    Rim.Text = "Ошибка в третьей части числа";
                    return;
                }else value += Conv3hNumToRom.Units(str[2]);
             }
            Rim.Text = Conv3hNumToRom.ToRoman(value);

        }
            private void Rus_MouseEnter(object sender, MouseEventArgs e)
        {
            
            if (string.Equals(Rus.Text, "Впишите трехзначное число на русском языке"))
                Rus.Text = "";
        }
    }
}