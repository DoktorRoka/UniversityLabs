using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafikaLab3
{
    public partial class Form1 : Form
    {

        public struct Simple
        {
            public double xx;
            public double yy;
            public int ii; // 0 - move (поднять перо), 1 - draw (опустить перо)
        };

        // Глобальные переменные для масштабирования (для кнопки чтения файла)
        double Xmin = 0.2, Xmax = 8.2, Ymin = 0.5, Ymax = 6.5;

        private void b_to_scratch_Click(object sender, EventArgs e)
        {
            string fileName = "SCRATCH";

            // Открываем файл для бинарной записи
            using (BinaryWriter fw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                int n = 6;                // Количество углов (шестиугольник)
                int iterations = 20;      // 20 вложенных фигур
                double p = 0.1;           // Коэффициент сдвига (10% от длины грани)
                double R = 100.0;         // Начальный радиус

                double[] x = new double[n];
                double[] y = new double[n];

                // Вычисляем вершины самого первого (внешнего) шестиугольника
                for (int i = 0; i < n; i++)
                {
                    double angle = i * (2.0 * Math.PI / n); // Угол 60 градусов
                    x[i] = R * Math.Cos(angle);
                    y[i] = R * Math.Sin(angle);
                }

                Simple s;

                // Цикл генерации 20 вложенных фигур
                for (int iter = 0; iter < iterations; iter++)
                {
                    // === Отрисовка текущего шестиугольника ===

                    // Перемещаем перо в первую точку (code = 0)
                    s.xx = x[0]; s.yy = y[0]; s.ii = 0;
                    fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);

                    // Рисуем линии ко всем остальным вершинам и замыкаем фигуру (code = 1)
                    for (int i = 1; i <= n; i++)
                    {
                        s.xx = x[i % n]; s.yy = y[i % n]; s.ii = 1;
                        fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);
                    }

                    // === Вычисление координат следующего вложенного шестиугольника ===
                    double[] nextX = new double[n];
                    double[] nextY = new double[n];

                    for (int i = 0; i < n; i++)
                    {
                        int nextIdx = (i + 1) % n;
                        // Новая точка лежит на отрезке между текущей и следующей вершиной
                        nextX[i] = x[i] + p * (x[nextIdx] - x[i]);
                        nextY[i] = y[i] + p * (y[nextIdx] - y[i]);
                    }

                    // Обновляем массивы координат для следующей итерации
                    Array.Copy(nextX, x, n);
                    Array.Copy(nextY, y, n);
                }
            }
            MessageBox.Show("Сгенерировано 20 вложенных шестиугольников в файл SCRATCH!", "Успех");
        }

        private void b_read_from_file_Click(object sender, EventArgs e)
        {
            // Сюда ты вставляешь код преподавателя (Два прохода: первый для Xmin/Xmax, второй для рисования)
            // Ниже приведен каркас алгоритма из методички:

            if (!File.Exists("SCRATCH"))
            {
                MessageBox.Show("Сначала сгенерируйте файл!");
                return;
            }

            Graphics dc = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Brushes.Blue, 1);
            Simple s;

            double xmin = double.MaxValue, xmax = double.MinValue;
            double ymin = double.MaxValue, ymax = double.MinValue;

            // ПЕРВЫЙ ПРОХОД: Поиск минимальных и максимальных координат
            using (BinaryReader fr = new BinaryReader(File.Open("SCRATCH", FileMode.Open)))
            {
                while (fr.BaseStream.Position < fr.BaseStream.Length)
                {
                    double x = fr.ReadDouble();
                    double y = fr.ReadDouble();
                    int code = fr.ReadInt32();

                    if (x < xmin) xmin = x;
                    if (x > xmax) xmax = x;
                    if (y < ymin) ymin = y;
                    if (y > ymax) ymax = y;
                }
            }

            // Расчет коэффициентов масштабирования (по методичке)
            double fx = (Xmax - Xmin) / (xmax - xmin);
            double fy = (Ymax - Ymin) / (ymax - ymin);
            double f = (fx < fy) ? fx : fy;

            double xC = 0.5 * (xmin + xmax);
            double yC = 0.5 * (ymin + ymax);
            double XC = 0.5 * (Xmin + Xmax);
            double YC = 0.5 * (Ymin + Ymax);

            double c1 = XC - f * xC;
            double c2 = YC - f * yC;

            // ВТОРОЙ ПРОХОД: Отрисовка
            int Xold = 0, Yold = 0;
            using (BinaryReader fr = new BinaryReader(File.Open("SCRATCH", FileMode.Open)))
            {
                while (fr.BaseStream.Position < fr.BaseStream.Length)
                {
                    double x = fr.ReadDouble();
                    double y = fr.ReadDouble();
                    int code = fr.ReadInt32();

                    // Перевод в экранные координаты
                    double calcX = f * x + c1;
                    double calcY = f * y + c2;

                    int screenX = IX(calcX);
                    int screenY = IY(calcY);

                    if (code == 1)
                    {
                        dc.DrawLine(pen, Xold, Yold, screenX, screenY);
                    }
                    Xold = screenX;
                    Yold = screenY;
                }
            }
        }

        private int IX(double x)
        {
            return (int)(x * (pictureBox1.Size.Width / 10.0) + 0.5); // 10.0 - условная ширина окна из методички
        }

        private int IY(double y)
        {
            return (int)(pictureBox1.Size.Height - y * (pictureBox1.Size.Height / 7.0) + 0.5); // 7.0 - условная высота
        }

        private void b_clear_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh(); // Очищает PictureBox
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void b_exit_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void b_change_output_size_Click(object sender, EventArgs e)
        {
            // Изменяем область вывода согласно введенным в TextBox значениям
            try
            {
                Xmin = Convert.ToDouble(tb_x1.Text);
                Ymin = Convert.ToDouble(tb_y1.Text);
                Xmax = Convert.ToDouble(tb_x2.Text);
                Ymax = Convert.ToDouble(tb_y2.Text);
                MessageBox.Show("Координаты области вывода изменены.");
            }
            catch
            {
                MessageBox.Show("Ошибка! Введите корректные числовые значения.");
            }
        }
    }
}
