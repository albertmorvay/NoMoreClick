﻿namespace NoMoreClick
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelAttributionIconPart1 = new System.Windows.Forms.Label();
            this.labelAttributionIconPart2 = new System.Windows.Forms.Label();
            this.linkLabelAttributionIconPart1 = new System.Windows.Forms.LinkLabel();
            this.linkLabelAttributionIconPart2 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelAttributionIconPart1
            // 
            this.labelAttributionIconPart1.AutoSize = true;
            this.labelAttributionIconPart1.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.labelAttributionIconPart1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAttributionIconPart1.Location = new System.Drawing.Point(96, 166);
            this.labelAttributionIconPart1.Name = "labelAttributionIconPart1";
            this.labelAttributionIconPart1.Size = new System.Drawing.Size(91, 13);
            this.labelAttributionIconPart1.TabIndex = 0;
            this.labelAttributionIconPart1.Text = "Icons made by ";
            this.labelAttributionIconPart1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAttributionIconPart2
            // 
            this.labelAttributionIconPart2.AutoSize = true;
            this.labelAttributionIconPart2.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.labelAttributionIconPart2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAttributionIconPart2.Location = new System.Drawing.Point(152, 179);
            this.labelAttributionIconPart2.Name = "labelAttributionIconPart2";
            this.labelAttributionIconPart2.Size = new System.Drawing.Size(31, 13);
            this.labelAttributionIconPart2.TabIndex = 1;
            this.labelAttributionIconPart2.Text = "from";
            this.labelAttributionIconPart2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkLabelAttributionIconPart1
            // 
            this.linkLabelAttributionIconPart1.AutoSize = true;
            this.linkLabelAttributionIconPart1.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.linkLabelAttributionIconPart1.Location = new System.Drawing.Point(189, 166);
            this.linkLabelAttributionIconPart1.Name = "linkLabelAttributionIconPart1";
            this.linkLabelAttributionIconPart1.Size = new System.Drawing.Size(85, 13);
            this.linkLabelAttributionIconPart1.TabIndex = 2;
            this.linkLabelAttributionIconPart1.TabStop = true;
            this.linkLabelAttributionIconPart1.Text = "Pixel perfect";
            this.linkLabelAttributionIconPart1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAttributionIconPart1_LinkClicked);
            // 
            // linkLabelAttributionIconPart2
            // 
            this.linkLabelAttributionIconPart2.AutoSize = true;
            this.linkLabelAttributionIconPart2.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.linkLabelAttributionIconPart2.Location = new System.Drawing.Point(189, 179);
            this.linkLabelAttributionIconPart2.Name = "linkLabelAttributionIconPart2";
            this.linkLabelAttributionIconPart2.Size = new System.Drawing.Size(103, 13);
            this.linkLabelAttributionIconPart2.TabIndex = 3;
            this.linkLabelAttributionIconPart2.TabStop = true;
            this.linkLabelAttributionIconPart2.Text = "www.flaticon.com";
            this.linkLabelAttributionIconPart2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAttributionIconPart2_LinkClicked);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 201);
            this.Controls.Add(this.linkLabelAttributionIconPart2);
            this.Controls.Add(this.linkLabelAttributionIconPart1);
            this.Controls.Add(this.labelAttributionIconPart2);
            this.Controls.Add(this.labelAttributionIconPart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NoMoreClick";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAttributionIconPart1;
        private System.Windows.Forms.Label labelAttributionIconPart2;
        private System.Windows.Forms.LinkLabel linkLabelAttributionIconPart1;
        private System.Windows.Forms.LinkLabel linkLabelAttributionIconPart2;
    }
}

