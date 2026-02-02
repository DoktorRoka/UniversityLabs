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

namespace Laba6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            this.DoubleBuffered = true;
            this.ClientSize = new Size(800, 500);
            this.Text = "Летающая тарелка — приземление";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // --- 1) Фон: градиент неба и поляны ---
            Rectangle skyRect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            LinearGradientBrush skyBrush = new LinearGradientBrush(skyRect, Color.MidnightBlue, Color.CornflowerBlue, 90f);
            g.FillRectangle(skyBrush, skyRect);
            // не забываем освободить позже
            skyBrush.Dispose();

            // Поляна
            Rectangle meadowRect = new Rectangle(0, ClientSize.Height - 150, ClientSize.Width, 150);
            LinearGradientBrush meadow = new LinearGradientBrush(meadowRect, Color.ForestGreen, Color.LimeGreen, 0f);
            g.FillRectangle(meadow, meadowRect);
            meadow.Dispose();

            // Звезды (маленькие эллипсы) — используем статические кисти (не диспоузим)
            Random rnd = new Random(12345);
            for (int i = 0; i < 40; i++)
            {
                int sx = rnd.Next(0, ClientSize.Width);
                int sy = rnd.Next(0, 120);
                int size = rnd.Next(1, 4);
                g.FillEllipse(Brushes.WhiteSmoke, sx, sy, size, size);
            }

            // --- Тень от НЛО (полупрозрачный эллипс) ---
            SolidBrush shadow = new SolidBrush(Color.FromArgb(100, 10, 10, 10));
            int sw = 260, sh = 60;
            int sx2 = ClientSize.Width / 2 - sw / 2;
            int sy2 = ClientSize.Height - 110;
            g.FillEllipse(shadow, sx2, sy2, sw, sh);
            shadow.Dispose();

            // --- Луч света: создаём GraphicsPath и LinearGradientBrush ---
            GraphicsPath beamPath = new GraphicsPath();
            Point beamTop = new Point(ClientSize.Width / 2, 250);
            Point beamLeft = new Point(ClientSize.Width / 2 - 90, ClientSize.Height - 130);
            Point beamRight = new Point(ClientSize.Width / 2 + 90, ClientSize.Height - 130);
            beamPath.AddPolygon(new Point[] { beamTop, beamRight, beamLeft });

            Rectangle beamBounds = new Rectangle(beamLeft.X, beamTop.Y, beamRight.X - beamLeft.X, beamLeft.Y - beamTop.Y);
            LinearGradientBrush beamBrush = new LinearGradientBrush(beamBounds, Color.FromArgb(180, 255, 255, 180), Color.FromArgb(60, 160, 255, 120), LinearGradientMode.Vertical);
            g.FillPath(beamBrush, beamPath);
            beamBrush.Dispose();
            // NOTE: оставляем beamPath для возможного рисования границ, но тут просто удалим
            beamPath.Dispose();

            // --- Травинки на поляне (DrawCurve с Pen) ---
            Pen grassPen = new Pen(Color.DarkGreen, 1.2f);
            int startX = 30;
            for (int i = 0; i < 40; i++)
            {
                int x = startX + i * 18;
                int y0 = ClientSize.Height - 30;
                Point p1 = new Point(x, y0);
                Point p2 = new Point(x - 6, y0 - 18 - rnd.Next(0, 12));
                Point p3 = new Point(x + 6, y0 - 30 - rnd.Next(0, 30));
                g.DrawCurve(grassPen, new Point[] { p1, p2, p3 });
            }
            grassPen.Dispose();

            // --- Сам аппарат ---
            int cx = ClientSize.Width / 2;
            int cy = 200;

            // Нижняя часть корпуса — использую GraphicsPath + PathGradientBrush (создал и удалю)
            Rectangle bodyLower = new Rectangle(cx - 180 / 2, cy + 20, 180, 60);
            GraphicsPath bodyLowerPath = new GraphicsPath();
            bodyLowerPath.AddEllipse(bodyLower);
            PathGradientBrush pgb = new PathGradientBrush(bodyLowerPath);
            pgb.CenterColor = Color.Silver;
            pgb.SurroundColors = new Color[] { Color.DimGray };
            g.FillPath(pgb, bodyLowerPath);
            g.DrawEllipse(Pens.Black, bodyLower);
            // уничтожаем
            pgb.Dispose();
            bodyLowerPath.Dispose();

            // Средняя секция корпуса
            Rectangle bodyMid = new Rectangle(cx - 240 / 2, cy - 10, 240, 80);
            LinearGradientBrush lg = new LinearGradientBrush(bodyMid, Color.LightGray, Color.DarkGray, LinearGradientMode.Vertical);
            g.FillEllipse(lg, bodyMid);
            g.DrawEllipse(Pens.Black, bodyMid);
            lg.Dispose();

            // Верхняя куполообразная кабина — нарисуем верхнюю дугу и полупрозрачное стекло
            

            // Декоративная золотая полоска
            Pen strip = new Pen(Color.Gold, 6f);
            g.DrawEllipse(strip, new Rectangle(cx - 200 / 2, cy - 5, 200, 40));
            strip.Dispose();

            // Огоньки по кругу (маленькие эллипсы). Используем локальные кисти и сразу диспоузим.
            int lightsCount = 8;
            for (int i = 0; i < lightsCount; i++)
            {
                double angle = i * (2 * Math.PI / lightsCount);
                int rx = (int)(Math.Cos(angle) * 90);
                int ry = (int)(Math.Sin(angle) * 20);
                int lx = cx + rx - 6;
                int ly = cy + ry + 10;
                Color lightCol = (i % 3 == 0) ? Color.Yellow : (i % 3 == 1 ? Color.Lime : Color.Orange);
                SolidBrush lb = new SolidBrush(lightCol);
                g.FillEllipse(lb, lx, ly, 12, 8);
                g.DrawEllipse(Pens.Black, lx, ly, 12, 8);
                lb.Dispose();
            }

            Rectangle dome = new Rectangle(cx - 100 / 2, cy - 40, 100, 100);
            GraphicsPath domePath = new GraphicsPath();
            domePath.AddArc(dome, 180, 180); // верхняя часть
            SolidBrush glass = new SolidBrush(Color.FromArgb(180, 180, 220, 255));
            g.FillPath(glass, domePath);
            g.DrawArc(Pens.DarkBlue, dome, 180, 180);
            domePath.Dispose();
            glass.Dispose();

            // Опорные ноги и подушки
            Pen legPen = new Pen(Color.SlateGray, 6f);
            g.DrawLine(legPen, cx - 60, cy + 50, cx - 110, cy + 120);
            g.DrawLine(legPen, cx + 60, cy + 50, cx + 110, cy + 120);
            g.FillEllipse(Brushes.DarkSlateGray, cx - 125, cy + 115, 40, 14);
            g.FillEllipse(Brushes.DarkSlateGray, cx + 85, cy + 115, 40, 14);
            legPen.Dispose();

            

            // Камень и дерево (статические кисти и Pens — не диспоузим)
            g.FillEllipse(Brushes.DarkGray, 80, ClientSize.Height - 100, 60, 34);
            g.DrawEllipse(Pens.Black, 80, ClientSize.Height - 100, 60, 34);

            g.FillRectangle(Brushes.SaddleBrown, 680, ClientSize.Height - 170, 8, 60);
            g.FillEllipse(Brushes.DarkGreen, 660, ClientSize.Height - 200, 48, 48);
            g.DrawEllipse(Pens.Black, 660, ClientSize.Height - 200, 48, 48);

            // Подпись — создадим Font и удалим после использования
            Font f = new Font("Arial", 12, FontStyle.Bold);
            g.DrawString("Летающая тарелка приземляется", f, Brushes.WhiteSmoke, 12, 12);
            f.Dispose();

            // если нужно, можно вызвать base.OnPaint
            base.OnPaint(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
