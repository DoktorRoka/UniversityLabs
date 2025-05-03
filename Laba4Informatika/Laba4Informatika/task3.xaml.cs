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
using System.Windows.Shapes;

namespace Laba4Informatika
{
    /// <summary>
    /// Логика взаимодействия для task3.xaml
    /// </summary>
    public partial class task3 : Window
    {
        public task3()
        {
            InitializeComponent();
        }

        private void b_count_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tb_a.Text, out double a) ||
                !double.TryParse(tb_b.Text, out double b) ||
                !double.TryParse(tb_c.Text, out double c))
            {
                tb_output.Text = "Ошибка: введите три числовых значения.";
                return;
            }

            bool exists = a > 0 && b > 0 && c > 0
                          && a + b > c
                          && a + c > b
                          && b + c > a;

            if (!exists)
            {
                tb_output.Text = "Из введённых чисел нельзя составить треугольник.";
                return;
            }

            string type;
            if (Math.Abs(a - b) < 1e-9 && Math.Abs(b - c) < 1e-9)
            {
                type = "Равносторонний";
            }
            else if (Math.Abs(a - b) < 1e-9
                  || Math.Abs(a - c) < 1e-9
                  || Math.Abs(b - c) < 1e-9)
            {
                type = "Равнобедренный";
            }
            else
            {
                type = "Разносторонний";
            }

            tb_output.Text = $"Треугольник существует.{Environment.NewLine}Тип: {type}.";
        }
    }
}
