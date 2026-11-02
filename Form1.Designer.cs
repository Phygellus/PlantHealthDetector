namespace PlantHealthDetector
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuralNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.healthDetectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.setWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.healthDetectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.exportWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.neuralNetworkToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openImageToolStripMenuItem,
            this.exportCSVFileToolStripMenuItem,
            this.exportWeightsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // neuralNetworkToolStripMenuItem
            // 
            this.neuralNetworkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.healthDetectorToolStripMenuItem,
            this.setWeightsToolStripMenuItem,
            this.trainModelToolStripMenuItem,
            this.healthDetectionToolStripMenuItem});
            this.neuralNetworkToolStripMenuItem.Name = "neuralNetworkToolStripMenuItem";
            this.neuralNetworkToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
            this.neuralNetworkToolStripMenuItem.Text = "NeuralNetwork";
            // 
            // openImageToolStripMenuItem
            // 
            this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            this.openImageToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.openImageToolStripMenuItem.Text = "Open Image";
            // 
            // exportCSVFileToolStripMenuItem
            // 
            this.exportCSVFileToolStripMenuItem.Name = "exportCSVFileToolStripMenuItem";
            this.exportCSVFileToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exportCSVFileToolStripMenuItem.Text = "Export CSV File";
            // 
            // healthDetectorToolStripMenuItem
            // 
            this.healthDetectorToolStripMenuItem.Name = "healthDetectorToolStripMenuItem";
            this.healthDetectorToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.healthDetectorToolStripMenuItem.Text = "Preparation";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(48, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(294, 282);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(465, 59);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(232, 56);
            this.textBox1.TabIndex = 2;
            // 
            // setWeightsToolStripMenuItem
            // 
            this.setWeightsToolStripMenuItem.Name = "setWeightsToolStripMenuItem";
            this.setWeightsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.setWeightsToolStripMenuItem.Text = "Set Weights";
            // 
            // trainModelToolStripMenuItem
            // 
            this.trainModelToolStripMenuItem.Name = "trainModelToolStripMenuItem";
            this.trainModelToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.trainModelToolStripMenuItem.Text = "Train Model";
            // 
            // healthDetectionToolStripMenuItem
            // 
            this.healthDetectionToolStripMenuItem.Name = "healthDetectionToolStripMenuItem";
            this.healthDetectionToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.healthDetectionToolStripMenuItem.Text = "Health Detection";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // exportWeightsToolStripMenuItem
            // 
            this.exportWeightsToolStripMenuItem.Name = "exportWeightsToolStripMenuItem";
            this.exportWeightsToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exportWeightsToolStripMenuItem.Text = "Export Weights";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCSVFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neuralNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem healthDetectorToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem exportWeightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem healthDetectionToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}

