﻿namespace Admeli.CajaBox
{
    partial class UCCuentaPagar
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCCuentaPagar));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelCrud = new System.Windows.Forms.FlowLayoutPanel();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkTodoProveedor = new Bunifu.Framework.UI.BunifuCheckbox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelTools = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBuscar = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSpeedPages = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPageCount = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.lblCurrentPage = new Bunifu.Framework.UI.BunifuMetroTextbox();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblPageAllItems = new System.Windows.Forms.Label();
            this.panelCuerpo = new System.Windows.Forms.Panel();
            this.dgvCuentasPagar = new System.Windows.Forms.DataGridView();
            this.datoCuentaPagarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.idProveedorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.razonSocialDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rucDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actividadPrincipalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.direccionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipoProveedorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCrud.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelTools.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panelCuerpo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuentasPagar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datoCuentaPagarBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCrud
            // 
            this.panelCrud.Controls.Add(this.btnModificar);
            this.panelCrud.Controls.Add(this.btnActualizar);
            this.panelCrud.Controls.Add(this.panel2);
            this.panelCrud.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCrud.Location = new System.Drawing.Point(0, 0);
            this.panelCrud.Margin = new System.Windows.Forms.Padding(4);
            this.panelCrud.Name = "panelCrud";
            this.panelCrud.Padding = new System.Windows.Forms.Padding(13, 18, 0, 0);
            this.panelCrud.Size = new System.Drawing.Size(1131, 74);
            this.panelCrud.TabIndex = 26;
            // 
            // btnModificar
            // 
            this.btnModificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            this.btnModificar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModificar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(225)))), ((int)(((byte)(228)))));
            this.btnModificar.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(236)))));
            this.btnModificar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Navy;
            this.btnModificar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.btnModificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnModificar.ForeColor = System.Drawing.Color.White;
            this.btnModificar.Image = ((System.Drawing.Image)(resources.GetObject("btnModificar.Image")));
            this.btnModificar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModificar.Location = new System.Drawing.Point(13, 18);
            this.btnModificar.Margin = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnModificar.Size = new System.Drawing.Size(187, 49);
            this.btnModificar.TabIndex = 3;
            this.btnModificar.Text = "  Modificar (F4)";
            this.btnModificar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModificar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModificar.UseVisualStyleBackColor = false;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(156)))), ((int)(((byte)(238)))));
            this.btnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(225)))), ((int)(((byte)(228)))));
            this.btnActualizar.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(236)))));
            this.btnActualizar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnActualizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Image = ((System.Drawing.Image)(resources.GetObject("btnActualizar.Image")));
            this.btnActualizar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActualizar.Location = new System.Drawing.Point(213, 18);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnActualizar.Size = new System.Drawing.Size(189, 49);
            this.btnActualizar.TabIndex = 5;
            this.btnActualizar.Text = "  Actualizar (F5)";
            this.btnActualizar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkTodoProveedor);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(419, 22);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(441, 46);
            this.panel2.TabIndex = 58;
            // 
            // chkTodoProveedor
            // 
            this.chkTodoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.chkTodoProveedor.ChechedOffColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.chkTodoProveedor.Checked = false;
            this.chkTodoProveedor.CheckedOnColor = System.Drawing.Color.DodgerBlue;
            this.chkTodoProveedor.ForeColor = System.Drawing.Color.White;
            this.chkTodoProveedor.Location = new System.Drawing.Point(11, 10);
            this.chkTodoProveedor.Margin = new System.Windows.Forms.Padding(5);
            this.chkTodoProveedor.Name = "chkTodoProveedor";
            this.chkTodoProveedor.Size = new System.Drawing.Size(20, 20);
            this.chkTodoProveedor.TabIndex = 55;
            this.chkTodoProveedor.OnChange += new System.EventHandler(this.chkTodoProveedor_OnChange);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.label1.Location = new System.Drawing.Point(40, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 19);
            this.label1.TabIndex = 56;
            this.label1.Text = "Mostrar todos los proveedores que compramos";
            // 
            // panelTools
            // 
            this.panelTools.Controls.Add(this.label4);
            this.panelTools.Controls.Add(this.textBuscar);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTools.Location = new System.Drawing.Point(0, 74);
            this.panelTools.Margin = new System.Windows.Forms.Padding(4);
            this.panelTools.Name = "panelTools";
            this.panelTools.Size = new System.Drawing.Size(1131, 64);
            this.panelTools.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(19, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Buscar:";
            // 
            // textBuscar
            // 
            this.textBuscar.BackColor = System.Drawing.Color.White;
            this.textBuscar.BorderColorFocused = System.Drawing.Color.DodgerBlue;
            this.textBuscar.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(225)))), ((int)(((byte)(228)))));
            this.textBuscar.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(225)))), ((int)(((byte)(228)))));
            this.textBuscar.BorderThickness = 1;
            this.textBuscar.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBuscar.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBuscar.isPassword = false;
            this.textBuscar.Location = new System.Drawing.Point(13, 7);
            this.textBuscar.Margin = new System.Windows.Forms.Padding(5);
            this.textBuscar.Name = "textBuscar";
            this.textBuscar.Padding = new System.Windows.Forms.Padding(3, 22, 7, 2);
            this.textBuscar.Size = new System.Drawing.Size(275, 49);
            this.textBuscar.TabIndex = 17;
            this.textBuscar.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBuscar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBuscar_KeyPress);
            // 
            // panelNavigation
            // 
            this.panelNavigation.Controls.Add(this.label2);
            this.panelNavigation.Controls.Add(this.lblSpeedPages);
            this.panelNavigation.Controls.Add(this.panel5);
            this.panelNavigation.Controls.Add(this.lblPageAllItems);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelNavigation.Location = new System.Drawing.Point(0, 360);
            this.panelNavigation.Margin = new System.Windows.Forms.Padding(4);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(1131, 74);
            this.panelNavigation.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.label2.Location = new System.Drawing.Point(979, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 43);
            this.label2.TabIndex = 11;
            this.label2.Text = "Mostrar cada";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSpeedPages
            // 
            this.lblSpeedPages.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSpeedPages.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblSpeedPages.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(211)))), ((int)(((byte)(215)))));
            this.lblSpeedPages.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.lblSpeedPages.BorderThickness = 1;
            this.lblSpeedPages.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblSpeedPages.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeedPages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.lblSpeedPages.isPassword = false;
            this.lblSpeedPages.Location = new System.Drawing.Point(1057, 16);
            this.lblSpeedPages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblSpeedPages.Name = "lblSpeedPages";
            this.lblSpeedPages.Size = new System.Drawing.Size(60, 43);
            this.lblSpeedPages.TabIndex = 10;
            this.lblSpeedPages.Text = "10";
            this.lblSpeedPages.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblSpeedPages.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lblSpeedPages_KeyPress);
            this.lblSpeedPages.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lblSpeedPages_KeyUp);
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.lblPageCount);
            this.panel5.Controls.Add(this.lblCurrentPage);
            this.panel5.Controls.Add(this.btnFirst);
            this.panel5.Controls.Add(this.btnLast);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.btnPrevious);
            this.panel5.Controls.Add(this.btnNext);
            this.panel5.Location = new System.Drawing.Point(336, 4);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(459, 68);
            this.panel5.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.label5.Location = new System.Drawing.Point(195, 4);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Página";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblPageCount
            // 
            this.lblPageCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPageCount.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblPageCount.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(211)))), ((int)(((byte)(215)))));
            this.lblPageCount.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.lblPageCount.BorderThickness = 1;
            this.lblPageCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblPageCount.Enabled = false;
            this.lblPageCount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.lblPageCount.isPassword = false;
            this.lblPageCount.Location = new System.Drawing.Point(248, 18);
            this.lblPageCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(60, 43);
            this.lblPageCount.TabIndex = 1;
            this.lblPageCount.Text = "1";
            this.lblPageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblCurrentPage
            // 
            this.lblCurrentPage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCurrentPage.BorderColorFocused = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblCurrentPage.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(211)))), ((int)(((byte)(215)))));
            this.lblCurrentPage.BorderColorMouseHover = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(158)))), ((int)(((byte)(166)))));
            this.lblCurrentPage.BorderThickness = 1;
            this.lblCurrentPage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblCurrentPage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.lblCurrentPage.isPassword = false;
            this.lblCurrentPage.Location = new System.Drawing.Point(152, 18);
            this.lblCurrentPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lblCurrentPage.Name = "lblCurrentPage";
            this.lblCurrentPage.Size = new System.Drawing.Size(60, 43);
            this.lblCurrentPage.TabIndex = 0;
            this.lblCurrentPage.Text = "1";
            this.lblCurrentPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lblCurrentPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lblCurrentPage_KeyPress);
            this.lblCurrentPage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lblCurrentPage_KeyUp);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFirst.FlatAppearance.BorderSize = 0;
            this.btnFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirst.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnFirst.Image")));
            this.btnFirst.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFirst.Location = new System.Drawing.Point(11, 18);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(0);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(60, 43);
            this.btnFirst.TabIndex = 11;
            this.btnFirst.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnLast.BackColor = System.Drawing.Color.White;
            this.btnLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLast.FlatAppearance.BorderSize = 0;
            this.btnLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnLast.Image = ((System.Drawing.Image)(resources.GetObject("btnLast.Image")));
            this.btnLast.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLast.Location = new System.Drawing.Point(389, 18);
            this.btnLast.Margin = new System.Windows.Forms.Padding(0);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(60, 43);
            this.btnLast.TabIndex = 10;
            this.btnLast.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLast.UseVisualStyleBackColor = false;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(216, 31);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 18);
            this.label6.TabIndex = 9;
            this.label6.Text = "DE";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevious.FlatAppearance.BorderSize = 0;
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btnPrevious.Image")));
            this.btnPrevious.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrevious.Location = new System.Drawing.Point(81, 18);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(60, 43);
            this.btnPrevious.TabIndex = 7;
            this.btnPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnNext.BackColor = System.Drawing.Color.White;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.Location = new System.Drawing.Point(319, 18);
            this.btnNext.Margin = new System.Windows.Forms.Padding(0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(60, 43);
            this.btnNext.TabIndex = 6;
            this.btnNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblPageAllItems
            // 
            this.lblPageAllItems.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPageAllItems.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageAllItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(110)))), ((int)(((byte)(122)))));
            this.lblPageAllItems.Location = new System.Drawing.Point(9, 16);
            this.lblPageAllItems.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageAllItems.Name = "lblPageAllItems";
            this.lblPageAllItems.Size = new System.Drawing.Size(93, 43);
            this.lblPageAllItems.TabIndex = 1;
            this.lblPageAllItems.Text = "10 Registros";
            this.lblPageAllItems.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCuerpo
            // 
            this.panelCuerpo.Controls.Add(this.dgvCuentasPagar);
            this.panelCuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCuerpo.Location = new System.Drawing.Point(0, 138);
            this.panelCuerpo.Margin = new System.Windows.Forms.Padding(4);
            this.panelCuerpo.Name = "panelCuerpo";
            this.panelCuerpo.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.panelCuerpo.Size = new System.Drawing.Size(1131, 222);
            this.panelCuerpo.TabIndex = 33;
            // 
            // dgvCuentasPagar
            // 
            this.dgvCuentasPagar.AllowUserToAddRows = false;
            this.dgvCuentasPagar.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvCuentasPagar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCuentasPagar.AutoGenerateColumns = false;
            this.dgvCuentasPagar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCuentasPagar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCuentasPagar.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(115)))), ((int)(((byte)(220)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCuentasPagar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCuentasPagar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idProveedorDataGridViewTextBoxColumn,
            this.razonSocialDataGridViewTextBoxColumn,
            this.rucDataGridViewTextBoxColumn,
            this.actividadPrincipalDataGridViewTextBoxColumn,
            this.direccionDataGridViewTextBoxColumn,
            this.tipoProveedorDataGridViewTextBoxColumn});
            this.dgvCuentasPagar.DataSource = this.datoCuentaPagarBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCuentasPagar.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCuentasPagar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCuentasPagar.GridColor = System.Drawing.Color.Gainsboro;
            this.dgvCuentasPagar.Location = new System.Drawing.Point(13, 12);
            this.dgvCuentasPagar.Margin = new System.Windows.Forms.Padding(4);
            this.dgvCuentasPagar.Name = "dgvCuentasPagar";
            this.dgvCuentasPagar.ReadOnly = true;
            this.dgvCuentasPagar.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCuentasPagar.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCuentasPagar.RowHeadersWidth = 40;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.dgvCuentasPagar.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvCuentasPagar.RowTemplate.Height = 30;
            this.dgvCuentasPagar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCuentasPagar.Size = new System.Drawing.Size(1105, 198);
            this.dgvCuentasPagar.TabIndex = 30;
            this.dgvCuentasPagar.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCuentasCobrar_CellDoubleClick);
            // 
            // datoCuentaPagarBindingSource
            // 
            this.datoCuentaPagarBindingSource.DataSource = typeof(Entidad.DatoCuentaPagar);
            // 
            // idProveedorDataGridViewTextBoxColumn
            // 
            this.idProveedorDataGridViewTextBoxColumn.DataPropertyName = "idProveedor";
            this.idProveedorDataGridViewTextBoxColumn.HeaderText = "IDProveedor";
            this.idProveedorDataGridViewTextBoxColumn.Name = "idProveedorDataGridViewTextBoxColumn";
            this.idProveedorDataGridViewTextBoxColumn.ReadOnly = true;
            this.idProveedorDataGridViewTextBoxColumn.Visible = false;
            // 
            // razonSocialDataGridViewTextBoxColumn
            // 
            this.razonSocialDataGridViewTextBoxColumn.DataPropertyName = "razonSocial";
            this.razonSocialDataGridViewTextBoxColumn.HeaderText = "Razón Social";
            this.razonSocialDataGridViewTextBoxColumn.Name = "razonSocialDataGridViewTextBoxColumn";
            this.razonSocialDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rucDataGridViewTextBoxColumn
            // 
            this.rucDataGridViewTextBoxColumn.DataPropertyName = "ruc";
            this.rucDataGridViewTextBoxColumn.FillWeight = 50F;
            this.rucDataGridViewTextBoxColumn.HeaderText = "RUC";
            this.rucDataGridViewTextBoxColumn.Name = "rucDataGridViewTextBoxColumn";
            this.rucDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actividadPrincipalDataGridViewTextBoxColumn
            // 
            this.actividadPrincipalDataGridViewTextBoxColumn.DataPropertyName = "actividadPrincipal";
            this.actividadPrincipalDataGridViewTextBoxColumn.HeaderText = "Actividad Principal";
            this.actividadPrincipalDataGridViewTextBoxColumn.Name = "actividadPrincipalDataGridViewTextBoxColumn";
            this.actividadPrincipalDataGridViewTextBoxColumn.ReadOnly = true;
            this.actividadPrincipalDataGridViewTextBoxColumn.Visible = false;
            // 
            // direccionDataGridViewTextBoxColumn
            // 
            this.direccionDataGridViewTextBoxColumn.DataPropertyName = "direccion";
            this.direccionDataGridViewTextBoxColumn.HeaderText = "Dirección";
            this.direccionDataGridViewTextBoxColumn.Name = "direccionDataGridViewTextBoxColumn";
            this.direccionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tipoProveedorDataGridViewTextBoxColumn
            // 
            this.tipoProveedorDataGridViewTextBoxColumn.DataPropertyName = "tipoProveedor";
            this.tipoProveedorDataGridViewTextBoxColumn.HeaderText = "Tipo de Proveedor";
            this.tipoProveedorDataGridViewTextBoxColumn.Name = "tipoProveedorDataGridViewTextBoxColumn";
            this.tipoProveedorDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // UCCuentaPagar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelCuerpo);
            this.Controls.Add(this.panelNavigation);
            this.Controls.Add(this.panelTools);
            this.Controls.Add(this.panelCrud);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCCuentaPagar";
            this.Size = new System.Drawing.Size(1131, 434);
            this.panelCrud.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelTools.ResumeLayout(false);
            this.panelTools.PerformLayout();
            this.panelNavigation.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panelCuerpo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCuentasPagar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datoCuentaPagarBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panelCrud;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Panel panel2;
        private Bunifu.Framework.UI.BunifuCheckbox chkTodoProveedor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelTools;
        private System.Windows.Forms.Label label4;
        private Bunifu.Framework.UI.BunifuMetroTextbox textBuscar;
        private System.Windows.Forms.Panel panelNavigation;
        private System.Windows.Forms.Label label2;
        private Bunifu.Framework.UI.BunifuMetroTextbox lblSpeedPages;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private Bunifu.Framework.UI.BunifuMetroTextbox lblPageCount;
        private Bunifu.Framework.UI.BunifuMetroTextbox lblCurrentPage;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPageAllItems;
        private System.Windows.Forms.Panel panelCuerpo;
        private System.Windows.Forms.DataGridView dgvCuentasPagar;
        private System.Windows.Forms.DataGridViewTextBoxColumn idClienteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreClienteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numeroDocumentoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreGrupoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource datoCuentaPagarBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idProveedorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn razonSocialDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rucDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actividadPrincipalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn direccionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipoProveedorDataGridViewTextBoxColumn;
    }
}
