namespace GrafikaLab3
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.b_to_scratch = new System.Windows.Forms.Button();
            this.b_read_from_file = new System.Windows.Forms.Button();
            this.b_clear = new System.Windows.Forms.Button();
            this.b_exit = new System.Windows.Forms.Button();
            this.b_change_output_size = new System.Windows.Forms.Button();
            this.tb_x1 = new System.Windows.Forms.TextBox();
            this.tb_y1 = new System.Windows.Forms.TextBox();
            this.tb_y2 = new System.Windows.Forms.TextBox();
            this.tb_x2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(531, 355);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // b_to_scratch
            // 
            this.b_to_scratch.Location = new System.Drawing.Point(12, 382);
            this.b_to_scratch.Name = "b_to_scratch";
            this.b_to_scratch.Size = new System.Drawing.Size(152, 23);
            this.b_to_scratch.TabIndex = 1;
            this.b_to_scratch.Text = "Записать в файл в скретч";
            this.b_to_scratch.UseVisualStyleBackColor = true;
            this.b_to_scratch.Click += new System.EventHandler(this.b_to_scratch_Click);
            // 
            // b_read_from_file
            // 
            this.b_read_from_file.Location = new System.Drawing.Point(170, 382);
            this.b_read_from_file.Name = "b_read_from_file";
            this.b_read_from_file.Size = new System.Drawing.Size(198, 23);
            this.b_read_from_file.TabIndex = 2;
            this.b_read_from_file.Text = "Прочитать из файла и отобразить";
            this.b_read_from_file.UseVisualStyleBackColor = true;
            this.b_read_from_file.Click += new System.EventHandler(this.b_read_from_file_Click);
            // 
            // b_clear
            // 
            this.b_clear.Location = new System.Drawing.Point(374, 382);
            this.b_clear.Name = "b_clear";
            this.b_clear.Size = new System.Drawing.Size(152, 23);
            this.b_clear.TabIndex = 3;
            this.b_clear.Text = "Очистить";
            this.b_clear.UseVisualStyleBackColor = true;
            this.b_clear.Click += new System.EventHandler(this.b_clear_Click);
            // 
            // b_exit
            // 
            this.b_exit.Location = new System.Drawing.Point(636, 415);
            this.b_exit.Name = "b_exit";
            this.b_exit.Size = new System.Drawing.Size(152, 23);
            this.b_exit.TabIndex = 5;
            this.b_exit.Text = "Выход";
            this.b_exit.UseVisualStyleBackColor = true;
            this.b_exit.Click += new System.EventHandler(this.b_exit_Click);
            // 
            // b_change_output_size
            // 
            this.b_change_output_size.Location = new System.Drawing.Point(598, 187);
            this.b_change_output_size.Name = "b_change_output_size";
            this.b_change_output_size.Size = new System.Drawing.Size(152, 23);
            this.b_change_output_size.TabIndex = 6;
            this.b_change_output_size.Text = "Изменить область вывода";
            this.b_change_output_size.UseVisualStyleBackColor = true;
            this.b_change_output_size.Click += new System.EventHandler(this.b_change_output_size_Click);
            // 
            // tb_x1
            // 
            this.tb_x1.Location = new System.Drawing.Point(598, 80);
            this.tb_x1.Name = "tb_x1";
            this.tb_x1.Size = new System.Drawing.Size(35, 20);
            this.tb_x1.TabIndex = 7;
            // 
            // tb_y1
            // 
            this.tb_y1.Location = new System.Drawing.Point(655, 80);
            this.tb_y1.Name = "tb_y1";
            this.tb_y1.Size = new System.Drawing.Size(35, 20);
            this.tb_y1.TabIndex = 8;
            // 
            // tb_y2
            // 
            this.tb_y2.Location = new System.Drawing.Point(655, 124);
            this.tb_y2.Name = "tb_y2";
            this.tb_y2.Size = new System.Drawing.Size(35, 20);
            this.tb_y2.TabIndex = 10;
            // 
            // tb_x2
            // 
            this.tb_x2.Location = new System.Drawing.Point(598, 124);
            this.tb_x2.Name = "tb_x2";
            this.tb_x2.Size = new System.Drawing.Size(35, 20);
            this.tb_x2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(598, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "X1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(652, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Y1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(652, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Y2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(598, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "X2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_y2);
            this.Controls.Add(this.tb_x2);
            this.Controls.Add(this.tb_y1);
            this.Controls.Add(this.tb_x1);
            this.Controls.Add(this.b_change_output_size);
            this.Controls.Add(this.b_exit);
            this.Controls.Add(this.b_clear);
            this.Controls.Add(this.b_read_from_file);
            this.Controls.Add(this.b_to_scratch);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Лабораторная работа 11";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button b_to_scratch;
        private System.Windows.Forms.Button b_read_from_file;
        private System.Windows.Forms.Button b_clear;
        private System.Windows.Forms.Button b_exit;
        private System.Windows.Forms.Button b_change_output_size;
        private System.Windows.Forms.TextBox tb_x1;
        private System.Windows.Forms.TextBox tb_y1;
        private System.Windows.Forms.TextBox tb_y2;
        private System.Windows.Forms.TextBox tb_x2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

