namespace Laboratornaya3IVT7App1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьДиректориюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.операцииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьКопирайтToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.батчРежимToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копирайтТекстToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копирайтПапкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dg_file = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dg_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pb_current_picture = new System.Windows.Forms.PictureBox();
            this.b_batch_mode = new System.Windows.Forms.Button();
            this.b_save = new System.Windows.Forms.Button();
            this.b_add_copyright = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.p_all_pictures = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_current_picture)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.операцииToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(935, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.открытьДиректориюToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.открытьToolStripMenuItem.Text = "Открыть...";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // открытьДиректориюToolStripMenuItem
            // 
            this.открытьДиректориюToolStripMenuItem.Name = "открытьДиректориюToolStripMenuItem";
            this.открытьДиректориюToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.открытьДиректориюToolStripMenuItem.Text = "Открыть директорию...";
            this.открытьДиректориюToolStripMenuItem.Click += new System.EventHandler(this.открытьДиректориюToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exitToolStripMenuItem.Text = "Закрыть";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // операцииToolStripMenuItem
            // 
            this.операцииToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьКопирайтToolStripMenuItem,
            this.сохранитьИзображениеToolStripMenuItem,
            this.батчРежимToolStripMenuItem});
            this.операцииToolStripMenuItem.Name = "операцииToolStripMenuItem";
            this.операцииToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.операцииToolStripMenuItem.Text = "Операции";
            // 
            // добавитьКопирайтToolStripMenuItem
            // 
            this.добавитьКопирайтToolStripMenuItem.Name = "добавитьКопирайтToolStripMenuItem";
            this.добавитьКопирайтToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.добавитьКопирайтToolStripMenuItem.Text = "Добавить копирайт";
            this.добавитьКопирайтToolStripMenuItem.Click += new System.EventHandler(this.добавитьКопирайтToolStripMenuItem_Click);
            // 
            // сохранитьИзображениеToolStripMenuItem
            // 
            this.сохранитьИзображениеToolStripMenuItem.Name = "сохранитьИзображениеToolStripMenuItem";
            this.сохранитьИзображениеToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.сохранитьИзображениеToolStripMenuItem.Text = "Сохранить изображение...";
            this.сохранитьИзображениеToolStripMenuItem.Click += new System.EventHandler(this.сохранитьИзображениеToolStripMenuItem_Click);
            // 
            // батчРежимToolStripMenuItem
            // 
            this.батчРежимToolStripMenuItem.Name = "батчРежимToolStripMenuItem";
            this.батчРежимToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.батчРежимToolStripMenuItem.Text = "\"Батч\" режим";
            this.батчРежимToolStripMenuItem.Click += new System.EventHandler(this.батчРежимToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.копирайтТекстToolStripMenuItem,
            this.копирайтПапкиToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // копирайтТекстToolStripMenuItem
            // 
            this.копирайтТекстToolStripMenuItem.Name = "копирайтТекстToolStripMenuItem";
            this.копирайтТекстToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.копирайтТекстToolStripMenuItem.Text = "Копирайт текст...";
            this.копирайтТекстToolStripMenuItem.Click += new System.EventHandler(this.копирайтТекстToolStripMenuItem_Click);
            // 
            // копирайтПапкиToolStripMenuItem
            // 
            this.копирайтПапкиToolStripMenuItem.Name = "копирайтПапкиToolStripMenuItem";
            this.копирайтПапкиToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.копирайтПапкиToolStripMenuItem.Text = "Копирайт папки...";
            this.копирайтПапкиToolStripMenuItem.Click += new System.EventHandler(this.копирайтПапкиToolStripMenuItem_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.помощьToolStripMenuItem.Text = "Помощь";
            this.помощьToolStripMenuItem.Click += new System.EventHandler(this.помощьToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dg_file,
            this.dg_width,
            this.dg_height,
            this.dg_text});
            this.dataGridView1.Location = new System.Drawing.Point(244, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(679, 205);
            this.dataGridView1.TabIndex = 1;
            // 
            // dg_file
            // 
            this.dg_file.HeaderText = "Файл";
            this.dg_file.Name = "dg_file";
            this.dg_file.Width = 250;
            // 
            // dg_width
            // 
            this.dg_width.HeaderText = "Ширина";
            this.dg_width.Name = "dg_width";
            // 
            // dg_height
            // 
            this.dg_height.HeaderText = "Высота";
            this.dg_height.Name = "dg_height";
            // 
            // dg_text
            // 
            this.dg_text.HeaderText = "Текст";
            this.dg_text.Name = "dg_text";
            this.dg_text.Width = 170;
            // 
            // pb_current_picture
            // 
            this.pb_current_picture.Location = new System.Drawing.Point(244, 238);
            this.pb_current_picture.Name = "pb_current_picture";
            this.pb_current_picture.Size = new System.Drawing.Size(421, 309);
            this.pb_current_picture.TabIndex = 2;
            this.pb_current_picture.TabStop = false;
            // 
            // b_batch_mode
            // 
            this.b_batch_mode.BackColor = System.Drawing.Color.Khaki;
            this.b_batch_mode.Location = new System.Drawing.Point(671, 497);
            this.b_batch_mode.Name = "b_batch_mode";
            this.b_batch_mode.Size = new System.Drawing.Size(252, 50);
            this.b_batch_mode.TabIndex = 3;
            this.b_batch_mode.Text = "\"Батч\" режим";
            this.b_batch_mode.UseVisualStyleBackColor = false;
            this.b_batch_mode.Click += new System.EventHandler(this.батчРежимToolStripMenuItem_Click);
            // 
            // b_save
            // 
            this.b_save.BackColor = System.Drawing.Color.Khaki;
            this.b_save.Location = new System.Drawing.Point(671, 441);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(252, 50);
            this.b_save.TabIndex = 4;
            this.b_save.Text = "Сохранить картинку";
            this.b_save.UseVisualStyleBackColor = false;
            this.b_save.Click += new System.EventHandler(this.сохранитьИзображениеToolStripMenuItem_Click);
            // 
            // b_add_copyright
            // 
            this.b_add_copyright.BackColor = System.Drawing.Color.Khaki;
            this.b_add_copyright.Location = new System.Drawing.Point(671, 385);
            this.b_add_copyright.Name = "b_add_copyright";
            this.b_add_copyright.Size = new System.Drawing.Size(252, 50);
            this.b_add_copyright.TabIndex = 5;
            this.b_add_copyright.Text = "Добавить копирайт";
            this.b_add_copyright.UseVisualStyleBackColor = false;
            this.b_add_copyright.Click += new System.EventHandler(this.добавитьКопирайтToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // p_all_pictures
            // 
            this.p_all_pictures.AutoScroll = true;
            this.p_all_pictures.AutoScrollMinSize = new System.Drawing.Size(0, 1000);
            this.p_all_pictures.Location = new System.Drawing.Point(12, 27);
            this.p_all_pictures.Name = "p_all_pictures";
            this.p_all_pictures.Size = new System.Drawing.Size(226, 520);
            this.p_all_pictures.TabIndex = 6;
            this.p_all_pictures.Paint += new System.Windows.Forms.PaintEventHandler(this.p_all_pictures_Paint_1);
            this.p_all_pictures.MouseClick += new System.Windows.Forms.MouseEventHandler(this.p_all_pictures_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(935, 559);
            this.Controls.Add(this.p_all_pictures);
            this.Controls.Add(this.b_add_copyright);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.b_batch_mode);
            this.Controls.Add(this.pb_current_picture);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(951, 598);
            this.MinimumSize = new System.Drawing.Size(951, 598);
            this.Name = "Form1";
            this.Text = "Копирайтер";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_current_picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьДиректориюToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem операцииToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьКопирайтToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьИзображениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem батчРежимToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копирайтТекстToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копирайтПапкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox pb_current_picture;
        private System.Windows.Forms.Button b_batch_mode;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.Button b_add_copyright;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_file;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_width;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_height;
        private System.Windows.Forms.DataGridViewTextBoxColumn dg_text;
        private System.Windows.Forms.Panel p_all_pictures;
    }
}

