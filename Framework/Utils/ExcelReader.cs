using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Lab9Automation.Models;

namespace Lab9Automation.Framework.Utils
{
    public class ExcelReader
    {
        public static List<LoginExcelRow> ReadLoginData(string filePath, string sheetName)
        {
            List<LoginExcelRow> rows = new List<LoginExcelRow>();

            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new XSSFWorkbook(fs);
            ISheet? sheet = workbook.GetSheet(sheetName);

            if (sheet == null)
                throw new Exception($"Không tìm thấy sheet: {sheetName}");

            IRow? headerRow = sheet.GetRow(0);
            if (headerRow == null)
                throw new Exception($"Sheet {sheetName} không có header.");

            Dictionary<string, int> columnIndexes = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                string header = GetCellValue(headerRow.GetCell(i));
                if (!string.IsNullOrWhiteSpace(header))
                {
                    columnIndexes[header.Trim()] = i;
                }
            }

            for (int r = 1; r <= sheet.LastRowNum; r++)
            {
                IRow? row = sheet.GetRow(r);
                if (row == null) continue;

                LoginExcelRow data = new LoginExcelRow
                {
                    Username = GetValue(row, columnIndexes, "username"),
                    Password = GetValue(row, columnIndexes, "password"),
                    ExpectedUrl = GetValue(row, columnIndexes, "expected_url"),
                    ExpectedError = GetValue(row, columnIndexes, "expected_error"),
                    Description = GetValue(row, columnIndexes, "description")
                };

                if (string.IsNullOrWhiteSpace(data.Description))
                {
                    data.Description = $"{sheetName}_Row_{r}";
                }

                rows.Add(data);
            }

            return rows;
        }

        private static string GetValue(IRow row, Dictionary<string, int> columns, string name)
        {
            if (!columns.ContainsKey(name)) return "";
            return GetCellValue(row.GetCell(columns[name]));
        }

        public static string GetCellValue(ICell? cell)
        {
            if (cell == null) return "";

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue.Trim();

                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString();
                    }

                    double num = cell.NumericCellValue;
                    return num % 1 == 0
                        ? ((long)num).ToString()
                        : num.ToString();

                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();

                case CellType.Formula:
                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            return cell.StringCellValue.Trim();

                        case CellType.Numeric:
                            double val = cell.NumericCellValue;
                            return val % 1 == 0
                                ? ((long)val).ToString()
                                : val.ToString();

                        case CellType.Boolean:
                            return cell.BooleanCellValue.ToString();

                        default:
                            return "";
                    }

                default:
                    return "";
            }
        }
    }
}