namespace Admeli.AlmacenBox.buscar
{
    partial class formGeografia
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formGeografia));
            this.panelFooter = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxNivel3 = new System.Windows.Forms.ComboBox();
            this.nivel3BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblNivel3 = new System.Windows.Forms.Label();
            this.cbxNivel2 = new System.Windows.Forms.ComboBox();
            this.nivel2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblNivel2 = new System.Windows.Forms.Label();
            this.cbxNivel1 = new System.Windows.Forms.ComboBox();
            this.nivel1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblNivel1 = new System.Windows.Forms.Label();
            this.cbxPaises = new System.Windows.Forms.ComboBox();
            this.paisBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelFooter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nivel3BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nivel2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nivel1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paisBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.panel1);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 343);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.panelFooter.Size = new System.Drawing.Size(335, 49);
            this.panelFooter.TabIndex = 56;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(-34, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 49);
            this.panel1.TabIndex = 6;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAceptar.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnAceptar.FlatAppearance.BorderSize = 0;
            this.btnAceptar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAceptar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.ForeColor = System.Drawing.Color.White;
            this.btnAceptar.Location = new System.Drawing.Point(58, 6);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(152, 36);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Guardar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(202)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClose.Location = new System.Drawing.Point(230, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 36);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Cerrar";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.panelHeader.Controls.Add(this.label4);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panelHeader.Size = new System.Drawing.Size(335, 49);
            this.panelHeader.TabIndex = 55;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "Lugar de Entrega";
            // 
            // cbxNivel3
            // 
            this.cbxNivel3.DataSource = this.nivel3BindingSource;
            this.cbxNivel3.DisplayMember = "nombre";
            this.cbxNivel3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxNivel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxNivel3.FormattingEnabled = true;
            this.cbxNivel3.Location = new System.Drawing.Point(18, 287);
            this.cbxNivel3.Name = "cbxNivel3";
            this.cbxNivel3.Size = new System.Drawing.Size(300, 28);
            this.cbxNivel3.TabIndex = 9;
            this.cbxNivel3.ValueMember = "idNivel3";
            this.cbxNivel3.Visible = false;
            // 
            // nivel3BindingSource
            // 
            this.nivel3BindingSource.DataSource = typeof(Entidad.Location.Nivel3);
            // 
            // lblNivel3
            // 
            this.lblNivel3.AutoSize = true;
            this.lblNivel3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNivel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.lblNivel3.Location = new System.Drawing.Point(16, 265);
            this.lblNivel3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNivel3.Name = "lblNivel3";
            this.lblNivel3.Size = new System.Drawing.Size(57, 19);
            this.lblNivel3.TabIndex = 102;
            this.lblNivel3.Text = "Nivel 3";
            this.lblNivel3.Visible = false;
            // 
            // cbxNivel2
            // 
            this.cbxNivel2.DataSource = this.nivel2BindingSource;
            this.cbxNivel2.DisplayMember = "nombre";
            this.cbxNivel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxNivel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxNivel2.FormattingEnabled = true;
            this.cbxNivel2.Location = new System.Drawing.Point(20, 219);
            this.cbxNivel2.Name = "cbxNivel2";
            this.cbxNivel2.Size = new System.Drawing.Size(300, 28);
            this.cbxNivel2.TabIndex = 8;
            this.cbxNivel2.ValueMember = "idNivel2";
            this.cbxNivel2.Visible = false;
            this.cbxNivel2.SelectedIndexChanged += new System.EventHandler(this.cbxNivel2_SelectedIndexChanged);
            // 
            // nivel2BindingSource
            // 
            this.nivel2BindingSource.DataSource = typeof(Entidad.Location.Nivel2);
            // 
            // lblNivel2
            // 
            this.lblNivel2.AutoSize = true;
            this.lblNivel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNivel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.lblNivel2.Location = new System.Drawing.Point(16, 197);
            this.lblNivel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNivel2.Name = "lblNivel2";
            this.lblNivel2.Size = new System.Drawing.Size(57, 19);
            this.lblNivel2.TabIndex = 100;
            this.lblNivel2.Text = "Nivel 2";
            this.lblNivel2.Visible = false;
            // 
            // cbxNivel1
            // 
            this.cbxNivel1.DataSource = this.nivel1BindingSource;
            this.cbxNivel1.DisplayMember = "nombre";
            this.cbxNivel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxNivel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxNivel1.FormattingEnabled = true;
            this.cbxNivel1.Location = new System.Drawing.Point(20, 158);
            this.cbxNivel1.Name = "cbxNivel1";
            this.cbxNivel1.Size = new System.Drawing.Size(300, 28);
            this.cbxNivel1.TabIndex = 7;
            this.cbxNivel1.ValueMember = "idNivel1";
            this.cbxNivel1.Visible = false;
            this.cbxNivel1.SelectedIndexChanged += new System.EventHandler(this.cbxNivel1_SelectedIndexChanged);
            // 
            // nivel1BindingSource
            // 
            this.nivel1BindingSource.DataSource = typeof(Entidad.Location.Nivel1);
            // 
            // lblNivel1
            // 
            this.lblNivel1.AutoSize = true;
            this.lblNivel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNivel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.lblNivel1.Location = new System.Drawing.Point(16, 136);
            this.lblNivel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNivel1.Name = "lblNivel1";
            this.lblNivel1.Size = new System.Drawing.Size(57, 19);
            this.lblNivel1.TabIndex = 98;
            this.lblNivel1.Text = "Nivel 1";
            this.lblNivel1.Visible = false;
            // 
            // cbxPaises
            // 
            this.cbxPaises.DataSource = this.paisBindingSource;
            this.cbxPaises.DisplayMember = "nombre";
            this.cbxPaises.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPaises.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxPaises.FormattingEnabled = true;
            this.cbxPaises.Location = new System.Drawing.Point(20, 96);
            this.cbxPaises.Name = "cbxPaises";
            this.cbxPaises.Size = new System.Drawing.Size(300, 28);
            this.cbxPaises.TabIndex = 6;
            this.cbxPaises.ValueMember = "idPais";
            this.cbxPaises.SelectedIndexChanged += new System.EventHandler(this.cbxPaises_SelectedIndexChanged);
            // 
            // paisBindingSource
            // 
            this.paisBindingSource.DataSource = typeof(Entidad.Location.Pais);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.label11.Location = new System.Drawing.Point(16, 74);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 19);
            this.label11.TabIndex = 96;
            this.label11.Text = "País";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // formGeografia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 392);
            this.Controls.Add(this.cbxNivel3);
            this.Controls.Add(this.lblNivel3);
            this.Controls.Add(this.cbxNivel2);
            this.Controls.Add(this.lblNivel2);
            this.Controls.Add(this.cbxNivel1);
            this.Controls.Add(this.lblNivel1);
            this.Controls.Add(this.cbxPaises);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formGeografia";
            this.Load += new System.EventHandler(this.UCProveedorGeneral_Load);
            this.panelFooter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nivel3BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nivel2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nivel1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paisBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxNivel3;
        private System.Windows.Forms.Label lblNivel3;
        private System.Windows.Forms.ComboBox cbxNivel2;
        private System.Windows.Forms.Label lblNivel2;
        private System.Windows.Forms.ComboBox cbxNivel1;
        private System.Windows.Forms.Label lblNivel1;
        private System.Windows.Forms.ComboBox cbxPaises;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.BindingSource nivel3BindingSource;
        private System.Windows.Forms.BindingSource nivel2BindingSource;
        private System.Windows.Forms.BindingSource nivel1BindingSource;
        private System.Windows.Forms.BindingSource paisBindingSource;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
