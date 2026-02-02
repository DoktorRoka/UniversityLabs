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

namespace Laba7
{
    public partial class Form1 : Form
    {
        Bitmap backgroundBitmap;
        Bitmap spriteBitMap;
        Graphics g_sprite;

        // Координаты и размеры ракеты (спрайта)
        int rocketX, rocketY;
        int rocketWidth = 80;
        int rocketHeight = 200;

        // Таймер анимации
        Timer timer;

        // Для мигания пламени
        bool flameToggle = false;

        // Скорость по Y
        int dy = 8;

        public Form1()
        {
            InitializeComponent();

        
            pictureBox1.Paint += PictureBox1_Paint; // ВАЖНО!
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer == null) return;

            if (!timer.Enabled)
            {
                // Если ракета уже ушла — сбросим позицию
                if (rocketY + rocketHeight < 0)
                {
                    rocketY = pictureBox1.Height - rocketHeight - 40;
                }
                timer.Enabled = true;
                button1.Text = "Стоп";
            }
            else
            {
                timer.Enabled = false;
                button1.Text = "Старт";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём/инициализируем фон pictureBox
            try
            {
                backgroundBitmap = new Bitmap(Image.FromFile("cosmodrome.jpg"));
            }
            catch
            {
                // рисуем простой фон программно
                backgroundBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(backgroundBitmap);

                // градиент неба
                LinearGradientBrush sky = new LinearGradientBrush(
                    new Rectangle(0, 0, backgroundBitmap.Width, backgroundBitmap.Height),
                    Color.LightSkyBlue, Color.DarkBlue, LinearGradientMode.Vertical);
                g.FillRectangle(sky, 0, 0, backgroundBitmap.Width, backgroundBitmap.Height);

                // земля / пусковой стол
                g.FillRectangle(Brushes.DimGray, 0, backgroundBitmap.Height - 140, backgroundBitmap.Width, 140);
                g.DrawRectangle(new Pen(Color.Black, 2), 20, backgroundBitmap.Height - 120, 120, 100);

                // простые башни/опоры
                g.FillRectangle(Brushes.Silver, backgroundBitmap.Width / 2 - 160, backgroundBitmap.Height - 300, 10, 160);
                g.FillRectangle(Brushes.Silver, backgroundBitmap.Width / 2 + 150, backgroundBitmap.Height - 300, 10, 160);

                // текст подсказка

                g.Dispose();
                sky.Dispose();
            }

            // Создаём bitmap спрайта
            spriteBitMap = new Bitmap(rocketWidth, rocketHeight);
            g_sprite = Graphics.FromImage(spriteBitMap);
            g_sprite.SmoothingMode = SmoothingMode.AntiAlias;

            // Начальные координаты ракеты
            rocketX = (pictureBox1.Width - rocketWidth) / 2;
            rocketY = pictureBox1.Height - rocketHeight - 40;

            // Нарисуем спрайт первый раз
            DrawRocketSprite();

            // Настройка timer
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += timer_Tick;

            // Первая отрисовка
            pictureBox1.Invalidate();
        }

        // КЛЮЧЕВОЙ МЕТОД - рисование через Paint
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (backgroundBitmap == null) return;

            // Рисуем фон
            e.Graphics.DrawImage(backgroundBitmap, 0, 0);

            // Рисуем ракету
            if (spriteBitMap != null)
            {
                e.Graphics.DrawImage(spriteBitMap, rocketX, rocketY);
            }
        }

        void DrawRocketSprite()
        {
            // Очищаем спрайт
            g_sprite.Clear(Color.Transparent);

            // Корпус ракеты
            Point[] body = new Point[]
            {
                new Point(rocketWidth/2, 0),
                new Point(rocketWidth - 10, 50),
                new Point(rocketWidth - 10, rocketHeight - 40),
                new Point(10, rocketHeight - 40),
                new Point(10, 50),
                new Point(rocketWidth/2, 0)
            };
            g_sprite.FillPolygon(Brushes.White, body);
            g_sprite.DrawPolygon(new Pen(Color.Black, 2), body);

            // Окно кабины
            g_sprite.FillEllipse(Brushes.LightBlue, rocketWidth / 2 - 18, 40, 36, 36);
            g_sprite.DrawEllipse(new Pen(Color.DarkBlue, 2), rocketWidth / 2 - 18, 40, 36, 36);

            // Хвостовые стабилизаторы
            Point[] finL = new Point[] { new Point(10, rocketHeight - 40), new Point(0, rocketHeight - 10), new Point(30, rocketHeight - 40) };
            Point[] finR = new Point[] { new Point(rocketWidth - 10, rocketHeight - 40), new Point(rocketWidth, rocketHeight - 10), new Point(rocketWidth - 30, rocketHeight - 40) };
            g_sprite.FillPolygon(Brushes.Red, finL);
            g_sprite.DrawPolygon(new Pen(Color.Black, 1), finL);
            g_sprite.FillPolygon(Brushes.Red, finR);
            g_sprite.DrawPolygon(new Pen(Color.Black, 1), finR);

            // Обводка корпуса
            g_sprite.DrawLine(new Pen(Color.Gray, 1), rocketWidth / 2, 10, rocketWidth / 2, rocketHeight - 50);

            if (flameToggle)
            {
                Point[] flame = new Point[]
                {
                    new Point(rocketWidth/2 - 18, rocketHeight - 30),
                    new Point(rocketWidth/2, rocketHeight + 40),
                    new Point(rocketWidth/2 + 18, rocketHeight - 30)
                };
                g_sprite.FillPolygon(Brushes.Orange, flame);
                g_sprite.FillPolygon(Brushes.Yellow, new Point[] {
                    new Point(rocketWidth/2 - 10, rocketHeight - 20),
                    new Point(rocketWidth/2, rocketHeight + 20),
                    new Point(rocketWidth/2 + 10, rocketHeight - 20)
                });
                g_sprite.DrawPolygon(new Pen(Color.Orange, 1), flame);
            }
            else
            {
                Point[] flame2 = new Point[]
                {
                    new Point(rocketWidth/2 - 14, rocketHeight - 30),
                    new Point(rocketWidth/2, rocketHeight + 20),
                    new Point(rocketWidth/2 + 14, rocketHeight - 30)
                };
                g_sprite.FillPolygon(Brushes.OrangeRed, flame2);
                g_sprite.FillPolygon(Brushes.Yellow, new Point[] {
                    new Point(rocketWidth/2 - 8, rocketHeight - 18),
                    new Point(rocketWidth/2, rocketHeight + 10),
                    new Point(rocketWidth/2 + 8, rocketHeight - 18)
                });
                g_sprite.DrawPolygon(new Pen(Color.OrangeRed, 1), flame2);
            }

            g_sprite.DrawRectangle(new Pen(Color.Black, 1), 5, 48, rocketWidth - 10, 4);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            rocketY -= dy;

            flameToggle = !flameToggle;

            if (rocketY + rocketHeight < 0)
            {
                timer.Enabled = false;
                button1.Text = "Старт";
                MessageBox.Show("Ракета ушла в космос!");
                return;
            }

            DrawRocketSprite();

            pictureBox1.Invalidate();
        }
    }
}