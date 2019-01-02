using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Admeli.Componentes
{
    class ExternalFiles
    {
        public static void ExportarDataGridViewExcel(DataGridView grd)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();
                hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                //Colocar los titulos del DataGridView
                int k = 0;
                for (int j = 0; j < grd.Columns.Count; j++)
                {
                    if (grd.Columns[j].Visible)
                    {
                        hoja_trabajo.Cells[1, k++ + 1] = grd.Columns[j].HeaderText;
                    }
                  
                }
                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < grd.Rows.Count; i++)
                {
                    k = 0;
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        if (grd.Columns[j].Visible)
                        {
                            hoja_trabajo.Cells[i + 2, k++ + 1] = grd.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                }
                libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
            }
        }
        public static DataTable ImporExcel()
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                //open.Filter = "Archivo Excel|*.xls;*.xlsx;*.xlsm;*.csv";
                open.Filter = "Archivo Excel|*.xls;*.xlsm;*.csv";
                open.Title = "Abrir archivo Excel";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    string ExcelFileName = open.FileName;
                    string connectionString = string.Empty;

                    if (Path.GetExtension(ExcelFileName) == ".xlsx")
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", ExcelFileName);
                    }
                    else
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFileName + ";Extended Properties=Excel 8.0;";
                    }
                    OleDbConnection connExcel = new OleDbConnection(connectionString);
                    OleDbCommand cmdExcel = new OleDbCommand();
                    OleDbDataAdapter oda = new OleDbDataAdapter();
                    cmdExcel.Connection = connExcel;

                    // Obtener el nombre de First Sheet
                    connExcel.Open();
                    DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                    // Leer datos de la primera hoja
                    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                    oda.SelectCommand = cmdExcel;

                    // LLenando datos de la tabla
                    DataTable dt = new DataTable();
                    oda.Fill(dt);
                    connExcel.Close();
                    return dt;

                    //// Tratamiento de la tabla
                    //DataRow row = dt.Rows[0];
                    //int index = 0;
                    //foreach (var item in row.ItemArray)
                    //{
                    //    dt.Columns[index].ColumnName = (item.ToString() == "") ? " " : item.ToString();
                    //    index++;
                    //}
                    //dt.Rows[0].Delete();
                    //dt.AcceptChanges();
                    //return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }

}
