using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Aspose.Cells;
using System.Web;

namespace Motr.Office
{
    public class Excel
    {
        private Workbook _workBook;
        public Excel()
        {
            _workBook = new Workbook();
        }
        public Excel ToExcel(DataTable table)
        {
            var workSeeht = _workBook.Worksheets[0];
            workSeeht.Name = table.TableName;
            for (Int32 columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
               // workSeeht.Cells[0, columnIndex] = table.Columns[columnIndex].ColumnName
                for (Int32 rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                   // workSeeht.Cells[rowIndex + 1, columnIndex] = table.Rows[rowIndex][columnIndex];
                }
            }
            return this;
        }
        public void Response(String fileName)
        {
            _workBook.Save(fileName, FileFormatType.Excel97, SaveType.Default, HttpContext.Current.Response);
        }
    }
}
