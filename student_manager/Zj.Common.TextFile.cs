using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Zj.Common
{
    class ZjTextFile
    {
        public static string Read(string filePaZj, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(filePaZj, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        public static void Write(string filePaZj, string content, Encoding encoding)
        {
            using (StreamWriter sw = new StreamWriter(filePaZj, false, encoding))
            {
                sw.Write(content);
            }
        }

        public static Encoding UTF8
        {
            get
            {
                return UTF8Encoding.UTF8;
            }
        }
        public static Encoding GBK
        {
            get
            {
                return Encoding.GetEncoding("GBK");
            }
        }
    }

    class ZjExcelFile
    {
        public static void WriteToExcelFile(string filePaZj, string sheetNaZj, string[,] daZj)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetNaZj);

                int rowCount = daZj.GetLength(0);
                int columnCount = daZj.GetLength(1);

                for (int row = 1; row <= rowCount; row++)
                {
                    for (int column = 1; column <= columnCount; column++)
                    {
                        worksheet.Cells[row, column].Value = daZj[row - 1, column - 1];
                    }
                }

                FileInfo file = new FileInfo(filePaZj);
                package.SaveAs(file);
            }
        }
    }
}
