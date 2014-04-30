//使用Excel的Com组件，需要安装Office.
//参考第二种方式:http://blog.csdn.net/zhaoyu008/article/details/6294454
#define IS_USE_EXCEL_COM
//不使用Excel的Com组件
//#undef IS_USE_EXCEL_COM
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
#if IS_USE_EXCEL_COM
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
#endif
namespace Util.Common
{
    /// <summary>
    /// 工具类：导入导出Excel的基类
    /// http://www.cnblogs.com/springyangwc/archive/2011/08/12/2136498.html
    /// </summary>
    public class UtilExcel
    {
#if IS_USE_EXCEL_COM
        private Excel.Application app = null;
        private Excel.Workbook workbook = null;
        private Excel.Worksheet worksheet = null;
        private Excel.Range workSheet_range = null;
        private static UtilExcel current;
#endif

        public static UtilExcel Current()
        {
            if (current == null) current = new UtilExcel();
            return current;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UtilExcel()
        {
            CreateDoc();
        }

        /// <summary>
        /// 实例化Excel应用，新建一个Excel工作簿，设置第一张表当前活动表
        /// </summary>
        public void CreateDoc()
        {
#if IS_USE_EXCEL_COM
            try
            {
                app = new Excel.Application();
                app.Visible = false;
                
                workbook = app.Workbooks.Add(1);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
            }
            catch (Exception e)
            {
                Console.Write("Error:" + e.Message);
            }
            finally
            {

            }
#endif
        }
        
        /// <summary>
        /// 当前工作簿的表计数
        /// </summary>
        public int GetSheetCount()
        {
#if IS_USE_EXCEL_COM
            return workbook.Sheets.Count;
#endif
        }

        /// <summary>
        /// 当前活动表的表名
        /// </summary>
        public string GetSheetName()
        {
#if IS_USE_EXCEL_COM
            return worksheet.Name;
#endif
        }

        /// <summary>
        /// 修改当前活动表的名称
        /// </summary>
        /// <param name="sheetName">表名</param>
        public void SetSheet(String sheetName)
        {
#if IS_USE_EXCEL_COM
            worksheet.Name = sheetName;
#endif
        }

        /// <summary>
        /// 在所有表尾部插入一张新表
        /// 如果不设置表名，则使用默认值
        /// </summary>
        /// <param name="sheetName">表名</param>
        public void AddSheet(String sheetName)
        {
#if IS_USE_EXCEL_COM
            worksheet = (Excel.Worksheet)workbook.Sheets.Add(After:(Excel.Worksheet)workbook.Sheets[workbook.Sheets.Count]);
            if (sheetName!= String.Empty)
            {
                worksheet.Name = sheetName;
            }
#endif
        }

        /// <summary>
        /// 设置当前活动表
        /// </summary>
        /// <param name="sheetNO">表索引值</param>
        public void SetActivateSheet(byte sheetNO)
        {
#if IS_USE_EXCEL_COM
            worksheet = (Excel.Worksheet)workbook.Sheets[sheetNO];
            worksheet.Activate();
#endif
        }
        

        /// <summary>
        /// 向当前活动表插入
        /// </summary>
        /// <param name="be"></param>
        public void InsertData(ExcelBE be)
        {
#if IS_USE_EXCEL_COM
            worksheet.Cells[be.Row, be.Col] = be.Text;
            workSheet_range = worksheet.get_Range(be.StartCell, be.EndCell);
            workSheet_range.Merge(be.IsMerge);
            workSheet_range.Interior.Color = GetColorValue(be.InteriorColor);
            workSheet_range.Borders.Color = System.Drawing.Color.Black.ToArgb();
            workSheet_range.ColumnWidth = be.ColumnWidth;
            workSheet_range.RowHeight = be.RowHeight;
            //to-do:枚举HorizontalAlignment
            if (be.HorizontalAlignmentIndex == 1)
            {
                workSheet_range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; ;
            }
            else if (be.HorizontalAlignmentIndex == 2)
            {
                workSheet_range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; ;
            }
            //to-do:设置单元格边框(未列在参数)
            workSheet_range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            workSheet_range.Borders.Color = System.Drawing.Color.White.ToArgb();

            workSheet_range.Font.Color = string.IsNullOrEmpty(be.FontColor) ? System.Drawing.Color.White.ToArgb() : System.Drawing.Color.Black.ToArgb();
            workSheet_range.Font.Name = be.FontName;
            workSheet_range.Font.Bold = be.FontBold;
            workSheet_range.Font.Size = be.FontSize;
            workSheet_range.NumberFormat = be.Formart;
#endif
        }

        /// <summary>
        /// 返回颜色的ARGB值
        /// </summary>
        /// <param name="interiorColor">颜色字符串</param>
        /// <returns></returns>
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
                case "GRAYDARK":
                    return System.Drawing.Color.DarkGray.ToArgb();
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
            dt = ExchangeColName(dt, fields);
            objConn.Close();
            objConn.Dispose();
            return dt;
        }

        /// <summary>
        /// 在当前列上加链接
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="screenTipMsg">鼠标移上去显示文字</param>
        public void AddLink(ExcelBE be,string sheetName, string screenTipMsg)
        {
            
            string hyperlinkTargetAddress = sheetName+"!A1";
            worksheet.Hyperlinks.Add(
                workSheet_range,
                string.Empty,
                hyperlinkTargetAddress,
                sheetName + "!A1-"+screenTipMsg,
                sheetName);
            workSheet_range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            workSheet_range.Borders.Color = System.Drawing.Color.White.ToArgb();

            workSheet_range.Font.Color = string.IsNullOrEmpty(be.FontColor) ? System.Drawing.Color.White.ToArgb() : System.Drawing.Color.Black.ToArgb();
            workSheet_range.Font.Name = be.FontName;
            workSheet_range.Font.Bold = be.FontBold;
            workSheet_range.Font.Size = be.FontSize;
            workSheet_range.NumberFormat = be.Formart;
        }

        /// <summary>
        /// 中文表头转为数据表字段
        /// </summary>
        /// <param name="dt">excel数据</param>
        /// <param name="fields">列名对照</param>
        /// <returns></returns>
        public static DataTable ExchangeColName(DataTable dt, Dictionary<string, string> fields)
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

        /// <summary>
        /// 导出
        /// </summary>
        public void DoExport()
        {
            app.Visible = true;
        }

        public static void Release()
        {
            if (current == null) return;
            if (current.workbook != null) current.workbook.Close(true);
            current. workbook = null;
            if (current.app != null) current.app.Quit();
            current.app = null;
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        /// <param name="filepath"></param>
        public void Save(string filepath)
        {
            workbook.Saved = true;
            workbook.SaveCopyAs(filepath);
        }
    }
}
