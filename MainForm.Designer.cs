namespace MVPSample.Views.Forms
{
    partial class MainForm
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
            this.SrcPicBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.rdRectangle = new System.Windows.Forms.RadioButton();
            this.rdEclipse = new System.Windows.Forms.RadioButton();
            this.rdCircle = new System.Windows.Forms.RadioButton();
            this.rdSquare = new System.Windows.Forms.RadioButton();
            this.rdRectangleRotate = new System.Windows.Forms.RadioButton();
            this.lbCordinates = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SrcPicBox
            // 
            this.SrcPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SrcPicBox.Location = new System.Drawing.Point(27, 29);
            this.SrcPicBox.Name = "SrcPicBox";
            this.SrcPicBox.Size = new System.Drawing.Size(450, 450);
            this.SrcPicBox.TabIndex = 0;
            this.SrcPicBox.TabStop = false;
            this.SrcPicBox.Paint += new System.Windows.Forms.PaintEventHandler(this.SrcPicBox_Paint);
            this.SrcPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseDown);
            this.SrcPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseMove);
            this.SrcPicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SrcPicBox_MouseUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(505, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load Image";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(505, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "Crop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(505, 101);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(100, 30);
            this.Delete.TabIndex = 3;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // rdRectangle
            // 
            this.rdRectangle.AutoSize = true;
            this.rdRectangle.Checked = true;
            this.rdRectangle.Location = new System.Drawing.Point(30, 503);
            this.rdRectangle.Name = "rdRectangle";
            this.rdRectangle.Size = new System.Drawing.Size(74, 17);
            this.rdRectangle.TabIndex = 4;
            this.rdRectangle.TabStop = true;
            this.rdRectangle.Text = "Rectangle";
            this.rdRectangle.UseVisualStyleBackColor = true;
            // 
            // rdEclipse
            // 
            this.rdEclipse.AutoSize = true;
            this.rdEclipse.Location = new System.Drawing.Point(255, 503);
            this.rdEclipse.Name = "rdEclipse";
            this.rdEclipse.Size = new System.Drawing.Size(59, 17);
            this.rdEclipse.TabIndex = 5;
            this.rdEclipse.TabStop = true;
            this.rdEclipse.Text = "Eclipse";
            this.rdEclipse.UseVisualStyleBackColor = true;
            // 
            // rdCircle
            // 
            this.rdCircle.AutoSize = true;
            this.rdCircle.Location = new System.Drawing.Point(180, 503);
            this.rdCircle.Name = "rdCircle";
            this.rdCircle.Size = new System.Drawing.Size(51, 17);
            this.rdCircle.TabIndex = 6;
            this.rdCircle.TabStop = true;
            this.rdCircle.Text = "Circle";
            this.rdCircle.UseVisualStyleBackColor = true;
            // 
            // rdSquare
            // 
            this.rdSquare.AutoSize = true;
            this.rdSquare.Location = new System.Drawing.Point(105, 503);
            this.rdSquare.Name = "rdSquare";
            this.rdSquare.Size = new System.Drawing.Size(59, 17);
            this.rdSquare.TabIndex = 7;
            this.rdSquare.TabStop = true;
            this.rdSquare.Text = "Square";
            this.rdSquare.UseVisualStyleBackColor = true;
            // 
            // rdRectangleRotate
            // 
            this.rdRectangleRotate.AutoSize = true;
            this.rdRectangleRotate.Location = new System.Drawing.Point(330, 503);
            this.rdRectangleRotate.Name = "rdRectangleRotate";
            this.rdRectangleRotate.Size = new System.Drawing.Size(109, 17);
            this.rdRectangleRotate.TabIndex = 8;
            this.rdRectangleRotate.TabStop = true;
            this.rdRectangleRotate.Text = "Rectangle Rotate";
            this.rdRectangleRotate.UseVisualStyleBackColor = true;
            // 
            // lbCordinates
            // 
            this.lbCordinates.AutoSize = true;
            this.lbCordinates.Location = new System.Drawing.Point(25, 9);
            this.lbCordinates.Name = "lbCordinates";
            this.lbCordinates.Size = new System.Drawing.Size(66, 13);
            this.lbCordinates.TabIndex = 9;
            this.lbCordinates.Text = "Coordinates:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 552);
            this.Controls.Add(this.lbCordinates);
            this.Controls.Add(this.rdRectangleRotate);
            this.Controls.Add(this.rdSquare);
            this.Controls.Add(this.rdCircle);
            this.Controls.Add(this.rdEclipse);
            this.Controls.Add(this.rdRectangle);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SrcPicBox);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SrcPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SrcPicBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.RadioButton rdRectangle;
        private System.Windows.Forms.RadioButton rdEclipse;
        private System.Windows.Forms.RadioButton rdCircle;
        private System.Windows.Forms.RadioButton rdSquare;
        private System.Windows.Forms.RadioButton rdRectangleRotate;
        private System.Windows.Forms.Label lbCordinates;
    }
}