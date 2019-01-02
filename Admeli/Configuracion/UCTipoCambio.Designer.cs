namespace Admeli.Configuracion
{
    partial class UCTipoCambio
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelContainer = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMonedaPorDefecto = new System.Windows.Forms.Label();
            this.textNombre = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.dtpFechaIngreso = new Bunifu.Framework.UI.BunifuDatepicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.panelContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContainer
            // 
            this.panelContainer.AutoScroll = true;
            this.panelContainer.BackColor = System.Drawing.Color.White;
            this.panelContainer.Controls.Add(this.label2);
            this.panelContainer.Controls.Add(this.label1);
            this.panelContainer.Controls.Add(this.lblMonedaPorDefecto);
            this.panelContainer.Controls.Add(this.textNombre);
            this.panelContainer.Controls.Add(this.dtpFechaIngreso);
            this.panelContainer.Controls.Add(this.panel1);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(11, 10);
            this.panelContainer.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Padding = new System.Windows.Forms.Padding(1);
            this.panelContainer.Size = new System.Drawing.Size(1085, 398);
            this.panelContainer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label2.Location = new System.Drawing.Point(29, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 112;
            this.label2.Text = "Fecha";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(28, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 111;
            this.label1.Text = "Ingresado por";
            // 
            // lblMonedaPorDefecto
            // 
            this.lblMonedaPorDefecto.AutoSize = true;
            this.lblMonedaPorDefecto.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonedaPorDefecto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.lblMonedaPorDefecto.Location = new System.Drawing.Point(28, 167);
            this.lblMonedaPorDefecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonedaPorDefecto.Name = "lblMonedaPorDefecto";
            this.lblMonedaPorDefecto.Size = new System.Drawing.Size(258, 19);
            this.lblMonedaPorDefecto.TabIndex = 106;
            this.lblMonedaPorDefecto.Text = "Moneda por Defecto: DOLARES";
            // 
            // textNombre
            // 
            this.textNombre.AllowDrop = true;
            this.textNombre.AutoSize = true;
            this.textNombre.BackColor = System.Drawing.Color.White;
            this.textNombre.BorderColorFocused = System.Drawing.Color.DodgerBlue;
            this.textNombre.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(157)))));
            this.textNombre.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(157)))));
            this.textNombre.BorderThickness = 1;
            this.textNombre.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textNombre.Enabled = false;
            this.textNombre.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textNombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textNombre.isPassword = false;
            this.textNombre.Location = new System.Drawing.Point(24, 119);
            this.textNombre.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.textNombre.Name = "textNombre";
            this.textNombre.Padding = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.textNombre.Size = new System.Drawing.Size(468, 42);
            this.textNombre.TabIndex = 104;
            this.textNombre.Text = "Juan Perez";
            this.textNombre.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textNombre_KeyPress);
            // 
            // dtpFechaIngreso
            // 
            this.dtpFechaIngreso.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpFechaIngreso.BorderRadius = 0;
            this.dtpFechaIngreso.ForeColor = System.Drawing.Color.DodgerBlue;
            this.dtpFechaIngreso.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaIngreso.FormatCustom = null;
            this.dtpFechaIngreso.Location = new System.Drawing.Point(24, 18);
            this.dtpFechaIngreso.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dtpFechaIngreso.Name = "dtpFechaIngreso";
            this.dtpFechaIngreso.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.dtpFechaIngreso.Size = new System.Drawing.Size(468, 57);
            this.dtpFechaIngreso.TabIndex = 103;
            this.dtpFechaIngreso.Value = new System.DateTime(2018, 3, 11, 0, 0, 0, 0);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAceptar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(1, 321);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 76);
            this.panel1.TabIndex = 101;
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
            this.btnAceptar.Location = new System.Drawing.Point(31, 20);
            this.btnAceptar.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(468, 41);
            this.btnAceptar.TabIndex = 98;
            this.btnAceptar.Text = "Guardar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // UCTipoCambio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCTipoCambio";
            this.Padding = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.Size = new System.Drawing.Size(1107, 418);
            this.Load += new System.EventHandler(this.UCTipoCambio_Load);
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelContainer;
        private Bunifu.Framework.UI.BunifuDatepicker dtpFechaIngreso;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAceptar;
        private Bunifu.Framework.UI.BunifuMetroTextbox textNombre;
        private System.Windows.Forms.Label lblMonedaPorDefecto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
