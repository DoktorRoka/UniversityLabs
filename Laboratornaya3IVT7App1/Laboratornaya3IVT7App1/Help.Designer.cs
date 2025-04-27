namespace Laboratornaya3IVT7App1
{
    partial class Help
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.b_close = new System.Windows.Forms.Button();
            this.l_author = new System.Windows.Forms.Label();
            this.l_about_batch = new System.Windows.Forms.Label();
            this.l_copyright = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // b_close
            // 
            this.b_close.BackColor = System.Drawing.Color.Honeydew;
            this.b_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.b_close.Location = new System.Drawing.Point(13, 269);
            this.b_close.Name = "b_close";
            this.b_close.Size = new System.Drawing.Size(266, 71);
            this.b_close.TabIndex = 0;
            this.b_close.Text = "Закрыть";
            this.b_close.UseVisualStyleBackColor = false;
            this.b_close.Click += new System.EventHandler(this.b_close_Click);
            // 
            // l_author
            // 
            this.l_author.AutoSize = true;
            this.l_author.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l_author.Location = new System.Drawing.Point(9, 9);
            this.l_author.Name = "l_author";
            this.l_author.Size = new System.Drawing.Size(224, 20);
            this.l_author.TabIndex = 1;
            this.l_author.Text = "Создатель: Калоша Максим";
            // 
            // l_about_batch
            // 
            this.l_about_batch.AutoSize = true;
            this.l_about_batch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l_about_batch.Location = new System.Drawing.Point(9, 47);
            this.l_about_batch.Name = "l_about_batch";
            this.l_about_batch.Size = new System.Drawing.Size(226, 60);
            this.l_about_batch.TabIndex = 2;
            this.l_about_batch.Text = "\"Батч\" режим обрабатывает\r\n все картинки,\r\n добавляет к ним копирайт";
            // 
            // l_copyright
            // 
            this.l_copyright.AutoSize = true;
            this.l_copyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l_copyright.Location = new System.Drawing.Point(9, 125);
            this.l_copyright.Name = "l_copyright";
            this.l_copyright.Size = new System.Drawing.Size(264, 40);
            this.l_copyright.TabIndex = 3;
            this.l_copyright.Text = "Копирайт директория меняет \r\nкуда будут сохраняться картинки";
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(291, 352);
            this.Controls.Add(this.l_copyright);
            this.Controls.Add(this.l_about_batch);
            this.Controls.Add(this.l_author);
            this.Controls.Add(this.b_close);
            this.MaximumSize = new System.Drawing.Size(307, 391);
            this.MinimumSize = new System.Drawing.Size(307, 391);
            this.Name = "Help";
            this.Text = "Помощь";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_close;
        private System.Windows.Forms.Label l_author;
        private System.Windows.Forms.Label l_about_batch;
        private System.Windows.Forms.Label l_copyright;
    }
}