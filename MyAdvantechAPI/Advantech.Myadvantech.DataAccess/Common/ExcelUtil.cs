using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    /// <summary>
    /// Common function for Excel
    /// </summary>
    public class ExcelUtil
    {
        /// <summary>
        /// Convert datatable to memory stream
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream DataTableToMemoryStream(DataTable dt)
        { 
            MemoryStream ms = new MemoryStream();
            XSSFWorkbook _workbook = new XSSFWorkbook();
            ISheet _sheet = _workbook.CreateSheet("Sheet1");

            //Header Row
            IRow headerRow = _sheet.CreateRow(0);
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
                headerRow.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);

            //Body Row
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                IRow bodyRow = _sheet.CreateRow(i + 1);
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    if (dt.Rows[i][j].ToString().StartsWith("="))
                        bodyRow.CreateCell(j).SetCellFormula(dt.Rows[i][j].ToString());
                    else
                        bodyRow.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            _workbook.Write(ms);
            return ms;
        }
    }
}
