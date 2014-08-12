using System.Collections.Generic;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Diagnostics;

namespace Util.Common
{
    /// <summary>
    /// 工具类：采用Npoi方式读写Excel
    /// 使用NPOI 2.0进行开发
    /// </summary>
    /// TODO:完成工具类【采用Npoi方式读写Excel】
    /// <see cref="http://tonyqus.sinaapp.com/" title="《NPOI指南》目录（草稿）"/>
    /// <see cref="https://npoi.codeplex.com/releases" title="下载NPOI"/>
    /// <see cref="http://msdn.microsoft.com/zh-tw/ee818993.aspx" title="在 Server 端存取 Excel 檔案的利器：NPOI Library"/>
    public static class UtilExcelNpoi
    {
        #region Excel2003 DataTable和Excel导入导出
        /// <summary>
        /// 将Excel文件中指定sheet索引的数据读出到DataTable中(xls)
        /// </summary>
        /// <param name="FileName">Excel文件名称</param>
        /// <param name="SheetIndex">Sheet索引</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string FileName, int SheetIndex=0)
        {
            DataTable dt = new DataTable();
            if (File.Exists(FileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                        ISheet sheet = hssfworkbook.GetSheetAt(SheetIndex);

                        //表头
                        IRow header = sheet.GetRow(sheet.FirstRowNum);
                        List<int> columns = new List<int>();
                        for (int i = 0; i < header.LastCellNum; i++)
                        {
                            object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                            if (obj == null || obj.ToString() == string.Empty)
                            {
                                dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                                //continue;
                            }
                            else
                                dt.Columns.Add(new DataColumn(obj.ToString()));
                            columns.Add(i);
                        }
                        //数据
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            DataRow dr = dt.NewRow();
                            bool hasValue = false;
                            foreach (int j in columns)
                            {
                                dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                                if (dr[j] != null && dr[j].ToString() != string.Empty)
                                {
                                    hasValue = true;
                                }
                            }
                            if (hasValue)
                            {
                                dt.Rows.Add(dr);
                            }
                        }
                        sheet = null;
                        hssfworkbook = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else dt = null;
            return dt;
        }

        /// <summary>
        /// 将Excel文件中指定sheet名称的数据读出到DataTable中(xls)
        /// </summary>
        /// <param name="FileName">Excel文件名称</param>
        /// <param name="SheetName">Sheet名称</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string FileName, string SheetName)
        {
            if (string.IsNullOrEmpty(SheetName)) return null;
            DataTable dt = new DataTable();
            if (File.Exists(FileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                        ISheet sheet = hssfworkbook.GetSheet(SheetName);

                        //表头
                        IRow header = sheet.GetRow(sheet.FirstRowNum);
                        List<int> columns = new List<int>();
                        for (int i = 0; i < header.LastCellNum; i++)
                        {
                            object obj = GetValueTypeForXLS(header.GetCell(i) as HSSFCell);
                            if (obj == null || obj.ToString() == string.Empty)
                            {
                                dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                                //continue;
                            }
                            else
                                dt.Columns.Add(new DataColumn(obj.ToString()));
                            columns.Add(i);
                        }
                        //数据
                        for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                        {
                            DataRow dr = dt.NewRow();
                            bool hasValue = false;
                            foreach (int j in columns)
                            {
                                dr[j] = GetValueTypeForXLS(sheet.GetRow(i).GetCell(j) as HSSFCell);
                                if (dr[j] != null && dr[j].ToString() != string.Empty)
                                {
                                    hasValue = true;
                                }
                            }
                            if (hasValue)
                            {
                                dt.Rows.Add(dr);
                            }
                        }
                        sheet = null;
                        hssfworkbook = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else dt = null;
            return dt;
        }

        /// <summary>
        /// 计数:所有的Sheet数量
        /// </summary>
        /// <param name="FileName">Excel文件名称</param>
        /// <returns></returns>
        public static int CountSheets(string FileName)
        {
            if (File.Exists(FileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                        return hssfworkbook.Count;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据Sheet索引获取Sheet名称
        /// </summary>
        /// <param name="FileName">Excel文件名称</param>
        /// <param name="SheetIndex">Sheet索引</param>
        /// <returns></returns>
        public static string GetSheetName(string FileName, int SheetIndex)
        {
            if (File.Exists(FileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                        string SheetName = hssfworkbook.GetSheetName(SheetIndex);
                        return SheetName;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return "";

        }

        /// <summary>
        /// 获取所有的Sheet的名称
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string[] GetExcelSheetNames(string FileName)
        {
            string[] Result=null;
            if (File.Exists(FileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs);
                        int Count = hssfworkbook.Count;
                        if (Count > 0)
                        {
                            Result = new string[Count];
                            for (int Index = 0; Index < Count; Index++)
                            {
                                Result[Index] = hssfworkbook.GetSheetName(Index);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            return Result;
        }


        /// <summary>
        /// 将DataTable数据导出到Excel文件中(xls)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FileName"></param>
        public static void DataTableToExcel(string FileName,DataTable dt,string SheetName="Sheet1")
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet(SheetName);

            //表头
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组
            MemoryStream stream = new MemoryStream();
            hssfworkbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件
            using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        /// <summary>
        /// 获取单元格类型(xls)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }
        #endregion
    }
}
