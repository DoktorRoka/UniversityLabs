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
    /// Логика взаимодействия для task2.xaml
    /// </summary>
    public partial class task2 : Window
    {
        public task2()
        {
            InitializeComponent();
        }

        private void b_countup_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(tb_vremya.Text, out double t) || t <= 0)
            {
                MessageBox.Show("Введите корректное положительное время t.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(tb_vrash.Text, out int N) || N <= 0)
            {
                MessageBox.Show("Введите корректное положительное целое число обращений N.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double T = t / N;

            double v = 1.0 / T;

            tb_output.Text =
                $"Период обращения T = {T:F3} единиц времени{Environment.NewLine}" +
                $"Частота обращения v = {v:F3} обращений в единицу времени";
        }
    }
}
