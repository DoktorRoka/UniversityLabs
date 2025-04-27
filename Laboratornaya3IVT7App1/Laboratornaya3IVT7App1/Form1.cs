using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Laboratornaya3IVT7App1;



namespace Laboratornaya3IVT7App1
{


    public partial class Form1 : Form
    {

        private AppSettings appSettings;

        private List<ImageInfo> myImages = new List<ImageInfo>();

        public Form1()
        {
            InitializeComponent();


            appSettings = SettingsManager.LoadSettings();

            p_all_pictures.Paint += p_all_pictures_Paint;
            p_all_pictures.KeyDown += p_all_pictures_KeyDown;

            p_all_pictures.TabStop = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ClearImages();
                try
                {
                    Bitmap bmp = new Bitmap(ofd.FileName);
                    string filename = Path.GetFileName(ofd.FileName);
                    bool hasWatermark = filename.StartsWith("Watermarked_");

                    ImageInfo info = new ImageInfo
                    {
                        Image = bmp,
                        HasWatermark = hasWatermark,
                        CopyrightText = hasWatermark ? appSettings.CopyrightText : "",
                        FileName = filename
                    };
                    myImages.Add(info);
                    p_all_pictures.Invalidate();
                    UpdateImageDataTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
            UpdateImageDataTable();
            RecalculateScrollArea();
        }

        private void открытьДиректориюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClearImages();
                foreach (string imagefile in Directory.EnumerateFiles(fbd.SelectedPath))
                {
                    try
                    {
                        Bitmap img = new Bitmap(imagefile);
                        string filename = Path.GetFileName(imagefile);
                        bool hasWatermark = filename.StartsWith("Watermarked_");

                        ImageInfo info = new ImageInfo
                        {
                            Image = img,
                            HasWatermark = hasWatermark,
                            CopyrightText = hasWatermark ? appSettings.CopyrightText : "",
                            FileName = filename
                        };
                        myImages.Add(info);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка загрузки файла: " + imagefile + "\n" + ex.Message);
                    }
                }

                p_all_pictures.Invalidate();
            }
            UpdateImageDataTable();
            RecalculateScrollArea();
        }


        private void p_all_pictures_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(p_all_pictures.BackColor);
            Point scrollOffset = p_all_pictures.AutoScrollPosition;

            for (int i = 0; i < myImages.Count; i++)
            {
                int x = 50 + scrollOffset.X;
                int y = 150 * i + 30 + scrollOffset.Y;
                int width = 130;
                int height = 130;

                if (i == selectedImageIndex)
                {
                    Rectangle selectionRect = new Rectangle(x - 3, y - 3, width + 6, height + 6);
                    e.Graphics.DrawRectangle(new Pen(Color.Blue, 2), selectionRect);
                }

                e.Graphics.DrawImage(myImages[i].Image, x, y, width, height);

                if (myImages[i].HasWatermark)
                {
                    Rectangle iconRect = new Rectangle(x + width - 20, y + height - 20, 18, 18);
                    e.Graphics.FillRectangle(Brushes.Yellow, iconRect);
                    e.Graphics.DrawString("W", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, iconRect);
                }
            }
        }

        private void ClearImages()
        {
            foreach (var info in myImages)
            {
                info.Image.Dispose();
            }
            myImages.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ClearImages();
            base.OnFormClosing(e);
        }

        private void p_all_pictures_Paint_1(object sender, PaintEventArgs e)
        {

        }
        public class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                this.DoubleBuffered = true;
            }
        }

        private void p_all_pictures_MouseClick(object sender, MouseEventArgs e)
        {
            p_all_pictures.Focus();

            Point virtualClickPoint = new Point(
                e.X - p_all_pictures.AutoScrollPosition.X,
                e.Y - p_all_pictures.AutoScrollPosition.Y);

            selectedImageIndex = -1;

            for (int i = 0; i < myImages.Count; i++)
            {
                int x = 50;
                int y = 150 * i + 30;
                Rectangle imageBounds = new Rectangle(x, y, 130, 130);

                if (imageBounds.Contains(virtualClickPoint))
                {
                    selectedImageIndex = i;

                    pb_current_picture.Image = myImages[i].Image;
                    pb_current_picture.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                }
            }

            p_all_pictures.Invalidate();
        }

        private void добавитьКопирайтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pb_current_picture.Image != null)
            {
                int index = myImages.FindIndex(info => info.Image == pb_current_picture.Image);
                if (index >= 0)
                {
                    ImageInfo selectedImage = myImages[index];

                    Bitmap watermarkedImage = (Bitmap)selectedImage.Image.Clone();

                    using (Graphics g = Graphics.FromImage(watermarkedImage))
                    {
                        string watermark = appSettings.CopyrightText;
                        Font font = new Font("Arial", 20, FontStyle.Bold);
                        Color color = Color.FromArgb(128, Color.White);
                        Brush brush = new SolidBrush(color);

                        SizeF textSize = g.MeasureString(watermark, font);
                        Point position = new Point(watermarkedImage.Width - (int)textSize.Width - 10,
                                                   watermarkedImage.Height - (int)textSize.Height - 10);

                        g.DrawString(watermark, font, Brushes.Black, new Point(position.X + 2, position.Y + 2));
                        g.DrawString(watermark, font, brush, position);
                    }

                    Bitmap oldImage = selectedImage.Image;

                    selectedImage.Image = watermarkedImage;
                    selectedImage.HasWatermark = true;
                    selectedImage.CopyrightText = appSettings.CopyrightText;
                    pb_current_picture.Image = watermarkedImage;

                    if (!object.ReferenceEquals(oldImage, watermarkedImage))
                    {
                        oldImage.Dispose();
                    }

                    p_all_pictures.Invalidate();
                    UpdateImageDataTable();
                }
                else
                {
                    MessageBox.Show("Selected image not found in gallery.");
                }
            }
            else
            {
                MessageBox.Show("No image is currently selected!");
            }
        }
        private void сохранитьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pb_current_picture.Image == null)
            {
                MessageBox.Show("Нет изображения для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(appSettings.SaveFolder))
            {
                try
                {
                    Directory.CreateDirectory(appSettings.SaveFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось создать папку для сохранения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int index = myImages.FindIndex(info => info.Image == pb_current_picture.Image);
            bool hasWatermark = (index >= 0) ? myImages[index].HasWatermark : false;

            string prefix = hasWatermark ? "Watermarked_" : "Image_";
            string fileName = Path.Combine(appSettings.SaveFolder, prefix + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");

            try
            {
                pb_current_picture.Image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Изображение успешно сохранено: " + fileName, "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения изображения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void батчРежимToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(appSettings.SaveFolder))
            {
                try
                {
                    Directory.CreateDirectory(appSettings.SaveFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось создать папку для сохранения: " + ex.Message,
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int processedCount = 0;
            for (int i = 0; i < myImages.Count; i++)
            {
                ImageInfo info = myImages[i];
                Bitmap watermarkedImage = new Bitmap(info.Image);
                try
                {
                    using (Graphics g = Graphics.FromImage(watermarkedImage))
                    {
                        string watermark = appSettings.CopyrightText;
                        Font font = new Font("Arial", 20, FontStyle.Bold);
                        Color color = Color.FromArgb(128, Color.White);
                        Brush brush = new SolidBrush(color);

                        SizeF textSize = g.MeasureString(watermark, font);
                        Point position = new Point(watermarkedImage.Width - (int)textSize.Width - 10,
                                                   watermarkedImage.Height - (int)textSize.Height - 10);

                        g.DrawString(watermark, font, Brushes.Black, new Point(position.X + 2, position.Y + 2));
                        g.DrawString(watermark, font, brush, position);
                    }

                    string fileName = Path.Combine(appSettings.SaveFolder,
                                        $"Watermarked_{DateTime.Now:yyyyMMdd_HHmmss}_{i}.png");
                    watermarkedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

                    if (pb_current_picture.Image == info.Image)
                    {
                        pb_current_picture.Image = watermarkedImage;
                    }

                    if (info.Image != null)
                    {
                        info.Image.Dispose();
                    }

                    info.Image = watermarkedImage;
                    info.HasWatermark = true;
                    info.CopyrightText = appSettings.CopyrightText;

                    processedCount++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обработки изображения " + i + ": " + ex.Message,
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            p_all_pictures.Invalidate();
            UpdateImageDataTable();

            MessageBox.Show($"Пакетное сохранение завершено. Обработано изображений: {processedCount}.",
                            "Batch Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void RecalculateScrollArea()
        {
            if (myImages.Count == 0)
            {
                p_all_pictures.AutoScrollMinSize = new Size(0, 0);
                return;
            }

            int totalHeight = (myImages.Count * 150) + 50;
            int minWidth = 200;

            p_all_pictures.AutoScrollMinSize = new Size(minWidth, totalHeight);
        }

        private void копирайтПапкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выбор папки сохранения.";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    appSettings.SaveFolder = folderDialog.SelectedPath;
                    SettingsManager.SaveSettings(appSettings);
                    MessageBox.Show("Папка выбрана!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void копирайтТекстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (TextSettings ts = new TextSettings(appSettings.CopyrightText))
            {
                if (ts.ShowDialog() == DialogResult.OK)
                {
                    string newCopyrightText = ts.InputText;

                    if (!string.IsNullOrEmpty(newCopyrightText))
                    {
                        appSettings.CopyrightText = newCopyrightText;

                        SettingsManager.SaveSettings(appSettings);

                        MessageBox.Show("Текст копирайта обновлен!",
                                        "Успешно",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели пустой текст. Оставляем без изменений",
                                        "Внимание",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public class ImageInfo
        {
            public Bitmap Image { get; set; }
            public bool HasWatermark { get; set; }
            public string CopyrightText { get; set; }
            public string FileName { get; set; }
        }
        private void UpdateImageDataTable()
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < myImages.Count; i++)
            {
                ImageInfo info = myImages[i];

                string imageName = info.FileName;

                int width = info.Image.Width;
                int height = info.Image.Height;

                string copyrightText = info.HasWatermark ? info.CopyrightText : "";

                dataGridView1.Rows.Add(imageName, width, height, copyrightText);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private int selectedImageIndex = -1;  

        private void p_all_pictures_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && selectedImageIndex >= 0 && selectedImageIndex < myImages.Count)
            {
                Bitmap imageToDispose = myImages[selectedImageIndex].Image;

                myImages.RemoveAt(selectedImageIndex);

                if (pb_current_picture.Image == imageToDispose)
                {
                    pb_current_picture.Image = null;
                }

                imageToDispose.Dispose();

                selectedImageIndex = -1;

                p_all_pictures.Invalidate();
                RecalculateScrollArea();
                UpdateImageDataTable();

                MessageBox.Show("Изображение удалено из галереи.", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }


}