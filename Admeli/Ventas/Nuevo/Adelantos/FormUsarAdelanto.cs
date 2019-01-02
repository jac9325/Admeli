﻿using Admeli.Componentes;
using Entidad;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Ventas.Nuevo.Adelantos
{

    public partial class FormUsarAdelanto : Form
    {


        private double total { get; set; }
        private double monto { get; set; }
        public bool omitir { get; set; }
        public bool guardar { get; set; }
        public double adelanto { get; set; }
        public double porcentaje { get; set; }
        int nroDecimales = ConfigModel.configuracionGeneral.numeroDecimales;
        string formato { get; set; }
        public FormUsarAdelanto(double monto,double total)
        {
            InitializeComponent();
            formato = "{0:n" + nroDecimales + "}";
            porcentaje = 10;
            omitir = false;
            guardar = false;
           
            this.monto = monto;
            this.total = total;
            txtAdelanto.Text = darformato(monto);
            txtAdelantoUsado.Text = darformato(total);
            txtAdelantoUsado.Focus();
            txtAdelanto.Select();
;        }
        private string darformato(object dato)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
        }
        private string darformatoGuardar(object dato)
        {

            string var1 = string.Format(CultureInfo.GetCultureInfo("en-US"), this.formato, dato);
            var1 = var1.Replace(",", "");
            return var1;
        }

        private double toDouble(string texto)
        {

            if (texto == "")
            {
                return 0;
            }
            return double.Parse(texto, CultureInfo.GetCultureInfo("en-US")); ;
        }
        private int toEntero(string texto)
        {
            if (texto == "")
            {
                return 0;
            }
            return Int32.Parse(texto, CultureInfo.GetCultureInfo("en-US")); ;
        }
        private  void btnGuardar_Click(object sender, EventArgs e)
        {
            bloquear(true);
            try
            {
                double totaladelantoaux = toDouble(txtAdelanto.Text.Trim());
                double adelantoaux = toDouble(txtAdelantoUsado.Text.Trim());
                if (adelantoaux > totaladelantoaux)
                {

                    txtAdelantoUsado.Text = darformato(totaladelantoaux);
                    MessageBox.Show("La cantidad usada excede a total de adelantos " , "Comprobar Adelanto", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    bloquear(false);
                    txtAdelanto.Focus();
                    return;
                }


                guardar = true;
                adelanto = toDouble(txtAdelantoUsado.Text);
                this.Close();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "btnGuardar Response", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bloquear(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            omitir = false;
            this.Close();
        }
        public void bloquear(bool state)
        {
            if (state)
            {
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
            this.Enabled = !state;
        }

        private void txtAdelanto_KeyPress(object sender, KeyPressEventArgs e)
        {
            Validator.isDecimal(e, txtAdelanto.Text);
        }

        private void txtAdelanto_OnValueChanged(object sender, EventArgs e)
        {
           
        }
    }
}
