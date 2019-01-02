namespace Admeli.Compras.Nuevo
{
    partial class FormProveedorNuevo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProveedorNuevo));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnContacto = new System.Windows.Forms.Button();
            this.btnGenerales = new System.Windows.Forms.Button();
            this.progressBarApp = new System.Windows.Forms.ProgressBar();
            this.panelMainNP = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(232)))));
            this.panel1.Controls.Add(this.btnContacto);
            this.panel1.Controls.Add(this.btnGenerales);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 86, 0, 0);
            this.panel1.Size = new System.Drawing.Size(80, 610);
            this.panel1.TabIndex = 7;
            // 
            // btnContacto
            // 
            this.btnContacto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContacto.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnContacto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.btnContacto.FlatAppearance.BorderSize = 0;
            this.btnContacto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.btnContacto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.btnContacto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContacto.Image = ((System.Drawing.Image)(resources.GetObject("btnContacto.Image")));
            this.btnContacto.Location = new System.Drawing.Point(0, 160);
            this.btnContacto.Margin = new System.Windows.Forms.Padding(0);
            this.btnContacto.Name = "btnContacto";
            this.btnContacto.Size = new System.Drawing.Size(80, 74);
            this.btnContacto.TabIndex = 1;
            this.btnContacto.UseVisualStyleBackColor = true;
            this.btnContacto.Click += new System.EventHandler(this.btnContacto_Click);
            // 
            // btnGenerales
            // 
            this.btnGenerales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerales.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGenerales.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.btnGenerales.FlatAppearance.BorderSize = 0;
            this.btnGenerales.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.btnGenerales.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.btnGenerales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerales.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerales.Image")));
            this.btnGenerales.Location = new System.Drawing.Point(0, 86);
            this.btnGenerales.Margin = new System.Windows.Forms.Padding(0);
            this.btnGenerales.Name = "btnGenerales";
            this.btnGenerales.Size = new System.Drawing.Size(80, 74);
            this.btnGenerales.TabIndex = 0;
            this.btnGenerales.UseVisualStyleBackColor = true;
            this.btnGenerales.Click += new System.EventHandler(this.btnGenerales_Click);
            // 
            // progressBarApp
            // 
            this.progressBarApp.BackColor = System.Drawing.Color.White;
            this.progressBarApp.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarApp.Location = new System.Drawing.Point(0, 0);
            this.progressBarApp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBarApp.MarqueeAnimationSpeed = 10;
            this.progressBarApp.Maximum = 200;
            this.progressBarApp.Name = "progressBarApp";
            this.progressBarApp.RightToLeftLayout = true;
            this.progressBarApp.Size = new System.Drawing.Size(1029, 4);
            this.progressBarApp.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarApp.TabIndex = 1;
            // 
            // panelMainNP
            // 
            this.panelMainNP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainNP.Location = new System.Drawing.Point(80, 4);
            this.panelMainNP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMainNP.Name = "panelMainNP";
            this.panelMainNP.Size = new System.Drawing.Size(949, 610);
            this.panelMainNP.TabIndex = 9;
            // 
            // FormProveedorNuevo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1029, 614);
            this.Controls.Add(this.panelMainNP);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBarApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormProveedorNuevo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "                                                    ";
            this.Load += new System.EventHandler(this.FormProveedorNuevo_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnContacto;
        private System.Windows.Forms.Button btnGenerales;
        protected System.Windows.Forms.ProgressBar progressBarApp;
        public System.Windows.Forms.Panel panelMainNP;
    }
}