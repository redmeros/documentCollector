using NPOI.SS.Util;

namespace ExcelListModule2;

public static class Helpers
{
    public static short GetColumnIndex(this string columnName)
    {
        var cellRef = new CellReference($"{columnName}1");
        return cellRef.Col;
    }
}