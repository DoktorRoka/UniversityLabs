using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba5_grafika
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();

            using (var g = this.CreateGraphics())
            {
                g.DrawString("Кликните мышкой по элементу PictureBox",
                    new Font("Arial", 10, FontStyle.Regular),
                    Brushes.Red, 5, 5);
            }

            pictureBox1.BackColor = Color.WhiteSmoke;

            pictureBox1.Click += pictureBox1_Click;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                Point[] poly =
                {
                    new Point(120, 40), new Point(160, 20), new Point(210, 30),
                    new Point(260, 20), new Point(310, 30), new Point(360, 20),
                    new Point(410, 30), new Point(430, 70), new Point(380, 85),
                    new Point(200, 85), new Point(120, 70)
                };
                using (var fill = new HatchBrush(HatchStyle.Wave, Color.MediumPurple, Color.White))
                using (var pen = new Pen(Color.MediumBlue, 2))
                { g.FillPolygon(fill, poly); g.DrawPolygon(pen, poly); }

                var title = "Оценки по программированию\nдевяти студентов группы";
                using (var fn = new Font("Arial", 10, FontStyle.Bold))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(title, fn, Brushes.Crimson, new Rectangle(125, 25, 300, 60), sf);
                }

                // Рамка
                g.DrawRectangle(Pens.MidnightBlue, 0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);

                string[] students = { "Ст1", "Ст2", "Ст3", "Ст4", "Ст5", "Ст6", "Ст7", "Ст8", "Ст9" };
                int[] grades = { 5, 4, 3, 5, 4, 4, 5, 3, 4 }; // ← замените на свои

                // Поля области построения
                int left = 55, right = pictureBox1.Width - 20;
                int top = 120, bottom = pictureBox1.Height - 40;

                // Оси (нулевой уровень – ось X)
                g.DrawLine(Pens.Black, left, bottom, right, bottom);
                g.DrawLine(Pens.Black, left, top, left, bottom);

                // Масштаб по пятибалльной шкале
                int max = Math.Max(5, grades.Max()); // максимум минимум 5
                float k = (bottom - top) / (float)max; // px на 1 балл

                // Сетка и подписи по Y (0..5)
                using (var gridPen = new Pen(Color.RoyalBlue, 1.6f) { DashStyle = DashStyle.Dash })
                using (var yFont = new Font("Arial", 8, FontStyle.Bold))
                {
                    var sfY = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                    for (int v = 0; v <= max; v++)
                    {
                        float y = bottom - v * k;
                        g.DrawLine(gridPen, left - 5, y, right, y);
                        g.DrawString(v.ToString(), yFont, Brushes.DarkBlue,
                                     new RectangleF(2, y - 9, left - 8, 18), sfY);
                    }
                }

                // Геометрия столбцов
                int n = grades.Length;
                int gap = 10;
                int plotWidth = (right - left);
                int barWidth = (plotWidth - (n + 1) * gap) / n;
                int x = left + gap;

                // Кисти
                using (var solid = new SolidBrush(Color.CornflowerBlue))
                using (var hatch = new HatchBrush(HatchStyle.BackwardDiagonal, Color.CornflowerBlue, Color.LightSkyBlue))
                using (var img = Image.FromFile("img.bmp"))
            
                using (var texture = new TextureBrush(img))
                using (var barPen = new Pen(Color.Black, 1))
                using (var xFont = new Font("Arial", 8, FontStyle.Bold))
                using (var valFont = new Font("Arial", 8, FontStyle.Regular))

                {
                    var sfX = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

                    for (int i = 0; i < n; i++)
                    {
                        int h = (int)Math.Round(grades[i] * k);
                        var rect = new Rectangle(x, bottom - h, barWidth, h);

                        if (i < 3) g.FillRectangle(solid, rect);
                        else if (i < 6) g.FillRectangle(hatch, rect);
                        else g.FillRectangle(texture, rect);

                        g.DrawRectangle(barPen, rect);

                        // метка студента
                        int tickX = x + barWidth / 2;
                        g.DrawLine(Pens.Black, tickX, bottom - 5, tickX, bottom + 5);
                        g.DrawString(students[i], xFont, Brushes.Black,
                                     new Rectangle(tickX - 25, bottom + 6, 50, 18), sfX);

                        // подпись значения над столбцом
                        g.DrawString(grades[i].ToString(), valFont, Brushes.Black,
                                     new PointF(tickX - 6, bottom - h - 14));

                        x += barWidth + gap;
                    }
                }
            }
        }
        private static Bitmap MakeTextureTile()
        {
            var bmp = new Bitmap(8, 8);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.MidnightBlue);
                using (var b = new SolidBrush(Color.SteelBlue))
                { g.FillRectangle(b, 0, 0, 4, 4); g.FillRectangle(b, 4, 4, 4, 4); }
            }
            return bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
