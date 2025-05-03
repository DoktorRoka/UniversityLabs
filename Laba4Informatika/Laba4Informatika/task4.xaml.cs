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
    /// Логика взаимодействия для task4.xaml
    /// </summary>
    public partial class task4 : Window
    {
        public task4()
        {
            InitializeComponent();
        }

        private void b_count_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tb_n.Text, out int n) || n <= 0)
            {
                MessageBox.Show("Введите положительное целое n.", "Ошибка");
                return;
            }
            if (!double.TryParse(tb_x.Text, out double x))
            {
                MessageBox.Show("Введите корректное число x.", "Ошибка");
                return;
            }

            double S = 0.0;
            for (int k = 1; k <= n; k++)
            {
                double numerator = Math.Pow(Math.Log(3), k);

                double denominator = k * x;

                double term = numerator / denominator * Math.Pow(x, k);

                S += term;
            }

            tb_output.Text = $"S({x}) = {S:F6}";
        }
    }
}
