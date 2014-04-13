using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web; 
using Excel = Microsoft.Office.Interop.Excel;

namespace Util.Common
{
    public class UtilExcel
    {
        private Excel.Application app = null;
        private Excel.Workbook workbook = null;
        private Excel.Worksheet worksheet = null;
        private Excel.Range workSheet_range = null;

        public UtilExcel()
        {
            createDoc();
        }

        public void createDoc()
        {
            try
            {
                app = new Excel.Application();
                app.Visible = false;
                workbook = app.Workbooks.Add(1);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
            }
            catch (Exception e)
            {
                Console.Write("Error");
            }
            finally
            {

            }
        }

        public void InsertData(ExcelBE be)
        {
            worksheet.Cells[be.Row, be.Col] = be.Text;
            workSheet_range = worksheet.get_Range(be.StartCell, be.EndCell);
            workSheet_range.Merge(be.IsMerge);
            workSheet_range.Interior.Color = GetColorValue(be.InteriorColor);
            workSheet_range.Borders.Color = System.Drawing.Color.Black.ToArgb();
            workSheet_range.ColumnWidth = be.Size;
            workSheet_range.Font.Color = string.IsNullOrEmpty(be.FontColor) ? System.Drawing.Color.White.ToArgb() : System.Drawing.Color.Black.ToArgb();
            workSheet_range.NumberFormat = be.Formart;
        }

        private int GetColorValue(string interiorColor)
        {
            switch (interiorColor)
            {
                case "YELLOW":
                    return System.Drawing.Color.Yellow.ToArgb();
                case "GRAY":
                    return System.Drawing.Color.Gray.ToArgb();
                case "GAINSBORO":
                    return System.Drawing.Color.Gainsboro.ToArgb();
                case "Turquoise":
                    return System.Drawing.Color.Turquoise.ToArgb();
                case "PeachPuff":
                    return System.Drawing.Color.PeachPuff.ToArgb();
                default:
                    return System.Drawing.Color.White.ToArgb();
            }
        }

        /// <summary>
        /// 读取excel文件数据
        /// </summary>
        /// <param name="filepath">文件物理路径</param>
        /// <param name="fields">字段映射</param>
        /// <returns></returns>
        public DataTable CallExcel(string filepath, Dictionary<string, string> fields)
        {
            string strConn = GetExcelConnectionString(filepath);
            OleDbConnection objConn = new OleDbConnection(strConn);
            objConn.Open();
            string sql = "select * from [Worksheet$]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, objConn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dt = exchangeColName(dt, fields);
            objConn.Close();
            objConn.Dispose();
            return dt;
        }

        /// <summary>
        /// 读取邦礼商品Excel
        /// </summary>
        /// <param name="filepath">文件物理路径</param>
        /// <param name="productfield">商品字段映射</param>
        /// <param name="imagefield">商品图片字段映射</param>
        /// <returns></returns>
        public static Dictionary<String, DataTable> CallExcelFromBonli(string filepath, Dictionary<string, string> productfields, Dictionary<string, string> imagefields)
        {
            string strConn = GetExcelConnectionString(filepath);
            OleDbConnection objConn = new OleDbConnection(strConn);
            objConn.Open();
            string productsql = "select * from [product$]";
            OleDbDataAdapter productAdapter = new OleDbDataAdapter(productsql, objConn);
            string imagesql = "select * from [seriesimg$]";
            OleDbDataAdapter imageAdapter = new OleDbDataAdapter(imagesql, objConn);
            DataTable product = new DataTable();
            productAdapter.Fill(product);
            DataTable image = new DataTable();
            imageAdapter.Fill(image);
            product = exchangeColName(product, productfields);
            image = exchangeColName(image, imagefields);
            Dictionary<String, DataTable> result = new Dictionary<String, DataTable>();
            result.Add("Product",product);
            result.Add("Image",image);
            objConn.Close();
            objConn.Dispose();
            return result;
        }

        /// <summary>
        /// 中文表头转为数据表字段
        /// </summary>
        /// <param name="dt">excel数据</param>
        /// <param name="fields">列名对照</param>
        /// <returns></returns>
        public static DataTable exchangeColName(DataTable dt, Dictionary<string, string> fields)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                bool exist = fields.ContainsKey(dt.Columns[i].ColumnName);
                if (exist)
                {
                    dt.Columns[i].ColumnName = fields[dt.Columns[i].ColumnName];
                }
            }
            return dt;
        }

        /// <summary>
        /// 判断连接符
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetExcelConnectionString(string filepath)
        {
            string connectionString = string.Empty;
            string fileExtension = filepath.Substring(filepath.LastIndexOf(".") + 1);
            switch (fileExtension)
            {
                case "xls":
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
                case "xlsx":
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
                    break;
            }
            return connectionString;
        }
    }
}
