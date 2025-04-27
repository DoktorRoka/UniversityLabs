using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.IO;

namespace Laboratornaya3IVT1_variant7_KARTI
{
    public partial class Form1 : Form
    {

        private class Card
        {
            public Bitmap Image;
            public Point Position;
            public float Angle;
        }

        // лицевая сторона карт ещё в колоде
        private List<Bitmap> deckImages = new List<Bitmap>();
        // карты уже выброшенные на стол
        private List<Card> tableCards = new List<Card>();

        private readonly Size cardSize = new Size(100, 148);
        private readonly Point deckPosition = new Point(20, 20);
        private Bitmap backImage;         

        private Card draggingCard = null;
        private Point dragOffset;
        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();


            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            typeof(Panel).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
              .SetValue(panel1, true, null);

            this.DoubleBuffered = true;
            this.KeyPreview = true;

            panel1.Paint += panel1_Paint;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            this.KeyDown += Form1_KeyDown;

            LoadCards();
        }

        private void LoadCards()
        {
            deckImages.Clear();
            tableCards.Clear();

            backImage = new Bitmap(Path.Combine(Application.StartupPath, "cards", "rubashka.jpg"));

            var files = Directory.GetFiles(Path.Combine(Application.StartupPath, "cards"), "*.jpg")
                                 .Where(f => !f.EndsWith("rubashka.jpg", StringComparison.OrdinalIgnoreCase));
            foreach (var f in files)
            {
                using (var img = Image.FromFile(f))
                    deckImages.Add(new Bitmap(img, cardSize));
            }

            deckImages = deckImages.OrderBy(_ => rnd.Next()).ToList();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                foreach (var c in tableCards)
                    deckImages.Add(c.Image);
                tableCards.Clear();
                panel1.Invalidate();
                e.Handled = true;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var clickPoint = e.Location;
            var deckRect = new Rectangle(deckPosition, cardSize);

            if (deckImages.Count > 0 && deckRect.Contains(clickPoint))
            {
                var img = deckImages[0];
                deckImages.RemoveAt(0);

                var newCard = new Card
                {
                    Image = img,
                    Position = deckPosition,
                    Angle = 0f
                };
                tableCards.Add(newCard);

                draggingCard = newCard;
                dragOffset = new Point(clickPoint.X - deckPosition.X,
                                         clickPoint.Y - deckPosition.Y);
                panel1.Invalidate();
                return;
            }

            for (int i = tableCards.Count - 1; i >= 0; i--)
            {
                var c = tableCards[i];
                var rect = new Rectangle(c.Position, cardSize);
                if (rect.Contains(clickPoint))
                {
                    draggingCard = c;
                    dragOffset = new Point(clickPoint.X - c.Position.X,
                                             clickPoint.Y - c.Position.Y);
                    tableCards.RemoveAt(i);
                    tableCards.Add(c);
                    panel1.Invalidate();
                    break;
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingCard == null) return;
            var p = e.Location;
            draggingCard.Position = new Point(
                p.X - dragOffset.X,
                p.Y - dragOffset.Y);
            panel1.Invalidate();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggingCard == null) return;
            draggingCard.Angle = (float)(rnd.NextDouble() * 90 - 45);
            draggingCard = null;
            panel1.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new LinearGradientBrush(
                       panel1.ClientRectangle,
                       Color.FromArgb(9, 9, 121),
                       Color.FromArgb(40, 183, 212),
                       LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, panel1.ClientRectangle);
            }

            foreach (var c in tableCards)
            {
                g.TranslateTransform(
                    c.Position.X + cardSize.Width / 2,
                    c.Position.Y + cardSize.Height / 2);
                g.RotateTransform(c.Angle);
                g.TranslateTransform(
                    -cardSize.Width / 2,
                    -cardSize.Height / 2);

                g.DrawImage(c.Image,
                            new Rectangle(0, 0, cardSize.Width, cardSize.Height));
                g.ResetTransform();
            }

            if (deckImages.Count > 0)
            {
                g.DrawImage(backImage,
                            new Rectangle(deckPosition, cardSize));
            }
        }

    }
}
