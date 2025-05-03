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
using System.Data;

namespace Laba4Informatika
{
    /// <summary>
    /// Логика взаимодействия для task1.xaml
    /// </summary>
    public partial class task1 : Window
    {
        public task1()
        {
            InitializeComponent();
        }

        private void tb_nporyadok_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void b_creatematrix_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tb_nporyadok.Text, out int n) || n <= 0)
            {
                MessageBox.Show("Введите целое положительное число.", "Ошибка");
                return;
            }

            var vals = Enumerable.Range(1, n * n).OrderBy(_ => Guid.NewGuid()).ToArray();
            int[,] A = new int[n, n];
            for (int i = 0, k = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    A[i, j] = vals[k++];

            int max = A[0, 0], maxR = 0, maxC = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (A[i, j] > max)
                        (max, maxR, maxC) = (A[i, j], i, j);

            for (int k = 0; k < n; k++)
            {
                A[maxR, k] = 0;
                A[k, maxC] = 0;
            }

            ugMatrix.Children.Clear();
            ugMatrix.Rows = n;
            ugMatrix.Columns = n;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ugMatrix.Children.Add(new TextBlock
                    {
                        Text = A[i, j].ToString(),
                        Width = 40,               
                        Height = 40,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(1),
                        Background = (i == maxR || j == maxC)
                                     ? System.Windows.Media.Brushes.LightCoral
                                     : System.Windows.Media.Brushes.LightGray
                    });
                }
            }
        }
    }
}
