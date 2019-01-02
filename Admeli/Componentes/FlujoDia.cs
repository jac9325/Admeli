using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Componentes
{
    public class FlujoDia
    {
        public Panel ContenedorDia { get; set; }
        public TableLayoutPanel DivisorInfo { get; set; }
        public Label lbTitulo { get; set; }
        public Label lbIngreso { get; set; }
        public Label lbEgreso { get; set; }
        public Label lbDiferencia { get; set; }



        public FlujoDia()
        {

          
            this.ContenedorDia = new Panel();
            this.ContenedorDia.SuspendLayout();
            this.DivisorInfo = new TableLayoutPanel();
            lbTitulo = new Label();
            lbIngreso = new Label();
            lbEgreso = new Label();
            lbDiferencia = new Label();

            this.ContenedorDia.Controls.Add(this.DivisorInfo);
            this.ContenedorDia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContenedorDia.Location = new System.Drawing.Point(3, 3);
            this.ContenedorDia.Name = "ContenedorDia";
            this.ContenedorDia.Size = new System.Drawing.Size(206, 109);
            this.ContenedorDia.TabIndex = 0;
            // 
            // DivisorInfo
            // 
            this.DivisorInfo.ColumnCount = 1;
            this.DivisorInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DivisorInfo.Controls.Add(this.lbDiferencia, 0, 3);
            this.DivisorInfo.Controls.Add(this.lbEgreso, 0, 2);
            this.DivisorInfo.Controls.Add(this.lbIngreso, 0, 1);
            this.DivisorInfo.Controls.Add(this.lbTitulo, 0, 0);
            this.DivisorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DivisorInfo.Location = new System.Drawing.Point(0, 0);
            this.DivisorInfo.Name = "tableLayoutPanel1";
            this.DivisorInfo.RowCount = 4;
            this.DivisorInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.DivisorInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.DivisorInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.DivisorInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.DivisorInfo.Size = new System.Drawing.Size(206, 109);
            this.DivisorInfo.TabIndex = 0;


            // 
            // lbTitulo
            // 
            this.lbTitulo.AutoSize = true;
            this.lbTitulo.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
          
            this.lbTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitulo.Location = new System.Drawing.Point(3, 0);
            this.lbTitulo.Name = "label12";
            this.lbTitulo.Size = new System.Drawing.Size(200, 27);
            this.lbTitulo.TabIndex = 0;
            this.lbTitulo.Text = "label12";
            this.lbTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbIngreso
            // 
            this.lbIngreso.AutoSize = true;
            this.lbIngreso.Dock = System.Windows.Forms.DockStyle.Fill;

            this.lbIngreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.lbIngreso.Location = new System.Drawing.Point(3, 27);
            this.lbIngreso.Name = "label13";
            this.lbIngreso.Size = new System.Drawing.Size(200, 27);
            this.lbIngreso.ForeColor = System.Drawing.Color.Blue;
            this.lbIngreso.BackColor = System.Drawing.Color.White;
            this.lbIngreso.TabIndex = 1;
            this.lbIngreso.Text = "label13";
            // 
            // lbEgreso
            // 
            this.lbEgreso.AutoSize = true;
            this.lbEgreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.lbEgreso.Dock = System.Windows.Forms.DockStyle.Fill;

            this.lbEgreso.Location = new System.Drawing.Point(3, 54);
            this.lbEgreso.Name = "label14";
            this.lbEgreso.ForeColor = System.Drawing.Color.Red;
            this.lbEgreso.BackColor = System.Drawing.Color.White;
            this.lbEgreso.Size = new System.Drawing.Size(200, 27);
            this.lbEgreso.TabIndex = 2;
            this.lbEgreso.Text = "label14";
            // 
            // lbDiferencia
            // 
            this.lbDiferencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDiferencia.AutoSize = true;
            this.lbDiferencia.Dock = System.Windows.Forms.DockStyle.Fill;
      
            this.lbDiferencia.Location = new System.Drawing.Point(3, 81);
            this.lbDiferencia.ForeColor = System.Drawing.Color.Green;
            this.lbDiferencia.BackColor = System.Drawing.Color.White;
            this.lbDiferencia.Name = "label15";
            this.lbDiferencia.Size = new System.Drawing.Size(200, 28);
            this.lbDiferencia.TabIndex = 3;
            this.lbDiferencia.Text = "label15";

        }
    }
}
