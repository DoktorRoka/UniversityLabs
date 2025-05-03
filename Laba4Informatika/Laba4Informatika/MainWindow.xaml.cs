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

namespace Laba4Informatika
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void b_task1_Click(object sender, RoutedEventArgs e)
        {
            var task1Window = new task1();
            task1Window.Show();
        }

        private void b_task2_Click(object sender, RoutedEventArgs e)
        {
            var task2Window = new task2();
            task2Window.Show();
        }

        private void b_task3_Click(object sender, RoutedEventArgs e)
        {
            var task3Window = new task3();
            task3Window.Show();
        }

        private void b_task4_Click(object sender, RoutedEventArgs e)
        {
            var task4Window = new task4();
            task4Window.Show();
        }
    }
}
