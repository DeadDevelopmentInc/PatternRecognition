using System.Drawing;
using System.Windows.Forms;

namespace PatternRecognition
{
    partial class MainWindow
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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.computeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.averageRecordResToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonClassify = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCompute = new System.Windows.Forms.Button();
            this.buttonCorrectYes = new System.Windows.Forms.Button();
            this.buttonCorrectNo = new System.Windows.Forms.Button();
            this.labelCorrect = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem6,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(531, 25);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 19);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(92, 22);
            this.toolStripMenuItem5.Text = "E&xit";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.changePassToolStripMenuItem});
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(44, 19);
            this.toolStripMenuItem6.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // changePassToolStripMenuItem
            // 
            this.changePassToolStripMenuItem.Name = "changePassToolStripMenuItem";
            this.changePassToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.changePassToolStripMenuItem.Text = "Change Pass";
            this.changePassToolStripMenuItem.Click += new System.EventHandler(this.changePassToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.computeToolStripMenuItem,
            this.averageRecordResToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 19);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // computeToolStripMenuItem
            // 
            this.computeToolStripMenuItem.Name = "computeToolStripMenuItem";
            this.computeToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.computeToolStripMenuItem.Text = "Compute";
            this.computeToolStripMenuItem.Click += new System.EventHandler(this.computeToolStripMenuItem_Click);
            // 
            // averageRecordResToolStripMenuItem
            // 
            this.averageRecordResToolStripMenuItem.Name = "averageRecordResToolStripMenuItem";
            this.averageRecordResToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.averageRecordResToolStripMenuItem.Text = "Average record res";
            this.averageRecordResToolStripMenuItem.Click += new System.EventHandler(this.averageRecordResToolStripMenuItem_Click);
            // 
            // buttonClassify
            // 
            this.buttonClassify.Location = new System.Drawing.Point(228, 28);
            this.buttonClassify.Name = "buttonClassify";
            this.buttonClassify.Size = new System.Drawing.Size(141, 32);
            this.buttonClassify.TabIndex = 20;
            this.buttonClassify.Text = "Classify";
            this.buttonClassify.UseVisualStyleBackColor = true;
            this.buttonClassify.Visible = false;
            this.buttonClassify.Click += new System.EventHandler(this.buttonClassify_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(173, 66);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 362);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(256, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Upload Image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCompute
            // 
            this.buttonCompute.Location = new System.Drawing.Point(12, 114);
            this.buttonCompute.Name = "buttonCompute";
            this.buttonCompute.Size = new System.Drawing.Size(115, 23);
            this.buttonCompute.TabIndex = 23;
            this.buttonCompute.Text = "ComputeBCF";
            this.buttonCompute.UseVisualStyleBackColor = true;
            this.buttonCompute.Visible = false;
            this.buttonCompute.Click += new System.EventHandler(this.buttonCompute_Click);
            // 
            // buttonCorrectYes
            // 
            this.buttonCorrectYes.Location = new System.Drawing.Point(25, 54);
            this.buttonCorrectYes.Name = "buttonCorrectYes";
            this.buttonCorrectYes.Size = new System.Drawing.Size(39, 23);
            this.buttonCorrectYes.TabIndex = 24;
            this.buttonCorrectYes.Text = "Yes";
            this.buttonCorrectYes.UseVisualStyleBackColor = true;
            this.buttonCorrectYes.Visible = false;
            this.buttonCorrectYes.Click += new System.EventHandler(this.buttonCorrectYes_Click);
            // 
            // buttonCorrectNo
            // 
            this.buttonCorrectNo.Location = new System.Drawing.Point(70, 54);
            this.buttonCorrectNo.Name = "buttonCorrectNo";
            this.buttonCorrectNo.Size = new System.Drawing.Size(39, 23);
            this.buttonCorrectNo.TabIndex = 25;
            this.buttonCorrectNo.Text = "No";
            this.buttonCorrectNo.UseVisualStyleBackColor = true;
            this.buttonCorrectNo.Visible = false;
            this.buttonCorrectNo.Click += new System.EventHandler(this.buttonCorrectNo_Click);
            // 
            // labelCorrect
            // 
            this.labelCorrect.AutoSize = true;
            this.labelCorrect.Location = new System.Drawing.Point(22, 38);
            this.labelCorrect.Name = "labelCorrect";
            this.labelCorrect.Size = new System.Drawing.Size(99, 13);
            this.labelCorrect.TabIndex = 26;
            this.labelCorrect.Text = "Answer be correct?";
            this.labelCorrect.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Please, upload image";
            // 
            // MainWindow
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(531, 388);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCorrect);
            this.Controls.Add(this.buttonCorrectNo);
            this.Controls.Add(this.buttonCorrectYes);
            this.Controls.Add(this.buttonCompute);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonClassify);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(22, 300);
            this.Name = "MainWindow";
            this.Text = "Image classification with BCF";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private SizeF AutoScaleDimensions;
        private AutoScaleMode AutoScaleMode;
        private Button buttonClassify;
        private PictureBox pictureBox1;
        private Button button1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem changePassToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem computeToolStripMenuItem;
        private Button buttonCompute;
        private Button buttonCorrectYes;
        private Button buttonCorrectNo;
        private Label labelCorrect;
        private Label label2;
        private ToolStripMenuItem averageRecordResToolStripMenuItem;
    }
}

